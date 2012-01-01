<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClienteListado.aspx.cs" Inherits="FacturaElectronica.Ui.Web.Pages.ClienteListado" %>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Buscar Clientes
    </h2>
    <div class="editionContainerFilter">
        <p>
            <span class="title2">Razon Social:</span>
            <asp:TextBox ID="txtRazonSocial" runat="server" ClientIDMode="Static" CssClass="inputs"></asp:TextBox>
        </p>
        <p>
            <span class="title2 secondColumn">CUIT:</span>
            <asp:TextBox ID="txtCuit" runat="server" ClientIDMode="Static" CssClass="inputs"></asp:TextBox>
        </p>
        <div class="clear">
        </div>
        <p>
            <span class="title2">Nombre Contacto:</span>
            <asp:TextBox ID="txtNombreContacto" runat="server" ClientIDMode="Static" CssClass="inputs"></asp:TextBox>
        </p>
        <p>
            <span class="title2 secondColumn">Apellido Contacto:</span>
            <asp:TextBox ID="txtApellidoContacto" runat="server" ClientIDMode="Static" CssClass="inputs"></asp:TextBox>
        </p>
        <div class="clear">
        </div>
        <p>
            <asp:Button ID="btnBuscar" ClientIDMode="Static" CssClass="btn" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
            <asp:Button ID="btnLimpiar" ClientIDMode="Static" CssClass="btn" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
        </p>
        <div class="clear">
        </div>
    </div>
    <h2>
        Listado de Clientes
    </h2>
    <asp:Panel ID="pnlResults" CssClass="editionContainerForGrid" runat="server">
        <asp:GridView ID="Grid" runat="server" CellPadding="4" ForeColor="#333333"
            GridLines="None" AutoGenerateColumns="False" DataKeyNames="Id" 
            Width="100%" AllowPaging="True"
            OnPageIndexChanging="Grid_PageIndexChanging" 
            OnRowCommand="Grid_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                <asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="CUIT" HeaderText="CUIT" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="NombreContacto" HeaderText="Nombre Contacto" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="ApellidoContacto" HeaderText="Apellido Contacto" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Editar">
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/Images/editar.png" CommandName="editar"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eliminar">
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/eliminar.png"
                            CommandName="eliminar" OnClientClick="return confirm('Esta seguro que desea eliminar el registro?');"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
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
            <EmptyDataTemplate>
                <p>
                    No existen clientes</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <br />
    <asp:Button ID="btnAgregarNuevo" CssClass="btn" runat="server" Text="Agregar" ClientIDMode="Static"
        OnClick="btnAgregarNuevo_Click" />
</asp:Content>
