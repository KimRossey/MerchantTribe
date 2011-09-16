<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="BVCommerce.CheckoutPayPalExpress" Codebehind="CheckoutPayPalExpress.aspx.cs" %>

<%@ Register src="BVModules/Controls/SiteTermsAgreement.ascx" tagname="SiteTermsAgreement" tagprefix="uc1" %>
<%@ Register src="BVModules/Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<%@ Register Src="BVModules/Controls/ContentColumnControl.ascx" TagName="ContentColumnControl" TagPrefix="uc11" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="checkout" id="paypalexpresscheckout">
        <uc2:MessageBox ID="MessageBox1" runat="server" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errormessage" />    



        <h1>PayPal Express Checkout</h1>
        <div class="section-sidebar">
            <uc11:ContentColumnControl ID="ContentColumnControl1" runat="server" ColumnID="601" />
        </div>

        <div class="sections">
        



       <asp:Panel ID="FixedInfoPanel" runat="server">
        <div class="section-shipping">
        <h2>
            Ship To:</h2>
        <table width="300" cellpadding="4" cellspacing="0">
            <tr>
                <td class="formlabel">
                    Country:
                </td>
                <td class="formfield">
                    <asp:Label ID="CountryLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    First, MI:&nbsp;
                </td>
                <td class="formfield">
                    <asp:Label ID="FirstNameLabel" runat="server" Text=""></asp:Label>
                    <asp:Label ID="MiddleInitialLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Last:
                </td>
                <td class="formfield">
                    <asp:Label ID="LastNameLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="Tr1" runat="server">
                <td class="formlabel">
                    Company:
                </td>
                <td class="formfield">
                    <asp:Label ID="CompanyLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    Address:
                </td>
                <td class="formfield">
                    <asp:Label ID="StreetAddress1Label" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    &nbsp;
                </td>
                <td class="formfield">
                    <asp:Label ID="StreetAddress2Label" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    City:
                </td>
                <td class="formfield">
                    <asp:Label ID="CityLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="formlabel">
                    State, Zip:&nbsp;
                </td>
                <td class="formfield">
                    <asp:Label ID="StateLabel" runat="server" Text=""></asp:Label>
                    <asp:Label ID="ZipLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="Tr4" runat="server">
                <td class="formlabel">
                    E-mail:
                </td>
                <td class="formfield">
                    <asp:Label ID="EmailLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="Tr2" runat="server">
                <td class="formlabel">
                    Phone:
                </td>
                <td class="formfield">
                    <asp:Label ID="PhoneNumberLabel" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="Tr3" runat="server">
                <td class="formlabel">
                    Paypal Address Status:
                </td>
                <td class="formfield">
                    <asp:Label ID="AddressStatusLabel" runat="server" Text=""></asp:Label>
                    <bvc5:BVCustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Paypal Address Must Be Confirmed."
                        ForeColor=" " CssClass="errormessage" OnServerValidate="CustomValidator1_ServerValidate">*</bvc5:BVCustomValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="EditAddressLinkButton" runat="server" CausesValidation="False"
                        OnClick="EditAddressLinkButton_Click">Edit Address</asp:LinkButton>
                </td>
            </tr>
        </table>
        </div>
        <div class="section-delivery">
        <h2>
            Shipping</h2>        
        <asp:RadioButtonList ID="ShippingRatesList" runat="server">
        </asp:RadioButtonList>
        <bvc5:BVCustomValidator ID="BVRequiredFieldValidator1" runat="server" Display="Dynamic"
            ErrorMessage="A Shipping Method Must Be Selected." ForeColor=" " CssClass="errormessage"
            OnServerValidate="BVRequiredFieldValidator1_ServerValidate">*</bvc5:BVCustomValidator>
        <asp:Label ID="ShippingMessage" runat="server" Text=""></asp:Label>
        </div>
        </asp:Panel>
    <div class="section-actions">
        <uc1:SiteTermsAgreement ID="SiteTermsAgreement1" runat="server" />
        <asp:ImageButton ID="CheckoutImageButton" runat="server" OnClick="CheckoutImageButton_Click" />
    </div>
    </div>
    <div class="clear">&nbsp;</div>
    <asp:ImageButton ID="btnKeepShopping" runat="server" AlternateText="Keep Shopping"
        CausesValidation="False" ImageUrl=""  OnClick="btnKeepShopping_Click" />
    
    
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="EndOfForm" Runat="Server">
</asp:Content>

