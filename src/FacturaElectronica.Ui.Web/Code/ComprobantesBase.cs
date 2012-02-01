using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FacturaElectronica.Common.Services;
using FacturaElectronica.Common.Contracts;
using Ubatic.Ui.Web.Code;
using System.Web.UI.WebControls;
using FacturaElectronica.Common.Constants;
using System.Globalization;
using System.Drawing;

namespace FacturaElectronica.Ui.Web.Code
{
    public abstract class ComprobantesBase : BasePage
    {
        #region [BaseControls]

        protected abstract DropDownList ddlTipoComprobanteControl
        {
            get;
        }
        protected abstract DropDownList ddlTipoContratoControl
        {
            get;
        }
        protected abstract DropDownList ddlMesFacturacionControl
        {
            get;
        }
        protected abstract DropDownList ddlAnioFacturacionControl
        {
            get;
        }

        //protected abstract TextBox txtRazonSocialControl
        //{
        //    get;
        //}

        protected abstract TextBox txtNroComprobanteControl
        {
            get;
        }

        protected abstract TextBox txtFechaVencDesdeControl
        {
            get;
        }
        protected abstract TextBox txtFechaVencHastaControl
        {
            get;
        }

        protected abstract GridView GridControl
        {
            get;
        }

        protected abstract Label lblCantRegControl
        {
            get;
        }

        #endregion [BaseControls]

        protected virtual void Page_Load()
        {
            if (!Page.IsPostBack)
            {
                this.InicializarControles();
                this.Buscar();
            }
        }

        protected virtual void CargarCriteria(ComprobanteCriteria criteria)
        {
            //criteria.RazonSocial = txtRazonSocialControl.Text.Trim();
            criteria.TipoComprobanteId = UIHelper.GetIntFromInputCbo(this.ddlTipoComprobanteControl);
            criteria.NroComprobante = UIHelper.GetLongFromInputText(this.txtNroComprobanteControl.Text.Trim());
            criteria.FechaVencDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaVencDesdeControl.Text);
            criteria.FechaVencHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaVencHastaControl.Text);
            if (criteria.FechaDeCargaHasta.HasValue)
            {
                criteria.FechaDeCargaHasta = criteria.FechaDeCargaHasta.Value.AddDays(1).AddSeconds(-1);
            }
            criteria.MesFacturacion = UIHelper.GetIntFromInputCbo(this.ddlMesFacturacionControl);
            criteria.AnioFacturacion = UIHelper.GetIntFromInputCbo(this.ddlAnioFacturacionControl);
            criteria.TipoContratoId = UIHelper.GetIntFromInputCbo(this.ddlTipoContratoControl);
        }

        //protected ComprobanteCriteria criteria
        //{
        //    get
        //    {
        //        return (ComprobanteCriteria)ViewState["criteria"];
        //    }
        //    set
        //    {
        //        ViewState["criteria"] = value;
        //    }
        //}

        protected virtual void Buscar()
        {
            // cargo los filtros
            ComprobanteCriteria criteria = new ComprobanteCriteria();
            CargarCriteria(criteria);
            List<ComprobanteArchivoAsociadoDto> list = this.ObtenerComprobantes(criteria);
            this.lblCantRegControl.Text = string.Format(" ({0})", list.Count);
            this.GridControl.DataSource = list;
            this.GridControl.DataBind();
            
        }

        protected virtual void LimpiarControles()
        {
            //txtRazonSocialControl.Text = string.Empty;
            this.ddlTipoComprobanteControl.SelectedIndex = 0;
            this.txtNroComprobanteControl.Text = string.Empty;
            this.txtFechaVencDesdeControl.Text = string.Empty;
            this.txtFechaVencHastaControl.Text = string.Empty;
            this.txtFechaVencDesdeControl.Text = string.Empty;
            this.txtFechaVencHastaControl.Text = string.Empty;
            this.ddlMesFacturacionControl.SelectedIndex = 0;
            this.ddlAnioFacturacionControl.SelectedIndex = 0;
            this.ddlTipoContratoControl.SelectedIndex = 0;
        }

        protected virtual void InicializarControles()
        {
            IComprobanteService comprobanteSvc = ServiceFactory.GetComprobanteService();

            // Tipos Comprobante
            List<TipoComprobanteDto> tiposComprobante = comprobanteSvc.ObtenerTiposComprobantes();
            UIHelper.LoadCbo(tiposComprobante, this.ddlTipoComprobanteControl, Constants.ValorInicialDdl, "Id", "Descripcion");

            // Tipos Contrato
            List<TipoContratoDto> tiposContrato = comprobanteSvc.ObtenerTiposContrato();
            UIHelper.LoadCbo(tiposContrato, this.ddlTipoContratoControl, Constants.ValorInicialDdl, "Id", "Descripcion");

            // Mes Facturacion
            this.CargarListaMeses(this.ddlMesFacturacionControl, false);

            // Anio Facturacion
            List<int> aniosFacturacion = comprobanteSvc.ObtenerAniosFacturacion();
            UIHelper.LoadBasicCbo(aniosFacturacion, this.ddlAnioFacturacionControl, Constants.ValorInicialDdl);
        }

        protected virtual void EstablecerFechaVencimiento(GridViewRowEventArgs e, ComprobanteArchivoAsociadoDto dto, int columnaFechaVencimiento)
        {
            e.Row.Cells[columnaFechaVencimiento].Text = dto.FechaVencimiento.HasValue ?
                                                        dto.FechaVencimiento.Value.ToString("dd/MM/yyyy") :
                                                        string.Format("{0} días de visualizado", dto.DiasVencimiento);
        }

        protected virtual void EstablecerColorEstado(GridViewRowEventArgs e, ComprobanteArchivoAsociadoDto dto, int columnaEstado)
        {
            e.Row.Cells[columnaEstado].ForeColor = dto.EstadoCodigo == CodigosEstadoArchivoAsociado.Visualizado ? Color.Green : Color.Red;
        }

        private void CargarListaMeses(DropDownList ddlMes, bool mesCorriente)
        {
            ddlMes.Items.Add(new ListItem(Constants.ValorInicialDdl, "-1"));

            DateTime mes = Convert.ToDateTime("1/1/2000");
            CultureInfo ci = new CultureInfo("es-AR");
            for (int i = 0; i < 12; i++)
            {

                DateTime proximoMes = mes.AddMonths(i);
                ListItem list = new ListItem();
                list.Text = ci.TextInfo.ToTitleCase(proximoMes.ToString("MMMM", ci));
                list.Value = proximoMes.Month.ToString();
                ddlMes.Items.Add(list);
            }

            if (mesCorriente == true)
            {
                ddlMes.Items.FindByValue(DateTime.Now.Month.ToString()).Selected = true;
            }
        }

        protected abstract List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria);
    }
}