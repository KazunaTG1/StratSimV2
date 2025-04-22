<%@ Page Title="Hedge Simulator | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="callandputs.aspx.cs" Inherits="StratSimV2.WebForm2" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Hedge Simulator</h2>
    <table>
        <tr>
            <td>
                <h3>Option 1</h3>
                <fin:OptionOI runat="server" ID="oiCall" Type="Call" />
                <h3>Option 2</h3>
                <fin:OptionOI runat="server" ID="oiPut" Type="Put" Strike="88" /><br /> 
                <h3>Simulation Settings</h3>
                <fin:OptionSimOI runat="server" ID="oiOptionSim" /><br />
                <asp:Button runat="server" ID="btnTrade" Text="Trade" OnClick="btnTrade_Click"  Width="100px" /><hr /><br />
                <asp:CheckBox AutoPostBack="true" runat="server" ID="btnAutoTrade" Text="Auto Trade" 
                    OnCheckedChanged="btnAutoTrade_CheckedChanged" Font-Underline="true" Font-Bold="true"/>
                <asp:Timer ID="timerTrade" runat="server" Interval="2000" OnTick="timerTrade_Tick" Enabled="false" /> every&nbsp;
                <asp:TextBox runat="server" ID="tbTickSpeed" TextMode="Number" Width="30px" Min="1" Value="2"
                    OnTextChanged="tbTickSpeed_TextChanged"/> seconds<br /><br /><hr />
                <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" />
                Starting Balance: <asp:TextBox runat="server" ID="tbBal" TextMode="Number" Text="500" /><br /><hr />
                <asp:Label runat="server" ID="lblError" /><br />
                
                
            </td>
            <td>
                <table>
                    <tr>
                        <td >
                            <h4>Option 1</h4>
                            <asp:Chart ID="chartCall" runat="server"/><br />
                            <asp:Label runat="server" ID="lblCallRes" />
            
                        </td>
                        <td>
                            <h4>Option 2</h4>
                            <asp:Chart ID="chartPut" runat="server"/><br />
                            <asp:Label runat="server" ID="lblPutRes" />
            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h4>Stock Price</h4>
                            <asp:Chart ID="chartStock" runat="server"/><br />
                            <asp:Label runat="server" ID="lblStockResult" />
                        </td>
                        <td >
                            <h4>Equity</h4>
                            <asp:Chart ID="chartEquity" runat="server" /><br />
                            <asp:Label runat="server" ID="lblEquity" /><br />
                            <asp:Label runat="server" ID="lblAvgPL" />
                        </td>
                        <td>
                            <asp:ListBox runat="server" ID="lbPayouts"  Height="300px"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    
    
</asp:Content>
