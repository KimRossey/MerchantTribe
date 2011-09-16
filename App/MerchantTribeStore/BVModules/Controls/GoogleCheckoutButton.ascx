<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Controls_GoogleCheckoutButton" Codebehind="GoogleCheckoutButton.ascx.cs" %>
<%@ Register Assembly="BVSoftware.GoogleCheckout" Namespace="GCheckout.Checkout"
    TagPrefix="cc1" %>
<div id="googlecheckout">
    <asp:ImageButton ID="GoogleCheckoutImageButton" runat="server" 
        onclick="GoogleCheckoutImageButton_Click" />
</div>