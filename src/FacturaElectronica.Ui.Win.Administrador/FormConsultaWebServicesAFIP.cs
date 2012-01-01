using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacturaElectronica.Afip.Business.Wsfe;
using System.IO;
using System.Xml;
using System.Globalization;
using FacturaElectronica.Afip.Ws.Wsfe;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Ui.Win.Administrador.Code;
using FacturaElectronica.Business.Services;
using FacturaElectronica.Common.Contracts;

namespace FacturaElectronica.Ui.Win.Administrador
{
    public partial class FormConsultaWebServicesAFIP : Form
    {
        #region [Constantes]

        private const string GetTiposMonedasNombre = "FEParamGetTiposMonedas";
        private const string GetTiposCbtesNombre = "FEParamGetTiposCbte";
        private const string GetTiposTributosNombre = "FEParamGetTiposTributos";
        private const string GetTiposIvaNombre = "FEParamGetTiposIva";
        private const string GetTiposOpcionalNombre = "FEParamGetTiposOpcional";
        private const string CompTotXRequestNombre = "FECompTotXRequest";
        private const string GetTiposDocNombre = "FEParamGetTiposDoc";
        private const string CompUltimoAutorizadoNombre = "FECompUltimoAutorizado";
        private const string GetTiposConceptoNombre = "FEParamGetTiposConcepto";
        private const string GetCotizacionNombre = "FEParamGetCotizacion";

        #endregion [Constantes]

        #region [Atributos]

        private FacturaElectronica.Afip.Ws.Wsfe.FEAuthRequest ticket;
        private WsfeClient client = new WsfeClient();
        private string opcionAnterior = string.Empty;

        #endregion [Atributos]

        public FormConsultaWebServicesAFIP()
        {
            InitializeComponent();
            this.ticket = client.ObtenerTicket();
            this.CargarMetodos();
            //
            this.lblResultados.Location = new Point(this.lblResultados.Location.X, this.lblResultados.Location.Y - this.panelMoneda.Height);
            this.btnConsultar.Location = new Point(this.btnConsultar.Location.X, this.btnConsultar.Location.Y - this.panelMoneda.Height);
            this.gridResultados.Location = new Point(this.gridResultados.Location.X, this.gridResultados.Location.Y - this.panelMoneda.Height);
            this.txtResultados.Location = new Point(this.txtResultados.Location.X, this.txtResultados.Location.Y - this.panelMoneda.Height);
            this.lblResultados.Visible = false;

        }

        private void InicializarControles()
        {
            this.lblResultados.Text = "Resultados:";
            this.lblResultados.Visible = true;
            this.panelMoneda.Visible = false;
            this.panelUltimoCbte.Visible = false;
            this.txtResultados.Visible = false;
            this.gridResultados.Visible = false;
        }

        #region [Cargar Combos]

        private void CargarMetodos()        
        {
            this.cbWebService.ValueMember = "Id";
            this.cbWebService.DisplayMember = "Nombre";
            IMaestrosService svc = ServiceFactory.GetMaestroService();
            this.cbWebService.DataSource = svc.ObtenerWebServicesAfip();
            this.cbWebService.SelectedIndex = 0;
        }

        private void CargarMonedas()
        {
            if (cbMoneda.Items.Count == 0)
            {
                FacturaElectronica.Afip.Ws.Wsfe.MonedaResponse resultado = client.GetTiposMonedas(this.ticket);
                this.cbMoneda.DisplayMember = "Desc";
                this.cbMoneda.ValueMember = "Id";
                this.cbMoneda.DataSource = resultado.ResultGet.OrderBy(m => m.Desc).ToList();
            }
        }

        private void CargarTiposCbte()
        {
            if (cbTiposCbte.Items.Count == 0)
            {
                FacturaElectronica.Afip.Ws.Wsfe.CbteTipoResponse resultado = client.GetTiposCbte(this.ticket);
                this.cbTiposCbte.DisplayMember = "Desc";
                this.cbTiposCbte.ValueMember = "Id";
                this.cbTiposCbte.DataSource = resultado.ResultGet;
            }
        }

        #endregion [Cargar Combos]

        private void cbWebService_SelectedIndexChanged(object sender, EventArgs e)
        {
            InicializarControles();
            if (cbWebService.SelectedItem != null)
            {
                bool mostrarPanelParametros = false;
                WebServiceAfipDto dto = cbWebService.SelectedItem as WebServiceAfipDto;
                string selectedValue = dto.Nombre.Trim();

                if (selectedValue == GetCotizacionNombre)
                {
                    mostrarPanelParametros = true;
                    this.panelMoneda.Visible = true;
                    this.CargarMonedas();
                    this.txtResultados.Visible = true;
                }
                else if (selectedValue == GetTiposMonedasNombre ||
                         selectedValue == GetTiposCbtesNombre ||
                         selectedValue == GetTiposTributosNombre ||
                         selectedValue == GetTiposIvaNombre ||
                         selectedValue == GetTiposOpcionalNombre ||
                         selectedValue == GetTiposDocNombre ||
                         selectedValue == GetTiposConceptoNombre)
                {
                    this.gridResultados.Visible = true;
                    this.gridResultados.DataSource = null;
                }
                else if (selectedValue == CompTotXRequestNombre)
                {
                    this.txtResultados.Visible = true;
                }
                else if (selectedValue == CompUltimoAutorizadoNombre)
                {
                    mostrarPanelParametros = true;
                    this.txtPtoVta.Text = string.Empty;
                    this.panelUltimoCbte.Visible = true;
                    this.CargarTiposCbte();
                    this.txtResultados.Visible = true;
                }
                if (!mostrarPanelParametros &&
                    (opcionAnterior == CompUltimoAutorizadoNombre ||
                     opcionAnterior == GetCotizacionNombre))
                {
                    this.lblResultados.Location = new Point(this.lblResultados.Location.X, this.lblResultados.Location.Y - this.panelMoneda.Height);
                    this.btnConsultar.Location = new Point(this.btnConsultar.Location.X, this.btnConsultar.Location.Y - this.panelMoneda.Height);
                    this.gridResultados.Location = new Point(this.gridResultados.Location.X, this.gridResultados.Location.Y - this.panelMoneda.Height);
                    this.gridResultados.Height += this.panelMoneda.Height;
                    this.txtResultados.Location = new Point(this.txtResultados.Location.X, this.txtResultados.Location.Y - this.panelMoneda.Height);
                    this.txtResultados.Height += this.panelMoneda.Height;
                    opcionAnterior = selectedValue;
                }
                else if (mostrarPanelParametros &&
                        (opcionAnterior != CompUltimoAutorizadoNombre &&
                         opcionAnterior != GetCotizacionNombre))
                {
                    this.lblResultados.Location = new Point(this.lblResultados.Location.X, this.lblResultados.Location.Y + this.panelMoneda.Height);
                    this.btnConsultar.Location = new Point(this.btnConsultar.Location.X, this.btnConsultar.Location.Y + this.panelMoneda.Height);
                    this.gridResultados.Location = new Point(this.gridResultados.Location.X, this.gridResultados.Location.Y + this.panelMoneda.Height);
                    this.gridResultados.Height -= this.panelMoneda.Height;
                    this.txtResultados.Location = new Point(this.txtResultados.Location.X, this.txtResultados.Location.Y + this.panelMoneda.Height);
                    this.txtResultados.Height -= this.panelMoneda.Height;
                    opcionAnterior = selectedValue;
                }
                else if (!mostrarPanelParametros && string.IsNullOrEmpty(opcionAnterior))
                {
                    this.gridResultados.Height += this.panelMoneda.Height;
                    this.txtResultados.Height += this.panelMoneda.Height;
                    opcionAnterior = selectedValue;
                }
                this.lblNombreWs.Text = dto.Nombre;
                this.lblDescripcionWs.Text = dto.Descripcion;
            }
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            this.txtResultados.Text = string.Empty;
            if (cbWebService.SelectedItem != null)
            {
                string selectedValue = (cbWebService.SelectedItem as WebServiceAfipDto).Nombre.Trim();
                if (selectedValue == GetTiposMonedasNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposMonedas(this.ticket).ResultGet;
                }
                else if (selectedValue == GetTiposCbtesNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposCbte(this.ticket).ResultGet;
                }
                else if (selectedValue == GetTiposTributosNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposTributos(this.ticket).ResultGet;
                }
                else if (selectedValue == GetTiposIvaNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposIva(this.ticket).ResultGet;
                }
                else if (selectedValue == GetTiposOpcionalNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposOpcional(this.ticket).ResultGet;
                }
                else if (selectedValue == CompTotXRequestNombre)
                {
                    this.txtResultados.Text = this.client.CompTotXRequest(this.ticket).RegXReq.ToString();
                }
                else if (selectedValue == GetTiposDocNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposDoc(this.ticket).ResultGet;
                }
                else if (selectedValue == CompUltimoAutorizadoNombre)
                {
                    if (string.IsNullOrEmpty(this.txtPtoVta.Text))
                    {
                        //MessageBox();
                        //return;
                    }

                    int ptoVtaInt = int.Parse(this.txtPtoVta.Text);
                    int cbteTipo = int.Parse(this.cbTiposCbte.SelectedValue.ToString());
                    FERecuperaLastCbteResponse result = this.client.CompUltimoAutorizado(this.ticket, ptoVtaInt, cbteTipo);
                    this.txtResultados.Text = string.Format(@"CbteNro: {0}{1}CbteTipo: {2}{3}PtoVta: {4}{5}",
                                                              result.CbteNro,
                                                              Environment.NewLine,
                                                              result.CbteTipo,
                                                              Environment.NewLine,
                                                              result.PtoVta,
                                                              Environment.NewLine);
                }
                else if (selectedValue == GetTiposConceptoNombre)
                {
                    this.gridResultados.DataSource = this.client.GetTiposConcepto(this.ticket).ResultGet;
                }
                else if (selectedValue == GetCotizacionNombre)
                {
                    string monId = this.cbMoneda.SelectedValue.ToString();
                    Cotizacion cotizacion = this.client.GetCotizacion(this.ticket, monId).ResultGet;
                    if (cotizacion != null)
                    {
                        this.txtResultados.Text = string.Format(@"Fecha: {0}{1}Cotizacion: ${2}{3}",
                                                                 DateTime.ParseExact(cotizacion.FchCotiz, "yyyyMMdd", CultureInfo.InvariantCulture).ToShortDateString(),
                                                                 Environment.NewLine,
                                                                 cotizacion.MonCotiz,
                                                                 Environment.NewLine);
                    }
                    else
                    {
                        this.txtResultados.Text = "No hay cotizacion disponible";
                    }

                }

                if (selectedValue == GetTiposMonedasNombre ||
                         selectedValue == GetTiposCbtesNombre ||
                         selectedValue == GetTiposTributosNombre ||
                         selectedValue == GetTiposIvaNombre ||
                         selectedValue == GetTiposOpcionalNombre ||
                         selectedValue == GetTiposDocNombre ||
                         selectedValue == GetTiposConceptoNombre)
                {
                    this.lblResultados.Text = string.Format("Resultados: {0} registros", this.gridResultados.RowCount);
                }
            }
        }
    }
}
