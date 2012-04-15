using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Ui.Web.Code;
using Ubatic.Ui.Web.Code;
using FacturaElectronica.Common.Contracts;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class MensajesAlertasListado : BasePage
    {
        private const string pagDetalle = "MensajesAlertasDetalle.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            HasPermissionToSeeMe(Operaciones.MensajesAlertas);
            this.btnBuscar.Attributes.Add("aspnetid", this.btnBuscar.ClientID);
            if (!this.IsPostBack)
            {
                if (this.Master.EsCliente)
                {
                    this.Grid.Columns[5].Visible = false;
                    this.btnAgregarNuevo.Visible = false;
                }
                else if (this.Master.EsAdministrador)
                {
                    this.Grid.Columns[3].Visible = false;
                }
                this.Buscar();
            }
        }

        private void Buscar()
        {
            // cargo los filtros
            MensajeCriteria criteria = new MensajeCriteria();
            CargarCriteria(criteria);

            List<MensajeDto> list = ServiceFactory.GetMensajeService().ObtenerMensajes(criteria);
            this.lblCantReg.Text = string.Format(" ({0})", list.Count);
            this.Grid.DataSource = list;
            this.Grid.DataBind();
        }

        private void CargarCriteria(MensajeCriteria criteria)
        {
            criteria.ClienteId = this.Master.EsCliente ? this.Master.ClienteId : default(long?);
            criteria.FechaDeCargaDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaDeCargaDesde.Text);
            criteria.FechaDeCargaHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaDeCargaHasta.Text);
            if (criteria.FechaDeCargaHasta.HasValue)
            {
                criteria.FechaDeCargaHasta = criteria.FechaDeCargaHasta.Value.AddDays(1).AddSeconds(-1);
            }
            criteria.Asunto = this.txtAsunto.Text.Trim();
            //criteria.NombreDeArchivo = this.txtNombreArchivo.Text.Trim();
            if(!this.IsPostBack && this.BaseMaster.EsCliente && this.Request.Params["Id"] != null && this.Request.Params["Id"] == "NL")
            {
                criteria.Leido = false;
            }
            
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ver" || e.CommandName == "eliminar")
            {
                try
                {
                    long clienteId = Convert.ToInt64(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                    if (e.CommandName == "ver")
                    {
                        this.Response.Redirect(string.Format("{0}?Id={1}", pagDetalle, clienteId), true);
                    }
                    else if (e.CommandName == "eliminar")
                    {

                        if (!ServiceFactory.GetMensajeService().EliminarMensaje(clienteId))
                        {
                            throw new Exception("El mensaje no se puede eliminar dado que ha sido leido por al menos un cliente");
                        }
                        Buscar();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.Instance.HandleException(ex);
                }
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
            this.Response.Redirect(pagDetalle, true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            this.txtFechaDeCargaDesde.Text = string.Empty;
            this.txtFechaDeCargaHasta.Text = string.Empty;
            this.txtAsunto.Text = string.Empty;
            //this.txtNombreArchivo.Text = string.Empty;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //  exporto la grilla
            GridViewExportUtil.Export("MensajesAlertas.xls", this.Grid);
        }

        protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MensajeDto dataItem = e.Row.DataItem as MensajeDto;
                // Mensaje
                e.Row.Cells[2].Text = dataItem.Texto.Length > 11 ? dataItem.Texto.Substring(0, 10) + "..." : dataItem.Texto;

                if (this.Master.EsCliente)
                {
                    string leido = string.Empty;
                    if (dataItem.Leido)
                    {
                        leido = "Si";
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.Green;
                    }
                    else 
                    {
                        leido = "No";
                        e.Row.Cells[3].ForeColor = System.Drawing.Color.Red;
                    }

                    e.Row.Cells[3].Text = leido;
                }
                
            }
        }
    }
}