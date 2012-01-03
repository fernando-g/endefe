<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="ComprobantesListado.aspx.cs" Inherits="FacturaElectronica.Ui.Web.Pages.ComprobantesListado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Styles/GridStyle.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/DirtyCheck.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script src="/Scripts/ajaxupload.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.datepick.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.datepick-es-AR.js" type="text/javascript"></script>
    <link href="/Styles/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        $(function () {
            $('#txtFechaVencDesde').datepick({ dateFormat: 'dd/mm/yyyy' });
            $('#txtFechaVencHasta').datepick({ dateFormat: 'dd/mm/yyyy' });
            $('#btnBuscar').toggleClass('bounce');
        });

        function openWindows(pdfUrl, estado) {
            if (document.all || document.layers) {
                w = screen.availWidth;
                h = screen.availHeight;
            }
            var popW = 300, popH = 200;

            var leftPos = (w - popW) / 2, topPos = (h - popH) / 2;
            window.open(pdfUrl, 'Popup', 'width=' + w + ',height=' + h + ',top=' + 0 + ',left=' + 0);
            if(estado != "V")
                window.open("InfoVisualizacion.aspx", 'popup', 'width=' + popW + ',height=' + popH + ',top=' + topPos + ',left=' + leftPos);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Buscar Comprobantes
        <!-- <img width="15px" height="15px" id="imgExpand" class="imgExpand" src="/Images/icon_blockexpanded.png"
            onclick="window.AppCommonObj.toggleVisibility(this.id, 'resultDiv');" alt="" /> -->
        <span class="clear"></span>

    </h2>
    <div class="editionContainerFilter" id="resultDiv">
        <div class="clear">
        </div>
        <asp:ValidationSummary ID="valSumm" runat="server" CssClass="failureNotification"
            ShowSummary="true" HeaderText="Se han encontrado los siguientes errores:" DisplayMode="BulletList" />
        <br />        
        <p>
            <span class="title2">Razon Social:</span>
            <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="inputs" Width="470px" MaxLength="200"></asp:TextBox>
        </p>
        <div class="clear">
        </div>
        <p>
            <span class="title2">Tipo Comprobante:</span>
            <asp:DropDownList ID="ddlTipoComprobante" runat="server" CssClass="cbo"></asp:DropDownList>
        </p>
        <p>
            <span class="title2 secondColumn">Nro. Comprobante:</span>
            <asp:TextBox ID="txtNroComprobante" runat="server" CssClass="inputs"></asp:TextBox>
        </p>
        <div class="clear">
        </div>
        <p>
            <span class="title2">Fecha de Venc. Desde:</span>
            <asp:TextBox ID="txtFechaVencDesde" runat="server" ClientIDMode="Static" CssClass="inputs"></asp:TextBox>
            <asp:CompareValidator ID="cvFechaDesde" runat="server" Text="*" Display="Static" CssClass="failureNotification" ControlToValidate="txtFechaVencDesde" ControlToCompare="txtFechaVencHasta" Operator="LessThanEqual" Type ="Date" 
                ErrorMessage="Fecha Venc. Desde debe ser menor o igual a Fecha Venc. Hasta"></asp:CompareValidator>

        </p>
        <p>
            <span class="title2 secondColumn">Hasta:</span>
            <asp:TextBox ID="txtFechaVencHasta" runat="server" ClientIDMode="Static" CssClass="inputs"></asp:TextBox>
        </p>
        <div class="clear">
        </div>
        <p>
            <span class="title2">Per&iacute;odo de Facturación:</span>
        </p>
        <div class="clear">
        </div>
        <p>
            <span class="title2">Mes</span>
            <asp:DropDownList ID="ddlMesFacturacion" runat="server" CssClass="cbo"></asp:DropDownList>        
        </p>
        <p>
            <span class="title2 secondColumn">A&ntilde;o</span>
            <asp:DropDownList ID="ddlAnioFacturacion" runat="server" CssClass="cbo"></asp:DropDownList>
        </p>
        <div class="clear">
        </div>
        <p>
            <span class="title2">Tipo Contrato:</span>
            <asp:DropDownList ID="ddlTipoContrato" runat="server" CssClass="cbo"></asp:DropDownList>
        </p>
        <div class="clear">
        </div>
        <p>
            <asp:Button ID="btnBuscar" ClientIDMode="Static" CssClass="btn" runat="server" 
                Text="Buscar" onclick="btnBuscar_Click"  />
            <asp:Button ID="btnLimpiar" ClientIDMode="Static" CssClass="btn" runat="server" 
                Text="Limpiar" onclick="btnLimpiar_Click" CausesValidation="false" />
        </p>
        <div class="clear">
        </div>
    </div>
    <h2>
        Listado de Comprobantes
    </h2>
    <asp:Panel ID="pnlResults" CssClass="editionContainerForGrid" runat="server">
        <asp:GridView ID="Grid" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" AutoGenerateColumns="False" DataKeyNames="ArchivoAsociadoId" Width="100%" AllowPaging="True"
            OnPageIndexChanging="Grid_PageIndexChanging" OnRowCommand="Grid_RowCommand"
            OnRowDataBound="Grid_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ArchivoAsociadoId" HeaderText="Id" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="ComprobanteId" Visible="false" />
                <asp:BoundField DataField="TipoComprobanteDescripcion" HeaderText="Tipo de Comprobante" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="NroComprobante" HeaderText="Nro. Comprobante" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha de Vencimiento" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EstadoDescripcion" HeaderText="Estado" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Factura Asociada">
                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="imgPdf" ImageUrl="~/Images/pdf.png" Width="16px" Height="16px" 
                        OnClientClick='<%# "openWindows(\"" + Eval("PathArchivo") + "\",\"" + Eval("EstadoCodigo") + "\");" %>' runat="server" CommandName="ver" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#4b6c9e" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
            <PagerStyle BackColor="#4b6c9e" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </asp:Panel>
    <asp:Button ID="btnExportToExcel" Text="Exportar a Excel" runat="server" 
        onclick="btnExportToExcel_Click" />
</asp:Content>
