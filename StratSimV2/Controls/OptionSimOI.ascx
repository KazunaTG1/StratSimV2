<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OptionSimOI.ascx.cs" Inherits="StratSimV2.Controls.OptionSimOI" %>
<asp:UpdatePanel runat="server" ID="pnlUpdate">
    <ContentTemplate>
        <table>
            <tr>
                <td>Direction</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDirection" AutoPostBack="true">
                        <asp:ListItem>Long</asp:ListItem>
                        <asp:ListItem>Short</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Quantity Type</td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlQuantity" AutoPostBack="true">
                        <asp:ListItem>% of Balance</asp:ListItem>
                        <asp:ListItem>$ USD</asp:ListItem>
                        <asp:ListItem># of Contracts</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Quantity</td>
                <td>
                    <asp:TextBox runat="server" ID="tbQuantity" TextMode="Number" Value="1" AutoPostBack="true" Min="1" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbTakeProfit" Text="Take Profit (%)" Checked="false" AutoPostBack="true" /></td>
                <td>
                    <asp:TextBox runat="server" ID="tbTakeProfit" TextMode="Number" Value="100" AutoPostBack="true" /></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbStopLoss" Text="Stop Loss (%)" Checked="false" AutoPostBack="true" /></td>
                <td>
                    <asp:TextBox runat="server" ID="tbStopLoss" TextMode="Number" Value="50" AutoPostBack="true" /></td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbCloseDTE" Text="Close DTE" Checked="false" AutoPostBack="true" /></td>
                <td>
                    <asp:TextBox runat="server" ID="tbCloseDTE" TextMode="Number" Value="7" AutoPostBack="true" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="lblResult" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
