<%@ Control EnableViewState="false" Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_ContentBlocks_Last_Products_Viewed_view" Codebehind="view.ascx.cs" %>
<div id="ProductGrid" runat="server" class="productgrid">
    <div class="decoratedblock">
        <div class="blockcontent">
            <h4><asp:Label EnableViewState="false" ID="LPVTitle" runat="server"></asp:Label></h4>            
            <asp:Literal ID="litItems" runat="server" EnableViewState="false" />
        </div>
    </div>
</div>