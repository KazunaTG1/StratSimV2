<%@ Page Title="Option P/L | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="OptionAnalysis.aspx.cs" Inherits="StratSimV2.WebForm4" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="fin" TagName="OptionSimOI" Src="~/Controls/OptionSimOI.ascx" %>
<%@ Register TagPrefix="fin" TagName="OptionOI" Src="~/Controls/OptionOI.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Option P/L Analysis</h2>
    <h3>Visualize P/L across Stock Prices</h3>
    <table>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <asp:Panel runat="server" ID="Panel1">
                                <h3>Option</h3>
                                <fin:OptionOI runat="server" ID="oiOption" UseMarketPrice="false" />
                            </asp:Panel><br />
                            <h3>Simulation Settings</h3>
                            <fin:OptionSimOI runat="server" ID="oiOptionSim" UseInterval="false"  />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <h3>P/L</h3>
                            <h4><asp:Label runat="server" ID="lblResPL" /></h4>
                            <asp:Chart runat="server" ID="chartPL" SkinID="Numerical" Width="1400" Height="600" /><br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>Option Prices</h3>
                            <h4><asp:Label runat="server" ID="lblResPrices" /></h4>
                            <asp:Chart runat="server" ID="chartPrices" SkinID="Numerical" Width="1400" Height="600" /><br />
                            <hr /><br />
                            <asp:CheckBox runat="server" ID="cbFindProbabilities" Text="Find Probabilities*" Checked="false"/><br /><br />
                            <small><i><b>*</b> (Finding probabilities are found through monte carlo simulations and may reduce performance)</i></small>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            Chart Scale:
                            <asp:TextBox runat="server" ID="tbScale" TextMode="Number" Min="1" Max="50" Step="1" Value="10"
                                OnTextChanged="Calculate" AutoPostBack="true" />
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="tbScale"
                                ErrorMessage="*" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
