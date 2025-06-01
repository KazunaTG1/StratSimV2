<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DcfPage.aspx.cs" Inherits="StratSimV2.WebForm8" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Discounted Cash Flow Fair Stock Value</h2>
    <h3>Estimate the True Worth of a Stock Based on Future Cash Flows</h3>
    <p style="text-align:center">The <b>Discounted Cash Flow (DCF)</b> model is a fundamental valuation method that calculates the 
    <i>intrinsic value</i> of a company based on the present value of its expected future cash flows.</p>
    <!-- The Compound Annual Growth Rate (CAGR) takes samples of Free Cash Flow and calculates the growth rate. -->

    <hr />
    <br />
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>Starting Free Cash Flow ($)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbFCF1" TextMode="Number" Value="68"  Step="0.01" Width="80px"/>
                        </td>
                        <td><asp:DropDownList runat="server" ID="ddlSfcfMultiplier" /></td>
                        <td>
                            <asp:TextBox runat="server" ID="tbFCFDate1" TextMode="Date" Text="2020-01-01" />
                        </td>
                    </tr>
                    <tr>
                        <td>Ending Free Cash Flow ($)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbFCF2" TextMode="Number" Value="100" Step="0.01" Width="80px" />
                        </td>
                        <td><asp:DropDownList runat="server" ID="ddlEFcfMultiplier" /></td>
                        <td>
                            <asp:TextBox runat="server" ID="tbFCFDate2" TextMode="Date" Text="2025-01-01" />
                        </td>
                    </tr>
                    <tr>
                        <td>Cash & Cash Equivalents ($)</td>
                        <td><asp:TextBox runat="server" ID="tbCash"  TextMode="Number" Step="0.01" Value="200" Width="80px"/></td>
                        <td><asp:DropDownList runat="server" ID="ddlCashMult" /></td>
                    </tr>
                    <tr>
                        <td>Market Cap ($)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbMarketCap" TextMode="Number" Value="2000" Step="0.01" Width="80px"/></td>
                        <td><asp:DropDownList runat="server" ID="ddlCapMult" /></td>
                    </tr>
                    <tr>
                        <td>Debt Outstanding ($)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbDebtOutstanding" TextMode="Number" Value="500" Step="0.01" Width="80px"/></td>
                        <td><asp:DropDownList runat="server" ID="ddlDebtMult" /></td>
                    </tr>
                    <tr>
                        <td>Outstanding Shares</td>
                        <td><asp:TextBox runat="server" ID="tbShares" TextMode="Number" Value="100"  Step="0.01" Width="80px"/></td>
                        <td><asp:DropDownList runat="server" ID="ddlSharesMult" /></td>
                    </tr>
                    <tr>
                        <td>Corporate Tax Rate (%)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbTaxRate" TextMode="Number" Value="25" Step="0.01" Width="80px"/></td>
                    </tr>
                    <tr>
                        <td>Risk-Free Rate (%)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbInterestRate" TextMode="Number" Value="4.5" Step="0.01" Width="80px"/></td>
                    </tr>
                    <tr>
                        <td>Stock Beta</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbStockBeta" TextMode="Number" Value="1.2" Step="0.01" Width="80px" /></td>
                    </tr>
                    <tr>
                        <td>Expected Market Return (%)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbExpectedReturn" TextMode="Number" Step="0.01" Value="9.5" Width="80px"/></td>
                    </tr>
                    <tr>
                        <td>Debt Yields (%)</td>
                        <td>
                            <asp:TextBox runat="server" ID="tbDebtYields" TextMode="Number" Value="6" Step="0.01" Width="80px"/></td>
                    </tr>
                    
                    <tr>
                        <td>Perpetual Growth Rate (%)</td>
                        <td><asp:TextBox runat="server" ID="tbPerpGrowth" TextMode="Number" Value="2.5" Step="0.01" Width="80px" /></td>
                    </tr>
                </table>
                <asp:Button runat="server" ID="btnCalculate" Text="Calculate" OnClick="btnCalculate_Click" />
            </td>
            <td>
                <table>
                    <tr>
                        <td><h2><asp:Label runat="server" ID="lblResult" /></h2>
                            <h4><asp:Label runat="server" ID="lblValues" /></h4><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Free Cash Flow</h3>
                            <h4><asp:Label runat="server" ID="lblFCF" /></h4>
                            <asp:Chart runat="server" ID="barFCF" SkinID="Bar"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Projected Free Cash Flow & Present Values</h3>
                            <asp:Chart runat="server" ID="barPFCF" SkinID="Bar"/>
                        </td>
                    </tr>
                    <tr>
                        
                        <td>
                            <h3>Capital Structure</h3>
                            <h4><asp:Label runat="server" ID="lblCapStruct" /></h4><br />
                            <asp:Chart runat="server" ID="barCapStructure" SkinID="Bar" /></td>
                    </tr>
                </table>
                
            </td>
        </tr>
    </table>

    
    
</asp:Content>
