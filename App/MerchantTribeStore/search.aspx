<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.Search" Title="Search" Codebehind="Search.aspx.cs" %>
<%@ Register Src="~/BVModules/Controls/Pager.ascx" TagName="Pager" TagPrefix="uc3" %>
<%@ Register Src="BVModules/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<%@ Import Namespace="MerchantTribe.Commerce" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Search</h1>
    <uc2:MessageBox ID="MessageBox1" runat="server" />
    <div class="searchcontrols">
    <asp:TextBox ID="q" ClientIDMode="Static" runat="server" EnableViewState="false"></asp:TextBox>
        <asp:ImageButton ID="btnGo" runat="server" AlternateText="Search" 
             onclick="btnGo_Click" />
    </div>    
    <div class="searchresults">
        <uc3:Pager ID="Pager1" runat="server" />
        <asp:Literal id="litSearchResults" runat="server" EnableViewState="false"></asp:Literal>                                
        <uc3:Pager ID="Pager2" runat="server" /><div class="clear"></div>
    </div>  
    &nbsp;  
</asp:Content>
