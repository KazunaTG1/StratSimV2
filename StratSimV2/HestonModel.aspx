<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="HestonModel.aspx.cs" Inherits="StratSimV2.WebForm11" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function renderMath() {
            if (window.MathJax) {
                MathJax.typesetPromise();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Heston Model</h1>
    <h3>This simulation models stock prices under the Heston stochastic volatility framework.</h3>
    <p style="text-align: center;">
        The Heston model assumes the volatility itself follows a random, mean-reverting process.
            Stock prices are simulated using two correlated sources of randomness: one for the price and one for the variance.
    </p>
    <h3>Stock Price (S)
        <br />
        \( dS_t=\mu S_t dt+\sqrt{v_t} S_t dW^S_t \)</h3>
    <h3>Variance (v)
        <br />
        \( dv_t = \kappa(\theta - v_t)dt + \sigma \sqrt{v_t} dW^v_t \)</h3>
    <asp:UpdatePanel runat="server" ID="pnlUpdate">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <label title="Stock Price at time 0">Stock Price (\( S_0 \)) </label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbStockPrice" TextMode="Number" Value="100" Step="0.01" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Variance at time 0">Volatility (\( \sqrt{v_0} \))</label></td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbVolatility" TextMode="Number" Value="20" Step="0.01" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Days to simulate">Days</label></td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbDays" TextMode="Number" Value="30" Step="1" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Risk-Free Rate / Expected Return">Risk-Free Rate (\( \mu \))</label></td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbExpectedReturn" TextMode="Number" Value="4.12" Step="0.01" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Speed of reversion of volatility to the mean">Volatility Mean Reversion (\( \kappa \))</label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbMeanReversion" TextMode="Number" Value="2.0" Step="0.01" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Long-term Average Volatility">Long-term Average Volatility (\( \theta \))</label></td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbLongTermVol" TextMode="Number" Value="20" Step="0.01" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Volatility of Volatility">Volatility of Volatility (\( \sigma \))</label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbVolVol" TextMode="Number" Value="30" Step="0.01" Width="100px" /></td>
                                <tr>
                                    <td>
                                        <label title="Correlation between stock and volatility">Volatility Correlation (\( \rho \))</label></td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbVolCorrelation" TextMode="Number" Value="-0.07" Step="0.01" Width="100px" /></td>
                                </tr>
                            <tr>
                                <td>
                                    <label title="Simulations">Simulations</label></td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbSimulations" TextMode="Number" Value="1" Step="1" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Interval Length">Interval</label></td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlInterval" Width="100px" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <label title="Show Volatility">
                                        <asp:CheckBox runat="server" ID="cbVol" Text="Show Volatility" OnCheckedChanged="cbVol_CheckedChanged" />
                                    </label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSimulate" Text="Simulate" OnClick="btnSimulate_Click" />

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdateProgress runat="server" DisplayAfter="200">
                                        <ProgressTemplate>
                                            <div style="font-weight: bold;">
                                                Loading, please wait...
                                            </div>
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
                                    <h3>Stock Path</h3>
                                    <asp:Chart runat="server" ID="chartStock" SkinID="Large" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
