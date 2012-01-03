using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Ui.Web.Code;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class UsuariosListado : BasePage
    {
        private const string pagDetalle = "UsuariosDetalle.aspx";
        private const string pagCambiarPass = "Account/ChangePassword.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.Buscar();
            }
        }

        private void Buscar()
        {
            // cargo los filtros
            /*int? pedidoId = UIHelper.GetIntFromInputText(this.txtPedidoId.Text);
            int? temporadaId = UIHelper.GetIntFromInputCbo(this.cboTemporada);
            int? destinoId = UIHelper.GetIntFromInputCbo(this.cboDestino);
            DateTime? fechaDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaDesde.Text);
            DateTime? fechaHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaHasta.Text);*/

            this.Grid.DataSource = ServiceFactory.GetSecurityService().ObtenerUsuarios(this.txtNombre.Text.Trim());
            this.Grid.DataBind();
        }
        
        protected void btnAgregarNuevo_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(pagDetalle, true);
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

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long usuarioId = Convert.ToInt32(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                if (e.CommandName == "editar")
                {
                    this.Response.Redirect(string.Format("{0}?Id={1}", pagDetalle, usuarioId), true);
                }
                else if (e.CommandName == "eliminar")
                {
                    ServiceFactory.GetSecurityService().EliminarUsuario(usuarioId);
                    Buscar();
                }
                else if (e.CommandName == "password")
                {
                    this.Response.Redirect(string.Format("{0}?Id={1}", pagDetalle, usuarioId), true);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }

        protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtNombre.Text = string.Empty;
        }
    }
}