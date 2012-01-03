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
    public partial class ComprobantesListado : ComprobantesBase
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
        protected override TextBox txtRazonSocialControl
        {
            get { return this.txtRazonSocial; }
        }
        protected override TextBox txtNroComprobanteControl
        {
            get { return this.txtNroComprobante; }
        }
        protected override  TextBox txtFechaVencDesdeControl
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

        #endregion [BaseControls]

        protected void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load();
        }

        protected override List<ComprobanteArchivoAsociadoDto> ObtenerComprobantes(ComprobanteCriteria criteria)
        {
            return ServiceFactory.GetComprobanteService().ObtenerComprobantesPorCliente(criteria);
        }

        protected void Grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ver")
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
                    int columnaFechaVencimiento = 4;
                    int columnaEstado = 5;                    
                    EstablecerFechaVencimiento(e, dto, columnaFechaVencimiento);                    
                    EstablecerColorEstado(e, dto, columnaEstado);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.HandleException(ex);
            }
        }
        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Buscar();
        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarControles();
        }
    }
}