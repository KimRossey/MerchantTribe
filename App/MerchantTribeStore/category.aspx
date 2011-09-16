<%@ Page Language="C#" AutoEventWireup="True" 
Inherits="BVCommerce.category" 
MasterPageFile="~/Site.master" Codebehind="Category.aspx.cs" %>
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
        <uc1:ContentColumnControl ID="ContentColumnControl1" runat="server" ColumnID="4" />
    </div>
    <div id="categorymain">
        <asp:Label ID="lblTitle" runat="Server"></asp:Label>
        <uc1:ContentColumnControl ID="PreContentColumn" runat="server" />
        <div id="categorybanner">
            <asp:Image runat="server" ID="BannerImage" /></div>
        <div id="categorydescription">
            <asp:Literal ID="DescriptionLiteral" runat="server"></asp:Literal>
        </div>
        <div id="categorygridsubtemplate">
                <asp:DataList ID="DataList2" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                    DataKeyField="bvin" onitemdatabound="DataList2_ItemDataBound"> 
                    <ItemStyle VerticalAlign="top" />
                    <ItemTemplate>
                        <div class="record">
                            <div class="recordimage">
                                <a id="recordimageanchor" runat="server" href=""><img id="recordimageimg" runat="server" src="" border="0" alt="" /></a>
                            </div>
                            <div class="recordname">
                                <a id="recordnameanchor" runat="server" href=""></a>
                            </div>
                            <div class="recordChildren">
                                <asp:Literal runat="server" ID="litRecord" EnableViewState="false"></asp:Literal>
                            </div>                            
                        </div>
                    </ItemTemplate>
                    <AlternatingItemStyle CssClass="alt" />
                </asp:DataList>
        </div>
        <div id="categorygridtemplate">
            <asp:Label ID="lblResults" runat="server"></asp:Label> <uc4:CategorySortOrder ID="CategorySortOrder1" runat="server" />                        
            <div id="categorygridtemplaterecords">
                <uc3:Pager ID="Pager1" runat="server" />
                <asp:Literal id="categoryitems" ClientIDMode="Static" runat="server"></asp:Literal>                                
                <uc3:Pager ID="Pager2" runat="server" />
            </div>
        </div>
        <uc1:ContentColumnControl ID="PostContentColumn" runat="server" />
    </div>
    <div class="clear"></div>
</asp:Content>

    
