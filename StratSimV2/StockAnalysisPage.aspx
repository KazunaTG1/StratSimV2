<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="StockAnalysisPage.aspx.cs" Inherits="StratSimV2.WebForm15" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Stock Analysis</h1>
        <p>Key valuation tools and simulations to assess stock prices and potential movements. 
            This section provides interactive models that help you estime fair value using discounted cash flows and forecast future price ranges.</p>
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkExpectedMove" NavigateUrl="~/ExpectedStockMove.aspx" 
        Text="Expected Stock Move" CssClass="lnk" /></h2>
        <h3>Forecast Future Price Ranges with Asymmetric Volatility</h3>
        <p>The Expected Move Simulator allows you to run thouasands of price path simulations for a given stock using asymmetric volatility assumptions
            (different implied volatilies for calls and puts).
        </p>
        <p>It calculates possible outcomes based on user input and presents visualizations that help traders and analysts gauge risk,
            potential gains/losses, and probability distributions.</p>
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkFairStockValue" Text="Fair Stock Value" CssClass="lnk" NavigateUrl="~/FairStockValue.aspx"/></h2>
        <h3>Estimate the True Worth of a Stock Based on Future Cash Flows</h3>
        <p>The <b>Discounted Cash Flow (DCF)</b> model is a fundamental valuation method that calculates the 
            <i>intrinsic value</i> of a company based on the present value of its expected future cash flows.</p>
        <p>This helps investors identify whether a stock is overvalued, undervalued, or fairly priced compared to its market price.</p>
    </div>
    
</asp:Content>
