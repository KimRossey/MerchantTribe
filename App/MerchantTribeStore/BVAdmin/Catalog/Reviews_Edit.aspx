<%@ Page MasterPageFile="~/BVAdmin/BVAdminNav.Master" ValidateRequest="False" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.Reviews_Edit" Codebehind="Reviews_Edit.aspx.cs" %>

<%@ Register TagPrefix="uc1" TagName="ProductReviewEditor" Src="~/BVAdmin/controls/ProductReviewEditor.ascx" %>
<%@ Register src="../Controls/ProductEditMenu.ascx" tagname="ProductEditMenu" tagprefix="uc5" %>
<%@ Register src="../Controls/ProductEditingDisplay.ascx" tagname="ProductEditing" tagprefix="uc5" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc5:ProductEditMenu ID="ProductEditMenu1" runat="server" />
</asp:Content>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Edit Product Review</h1>
    <uc1:ProductReviewEditor ID="ProductReviewEditor1" runat="server"></uc1:ProductReviewEditor>
</asp:Content>
