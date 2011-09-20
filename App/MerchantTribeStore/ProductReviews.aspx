<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.ProductReviews" Codebehind="ProductReviews.aspx.cs" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Literal ID="litProduct" runat="server" EnableViewState="false"></asp:Literal>        
    <h1>Product Reviews: <asp:Literal ID="litProductName" runat="server" EnableViewState="false" /></h1>
    <div class="ProductReviewRating">
        <asp:Label ID="lblRating" runat="server">Average Rating</asp:Label>
        <asp:Image ID="imgAverageRating" runat="server" ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/Stars3.png">
        </asp:Image></div>
    <asp:DataList ID="dlReviews" runat="server" DataKeyField="Bvin" 
        ondeletecommand="dlReviews_DeleteCommand" oneditcommand="dlReviews_EditCommand" 
        onitemdatabound="dlReviews_ItemDataBound">
        <ItemTemplate>
            <div class="ProductReview">
                <asp:Image ID="imgReviewRating" runat="server" ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/Stars3.png" /><br />
                <span class="productreviewdescription">
                    <asp:Label ID="lblReviewDescription" runat="server">Review...</asp:Label></span>
                <asp:Panel ID="pnlKarma" runat="server">
                    <span class="ProductReviewKarma">
                        <asp:Label ID="lblProductReviewKarma" runat="server">Was this review helpful?</asp:Label> 
                        <asp:ImageButton CommandName="Edit" CausesValidation="False" ID="btnReviewKarmaYes"
                            ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/Yes.png" AlternateText="Yes"
                            runat="server"></asp:ImageButton> 
                        <asp:ImageButton CommandName="Delete" CausesValidation="False" ID="btnReviewKarmaNo"
                            ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/No.png" AlternateText="No" runat="server">
                        </asp:ImageButton></span>
                </asp:Panel>
            </div>
        </ItemTemplate>
    </asp:DataList>
    <div class="clear"></div>
</asp:Content>

