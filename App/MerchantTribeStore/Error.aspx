<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="BVCommerce.ErrorPage" title="Untitled Page" Codebehind="Error.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1><asp:Literal ID="HeaderLiteral" runat="server"></asp:Literal></h1>
    <div class="errorcontent">
        <asp:Literal ID="ErrorContentLiteral" runat="server"></asp:Literal>
    </div>    
</asp:Content>

