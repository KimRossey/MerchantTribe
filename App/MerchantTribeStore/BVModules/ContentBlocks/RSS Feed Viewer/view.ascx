<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_ContentBlocks_RSS_Feed_Viewer_view" Codebehind="view.ascx.cs" %>
<div class="rssfeedviewer">
    <div class="rssfeedviewerwrapper">
        <asp:Panel ID="pnlTitle" runat="server"><h4><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h4></asp:Panel>
        <asp:Panel ID="pnlDescription" runat="server">
            <asp:Label ID="lblDescription" runat="server"></asp:Label></asp:Panel>
        <asp:DataList ID="DataList1" runat="server">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Bind("Link") %>' Text='<%#Bind("Title") %>'></asp:HyperLink><br />
                <asp:Label Visible="false" ID="Label1" runat="server" Text='<%#Bind("Description") %>'></asp:Label>
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
