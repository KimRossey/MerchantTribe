<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.ProductPage" Codebehind="product.aspx.cs" %>
<%@ Register Src="BVModules/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc5" %>
<%@ Register Src="BVModules/Controls/CategoryBreadCrumbTrail.ascx" TagName="CategoryBreadCrumbTrail" TagPrefix="ucc2" %>
<%@ Register Src="BVModules/Controls/ContentColumnControl.ascx" TagName="ContentColumnControl" TagPrefix="ucc1" %>
<%@ Register src="BVModules/Controls/VolumeDiscounts.ascx" tagname="VolumeDiscounts" tagprefix="uc1" %>
<%@ Register src="BVModules/Controls/ProductTypeDisplay.ascx" tagname="ProductTypeDisplay" tagprefix="uc2" %>
<%@ Register src="BVModules/Controls/ProductReviewDisplay.ascx" tagname="ProductReviewDisplay" tagprefix="uc3" %>
<%@ Register src="BVModules/Controls/RelatedItems.ascx" tagname="RelatedItems" tagprefix="uc4" %>

<asp:Content ID="headcontent" ContentPlaceHolderID="HeadContent" runat="server">    
    <script src="<%=TabScriptSource%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">        
    <div id="productpage">
        <ucc2:CategoryBreadCrumbTrail ID="CategoryBreadCrumbTrail1" runat="server" />
        <ucc1:ContentColumnControl ID="PreContentColumn" runat="server" />
        <div class="productpagemain">
        <div class="imagecolumn">            
            <div class="productimage">
                <asp:Image ID="imgMain" runat="server" ClientIDMode="Static" /><input type="hidden" id="imgMainLast" value="" />
            </div>                                                    
            <asp:Literal ID="litAdditionalImages" runat="server" ClientIDMode="Static" EnableViewState="false" />
        </div>
        <div class="actioncolumn">
            <h1><span><asp:Label ID="lblName" runat="server" ></asp:Label></span></h1>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errormessage" />
                <span id="sku"><asp:label ID="lblSku" runat="server"></asp:label></span>
                <span class="stockdisplay"><asp:Literal ID="litStockDisplay" runat="server"></asp:Literal></span>
                <asp:panel ID="ProductControlsPanel" runat="server">
                    <div id="productcontrols">
                        <asp:PlaceHolder id="phOptions" runat="server" />
                        <div id="pricewrapper"><asp:Literal id="litPrices" runat="server"></asp:Literal></div>
                        <label for="quantityfield">Quantity</label>
                        <span class="choice"><asp:TextBox ID="QuantityField" ClientIDMode="static" runat="server" Text="1" CssClass="forminput short"></asp:TextBox></span>                        
                        <div id="localmessage"><asp:Literal ID="litMessage" runat="server" /></div>
                        <div class="buttons">
                            <asp:ImageButton id="btnAddToCart" ClientIDMode="static" runat="server" 
                                onclick="btnAddToCart_Click" />                            
                        </div>
                        <uc1:VolumeDiscounts ID="VolumeDiscounts1" runat="server" />
                        <div class="clear"></div>
                     </div>
                </asp:panel>                
                <asp:label ID="lblDescription" runat="server"></asp:label>                                            
                <uc2:ProductTypeDisplay ID="ProductTypeDisplay1" runat="server" />                
        </div>
            <div class="clear">&nbsp;</div>
        </div>
        <div class="informationcolumn">            
            <ul class="tabnavigation">
                <li runat="server" id="TabNavReviews"><a href="#tabreviews">Reviews</a></li>
                <li runat="server" id="TabNavSuggested"><a href="#tabsuggesteditems"><asp:Literal ID="litRelatedItemsTitle2" runat="server" EnableViewState="false"></asp:Literal></a></li>
                <asp:Literal ID="litOtherTabsNav" runat="server"></asp:Literal>
            </ul>
            <div class="tabs">
                <asp:Panel ID="pnlReviews" runat="server">
                    <div id="tabreviews">
                        <div class="padded"><h2>Product Reviews</h2>
                        <uc3:ProductReviewDisplay ID="ProductReviewDisplay1" runat="server" />
                        </div>
                    </div>    
                </asp:Panel>            
                <asp:Panel ID="pnlSuggested" runat="server">
                    <div id="tabsuggesteditems">                
                        <div class="padded"><h2><asp:Literal ID="litRelatedItemsTitle" runat="server" EnableViewState="false"></asp:Literal></h2>
                        <uc4:RelatedItems ID="RelatedItems1" MaxItemsToShow="3" IncludeAutoSuggestions="true" runat="server" />                        
                        </div>
                    </div>                
                </asp:Panel>
                <asp:Literal ID="litOtherTabs" runat="server"></asp:Literal>
            </div>
            
        </div>
        <ucc1:ContentColumnControl ID="PostContentColumn" runat="server" />
    </div><asp:HiddenField ID="productbvin" runat="server" ClientIDMode="Static" />  
    <div class="modal2">
        <div class="popoverframe2">
            <a id="dialogclose" href="#">Close</a><br />
            <iframe id="popoverpage2"></iframe><br />
            <a id="dialogclose2" href="#">Close</a>
        </div>    
    </div>                  
</asp:Content>
