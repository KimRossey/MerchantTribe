<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_Search" Codebehind="Search.ascx.cs" %>
<div class="searchboxform">
    <h4><span><asp:Label ID="lblTitle" runat="server" Text="Search" AssociatedControlID="KeywordField"></asp:Label></span></h4>    
    <asp:Panel runat="server" DefaultButton="btnSearch" ID="pnlSearchBox">
        <span class="searchspan">
            <asp:TextBox ID="KeywordField" Columns="15" runat="server" CssClass="forminput"></asp:TextBox>
            <asp:ImageButton CausesValidation="false" CssClass="searchbutton" 
            ID="btnSearch" runat="server" ImageUrl="~/BVAdmin/images/buttons/go.png" 
            onclick="btnSearch_Click" />
        </span>
    </asp:Panel>
</div>