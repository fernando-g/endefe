<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="FacturaElectronica.Ui.Web._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Bienvenido al Portal Web de Factura Electronica de ENDESA CEMSA S.A
    </h2>
    <p>
        Para saber mas acerca de nosotros visite
        <asp:LinkButton ID="lnkEndesa" runat="server" OnClick="lnkEndesa_Click">www.endesacemsa.com</asp:LinkButton>
    </p>
    <p>
        Puede encontrar datos de contacto sobre la empresa en la opción de Menu <a href="Pages/Contacto.aspx"
            title="Contacto">Contacto</a>.
    </p>
    <br />
    <asp:Panel ID="pnlEstadoComprobantes" runat="server" Visible="false">
        <table cellpadding="4" cellspacing="0" class="table_estados">
            <thead>
                <tr style="height: 50px">
                    <th colspan="3">
                        <h3>
                            Estado Actual de sus Comprobantes a la fecha
                            <asp:Label ID="lblFechaActual" runat="server"></asp:Label>
                        </h3>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr onclick="Nav('Pages/ComprobantesListado.aspx','?Estado=V');" style="cursor: pointer">
                    <td style="width: 50px">
                        <img src="Images/visualizado.png" alt="" />
                    </td>
                    <td style="text-align: left; width: 300px">
                        Comprobantes Visualizados
                    </td>
                    <td>
                        <asp:Label ID="lblVisualizados" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr onclick="Nav('Pages/ComprobantesListado.aspx','?Estado=NV');" style="cursor: pointer">
                    <td>
                        <img src="Images/no_visualizado.png" alt="" />
                    </td>
                    <td>
                        Comprobantes No Visualizados
                    </td>
                    <td>
                        <asp:Label ID="lblNoVisualizados" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr onclick="Nav('Pages/ComprobantesListado.aspx','?Estado=NV&Venc=1');" style="cursor: pointer">
                    <td>
                        <img src="Images/no_visualizado_vencido.png" alt="" />
                    </td>
                    <td>
                        Comprobantes No Visualizados Vencidos
                    </td>
                    <td>
                        <asp:Label ID="lblNoVisualizadosVencidos" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h3>
                            Total de comprobantes</h3>
                    </td>
                    <td>
                        <h3>
                            <asp:Label ID="lblTotalComprobantes" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
