<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVModules_Controls_RelatedItems" Codebehind="RelatedItems.ascx.cs" %>
<asp:Panel ID="pnlMain" runat="server" Visible="True">
    <div class="relateditems">        
        <asp:Literal ID="litRelatedItems" runat="server" EnableViewState="false" />
    </div><asp:HiddenField ID="bvinField" runat="server" /><asp:HiddenField ID="MaxItemsToShowField" runat="server" Value="3" />
    <asp:HiddenField ID="IncludeAutoSuggestionsField" runat="server" Value="false" />
</asp:Panel>
