<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_CategorySortOrder" Codebehind="CategorySortOrder.ascx.cs" %>
<div class="categorysortorder">
    <asp:Label ID="SortOrderLabel" runat="server" Text="Sort Order" AssociatedControlID="SortOrderDropDownList"></asp:Label>
    <asp:DropDownList ID="SortOrderDropDownList" runat="server" AutoPostBack="true">            
    </asp:DropDownList>
</div>
