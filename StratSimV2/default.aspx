<%@ Page Title="Home | QuantLabs" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="StratSimV2.WebForm6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .main {
            text-align:center;
            font-size:1.3em;
            padding: 1em 15em;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Welcome to QuantLabs!</h1>
        <h2>Option Simulators</h2>
        <p>QuantLabs Option Simulator is an interactive platform designed to help you test, refine, and 
            develop options trading strategies using realistic market behavior modeled by geometric Brownian motion.
            This simulator lets you customize key option parameters, 
            as well as trading parameters such as a take profit, stop loss, and close days to expiration.</p>
        <p>Track option prices, stock movements, and equity growth over time as you simulate trades and configure risk settings.
            Whether you're optimizing profit targets or exploring how volatility affects performance, this tool empowers you with 
            data-driven insights to improve your edge in the options market.</p><br />
        <h3><asp:HyperLink runat="server" ID="lnkSingleOption" NavigateUrl="~/SingleOption.aspx" Text="Single Option" ForeColor="#F6B2FF" /></h3>
        <p>The Single Option simulator allows you to model the performance of an individual call or put using geometric Brownian motion to
            simulate market behavior. Customize option and trading parameters.
        </p>
        <p>This tool is perfect for analyzing the outcome of standalone directional trades and evaluating how your strategy performs
            under different market conditions
        </p>
        <h3><asp:HyperLink runat="server" ID="lnkTwoOptions" NavigateUrl="~/callandputs.aspx" Text="Two Options"
            ForeColor="#F6B2FF" /></h3>
        <p>The Double Options simulator is designed for testing multi-leg strategies such as hedging and spreads.
            By simulating the simultaneous behavior of two separate options, this tool helps you explore how combined positions react to price movement,
            volatility shifts, and time decay.
        </p>
        <p>Whether you're building straddles, strangles, or protective hedges, the simulator provides a clear breakdown of performance
            and risk for both legs in real-time.
        </p>
        <h3 ><asp:HyperLink runat="server" ID="lnkExpectedValue" NavigateUrl="~/ExpectedOptionValue.aspx"
            Text="Expected Option Value" ForeColor="#F6B2FF" /></h3>
        <p>The Expected Return module runs multiple simulations of a single options trade to help you understand long-term performance. 
            By using geometric Brownian motion across many price paths, this tool estimates your average profit/loss, win rate, 
            and equity growth over time.
        </p>
        <p>Each run reflects the effect of your custom trade settings so you can evaluate how consistent and profitable your strategy might
           be in the long term.
        </p>
        <hr />
        <br />
        <h2>Option Analysis</h2>
        <p>Option Analysis is designed to give traders a deeper understanding of how an option behaves under different market conditions.
            Whether you're evaluating profitability or sensitivity to market factors, this tool helps you visualize the full risk and reward
            profile of your strategy.
        </p>
        <p>Choose between P/L analysis and option Greek analysis to explore how price movement, volatility, and time impact your trades and an
            options price in a clear, interactive way</p><br />
        <h3 ><asp:HyperLink runat="server" ID="lnkAnalysisPL" NavigateUrl="~/OptionAnalysis.aspx" Text="Option P/L Analysis" 
            ForeColor="#F6B2FF"/></h3>
        <p>P/L Analysis lets you visualize the potential profit and loss of a single option trade across different stock prices.
            Input the option values and instantly see breakeven points, max gain/loss, and the stock price required to hit your take-profit target.
        </p>
        <p>
            Using Monte Carlo simulation, the tool also estimates probabilities for outcomes like 
            closing the trade in profit, hitting the take profit, stop loss, closing before expiration, or expiring.
        </p><br />
        <h3><asp:HyperLink runat="server" ID="lnkAnalysisGreek" NavigateUrl="~/OptionGreekAnalysis.aspx" Text="Option Greek Analysis"
            ForeColor="#F6B2FF"/></h3>
        <p>Option Greek Analysis allows you to explore how the option's value responds to changes in market variables.
            Toggle between Greeks like Delta, Gamma, Theta, Vega, and Rho to see how each metric evolves across a range of stock prices.
        </p>
        <p>The interactive chart overlays both the option price and the selected Greek, providing clear insight into how sensitive your position
            is to price movement, volatility, time decay, and interest rate changes.
        </p>
    </div>
    
</asp:Content>
