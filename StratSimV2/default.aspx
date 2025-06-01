<%@ Page Title="Home | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="StratSimV2.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        
        
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Welcome to QuantLabs!</h1>
        <br />
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkMarketSimulator" 
            NavigateUrl="~/MarketSimulator.aspx" Text="Market Simulator" CssClass="lnk"/></h2>
        <h3>Simulate stock price paths using stochastic models.</h3>
        <p> Including Geometric Brownian Motion with constant volatility and the Heston model with stochastic, mean-reverting volatility. 
            These simulations provide realistic scenarios for stock prices behavior under both simple and complex assumptions, 
            making them valuable for financial modeling, risk analysis, and option pricing.
        </p>
        <br /><hr />
        <h2><asp:HyperLink runat="server" ID="lnkOptionSims" NavigateUrl="~/OptionSimsPage.aspx" CssClass="lnk"> Option Simulators</asp:HyperLink></h2>
        <h3>Track option prices, stock movements, and equity growth over time</h3>
        <p>QuantLabs Option Simulator is an interactive platform designed to help you test, refine, and 
            develop options trading strategies using realistic market behavior modeled by geometric Brownian motion.
            This simulator lets you customize key option parameters, 
            as well as trading parameters such as a take profit, stop loss, and close days to expiration.</p>
        <p>Track option prices, stock movements, and equity growth over time as you simulate trades and configure risk settings.
            Whether you're optimizing profit targets or exploring how volatility affects performance, this tool empowers you with 
            data-driven insights to improve your edge in the options market.</p><br />
        
        <hr />
        <br />
        
        <h2><asp:HyperLink runat="server" ID="lnkOptionAnalysis" NavigateUrl="~/OptionAnalysisPage.aspx" CssClass="lnk">Option Analysis</asp:HyperLink></h2>
        <h3>Explore how price movement, volatility, and time impact your trades</h3>
        <p>Option Analysis is designed to give traders a deeper understanding of how an option behaves under different market conditions.
            Whether you're evaluating profitability or sensitivity to market factors, this tool helps you visualize the full risk and reward
            profile of your strategy.
        </p>
        <p>Choose between P/L analysis and option Greek analysis to explore how price movement, volatility, and time impact your trades and an
            options price in a clear, interactive way</p><br />
        <hr />
        <h2><asp:HyperLink runat="server" CssClass="lnk" NavigateUrl="~/StockAnalysisPage.aspx"
            ID="lnkStockAnalysis">Stock Analysis</asp:HyperLink> </h2>
        <h3>Analyze Stock Value and Future Price Movements</h3>
        <p>This section offers tools to evaluate the financial worth of a company and simulate potential stock price scenarios. 
            Gain insights into whether a stock is undervalued or overvalued using fundamental valuation techniques.</p>
        <p>Use models like Discounted Cash Flow to estimate intrinsic value, 
            and simulate price paths with asymmetric volatility to explore risk and return under varying market conditions.</p>
        <hr />
    </div>
    
</asp:Content>
