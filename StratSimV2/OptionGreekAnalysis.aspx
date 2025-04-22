<%@ Page Title="Option Greeks | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="OptionGreekAnalysis.aspx.cs" Inherits="StratSimV2.WebForm5" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Option Greek Analysis</h2>
    <table>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlGreeks" AutoPostBack="true" OnSelectedIndexChanged="ddlGreeks_SelectedIndexChanged" /><br />
                <h3>Simulation Settings</h3>
                <fin:OptionOI runat="server" ID="oiOption" UseMarketPrice="false"/>
                
            </td>
            <td>
                <h3><asp:Label runat="server" ID="lblTitle" Text="Delta" /></h3>
                <asp:Chart runat="server" ID="chartGreek" SkinID="Numerical" Width="1000" Height="500" /><br />
                <asp:Label runat="server" ID="lblResult" />
            </td>
        </tr>
    </table>
</asp:Content>
