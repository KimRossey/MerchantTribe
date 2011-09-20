<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_AddToWishlist" Codebehind="AddToWishlist.ascx.cs" %>
<div id="wishlist">
    <asp:ImageButton ID="AddToWishlist" runat="server" 
        ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/AddToWishlist.png" 
        AlternateText="Add To Wishlist" onclick="AddToWishlist_Click" />
</div>
