<%@ Control EnableViewState="false" Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_AdminPanel" Codebehind="AdminPanel.ascx.cs" %>
<asp:Panel EnableViewState="false" ID="pnlMain" runat="server" Visible="false" Width="100%">
    <div id="adminpanel" style="background: #ccc url('<%=Page.ResolveUrl("~/images/system/AdminPanelBg.png")%>') repeat-x;height:30px;padding:0 20px;color:#333;text-align:left;">                               
        <asp:Literal ID="litAdminLink" runat="server" EnableViewState="false"></asp:Literal>                        
    </div>
</asp:Panel>