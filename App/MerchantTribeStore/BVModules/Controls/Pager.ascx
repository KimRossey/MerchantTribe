<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_Pager" Codebehind="Pager.ascx.cs" %>
<div class="pager">
    <ul>
        <li id="FirstListItem" runat="server"></li>
        <li id="PreviousListItem" runat="server"></li>        
        <asp:PlaceHolder ID="PagesPlaceHolder" runat="server"></asp:PlaceHolder>
        <li id="NextListItem" runat="server"></li>
        <li id="LastListItem" runat="server"></li>
    </ul>
</div>