<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="OptionSimsPage.aspx.cs" Inherits="StratSimV2.WebForm13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Option Simulators</h1>
        <p>QuantLabs Option Simulator is an interactive platform designed to help you test, refine, and 
            develop options trading strategies using realistic market behavior modeled by geometric Brownian motion.
            This simulator lets you customize key option parameters, 
            as well as trading parameters such as a take profit, stop loss, and close days to expiration.</p>
        <p>Track option prices, stock movements, and equity growth over time as you simulate trades and configure risk settings.
            Whether you're optimizing profit targets or exploring how volatility affects performance, this tool empowers you with 
            data-driven insights to improve your edge in the options market.</p><br />
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkSingleOption" NavigateUrl="~/SingleOption.aspx" 
            Text="Single Option" CssClass="lnk" /></h2>
        <h3>Model the Performance of an Individual Option</h3>
        <p>The Single Option simulator allows you to model the performance of an individual call or put using geometric Brownian motion to
            simulate market behavior. Customize option and trading parameters.
        </p>
        <p>This tool is perfect for analyzing the outcome of standalone directional trades and evaluating how your strategy performs
            under different market conditions
        </p>
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkTwoOptions" NavigateUrl="~/callandputs.aspx" Text="Two Options"
            CssClass="lnk" /></h2>
        <h3>Test Multi-Leg Strategies</h3>
        <p>The Double Options simulator is designed for testing multi-leg strategies such as hedging and spreads.
            By simulating the simultaneous behavior of two separate options, this tool helps you explore how combined positions react to price movement,
            volatility shifts, and time decay.
        </p>
        <p>Whether you're building straddles, strangles, or protective hedges, the simulator provides a clear breakdown of performance
            and risk for both legs in real-time.
        </p>
        <hr />
        <h2 ><asp:HyperLink runat="server" ID="lnkExpectedValue" NavigateUrl="~/ExpectedOptionValue.aspx"
            Text="Expected Option Value" CssClass="lnk" /></h2>
        <h3>Estimate the Expected Payout of an Option Strategy</h3>
        <p>The Expected Return module runs multiple simulations of a single options trade to help you understand long-term performance. 
            By using geometric Brownian motion across many price paths, this tool estimates your average profit/loss, win rate, 
            and equity growth over time.
        </p>
        <p>Each run reflects the effect of your custom trade settings so you can evaluate how consistent and profitable your strategy might
           be in the long term.
        </p>
    </div>
    
</asp:Content>
