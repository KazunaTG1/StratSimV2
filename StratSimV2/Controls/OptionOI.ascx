<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OptionOI.ascx.cs" Inherits="StratSimV2.OptionOI" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <table>
                <tr>
                    <td><b>Symbol</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbSymbol" Width="100px" Text="QQQ"/>
                    </td>
                </tr>
                <tr>
                    <td><b>Option Type</b></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlType" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="option_changed">
                            <asp:ListItem>Call</asp:ListItem>
                            <asp:ListItem>Put</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td><b>Expiration</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbExpiration" TextMode="Date"
                            Text="2025-06-20"  AutoPostBack="true" OnTextChanged="tbExpiration_TextChanged" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqExpiry" ControlToValidate="tbExpiration"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td><b>Days to Expiration (DTE)</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbDTE" TextMode="Number" Value="30" Min="2"
                            OnTextChanged="tbDTE_TextChanged" Width="100px" AutoPostBack="true"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqDTE" ControlToValidate="tbDTE" ErrorMessage="*" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td><b>Underlying Price ($)</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbStockPrice" TextMode="Number" Width="100px"
                            Value="100"  Step="0.10" AutoPostBack="true" OnTextChanged="option_changed"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqStockPrice" ControlToValidate="tbStockPrice"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td><b>Strike ($)</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbStrike" TextMode="Number" Width="100px"
                            Value="115" Step="0.10"  AutoPostBack="true" OnTextChanged="tbStrike_TextChanged"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqStrike" ControlToValidate="tbStrike"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td><b>Moneyness</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbMoneyness" TextMode="Number" Value="1.1" Step="0.01" AutoPostBack="true"
                            OnTextChanged="tbMoneyness_TextChanged" Width="100px"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqMoneyness" ControlToValidate="tbMoneyness"
                            ErrorMessage="*" ForeColor="Red" />
                    </td>
                </tr>
                <tr>
                    <td><b>Implied Volatility (%)</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbIV" TextMode="Number" Width="100px"
                            Value="30" Step="0.10" Min="0" Max="1000"  AutoPostBack="true" OnTextChanged="option_changed"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqIV" ControlToValidate="tbIV"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td><b>Interest Rate (%)</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbInterest" TextMode="Number" Width="100px"
                            Value="4.21" Step="0.01" Min="0" Max="100" AutoPostBack="true" OnTextChanged="option_changed" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqInterest" ControlToValidate="tbInterest"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr runat="server" id="rowMarketPrice">
                    <td><b>Market Price ($)</b></td>
                    <td>
                        <asp:TextBox runat="server" ID="tbMarketPrice" TextMode="Number" Width="100px"
                            Value=".35" Step="0.01" Min="0.01" Max="10000" AutoPostBack="true" OnTextChanged="option_changed" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblResult" />
                    </td>
                </tr>
            </table>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
