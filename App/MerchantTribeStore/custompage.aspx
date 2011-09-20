<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.custompage" Codebehind="custompage.aspx.cs" %>
<%@ Register Src="BVModules/Controls/CategoryBreadCrumbTrail.ascx" TagName="CategoryBreadCrumbTrail" TagPrefix="uc2" %>
<%@ Register Src="BVModules/Controls/ContentColumnControl.ascx" TagName="ContentColumnControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
  
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <uc2:CategoryBreadCrumbTrail ID="CategoryBreadCrumbTrail1" runat="server" />    
    <asp:Literal ID="litPreLeft" runat="server" EnableViewState="false" />    
    <uc1:ContentColumnControl ID="ContentColumnControl1" runat="server" ColumnID="4" />    
    <asp:Literal ID="litPostLeft" runat="server" EnableViewState="false" />
    <asp:Literal ID="litMain" runat="server" EnableViewState="false" />                   
    <div class="clear"></div>
</asp:Content>    
<asp:Content ID="Content3" ContentPlaceHolderID="EndOfForm" Runat="Server">
</asp:Content>

