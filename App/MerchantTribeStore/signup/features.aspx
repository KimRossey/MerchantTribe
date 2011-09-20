<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="MerchantTribeStore.signup_features" Codebehind="features.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
    <div class="superh1">
    <h1>BV Commerce Hosted Features</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="column-main">
        <div class="block">
            <h1 class="c">BV Commerce Hosted Features</h1>
            <h2 class="c">...and more are on their way</h2>
            
            <asp:Literal ID="litFeatures" runat="server" EnableViewState="false"></asp:Literal>

        </div>
    </div>
    <div class="column-side">
        <uc1:SignUpMenu ID="SignUpMenu1" runat="server" />
    </div>
    <div class="clear">
        &nbsp;</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfForm" Runat="Server">
</asp:Content>

