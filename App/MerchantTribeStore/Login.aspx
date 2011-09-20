<%@ Page MasterPageFile="~/Site.master" Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.login2" Codebehind="Login.aspx.cs" %>

<%@ Register Src="BVModules/Controls/ManualBreadCrumbTrail.ascx" TagName="ManualBreadCrumbTrail"
    TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="LoginControl" Src="BvModules/Controls/LoginControl.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewUserControl" Src="BvModules/Controls/NewUserControl.ascx" %>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
<div class="loginpage">
    <uc2:ManualBreadCrumbTrail ID="ManualBreadCrumbTrail1" runat="server" />
    <h1>
        <span>
            <asp:Label ID="TitleLabel" runat="server">Login</asp:Label></span></h1>
    <div class="sidebysidea"><h2>Current User Login</h2>
    <uc1:LoginControl ID="LoginControl1" runat="server"></uc1:LoginControl>
                        <asp:HyperLink ID="ContactUsHyperLink" CssClass="PrivateNewAccount" runat="server" NavigateUrl="~/ContactUs.aspx">Contact Us</asp:HyperLink>
                        </div>
    <div class="sidebysideb"><asp:Panel ID="pnlNewUser" runat="server"><h2>New Users Create Account</h2>
    <uc1:NewUserControl ID="NewUserControl1" runat="server" LoginAfterCreate="true"></uc1:NewUserControl>
    </asp:Panel>
    </div>
    <div class="clear"></div>        
</div>
</asp:Content>
