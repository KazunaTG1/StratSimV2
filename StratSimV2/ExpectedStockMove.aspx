<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ExpectedStockMove.aspx.cs" Inherits="StratSimV2.WebForm7" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Expected Move of a Stock</h2>
    <h3>Forecast Future Price Ranges with Asymmetric Volatility</h3>
    <table>
        <tr>
            <td>
                <h3>Stock</h3>
                <table>
                    <tr>
                        <td>Stock Price ($)</td>
                        <td><asp:TextBox runat="server" ID="tbStock" TextMode="Number" Value="100" Min="0" Max="10000"/></td>
                        <td><asp:RequiredFieldValidator runat="server" ID="reqStock"
                            ControlToValidate="tbStock" ForeColor="Red" ErrorMessage="*" /></td>
                    </tr>
                    <tr>
                        <td>Days</td>
                        <td><asp:TextBox runat="server" ID="tbDays" TextMode="Number" Value="30" Min="1" /></td>
                        <td><asp:RequiredFieldValidator runat="server" ID="reqDays"
                            ControlToValidate="tbDays" ForeColor="Red" ErrorMessage="*"/></td>
                    </tr>
                    <tr>
                        <td>Call Implied Volatility (%)</td>
                        <td><asp:TextBox runat="server" ID="tbCallIV" TextMode="Number" Value="30" Min="1" Max="1000" /></td>
                        <td><asp:RequiredFieldValidator runat="server" ID="reqCallIV"
                            ControlToValidate="tbCallIV" ErrorMessage="*" ForeColor="Red"/></td>
                    </tr>
                    <tr>
                        <td>Put Implied Volatility (%)</td>
                        <td><asp:TextBox runat="server" ID="tbPutIV" TextMode="Number" Value="30" Min="1" Max="1000" /></td>
                        <td><asp:RequiredFieldValidator runat="server" ID="reqPutIV"
                            ForeColor="Red" ControlToValidate="tbPutIV" ErrorMessage="*"/></td>
                    </tr>
                    <tr>
                        <td>Interest Rate (%)</td>
                        <td><asp:TextBox runat="server" ID="tbInterest" TextMode="Number" Value="4.12" Step="0.01"
                            Min="0.01" Max="100" /></td>
                        <td><asp:RequiredFieldValidator runat="server" ID="reqInterest"
                            ControlToValidate="tbInterest" ErrorMessage="*" ForeColor="Red"/></td>
                    </tr>
                    <tr>
                        <td>Simulations</td>
                        <td><asp:TextBox runat="server" ID="tbSimulations" TextMode="Number" Value="1" Min="1" Max="5000" Step="1" /></td>
                        <td><asp:RequiredFieldValidator runat="server" ID="reqSims" ControlToValidate="tbSimulations" 
                            ErrorMessage="*" ForeColor="Red" /></td>
                    </tr>
                    <tr>
                        <td>Interval</td>
                        <td><asp:DropDownList runat="server" ID="ddlInterval" /></td>
                    </tr>
                    <tr>
                        <td>Output Type</td>
                        <td><asp:DropDownList runat="server" ID="ddlOutputType" /></td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnSimulate" Text="Simulate" Width="300px" OnClick="btnSimulate_Click"/><br />
                <asp:UpdateProgress runat="server" DisplayAfter="200">
                    <ProgressTemplate>
                        <div style="font-weight: bold;">
                            Loading, please wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <h3>Stock Price Paths</h3>
                            <asp:Chart runat="server" ID="chartStock" SkinID="Large"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Stock Price Frequencies</h3>
                            <asp:Chart runat="server" ID="chartFreq" SkinID="Histogram" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
