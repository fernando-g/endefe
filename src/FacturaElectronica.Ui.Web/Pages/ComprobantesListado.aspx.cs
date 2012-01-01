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
            List<TipoComprobanteDto> tiposComprobante = comprobanteSvc.ObtenerTiposComprobantes();
            UIHelper.LoadCbo(tiposComprobante, this.ddlTipoComprobante, "-Seleccionar-", "Id", "Descripcion");
        }


        private void Buscar()
        {
            // cargo los filtros
            /*int? pedidoId = UIHelper.GetIntFromInputText(this.txtPedidoId.Text);
            int? temporadaId = UIHelper.GetIntFromInputCbo(this.cboTemporada);
            int? destinoId = UIHelper.GetIntFromInputCbo(this.cboDestino);*/
            ComprobanteCriteria criteria = new ComprobanteCriteria();
            criteria.FechaVencDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaVencDesde.Text);
            criteria.FechaVencHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaVencHasta.Text);

            this.Grid.DataSource = ServiceFactory.GetComprobanteService().ObtenerComprobantes(criteria);
            this.Grid.DataBind();
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "editar")
                {
                    int proyectoId = Convert.ToInt32(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                    this.Response.Redirect(string.Format("/Pages/ProyectosDetalle.aspx?Id={0}", proyectoId), true);
                }
                else if (e.CommandName == "eliminar")
                {
                    int proyectoId = Convert.ToInt32(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                    //AdminProyectoService.EliminarProyecto(proyectoId);
                    //Buscar();
                }
            }
            catch (Exception ex)
            {
                //ExceptionManager.Instance.HandleException(ex);
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
                    e.Row.Cells[0].Attributes.Add("title", "Editar");
                    e.Row.Cells[1].Attributes.Add("title", "Eliminar");
                    ImageButton btnEliminarCtrl = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnEliminarCtrl.OnClientClick = "return confirm('Está seguro que desea eliminar el siguiente registro?')";
                }
            }
            catch (Exception ex)
            {
                //ExceptionManager.Instance.HandleException(ex);
            }
        }
        
        protected void btnAgregarNuevo_Click(object sender, EventArgs e)
        {
            this.Response.Redirect("/Pages/ProyectosDetalle.aspx", true);
        }
    }
}