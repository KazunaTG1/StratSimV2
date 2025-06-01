<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TradingSimulator.aspx.cs" Inherits="StratSimV2.WebForm18" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Trading Simulator</h1>
    <table>
        <tr>
            <td class="fixed-cell">
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
                            <table>
                                <tr>
                                    <td colspan="2"><h3>Positions</h3></td>
                                </tr>
                                <tr runat="server" id="rowOptionPosition" visible="false">
                                    <td colspan="2">
                                        <h3>Option Position</h3>
                                        <asp:GridView runat="server" ID="gvOpPositions"
                                           AutoGenerateDeleteButton="true" OnRowDeleting="gvOpPositions_RowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="Type" 
                                                    HeaderText="Type" SortExpression="Type" />
                                                <asp:BoundField DataField="Strike" 
                                                    HeaderText="Strike" SortExpression="Strike" DataFormatString="{0:C2}" />
                                                <asp:BoundField DataField="StockPrice" 
                                                    HeaderText="Stock Price" SortExpression="StockPrice" DataFormatString="{0:C2}" />
                                                <asp:BoundField DataField="ImpliedVolatility" 
                                                    HeaderText="Implied Volatility" SortExpression="ImpliedVolatility" DataFormatString="{0:P2}" />
                                                <asp:BoundField DataField="Expiration"
                                                    HeaderText="Expiration" SortExpression="Expiration" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Quantity"
                                                    HeaderText="Quantity" SortExpression="Quantity" DataFormatString="{0:F0}" />
                                                <asp:BoundField DataField="TheorPrice"
                                                    HeaderText="Theor Price" SortExpression="TheorPrice" DataFormatString="{0:C2}" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr runat="server" id="rowStockPosition" visible="false">
                                    <td colspan="2">
                                        <h3>Stock Position</h3>
                                        <asp:GridView runat="server" ID="gvStockPosition" AutoGenerateDeleteButton="true"
                                            OnRowDeleting="gvStockPosition_RowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="Price"
                                                    HeaderText="Price" SortExpression="Price" DataFormatString="{0:C2}" />
                                                <asp:BoundField DataField="Quantity"
                                                    HeaderText="Quantity" SortExpression="Quantity" DataFormatString="{0:F2}" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"><h4>New Position</h4></td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Direction</b>
                                    </td>
                                    <td><asp:DropDownList runat="server" ID="ddlDirection" /></td>
                                </tr>
                                <tr>
                                    <td><b>Equity Type</b></td>
                                    <td><asp:DropDownList runat="server" ID="ddlEquityType" 
                                        AutoPostBack="true" OnSelectedIndexChanged="ddlEquityType_SelectedIndexChanged"/></td>
                                </tr>
                                <tr>
                                    <td><b>Quantity</b></td>
                                    <td><asp:TextBox runat="server" ID="tbQuantity" TextMode="Number" Value="1" Step="0.01" /></td>
                                </tr>
                                <tr runat="server" id="rowOptionType" visible="false">
                                    <td><b>Option Type</b></td>
                                    <td><asp:DropDownList runat="server" ID="ddlOptionType" /></td>
                                </tr>
                                <tr runat="server" id="rowStrike" visible="false">
                                    <td><b>Strike</b></td>
                                    <td><asp:TextBox runat="server" ID="tbStrike" TextMode="Number" Value="100" Step="0.01" /></td>
                                </tr>
                                <tr runat="server" id="rowDTE" visible="false">
                                    <td><b>Days to Expiration</b></td>
                                    <td><asp:TextBox runat="server" ID="tbOpDTE" TextMode="Number" Value="90" Step="0.01" /></td>
                                </tr>
                                <tr runat="server" id="rowIV" visible="false">
                                    <td><b>Implied Volatility</b></td>
                                    <td><asp:TextBox runat="server" ID="tbOpIV" TextMode="Number" Step="0.01" Value="30" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button runat="server" ID="btnAddPosition" Text="Add Position" OnClick="btnAddPosition_Click" />
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnClearPositions" Text="Clear Positions" OnClick="btnClearPositions_Click" />
                                    </td>
                                    
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br /><hr />
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
                <div class="scrollable-cell">
                    <table>
                        <tr>
                            <td>
                                <h2>Equity</h2>
                                <h4><asp:Literal runat="server" ID="litEquity" /></h4>
                                <asp:Chart runat="server" ID="chartEquity" SkinID="Large" />
                            </td>
                        </tr>
                        <tr>
                            <td><h2>Payouts</h2>
                                <h4><asp:Literal runat="server" ID="litPayouts" /></h4>
                                <asp:Chart runat="server" ID="chartPayouts" SkinID="Histogram" />
                            </td>
                        </tr>
                        <tr>
                            <td><h2>Trade P/L</h2>
                                <h4><asp:Literal runat="server" ID="litPL" /></h4>
                                <asp:Chart runat="server" ID="chartPL" SkinID="Large" />
                            </td>
                        </tr>
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
                                <h3>Stock Path</h3>
                                <asp:GridView runat="server" ID="gvStockPath" SkinID="StockPaths" />
            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h2>Option Prices</h2>
                                <hr />
                                <h4><asp:Label runat="server" ID="lblOpResult" /></h4>
                                <asp:Chart runat="server" ID="chartOption" SkinID="Large" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3>Option Paths</h3>
                                <asp:GridView runat="server" ID="gvOptionPaths" SkinID="StockPaths" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h2>Option PL</h2><hr />
                                <asp:Chart runat="server" ID="chartOptionPL" SkinID="Large" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3>Option PL</h3>
                                <asp:GridView runat="server" ID="gvOptionPL" SkinID="Trades" />
                            </td>
                        </tr>
                    </table>
                </div>
                
            </td>
        </tr>
    </table>
</asp:Content>
