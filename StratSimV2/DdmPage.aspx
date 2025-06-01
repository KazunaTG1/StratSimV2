<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DdmPage.aspx.cs" Inherits="StratSimV2.WebForm10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Dividend Discount Model (DDM) Fair Value Price</h1>
    <h3>For dividend-paying stocks, the fair value price is the present value of expected future dividends.</h3>
    <hr />
    <br />
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>Previous Dividend</td>
                        <td><asp:TextBox runat="server" ID="tbStartDividend" TextMode="Number" Value="2" Step="0.01" /></td>
                        <td><asp:TextBox runat="server" ID="tbStartDividendDate" TextMode="Date" /></td>
                    </tr>
                    <tr>
                        <td>Current Dividend</td>
                        <td><asp:TextBox runat="server" ID="tbEndDividend" TextMode="Number" Value="2.5" Step="0.01"/></td>
                        <td><asp:TextBox runat="server" ID="tbEndDividendDate" TextMode="Date" /></td>
                    </tr>
                    <tr>
                        <td>Risk-Free Rate (%)</td>
                        <td><asp:TextBox runat="server" ID="tbRiskFreeRate" TextMode="Number" Value="4.12" Step="0.01" /></td>
                    </tr>
                    <tr>
                        <td>Stock Beta</td>
                        <td><asp:TextBox runat="server" ID="tbStockBeta" TextMode="Number" Value="1" Step="0.0001" /></td>
                    </tr>
                    <tr>
                        <td>Expected Market Return</td>
                        <td><asp:TextBox runat="server" ID="tbExpectedReturn" TextMode="Number" Value="9.5" Step="0.01" /></td>
                    </tr>
                </table>
            </td>
            <td>
                <asp:Label runat="server" ID="lblResult" />
            </td>
        </tr>
    </table>
    
    <asp:Button runat="server" ID="btnCalculate" Text="Calculate" OnClick="btnCalculate_Click" />
</asp:Content>
