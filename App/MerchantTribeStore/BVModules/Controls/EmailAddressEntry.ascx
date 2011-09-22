<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_EmailAddressEntry" Codebehind="EmailAddressEntry.ascx.cs" %>
<table>
    <tr>
        <td class="formlabel">
            Email Address:
        </td>
        <td class="formfield">
            <asp:TextBox ID="EmailTextBox" CssClass="E-mail Address" runat="server"></asp:TextBox> 
            <asp:RequiredFieldValidator ID="EmailAddressRequiredFieldValidator" runat="server" EnableClientScript="True" ErrorMessage="E-mail Address Is Required." ControlToValidate="EmailTextBox" ForeColor=" " CssClass="errormessage" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="BVRegularExpressionValidator1" ForeColor=" " CssClass="errormessage"
                runat="server" ControlToValidate="EmailTextBox" Display="Dynamic" ErrorMessage="Please enter a valid email address"
                ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>
    </tr>
</table>