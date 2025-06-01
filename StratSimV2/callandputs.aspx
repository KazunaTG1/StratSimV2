<%@ Page Title="Hedge Simulator | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="callandputs.aspx.cs" Inherits="StratSimV2.WebForm2" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Hedge Simulator</h2>
    <h3>Test Multi-Leg Strategies</h3>
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
                <asp:Button runat="server" ID="btnReset" Text="Reset" OnClick="btnReset_Click" /><br />
                <asp:UpdateProgress runat="server" DisplayAfter="200">
                    <ProgressTemplate>
                        <div style="font-weight: bold;">
                            Loading, please wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress><br /><br />
                Starting Balance: <asp:TextBox runat="server" ID="tbBal" TextMode="Number" Text="500" /><br /><hr />
                <asp:Label runat="server" ID="lblError" /><br />
                
                
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <h3>P/L</h3>
                            <h4><asp:Label runat="server" ID="lblPL" /></h4>
                            <asp:Chart ID="chartPL" runat="server" SkinID="Large"/>
                        </td>
                        
                    </tr>
                    <tr>
                        
                        <td>
                            <h3>Equity</h3>
                            <h4>
                                <asp:Label runat="server" ID="lblEquity" />
                                <asp:Label runat="server" ID="lblAvgPL" />

                            </h4>
                            <asp:Chart ID="chartEquity" runat="server" SkinID="Large" />
                        </td>
                        <td>
                            <h3>Payouts</h3>
                            <asp:ListBox runat="server" ID="lbPayouts"  Height="300px"/>
                        </td>
                    </tr>
                    <tr>
                        <td >
                            <h3>Option 1</h3>
                            <h4><asp:Label runat="server" ID="lblCallRes" /></h4>
                            <asp:Chart ID="chartCall" runat="server" SkinID="Large"/>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Option 2</h3>
                            <h4><asp:Label runat="server" ID="lblPutRes" /></h4>
                            <asp:Chart ID="chartPut" runat="server" SkinID="Large"/><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Stock Price</h3>
                            <h4><asp:Label runat="server" ID="lblStockResult" /></h4>
                            <asp:Chart ID="chartStock" runat="server" SkinID="Large"/><br />
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
    
    
</asp:Content>
