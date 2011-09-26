<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.checkout" Codebehind="checkout.aspx.cs" %>

<%@ Register Src="~/BVModules/Controls/SiteTermsAgreement.ascx" TagName="SiteTermsAgreement" TagPrefix="uc9" %>
<%@ Register Src="~/BVModules/Controls/EmailAddressEntry.ascx" TagName="EmailAddressEntry" TagPrefix="uc2" %>
<%@ Register Src="~/BVModules/Controls/Payment.ascx" TagName="Payment" TagPrefix="uc8" %>
<%@ Register Src="~/BVModules/Controls/Shipping.ascx" TagName="Shipping" TagPrefix="uc7" %>
<%@ Register Src="~/BVModules/Controls/PaypalExpressCheckoutButton.ascx" TagName="PaypalExpressCheckoutButton" TagPrefix="uc3" %>
<%@ Register Src="BVModules/Controls/AddressBilling.ascx" TagName="AddressBilling" TagPrefix="uc4" %>
<%@ Register Src="BVModules/Controls/AddressShipping.ascx" TagName="AddressShipping" TagPrefix="uc6" %>
<%@ Register Src="BVModules/Controls/ContentColumnControl.ascx" TagName="ContentColumnControl" TagPrefix="uc11" %>
<%@ Register src="BVModules/Controls/ViewOrderItems.ascx" tagname="ViewOrderItems" tagprefix="uc12" %>


<%@ Register src="BVModules/Controls/PaymentRewardsPoints.ascx" tagname="PaymentRewardsPoints" tagprefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<div class="checkout">
    <asp:Literal ID="litValidationSummary" EnableViewState="false" runat="server" ClientIDMode="Static" /></asp:Literal>
    <h1>Checkout</h1>
    <div class="section-sidebar">
        <uc11:ContentColumnControl ID="ContentColumnControl1" runat="server" ColumnID="601" />
    </div>
    <div class="sections">
        <div class="section-email">
            <h2>Email Address</h2>
            <asp:TextBox ID="customeremail" ClientIDMode="Static" runat="server" TabIndex="100"></asp:TextBox><span class="requiredfield">*</span><br />
            <div id="loginform">
                <asp:Panel ID="pnlLoggedIn" runat="server" Visible="false">
                    You are logged into your account. Thank You.
                </asp:Panel>
                <asp:Panel ID="pnlNotLoggedIn" runat="server" Visible="true">
                    It looks like you have an account already. Please login:<br />
                    LOGIN CONTROL HERE
                </asp:Panel>
            </div>&nbsp;
        </div>    
        <div class="section-shipping">
            <h2>Shipping Address</h2>
                <uc6:AddressShipping ID="AddressShipping1" runat="server" />
                <asp:HiddenField runat="server" ID="TempShippingRegion" ClientIDMode="Static" />                
        </div>
        <div class="section-delivery">
            <h2>Delivery Options</h2>
            <uc7:Shipping ID="Shipping" runat="server" TabIndex="300" />                        
        </div>
        <div class="section-billing">
            <h2>Billing Address</h2>
            <asp:CheckBox ID="chkBillSame" runat="server" ClientIDMode="Static" Checked="true" TabIndex="400" /> My billing address is the same as my shipping address
            <div id="billingwrapper">
                <uc4:AddressBilling ID="AddressBilling1" runat="server" />        
                <asp:HiddenField runat="server" ID="TempBillingRegion" ClientIDMode="Static" />                
            </div>
        </div>
        <asp:Panel ID="pnlRewardsPoints" Visible="false" runat="server">
            <div class="section-payment">
                <h2><asp:Label ID="lblRewardsPointsName" runat="server">Rewards Points</asp:Label></h2>
                <uc5:PaymentRewardsPoints ID="PaymentRewardsPoints1" runat="server" />
            </div>
        </asp:Panel>        
        <div class="section-payment">
            <h2>Payment Method</h2>
            <uc8:Payment ID="Payment" runat="server" TabIndex="500" />
        </div>
        <div class="section-extras">
            <h2>Special Instructions</h2>
            <asp:TextBox ID="SpecialInstructions" TextMode="MultiLine" runat="server" Columns="80"
                    Rows="4" Wrap="true" TabIndex="600"></asp:TextBox>    
        </div>
        <div class="section-actions">
            <uc9:SiteTermsAgreement ID="SiteTermsAgreement1" runat="server" />
            <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="Place Order" ImageUrl="~/BVModules/Themes/Bvc5/Images/Buttons/PlaceOrder.png"
                    TabIndex="3001" onclick="btnSubmit_Click" />
        </div>
        <div class="section-totals">
            <h2>Order Summary</h2>    
            <div id="totalsastable"><asp:Literal ID="litTotals" runat="server"></asp:Literal></div>
        </div>
        <div class="section-cart">    
            <uc12:ViewOrderItems DisableReturns="true" ID="ViewOrderItems1" runat="server" />    
        </div>
    </div>
    <div class="clear">&nbsp;</div>
</div><asp:HiddenField ID="orderbvin" ClientIDMode="Static" runat="server" />
<div class="modal">
    <div class="popoverframe">
    <iframe id="popoverpage"></iframe><br />
    <a id="dialogclose" href="#">Close</a>
    </div>    
</div>

</asp:Content>
