using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ubatic.Ui.Web.Code;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Ui.Web.Code;
using FacturaElectronica.Common.Services;
using System.Globalization;
using System.Drawing;
using FacturaElectronica.Common.Constants;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class ComprobantesListado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.InicializarControles();
                this.Buscar();
            }
        }

        private void InicializarControles()
        {
            IComprobanteService comprobanteSvc = ServiceFactory.GetComprobanteService();
            
            // Tipos Comprobante
            List<TipoComprobanteDto> tiposComprobante = comprobanteSvc.ObtenerTiposComprobantes();
            UIHelper.LoadCbo(tiposComprobante, this.ddlTipoComprobante, Constants.ValorInicialDdl, "Id", "Descripcion");

            // Tipos Contrato
            List<TipoContratoDto> tiposContrato = comprobanteSvc.ObtenerTiposContrato();
            UIHelper.LoadCbo(tiposContrato, this.ddlTipoContrato, Constants.ValorInicialDdl, "Id", "Descripcion");

            // Mes Facturacion
            this.CargarListaMeses(this.ddlMesFacturacion, false);

            // Anio Facturacion
            List<int> aniosFacturacion = comprobanteSvc.ObtenerAniosFacturacion();
            UIHelper.LoadBasicCbo(aniosFacturacion, this.ddlAnioFacturacion, Constants.ValorInicialDdl);

        }

        public void CargarListaMeses(DropDownList ddlMes, bool mesCorriente)
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

        private void Buscar()
        {
            // cargo los filtros
            ComprobanteCriteria criteria = new ComprobanteCriteria();
            CargarCriteria(criteria);

            this.Grid.DataSource = ServiceFactory.GetComprobanteService().ObtenerComprobantes(criteria);
            this.Grid.DataBind();
        }

        private void CargarCriteria(ComprobanteCriteria criteria)
        {
            criteria.RazonSocial = txtRazonSocial.Text.Trim();
            criteria.TipoComprobanteId = UIHelper.GetIntFromInputCbo(this.ddlTipoComprobante);
            criteria.NroComprobante = UIHelper.GetLongFromInputText(this.txtNroComprobante.Text.Trim());
            criteria.FechaVencDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaVencDesde.Text);
            criteria.FechaVencHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaVencHasta.Text);
            criteria.FechaVencDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaVencDesde.Text);
            criteria.FechaVencHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaVencHasta.Text);
            criteria.MesFacturacion = UIHelper.GetIntFromInputCbo(this.ddlMesFacturacion);
            criteria.AnioFacturacion = UIHelper.GetIntFromInputCbo(this.ddlAnioFacturacion);
            criteria.TipoContratoId = UIHelper.GetIntFromInputCbo(this.ddlTipoContrato);
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "eliminar")
                {
                    long archivoAsociado = Convert.ToInt64(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);                    
                    Buscar();
                }
                else if (e.CommandName == "ver")
                {
                    VisualizacionComprobanteDto dto = new VisualizacionComprobanteDto();
                    dto.ArchivoAsociadoId = Convert.ToInt64(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);                    
                    dto.Ip = this.Request.UserHostAddress;
                    IComprobanteService svc = ServiceFactory.GetComprobanteService();
                    svc.AgregarVisualizacion(dto);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }

        protected void Grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                this.Grid.PageIndex = e.NewPageIndex;

                Buscar();
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }

        protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ComprobanteArchivoAsociadoDto dto = e.Row.DataItem as ComprobanteArchivoAsociadoDto;
                    int columnaFechaVencimiento = 5;
                    e.Row.Cells[columnaFechaVencimiento].Text = dto.FechaVencimiento.HasValue ? 
                                                                dto.FechaVencimiento.Value.ToString("dd/MM/yyyy") :
                                                                string.Format("{0} días de visualizado", dto.DiasVencimiento);
                    int columnaEstado = 6;
                    e.Row.Cells[columnaEstado].ForeColor = dto.EstadoCodigo == CodigosEstadoArchivoAsociado.Visualizado ? Color.Green : Color.Red;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }
        
        protected void btnAgregarNuevo_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/Pages/ProyectosDetalle.aspx", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtRazonSocial.Text = string.Empty;
            this.ddlTipoComprobante.SelectedIndex = 0;
            this.txtNroComprobante.Text = string.Empty;
            this.txtFechaVencDesde.Text = string.Empty;
            this.txtFechaVencHasta.Text = string.Empty;
            this.txtFechaVencDesde.Text = string.Empty;
            this.txtFechaVencHasta.Text = string.Empty;
            this.ddlMesFacturacion.SelectedIndex = 0;
            this.ddlAnioFacturacion.SelectedIndex = 0;
            this.ddlTipoContrato.SelectedIndex = 0;
        }
    }
}