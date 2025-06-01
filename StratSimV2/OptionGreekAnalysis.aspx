<%@ Page Title="Option Greeks | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="OptionGreekAnalysis.aspx.cs" Inherits="StratSimV2.WebForm5" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Option Greek Analysis</h1>
    <h2>Visualize the Change in Option Price Based On Market Variables</h2>
    <table>
        <tr>
            <td>
                <asp:DropDownList runat="server" ID="ddlGreeks" AutoPostBack="true" OnSelectedIndexChanged="ddlGreeks_SelectedIndexChanged" /><br />
                <h3>Simulation Settings</h3>
                <fin:OptionOI runat="server" ID="oiOption" UseMarketPrice="false"/>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <h2><asp:Label runat="server" ID="lblTitle" Text="Delta" /></h2>
                            <p><asp:Label runat="server" ID="lblDescription" /></p>
                            <hr />
                            <h3>Change ($)</h3>
                            <h4><asp:Label runat="server" ID="lblResult" /></h4>
                            
                            Scale: &nbsp;<asp:TextBox runat="server" ID="tbScale" TextMode="Number" Value="7" Step="1" Min="1"
                                Width="50px" AutoPostBack="true" OnTextChanged="Page_Load" /><br />
                            <asp:Chart runat="server" ID="chartGreek" SkinID="Numerical" Width="1400" Height="600" /><br />
                            <hr />
                        </td>
        
                    </tr>
                    <tr>
                        <td>
                            <h3>Rate of Change (%)</h3>
                            <h4><asp:Label runat="server" ID="lblRateResult" /></h4>
                            <asp:Chart runat="server" ID="chartGreekRate" SkinID="Numerical" Width="1400" Height="600" /><br />
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
