using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FacturaElectronica.Afip.Business;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Ui.Win.Administrador.Code;
using System.Configuration;
using FacturaElectronica.Common.Contracts.Search;

namespace FacturaElectronica.Ui.Win.Administrador
{
    public partial class FormAutorizador : Form
    {
        private StringBuilder validacionEntradaDatos;
        private ProcesoAutorizador autorizador = new ProcesoAutorizador(true);
        private CorridaAutorizacionDto corridaDto = null;
        private IProcesoCorridaService procesoCorridaSvc = ServiceFactory.GetProcesoCorridaService();
        private DateTime fechaLog = default(DateTime);

        public FormAutorizador()
        {
            InitializeComponent();
            this.openFileDialog.Filter = "XML Files|*.xml";
            this.InicializarValores();
            this.timerAutorizacion.Interval = 1000;
        }

        private void ExaminarButton_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = this.openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    this.InicializarValores();
                    this.FileTextBox.Text = this.openFileDialog.FileName;
                    this.btnAutorizar.Enabled = true;
                    this.btnVerDetalleCorrida.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensajeEnLog(ex.Message);
            }   
        }

        private void AutorizarButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.timerAutorizacion.Enabled = true;
                this.timerAutorizacion.Start();
                this.btnExaminar.Enabled = false;
                this.btnAutorizar.Enabled = false;
                this.LogTextBox.Text = string.Empty;

                // Validar Entrada
                if (!this.ValidarDatosDeEntrada())
                {
                    MessageBox.Show(this.validacionEntradaDatos.ToString(),
                                    "Validación Datos de entrada",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Exclamation);

                    return;
                }

                // Primero copio el archivo en el servidor para que lo procese el web service

                string origenPath = this.FileTextBox.Text.Trim();
                string destinationPath = ConfigurationManager.AppSettings["PathDestinoArchivosXMLParaProcesar"];
                string fileName = Path.GetFileName(origenPath);

                destinationPath = Path.Combine(destinationPath, DateTime.Now.ToString("yyyy"));
                destinationPath = Path.Combine(destinationPath, DateTime.Now.ToString("MM"));

                if(!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                destinationPath = Path.Combine(destinationPath, fileName);

                File.Copy(origenPath, destinationPath, true);

                this.corridaDto = procesoCorridaSvc.CrearNuevaCorrida(destinationPath);
                this.fechaLog = DateTime.Now;

                procesoCorridaSvc.EjecutarCorrida(this.corridaDto.Id);
            }
            catch (Exception ex)
            {
                this.MostrarMensajeEnLog(ex.Message);
            }            
        }

        private void InicializarValores()
        {
            this.LogTextBox.Text = string.Empty;
            this.lblComprobantesAutorizados.Text = "0";
            this.lblComprobantesConObs.Text = "0";
            this.lblErroresEncontrados.Text = "0";
            this.lblEventos.Text = "0";
            this.btnExaminar.Enabled = true;
            this.btnAutorizar.Enabled = false;
            this.btnVerDetalleCorrida.Enabled = false;
            this.progressBar.Value = 0;
        }

        private bool ValidarDatosDeEntrada()
        {
            validacionEntradaDatos = new StringBuilder();
            
            if(string.IsNullOrEmpty(FileTextBox.Text.Trim()))
            {
                validacionEntradaDatos.AppendLine("Debe seleccionar un archivo");
            }

            if (!File.Exists(this.FileTextBox.Text))
            {
                validacionEntradaDatos.AppendLine("El archivo seleccionado no existe.");
            }

            return validacionEntradaDatos.Length == 0;
        }

        #region Proceso Asincronico              

        private void MostrarFinCorrida()
        {
            this.timerAutorizacion.Stop();           
            this.progressBar.Value = 100;          

            this.corridaDto = this.procesoCorridaSvc.ObtenerCorridas(new CorridaSearch(){ CorridaId= this.corridaDto.Id }).Single();
            //this.corridaDto = 
            if (corridaDto != null)
            {
                if (corridaDto.ComprobantesAutorizados != null)
                {
                    this.lblComprobantesAutorizados.Text = corridaDto.ComprobantesAutorizados.Count().ToString();
                }
                if (corridaDto.ComprobantesConObservaciones != null)
                {
                    this.lblComprobantesConObs.Text = corridaDto.ComprobantesConObservaciones.Count().ToString();
                }
                if (corridaDto.Errores != null)
                {
                    this.lblErroresEncontrados.Text = corridaDto.Errores.Count().ToString();
                }
                if (corridaDto.Eventos != null)
                {
                    this.lblEventos.Text = corridaDto.Eventos.Count().ToString();
                }
            }

            this.timerAutorizacion.Enabled = false;
            this.btnExaminar.Enabled = true;
            this.btnAutorizar.Enabled = false;
            this.btnVerDetalleCorrida.Enabled = true;
        }

        #endregion Proceso Asincronico

        private void timerAutorizacion_Tick(object sender, EventArgs e)
        {
            try
            {
                // Leer LogCorrida y reportar progreso
                MostrarLog();
                if (this.progressBar.Value < 98)
                {
                    this.progressBar.Value += 1;
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensajeEnLog(ex.Message);
                this.timerAutorizacion.Stop();
            }   
        }

        private void MostrarMensajeEnLog(string text)
        {
            this.LogTextBox.Text += text + Environment.NewLine;
        }

        private void MostrarLog()
        {
            List<LogCorridaDto> logs = this.procesoCorridaSvc.ConsultarLog(this.corridaDto.Id, this.fechaLog);
            if (logs != null && logs.Count() > 0)
            {
                foreach (LogCorridaDto log in logs)
                {
                    if (log.CorridaTerminada)
                    {
                        MostrarFinCorrida();
                        break;
                    }
                    else
                    {
                        this.LogTextBox.Text += log.Mensaje + Environment.NewLine;
                    }
                }

                fechaLog = logs.Last().Fecha;
            }
        }

        private void btnVerDetalleCorrida_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamar a form de detalle
                FormAutorizadorResultados frm = new FormAutorizadorResultados(this.corridaDto);
                frm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                this.MostrarMensajeEnLog(ex.Message);
            }               
        }
    }
}
