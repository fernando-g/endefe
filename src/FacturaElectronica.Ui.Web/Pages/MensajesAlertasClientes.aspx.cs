using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Ui.Web.Code;
using FacturaElectronica.Common.Contracts;
using System.Drawing;
using FacturaElectronica.Common.Services;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class MensajesAlertasClientes : BasePage
    {
        private IMensajeService mensajeSvc = null;
        private const string pagListado = "MensajesAlertasListado.aspx";

        public long MensajeId 
        {
            get
            {                
                return long.Parse(ViewState["Id"].ToString());
            }
            set
            {
                ViewState["Id"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HasPermissionToSeeMe(Operaciones.MensajesAlertasClientes);
            this.mensajeSvc = ServiceFactory.GetMensajeService();
            if (!this.IsPostBack)
            {
                if (this.Request.QueryString["Id"] != null)
                {
                    MensajeDto mensaje = this.mensajeSvc.ObtenerMensaje(long.Parse(this.Request.QueryString["Id"].ToString()));
                    this.MensajeId = mensaje.Id;
                    this.lblMensaje.Text = mensaje.Asunto;
                }
                this.Buscar();
            }
        }

        private void Buscar()
        {
            // cargo los filtros
            MensajeClienteCriteria criteria = new MensajeClienteCriteria();
            CargarCriteria(criteria);

            List<MensajeClienteDto> list = this.mensajeSvc.ObtenerClientesEstados(criteria);
            this.lblCantReg.Text = string.Format(" ({0})", list.Count);
            this.Grid.DataSource = list;
            this.Grid.DataBind();
        }

        private void CargarCriteria(MensajeClienteCriteria criteria)
        {
            criteria.MensajeId = this.MensajeId;
            criteria.RazonSocial = this.txtRazonSocial.Text.Trim();
            criteria.CuitDesde = this.txtCuitDesde.Text.Trim() != string.Empty ? long.Parse(this.txtCuitDesde.Text.Trim()) : default(long?);
            criteria.CuitHasta = this.txtCuitHasta.Text.Trim() != string.Empty ? long.Parse(this.txtCuitHasta.Text.Trim()) : default(long?);
            if(this.ddlEstado.SelectedValue != "0")
            {
                criteria.Leido = this.ddlEstado.SelectedValue == "1";
            }
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MensajeClienteDto item = e.Row.DataItem as MensajeClienteDto;
                e.Row.Cells[2].ForeColor = item.Leido ? Color.Green : Color.Red;
                e.Row.Cells[2].Text = item.Leido ? "Sí" : "No";
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(pagListado, true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtRazonSocial.Text = string.Empty;
            this.txtCuitDesde.Text = string.Empty;
            this.txtCuitHasta.Text = string.Empty;
            this.ddlEstado.SelectedIndex = 0;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //  exporto la grilla
            GridViewExportUtil.Export("MensajeClientes.xls", this.Grid);
        }
    }
}