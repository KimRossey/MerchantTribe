<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="categorydrilldown.aspx.cs" Inherits="MerchantTribeStore.categorydrilldown" %>
<%@ Register Src="BVModules/Controls/CategorySortOrder.ascx" TagName="CategorySortOrder"
    TagPrefix="uc4" %>
<%@ Register Src="BVModules/Controls/Pager.ascx" TagName="Pager" TagPrefix="uc3" %>
<%@ Register Src="BVModules/Controls/CategoryBreadCrumbTrail.ascx" TagName="CategoryBreadCrumbTrail"
    TagPrefix="uc2" %>
<%@ Register Src="BVModules/Controls/ContentColumnControl.ascx" TagName="ContentColumnControl"
    TagPrefix="uc1" %>
    
    <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc2:CategoryBreadCrumbTrail ID="CategoryBreadCrumbTrail1" runat="server" />
    <div id="categoryleft">
        <asp:Literal ID="litFilters" runat="server" EnableViewState="false" />
    </div>
    <div id="categorymain">
        <asp:Label ID="lblTitle" runat="Server"></asp:Label>
        <div id="categorydescription">
            <asp:Literal ID="DescriptionLiteral" runat="server"></asp:Literal>
        </div>        
        <div id="categorygridtemplate">
            <div id="categorygridtemplaterecords">
                <uc3:Pager ID="Pager1" runat="server" />
                <asp:Literal id="categoryitems" ClientIDMode="Static" runat="server"></asp:Literal>                                
                <uc3:Pager ID="Pager2" runat="server" />
            </div>
        </div>
    </div>
    <div class="clear"></div>
</asp:Content>

    
