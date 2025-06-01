<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="DeltaHedge.aspx.cs" Inherits="StratSimV2.WebForm16" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Delta Hedge</h1>
    <table>
        <tr>
            <td>
                <fin:OptionOI runat="server" ID="oiOption" UseMarketPrice="false" />
                <asp:Button runat="server" ID="btnSimulate" Text="Simulate" OnClick="btnSimulate_Click" />
                <asp:Button runat="server" ID="btnReset" Text="Reset" />
                <br />
                <table>
                    <tr>
                        <td><b>Hedge Ratio</b></td>
                        <td><asp:TextBox runat="server" ID="tbRatio" TextMode="Number" Value="1" Min="1" Step="0.01" Max="100" Width="100px" /></td>
                    </tr>
                    <tr>
                        <td><b>Starting Balance</b></td>
                        <td><asp:TextBox runat="server" ID="tbSBal" TextMode="Number" Value="1" Min="1" Step="1" Width="100px" /></td>
                    </tr>
                </table>
                
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <h2>P/L</h2>
                            <asp:CheckBox runat="server" ID="cbTotalPL" Text="Total P/L" Checked="true" />
                            <asp:CheckBox runat="server" ID="cbStockVal" Text="Stock Value" />
                            <asp:CheckBox runat="server" ID="cbOptionPL" Text="Option P/L" />
                            <asp:CheckBox runat="server" ID="cbOptionVal" Text="Option Value" />
                            <asp:CheckBox runat="server" ID="cbCash" Text="Cash" /><br />
                            <hr />
                            <asp:Chart runat="server" ID="chartPL" SkinID="Large" />

                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblPL" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h2>Stock</h2>
                            <asp:Chart runat="server" ID="chartStock" SkinID="Large" />

                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblStockRes" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h2>Option</h2>
                            <asp:Chart runat="server" ID="chartOption" SkinID="Large" />
                        </td>
                        <td>
                            <asp:Label runat="server" ID="lblOptionResult" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h2>Equity</h2>
                            <asp:Chart runat="server" ID="chartEquity" SkinID="Large" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
