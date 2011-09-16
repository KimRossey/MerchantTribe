<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVAdmin_Controls_IntegerModifierField" Codebehind="IntegerModifierField.ascx.cs" %>
<asp:TextBox ID="IntegerTextBox" runat="server"></asp:TextBox>
<bvc5:BVRegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="IntegerTextBox"
    Display="Dynamic" ErrorMessage="Field must be numeric." ValidationExpression="\d{1,14}" CssClass="errormessage" ForeColor=" ">*</bvc5:BVRegularExpressionValidator>
<asp:DropDownList ID="IntegerDropDownList" runat="server">
    <asp:ListItem Selected="True">Set To</asp:ListItem>
    <asp:ListItem>Add To</asp:ListItem>
    <asp:ListItem>Subtract From</asp:ListItem>
</asp:DropDownList>
