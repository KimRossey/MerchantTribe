<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="BVCommerce.BVModules_Controls_ProductReviewDisplay" Codebehind="ProductReviewDisplay.ascx.cs" %>
<asp:Panel ID="pnlReviewDisplay" EnableViewState="False" runat="server" CssClass="ProductReviews">
    <div class="reviews">
    <div class="reviewlist"><div class="ProductReviewRating">
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
    <div class="ProductReviewLinks">
        <asp:HyperLink ID="lnkAllReviews" runat="server">View All Reviews</asp:HyperLink><br />        
    </div></div>
    <div class="reviewform">
        <asp:Literal id="litReviewMessage" runat="server"></asp:Literal>
        <asp:Panel ID="pnlNewReview" runat="server">
            <h3>Write a Review for this Product</h3>
            <table class="formtable">
            <tr>
                            <td class="formlabel" valign="top" align="right">
                                Rating</td>
                            <td>
                                <asp:DropDownList ID="lstNewReviewRating" runat="server">
                                    <asp:ListItem Value="5" Selected="True">5 Stars</asp:ListItem>
                                    <asp:ListItem Value="4">4 Stars</asp:ListItem>
                                    <asp:ListItem Value="3">3 Stars</asp:ListItem>
                                    <asp:ListItem Value="2">2 Stars</asp:ListItem>
                                    <asp:ListItem Value="1">1 Star</asp:ListItem>
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="formlabel" valign="top" align="right">
                                Your Review</td>
                            <td>
                                <asp:TextBox ID="NewReviewDescription" runat="server" Columns="30" Rows="6" TextMode="MultiLine"
                                    MaxLength="6000" CssClass="FormInput"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="formlabel">
                                &nbsp;</td>
                            <td>
                                <asp:ImageButton ID="btnSubmitReview" runat="server" AlternateText="Submit" onclick="btnSubmitReview_Click"></asp:ImageButton></td>
                        </tr>
            </table>
        </asp:Panel>
    </div>
    <div class="clear"></div>
</div>
    <asp:HiddenField ID="bvinField" runat="server" />
</asp:Panel>
