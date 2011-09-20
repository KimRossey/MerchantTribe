<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVModules_Controls_PrintThisPage" Codebehind="PrintThisPage.ascx.cs" %>
<div id="printthispage">
    <a onclick="JavaScript:if (window.print) {window.print();} else { alert('Please choose the print button from your browser.  Usually in the menu dropdowns at File: Print'); } "
        href="#">
        <asp:Image AlternateText="Print this page." Style="cursor: hand" ID="imgPrint" runat="server" ImageUrl="~/BVModules/Themes/Bvc5/images/buttons/PrintThisPage.png">
        </asp:Image>
    </a>
</div>
