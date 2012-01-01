﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="UsuariosListado.aspx.cs" Inherits="FacturaElectronica.Ui.Web.Pages.UsuariosListado" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <script src="/Scripts/site.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Buscar Usuarios
    </h2>
    <div class="editionContainerFilter">
        <p>
            <span class="title2">Nombre:</span>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="inputs"></asp:TextBox>
        </p>
        <div class="clear">
        </div>
        <p>
            <asp:Button ID="btnBuscar" ClientIDMode="Static" CssClass="btn" runat="server" Text="Buscar"
                OnClick="btnBuscar_Click" />
            <asp:Button ID="btnLimpiar" ClientIDMode="Static" CssClass="btn" runat="server" Text="Limpiar"
                OnClick="btnLimpiar_Click" />
        </p>
        <div class="clear">
        </div>
    </div>
    <h2>
        Listado de Usuarios
    </h2>
    <asp:Panel ID="pnlResults" CssClass="editionContainerForGrid" runat="server">
        <asp:GridView ID="Grid" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
            AutoGenerateColumns="False" DataKeyNames="Id" Width="100%" AllowPaging="True"
            OnPageIndexChanging="Grid_PageIndexChanging" OnRowCommand="Grid_RowCommand" OnRowDataBound="Grid_RowDataBound">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-HorizontalAlign="Center"
                    HeaderStyle-Width="30px" Visible="false" />
                <asp:BoundField DataField="NombreUsuario" HeaderText="Nombre de Usuario" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="Editar">
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/Images/editar.png" CommandName="editar"
                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cambiar Password">
                    <HeaderStyle HorizontalAlign="Center" Width="40px" />
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:ImageButton ID="btnPassword" runat="server" ImageUrl="~/Images/key.gif" CommandName="password"
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
                    No existen usuarios</p>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <br />
    <asp:Button ID="btnAgregarNuevo" CssClass="btn" runat="server" Text="Agregar" ClientIDMode="Static"
        OnClick="btnAgregarNuevo_Click" />
</asp:Content>
