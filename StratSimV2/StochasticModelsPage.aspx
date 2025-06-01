<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="StochasticModelsPage.aspx.cs" Inherits="StratSimV2.WebForm12" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="main">
        <h1>Stochastic Stock Price Models</h1>
        <p>Simulate stock price paths using stochastic models. Including Geometric Brownian Motion with constant volatility
                and the Heston model with stochastic, mean-reverting volatility. These simulations provide realistic scenarios for stock prices
                behavior under both simple and complex assumptions, making them valuable for financial modeling, risk analysis, and option pricing.</p>
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkHestonModel" Text="Heston Model" NavigateUrl="~/HestonModel.aspx" CssClass="lnk" /></h2>
        <h3>This simulation models stock prices under the Heston stochastic volatility framework.</h3>
        <p>Unlike models with constant volatility, the Heston model assumes the volatility itself follows a random, mean-reverting process.
            Stock prices are simulated using two correlated sources of randomness: one for the price and one for the variance.
        </p>
        <p>This approach captures real-world phenomena such as volatility clustering and implied volatility skews, 
            which are crucial for accurate option pricing.
        </p>
        <p>Since an option's value highly sensitive to volatility, modeling the randomness of volatility is essential for realistically simulating 
            the path of an options price and realistically estimating the option's fair price and risk profile.
        </p>
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkGBM" Text="Geometric Brownian Motion <i>(Coming Soon)</i>" CssClass="nolnk" /></h2>
        <hr />
        <h2><asp:HyperLink runat="server" ID="lnkAsymmetricGBM" Text="Asymmetric Geometric Brownian Motion <i>(Coming Soon)</i>" CssClass="nolnk" /></h2>
    </div>
    
</asp:Content>
