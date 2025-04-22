<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OptionOI.ascx.cs" Inherits="StratSimV2.OptionOI" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>
            <table>
                <tr>
                    <td>Symbol</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbSymbol" Text="QQQ"/>
                    </td>
                </tr>
                <tr>
                    <td>Option Type</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlType"  AutoPostBack="true" OnSelectedIndexChanged="option_changed">
                            <asp:ListItem>Call</asp:ListItem>
                            <asp:ListItem>Put</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Expiration</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbExpiration" TextMode="Date" 
                            Text="2025-05-23"  AutoPostBack="true" OnTextChanged="option_changed" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqExpiry" ControlToValidate="tbExpiration"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td>Strike ($)</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbStrike" TextMode="Number" 
                            Value="115" Step="0.10"  AutoPostBack="true" OnTextChanged="option_changed"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqStrike" ControlToValidate="tbStrike"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td>Underlying Price ($)</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbStockPrice" TextMode="Number" 
                            Value="100"  Step="0.10" AutoPostBack="true" OnTextChanged="option_changed"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqStockPrice" ControlToValidate="tbStockPrice"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td>Implied Volatility (%)</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbIV" TextMode="Number" 
                            Value="30" Step="0.10" Min="0" Max="1000"  AutoPostBack="true" OnTextChanged="option_changed"/>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqIV" ControlToValidate="tbIV"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr>
                    <td>Interest Rate (%)</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbInterest" TextMode="Number" 
                            Value="4.21" Step="0.01" Min="0" Max="100" AutoPostBack="true" OnTextChanged="option_changed" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator runat="server" ID="reqInterest" ControlToValidate="tbInterest"
                            ErrorMessage="*" ForeColor="Red"/>
                    </td>
                </tr>
                <tr runat="server" id="rowMarketPrice">
                    <td>Market Price ($)</td>
                    <td>
                        <asp:TextBox runat="server" ID="tbMarketPrice" TextMode="Number" 
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
