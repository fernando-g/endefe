﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteComprobantes.aspx.cs" Inherits="FacturaElectronica.Ui.Web.Pages.ReporteComprobantes" %>

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

        $(document).ready(function () {
            $("a[rel='pop-up']").click(function () {
                var caracteristicas = "height=700,width=800,scrollTo,resizable=1,scrollbars=1,location=0";
                window.open(this.href, 'Popup', caracteristicas);
                window.open("http://www.google.com", null, "toolbars=no,menubar=no,location=no,scrollbars=yes,resizable=yes,status=yes");
                return false;
            });
        });  

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Buscar Comprobantes
    </h2>
    <div class="editionContainerFilter">
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
                Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button ID="btnLimpiar" ClientIDMode="Static" CssClass="btn" runat="server" 
                Text="Limpiar" onclick="btnLimpiar_Click" />
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
                <asp:BoundField DataField="FechaDeCarga" HeaderText="Fecha de Carga" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" DataFormatString="{0: dd/MM/yyyy HH:mm:ss}" />
                <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha de Vencimiento" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EstadoDescripcion" HeaderText="Estado" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Factura Asociada">
                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a href="<%# DataBinder.Eval(Container, "DataItem.PathArchivo")  %>" target="_blank" rel="pop-up"
                            >
                            <img src="../Images/pdf.png" width="16px" height="16px" alt="pdffile" style="border-style:none" /></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Borrar Comprobante">
                    <HeaderStyle HorizontalAlign="Center" Width="30px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/eliminar.png"
                            CommandName="eliminar" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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
</asp:Content>