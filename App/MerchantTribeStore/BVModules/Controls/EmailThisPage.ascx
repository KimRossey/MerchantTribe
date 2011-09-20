<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_EmailThisPage" Codebehind="EmailThisPage.ascx.cs" %>

<div id="EmailThisPage">
    <a id="EmailLink" onclick="JavaScript:void(0)" runat="server">
    <asp:Image 
    Style="cursor: hand" 
    ID="imgEmail" 
    ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/EmailThisPage.png" 
    runat="server"
    AlternateText="Email This Page">
    </asp:Image>
    </a>
</div>
