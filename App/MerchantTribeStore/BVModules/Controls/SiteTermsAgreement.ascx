<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_SiteTermsAgreement" Codebehind="SiteTermsAgreement.ascx.cs" %>
<asp:CheckBox ID="AgreeToTermsCheckBox" runat="server" /><asp:Literal ID="AgreeLiteral" runat="server"></asp:Literal>
<div><asp:HyperLink ID="ShippingHyperLink" CssClass="viewSiteTerms" runat="server" EnableViewState="false">View <asp:Literal ID="ViewSiteTermsLiteral" runat="server">Terms And Conditions</asp:Literal></asp:HyperLink></div>
