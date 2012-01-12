using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FacturaElectronica.Ui.Web.Code;
using FacturaElectronica.Common.Contracts;
using FacturaElectronica.Common.Services;
using Ubatic.Ui.Web.Code;
using FacturaElectronica.Common.Constants;
using System.Drawing;

namespace FacturaElectronica.Ui.Web.Pages
{
    public partial class ReporteComprobantes : ComprobantesBase
    {
        #region [BaseControls]

        protected override DropDownList ddlTipoComprobanteControl
        {
            get { return this.ddlTipoComprobante; }
        }
        protected override DropDownList ddlTipoContratoControl
        {
            get { return this.ddlTipoContrato; }
        }
        protected override DropDownList ddlMesFacturacionControl
        {
            get { return this.ddlMesFacturacion; }
        }
        protected override DropDownList ddlAnioFacturacionControl
        {
            get { return this.ddlAnioFacturacion; }
        }
        //protected override TextBox txtRazonSocialControl
        //{
        //    get { return this.txtRazonSocial; }
        //}
        protected override TextBox txtNroComprobanteControl
        {
            get { return this.txtNroComprobante; }
        }
        protected override TextBox txtFechaVencDesdeControl
        {
            get { return this.txtFechaVencDesde; }
        }
        protected override TextBox txtFechaVencHastaControl
        {
            get { return this.txtFechaVencHasta; }
        }
        protected override GridView GridControl
        {
            get { return this.Grid; }
        }
        protected override Label lblCantRegControl
        {
            get { return this.lblCantReg; }
        }

        #endregion [BaseControls]

        protected void Page_Load(object sender, EventArgs e)
        {
            HasPermissionToSeeMe(Operaciones.ReporteComprobantes);
            base.Page_Load();
        }

        protected override void CargarCriteria(ComprobanteCriteria criteria)
        {
            base.CargarCriteria(criteria);

            criteria.RazonSocial = this.txtRazonSocial.Text.Trim();
            criteria.FechaDeCargaDesde = UIHelper.GetDateTimeFromInputText(this.txtFechaDeCargaDesde.Text);
            criteria.FechaDeCargaHasta = UIHelper.GetDateTimeFromInputText(this.txtFechaDeCargaHasta.Text);
            criteria.DocumentosVencidos = this.chkDocumentosVencidos.Checked;
            criteria.MontoTotalDesde = UIHelper.GetDecimalFromInputText(this.txtMontoTotalDesde.Text.Trim());
            criteria.MontoTotalHasta = UIHelper.GetDecimalFromInputText(this.txtMontoTotalHasta.Text.Trim());
        }

        protected override List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria)
        {
            return ServiceFactory.GetComprobanteService().ObtenerComprobantes(criteria);
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                long archivoAsociadoId = Convert.ToInt64(this.Grid.DataKeys[Convert.ToInt32(e.CommandArgument)].Value);
                if (e.CommandName == "ver")
                {
                    this.Response.Redirect(string.Format("~/Handlers/PdfHandler.ashx?file={0}", archivoAsociadoId));
                }
                else if (e.CommandName == "eliminar")
                {
                    IComprobanteService svc = ServiceFactory.GetComprobanteService();
                    svc.CambiarEstado(archivoAsociadoId, CodigosEstadoArchivoAsociado.Eliminado);
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

        protected void Grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ComprobanteArchivoAsociadoDto dto = e.Row.DataItem as ComprobanteArchivoAsociadoDto;
                    int columnaFechaVencimiento = 4;
                    int columnaFechaVisualizacion = 5;
                    int columnaEstado = 7;
                    int columnaBorrarComprobante = 10;
                    EstablecerFechaVencimiento(e, dto, columnaFechaVencimiento);
                    EstablecerColorEstado(e, dto, columnaEstado);
                    EstablecerFechaVisualizacion(e, dto, columnaFechaVisualizacion);
                    EstablecerBorrarComprobante(e, dto, columnaBorrarComprobante, columnaEstado);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }

        private void EstablecerFechaVisualizacion(GridViewRowEventArgs e, ComprobanteArchivoAsociadoDto dto, int columnaFechaVisualizacion)
        {
            TableCell cell = e.Row.Cells[columnaFechaVisualizacion];
            if (dto.FechaVisualizacion.HasValue)
            {
                cell.Text = dto.FechaVisualizacion.Value.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
            else
            {
                if (dto.EstadoCodigo == CodigosEstadoArchivoAsociado.NoVisualizado)
                {
                    cell.Text = dto.EstadoDescripcion;
                    cell.ForeColor = Color.Red;
                }
            }                        
        }

        private void EstablecerBorrarComprobante(GridViewRowEventArgs e, ComprobanteArchivoAsociadoDto dto, int columnaBorrarComprobante, int columnaEstado)
        {
            if (dto.EstadoCodigo == CodigosEstadoArchivoAsociado.Eliminado || 
                dto.EstadoCodigo == CodigosEstadoArchivoAsociado.Visualizado)
            {
                e.Row.Cells[columnaBorrarComprobante].Text = "--";
                e.Row.Cells[columnaBorrarComprobante].Controls.Clear();
                if (dto.EstadoCodigo == CodigosEstadoArchivoAsociado.Eliminado)
                {
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        int cellIndex = e.Row.Cells.GetCellIndex(cell);
                        if (cell.Visible && 
                            cell.Controls.Count == 0  && 
                            cellIndex != columnaEstado &&
                            cellIndex != columnaBorrarComprobante)
                        {
                            cell.CssClass = "lineThrough";
                        }
                    }
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            base.LimpiarControles();

            this.txtRazonSocial.Text = string.Empty;
            this.txtFechaDeCargaDesde.Text = string.Empty;
            this.txtFechaDeCargaHasta.Text = string.Empty;
            this.chkDocumentosVencidos.Checked = false;
            this.txtMontoTotalDesde.Text = string.Empty;
            this.txtMontoTotalHasta.Text = string.Empty;
        }

        protected void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //  exporto la grilla
            GridViewExportUtil.Export("Comprobantes.xls", this.Grid);
        }
    }
}