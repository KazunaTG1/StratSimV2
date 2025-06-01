<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="OptionAnalysisPage.aspx.cs" Inherits="StratSimV2.WebForm14" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Option Analysis</h1>
        <p>Option Analysis is designed to give traders a deeper understanding of how an option behaves under different market conditions.
            Whether you're evaluating profitability or sensitivity to market factors, this tool helps you visualize the full risk and reward
            profile of your strategy.
        </p>
        <p>Choose between P/L analysis and option Greek analysis to explore how price movement, volatility, and time impact your trades and an
            options price in a clear, interactive way</p><br />
        <hr />
        <h2 ><asp:HyperLink runat="server" ID="lnkAnalysisPL" NavigateUrl="~/OptionAnalysis.aspx" Text="Option P/L Analysis" 
            CssClass="lnk" /></h2>
        <h3>Visualize P/L across Stock Prices</h3>
        <p>P/L Analysis lets you visualize the potential profit and loss of a single option trade across different stock prices.
            Input the option values and instantly see breakeven points, max gain/loss, and the stock price required to hit your take-profit target.
        </p>
        <p>
            Using Monte Carlo simulation, the tool also estimates probabilities for outcomes like 
            closing the trade in profit, hitting the take profit, stop loss, closing before expiration, or expiring.
        </p><br />
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkAnalysisGreek" NavigateUrl="~/OptionGreekAnalysis.aspx" Text="Option Greek Analysis"
            CssClass="lnk" /></h2>
        <h3>Visualize the Change in Option Price Based On Market Variables</h3>
        <p>Option Greek Analysis allows you to explore how the option's value responds to changes in market variables.
            Toggle between Greeks like Delta, Gamma, Theta, Vega, and Rho to see how each metric evolves across a range of stock prices.
        </p>
        <p>The interactive chart overlays both the option price and the selected Greek, providing clear insight into how sensitive your position
            is to price movement, volatility, time decay, and interest rate changes.
        </p>
        <hr />
    </div>
    
</asp:Content>
