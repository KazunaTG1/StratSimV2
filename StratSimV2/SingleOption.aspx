<%@ Page Title="Option Sim | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SingleOption.aspx.cs" Inherits="StratSimV2.WebForm1" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Single Option</h2>
    <h3>Model the Performance of an Individual Option</h3>
    <table>
        <tr>
            <td>
                <h3>Option</h3>
                <fin:OptionOI runat="server" ID="oiOption"  /><br />
                <h3>Simulation Settings</h3>
                <fin:OptionSimOI runat="server" ID="oiOptionSim" />
                <br />
                <asp:Button runat="server" ID="btnTrade" Text="Trade" OnClick="btnTrade_Click" Width="200px" /><br />
                <asp:UpdateProgress runat="server" DisplayAfter="200">
                    <ProgressTemplate>
                        <div style="font-weight: bold;">
                            Loading, please wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <hr /><br />
                <asp:CheckBox AutoPostBack="true" runat="server" ID="btnAutoTrade" Text="Auto Trade" 
                    OnCheckedChanged="btnAutoTrade_CheckedChanged" Font-Underline="true" Font-Bold="true" />
                <asp:Timer ID="timerTrade" runat="server" Interval="2000" OnTick="timerTrade_Tick" Enabled="false" /> every&nbsp;
                <asp:TextBox runat="server" ID="tbTickSpeed" TextMode="Number" Width="30px" Min="1" Value="2"
                    OnTextChanged="tbTickSpeed_TextChanged"/> seconds<br /><br /><hr />
                <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                Starting Balance: <asp:TextBox runat="server" ID="tbBal" TextMode="Number" Text="500" Width="100px" /><br /><hr />
                
            </td>
            <td>
                <table>
                    <tr>
                        <td style="vertical-align:top;">
                            <h3>Option Price</h3>
                            <h4><asp:Label runat="server" ID="lblResult" /></h4>
                            <asp:Chart runat="server" ID="chartOption" SkinID="Large" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <h3>Stock Price</h3>
                            <h4><asp:Label runat="server" ID="lblStockResult" /></h4>
                            <asp:Chart runat="server" ID="chartStock" SkinID="Large" />
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align:top;">
                            <h3>Equity</h3>
                            <h4><asp:Label runat="server" ID="lblError" />
                                <asp:Label runat="server" ID="lblEquity" />
                                <asp:Label runat="server" ID="lblAvgPL" /></h4>
                            <asp:Chart ID="chartEquity" runat="server" SkinID="Large" />
                        </td>
                        <td>
                            <h3>Payouts</h3>
                            <asp:ListBox runat="server" ID="lbPayouts"  Height="300px"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    
    
</asp:Content>
