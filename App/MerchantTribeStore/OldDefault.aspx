<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OldDefault.aspx.cs" Inherits="MerchantTribeStore.Default" %>

<%@ Register Src="BVModules/Controls/ContentColumnControl.ascx" TagName="ContentColumnControl"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="homepage">
        <div id="homepagecolumn1">
            <uc1:ContentColumnControl ID="ContentColumnControl1" runat="server" ColumnID="1"/>
        </div>
        <div id="homepagecolumn2">
            <uc1:ContentColumnControl ID="ContentColumnControl2" runat="server" ColumnID="2" />
        </div>
        <div id="homepagecolumn3">
            <uc1:ContentColumnControl ID="ContentColumnControl3" runat="server" ColumnID="3" />
        </div><div class="clear"></div>  
</div>  
</asp:Content>
