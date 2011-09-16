<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVAdmin_Controls_PercentAmountSelection" Codebehind="PercentAmountSelection.ascx.cs" %>
<asp:TextBox ID="AmountTextBox" runat="server" Width="136px"></asp:TextBox>
<bvc5:BVCustomValidator ID="PercentCustomValidator" runat="server" Display="Dynamic"
    ErrorMessage="Percent must be between 0.00 and 100.00 percent." 
    CssClass="errormessage" ForeColor=" " 
    onservervalidate="PercentCustomValidator_ServerValidate">*</bvc5:BVCustomValidator>
<asp:DropDownList ID="AmountDropDownList" runat="server">
    <asp:ListItem Selected="True">Percent</asp:ListItem>
    <asp:ListItem>Amount</asp:ListItem>
</asp:DropDownList>&nbsp;
