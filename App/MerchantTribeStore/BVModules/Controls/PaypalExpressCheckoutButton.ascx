<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Controls_PaypalExpressCheckoutButton" Codebehind="PaypalExpressCheckoutButton.ascx.cs" %>
<asp:Panel ID="PaypalExpress" runat="server">&nbsp;<br />
<span style="padding:0 60px 0 0;">- OR - </span><br />&nbsp;<br />
    <asp:ImageButton ID="PaypalImageButton" runat="server" AlternateText="Checkout with Paypal"
        ImageUrl="https://www.paypal.com/en_US/i/btn/btn_xpressCheckout.gif" 
        Style="margin-right: 7px;" CausesValidation="false" 
        onclick="PaypalImageButton_Click" />    
</asp:Panel>