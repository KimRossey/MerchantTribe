<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVAdmin_Controls_FloatModifierField" Codebehind="FloatModifierField.ascx.cs" %>
<asp:TextBox ID="FloatTextBox" runat="server"></asp:TextBox>
<bvc5:BVRegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="FloatTextBox"
    Display="Dynamic" ErrorMessage="Must be in the format ###.###" ValidationExpression="\d{1,7}.\d{1,10}" CssClass="errormessage" ForeColor=" ">*</bvc5:BVRegularExpressionValidator>
<asp:DropDownList ID="FloatDropDownList" runat="server">
    <asp:ListItem Selected="True">Set To</asp:ListItem>
    <asp:ListItem>Add To</asp:ListItem>
    <asp:ListItem>Subtract From</asp:ListItem>
</asp:DropDownList>
