<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="MarketSimulator.aspx.cs" Inherits="StratSimV2.WebForm17" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Market Simulator</h1>
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td><b>Market Model</b></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlMarketModel" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlMarketModel_SelectedIndexChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td><b>Stock Price</b></td>
                        <td>
                            <asp:TextBox runat="server" ID="tbStockPrice" TextMode="Number" Value="100" Step="0.01" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td><b>Days</b></td>
                        <td>
                            <asp:TextBox runat="server" ID="tbDTE" TextMode="Number" Value="90" Step="1" Min="1" Width="100px" /></td>
                    </tr>
                    <tr>
                        <td><b>Interest Rate</b></td>
                        <td>
                            <asp:TextBox runat="server" ID="tbInterestRate" TextMode="Number" Value="4.12" Step="0.01" Min="0.01" Width="100px" /></td>
                    </tr>
                    <tr>
                        <td><b>Simulations</b></td>
                        <td>
                            <asp:TextBox runat="server" ID="tbSimulations" TextMode="Number" Value="1" Step="1" Min="1" Width="100px" /></td>
                    </tr>
                    <tr>
                        <td><b>Interval</b></td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlInterval" /></td>
                    </tr>
                    <tr runat="server" id="rowGBM" visible="false">
                        <td colspan="2">
                            <h3>Geometric Brownian Motion Parameters</h3>
                            <table>
                                <tr>
                                    <td><b>Implied Volatility</b></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbIV" TextMode="Number" Value="30" Step="0.01" Width="100px" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="rowAsymGBM" visible="false">
                        <td colspan="2">
                            <h3>Asymmetric GBM Parameters</h3>
                            <table>
                                <tr>
                                    <td><b>Call Implied Volatility (%)</b></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbCallIV" TextMode="Number" Step="0.01" Value="30" Width="100px" /></td>
                                </tr>
                                <tr>
                                    <td><b>Put Implied Volatility (%)</b></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbPutIV" TextMode="Number" Step="0.01" Value="30" Width="100px" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr runat="server" id="rowHeston" visible="false">
                        <td colspan="2">
                            <h3>Heston Model Parameters</h3>
                            <table>
                                <tr>
                                    <td>
                                        <b>
                                            <label title="Variance at time 0">Volatility</label></b></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbVolatility" TextMode="Number" Value="20" Step="0.01" Width="100px" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <label title="Speed of reversion of volatility to the mean">Volatility Mean Reversion</label></b>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbMeanReversion" TextMode="Number" Value="2.0" Step="0.01" Width="100px" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <label title="Long-term Average Volatility">Long-term Average Volatility</label></b></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbLongTermVol" TextMode="Number" Value="20" Step="0.01" Width="100px" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>
                                            <label title="Volatility of Volatility">Volatility of Volatility</label></b>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbVolVol" TextMode="Number" Value="30" Step="0.01" Width="100px" /></td>
                                    <tr>
                                        <td>
                                            <b>
                                                <label title="Correlation between stock and volatility">Volatility Correlation</label></b></td>
                                        <td>
                                            <asp:TextBox runat="server" ID="tbVolCorrelation" TextMode="Number" Value="-0.07" Step="0.01" Width="100px" /></td>
                                    </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox runat="server" ID="cbShowVol" Text="Show Volatility" Checked="false" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" ID="btnSimulate" Text="Simulate" OnClick="btnSimulate_Click" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdateProgress runat="server" ID="updateProgress" AssociatedUpdatePanelID="pnlUpdate">
                                <ProgressTemplate>
                                    Loading...
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <h2><%= ddlMarketModel.SelectedItem.Text %> Stock Path</h2>
                            <asp:Literal runat="server" ID="litDescription" /><br />
                            <hr />
                            <h4>
                                <asp:Label runat="server" ID="lblResult" /></h4>
                            <asp:Chart runat="server" ID="chartStock" SkinID="Large" />
                        </td>
                    </tr>
                    
                    <tr>
                        <td>
                            <h3>Simulations</h3>
                            <asp:Button runat="server" ID="btnAllSims" Text="All Sims" OnClick="btnAllSims_Click" />
                            <asp:GridView ID="gvStockPaths" SkinID="StockPaths"
                                runat="server"
                                OnPageIndexChanging="gvStockPaths_PageIndexChanging" 
                                OnRowDataBound="gvStockPaths_RowDataBound" 
                                OnSorting="gvStockPaths_Sorting"
                                OnSelectedIndexChanged="gvStockPaths_SelectedIndexChanged" />

                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Stock Price Frequencies</h3>
                            <asp:Chart runat="server" ID="chartFreq" SkinID="Histogram" />
                            <br />
                            Bins: &nbsp;
                            <asp:TextBox runat="server" ID="tbBins" TextMode="Number" Value="20" Min="1" Step="1" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>


</asp:Content>
