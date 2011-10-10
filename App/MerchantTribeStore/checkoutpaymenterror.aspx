<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="checkoutpaymenterror.aspx.cs" Inherits="MerchantTribeStore.checkoutpaymenterror" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<div class="checkout">
    <asp:Literal ID="litValidationSummary" EnableViewState="false" runat="server" ClientIDMode="Static" /></asp:Literal>
    <div class="flash-message-warning">The payment information you provided was not able to be processed. Please update your payment information and try again.</div>
    <h1>Update Payment Information</h1>
   
    <div class="sections">        
        <div class="section-billing">
            <h2>Billing Address</h2>
            BILLING ADDRESS HERE
        </div>
        <div class="section-payment">
            <h2>Payment Method</h2>
            PAYMENT HERE
        </div>
        <div class="section-actions">
            <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="Place Order" ImageUrl="~/BVModules/Themes/Bvc5/Images/Buttons/PlaceOrder.png"
                    TabIndex="3001" onclick="btnSubmit_Click" /><br />
                    &nbsp;<br />
                    <asp:LinkButton ID="lnkCancel" runat="server" onclick="lnkCancel_Click">Cancel Order</asp:LinkButton>
        </div>        
    </div>
    <div class="clear">&nbsp;</div>
</div>

</asp:Content>

