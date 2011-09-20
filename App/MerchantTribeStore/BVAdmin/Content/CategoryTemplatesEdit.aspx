<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_CategoryTemplatesEdit" title="Untitled Page" Codebehind="CategoryTemplatesEdit.aspx.cs" %>

<%@ Register Assembly="MerchantTribe.Commerce" Namespace="MerchantTribe.Commerce" TagPrefix="cc1" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Category Templates Edit</h1>    
    <uc1:MessageBox ID="MessageBox1" runat="server" />
    <asp:Panel ID="CategoryEditorPanel" runat="server">
                                               
    </asp:Panel>                
</asp:Content>


