<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="FairStockValue.aspx.cs" Inherits="StratSimV2.WebForm9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Fair Stock Value</h1>
        <hr />
        <h2>Fundamental Long-Term Valuation</h2>
        <h3><asp:HyperLink runat="server" ID="lnkDCF" NavigateUrl="~/DcfPage.aspx" Text="Discounted Cash Flow (DCF) Method" CssClass="lnk" /></h3>
        <p>The <b>Discounted Cash Flow (DCF)</b> model is a fundamental valuation method that calculates the 
        <i>intrinsic value</i> of a company based on the present value of its expected future cash flows.</p>
        <p>This helps investors identify whether a stock is overvalued, undervalued, or fairly priced compared to its market price.</p>
        <h3><asp:HyperLink runat="server" ID="lnkDDM" Text="Dividend Discount Model (DDM) <i>(Coming Soon)</i>" CssClass="nolnk"/></h3>
        <p>For dividend-paying stocks, the fair price is the present value of expected future dividends.</p>
        <hr />
        <h2>Quick Market-Based Estimates</h2>
        <h3><asp:HyperLink runat="server" ID="lnkRelativeValue" Text="Relative Valuation <i>(Coming Soon)</i>" CssClass="nolnk" /></h3>
        <p>Compare the stock to peers using multiple like P/E, EV/EBITDA, P/B, or PEG</p>
        <hr />
        <h2>Probabilistic Models</h2>
        <h3><asp:HyperLink runat="server" ID="lnkQuantFactor" Text="Quantitative Factor Models <i>(Coming Soon)</i>" CssClass="nolnk" /></h3>
        <p>Use regressions or machine learning to estimate price based on factors (e.g. value, size, momentum, quality).</p>
        <h3><asp:HyperLink runat="server" ID="lnkMonteCarlo" Text="Monte Carlo Simulation <i>(Coming Soon)</i>" CssClass="nolnk" /></h3>
        <p>Project future price paths using stochastic models, discount expected payoffs, and average.</p>
    </div>
    
</asp:Content>
