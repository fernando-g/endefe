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

namespace FacturaElectronica.Ui.Win.Administrador
{
    public partial class FormAutorizador : Form
    {
        private StringBuilder validacionEntradaDatos;
        private ProcesoAutorizador autorizador = new ProcesoAutorizador(true);
        private CorridaAutorizacionDto corridaDto = null;
        private ICorridaService corridaSvc = ServiceFactory.GetCorridaService();
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
            DialogResult result = this.openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.InicializarValores();
                this.FileTextBox.Text = this.openFileDialog.FileName;
                this.btnAutorizar.Enabled = true;
                this.btnVerDetalleCorrida.Enabled = false;
            }

        }

        private void AutorizarButton_Click(object sender, EventArgs e)
        {
            this.timerAutorizacion.Enabled = true;
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

            this.corridaDto = corridaSvc.CrearNuevaCorrida(this.FileTextBox.Text.Trim());
            this.fechaLog = DateTime.Now;

            //// Obtener Ticket de Acceso y Validar Contra la AFIP            
            object[] arguments = new object[]{ corridaDto};
            this.backgroundWorker.RunWorkerAsync(arguments);
            
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

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = e.Argument as object[];
            CorridaAutorizacionDto corridaDto = arguments[0] as CorridaAutorizacionDto;
            ProcesoAutorizador autorizador = new ProcesoAutorizador(true);
            e.Result = autorizador.AutorizarComprobantes(corridaDto);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.MostrarLog();
            this.progressBar.Value = 100;
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message,
                                "Exception",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);            
            }

            this.corridaDto = e.Result as CorridaAutorizacionDto;
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
            // Leer LogCorrida y reportar progreso
            MostrarLog();
            if(this.backgroundWorker.IsBusy)
                this.backgroundWorker.ReportProgress(10);
        }

        private void MostrarLog()
        {
            List<LogCorridaDto> logs = this.corridaSvc.ConsultarLog(this.corridaDto.Id, this.fechaLog);
            if (logs != null && logs.Count() > 0)
            {
                foreach (LogCorridaDto log in logs)
                {
                    this.LogTextBox.Text += log.Mensaje + Environment.NewLine;
                }
                fechaLog = logs.Last().Fecha;
            }
        }

        private void btnVerDetalleCorrida_Click(object sender, EventArgs e)
        {
            // Llamar a form de detalle
            FormAutorizadorResultados frm = new FormAutorizadorResultados(this.corridaDto);
            frm.ShowDialog(this);
            
        }
    }
}
