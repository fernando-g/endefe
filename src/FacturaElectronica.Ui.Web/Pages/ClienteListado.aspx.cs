using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Ui.Web.Code;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class ClienteListado : BasePage
    {
        private const string pagDetalle = "ClienteDetalle.aspx";

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
            ClienteCriteria criteria = new ClienteCriteria();
            criteria.RazonSocial = txtRazonSocial.Text.Trim();
            criteria.CUIT = txtCuit.Text.Trim() != string.Empty ? long.Parse(txtCuit.Text.Trim()) : default(long?);

            this.Grid.DataSource = ServiceFactory.GetClienteService().ObtenerClientes(criteria);
            this.Grid.DataBind();
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long clienteId = Convert.ToInt64(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                if (e.CommandName == "editar")
                {
                    this.Response.Redirect(string.Format("{0}?Id={1}", pagDetalle, clienteId), true);
                }
                else if (e.CommandName == "eliminar")
                { 
                    ServiceFactory.GetClienteService().EliminarCliente(clienteId);
                    Buscar();
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

        protected void btnAgregarNuevo_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(pagDetalle , true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)        
        {
            this.txtRazonSocial.Text = string.Empty;
            this.txtCuit.Text = string.Empty;
            this.txtApellidoContacto.Text = string.Empty;
            this.txtNombreContacto.Text = string.Empty;
        }
    }
}