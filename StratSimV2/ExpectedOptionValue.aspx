<%@ Page Title="Expected Option Return | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ExpectedOptionValue.aspx.cs" Inherits="StratSimV2.WebForm3" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Expected Return of a Single Option</h2>
    <table>
        <tr>
            <td>
                <h3>Option</h3>
                <fin:OptionOI runat="server" ID="oiOption" /><br />
                <h3>Simulation Settings</h3>
                <fin:OptionSimOI runat="server" ID="oiOptionSim" /><br />
                <table>
                    <tr>
                        <td>Trades</td>
                        <td><asp:TextBox runat="server" ID="tbTrades" TextMode="Number" Text="100" /></td>
                    </tr>
                    <tr>
                        <td>Starting Balance</td>
                        <td><asp:TextBox runat="server" ID="tbSBal" TextMode="Number" Text="1000" /></td>
                    </tr>
                </table>
                <br /><br />
                <asp:Button runat="server" ID="btnSimulate" Text="Simulate" OnClick="btnSimulate_Click" />
                <br />
                <asp:Label runat="server" ID="lblError" />
            </td>
            <td>
                <table>
                    <tr>
                        <td style="vertical-align:top;">
                            <h4>Option Price</h4>
                            
                            <asp:Chart ID="chartOption" runat="server" SkinID="Large"/><br />
                            <asp:CheckBox runat="server" ID="cbLog" Checked="false" 
                                Text="Use Logarithmic Scale" OnCheckedChanged="cbLog_CheckedChanged" /><br /><hr />
                            <asp:Label runat="server" ID="lblResult" />
            
                        </td>
                        <td style="vertical-align:top;">
                            <h4>Stock Price</h4>
                            <asp:Chart ID="chartStock" runat="server" SkinID="Large"/><br />
                            <asp:Label runat="server" ID="lblStockResult" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="vertical-align:top;">
                            <h4>Equity</h4>
                            <asp:Chart ID="chartEquity" runat="server" SkinID="Large" /><br />
                            <asp:Label runat="server" ID="lblEquity" />
                        </td>
                    </tr>
                </table>
                
            </td>
        </tr>
    </table>
</asp:Content>
