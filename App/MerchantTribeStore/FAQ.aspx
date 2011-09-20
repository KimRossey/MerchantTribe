<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.FAQ" Title="Frequently Asked Questions" Codebehind="FAQ.aspx.cs" %>

<%@ Register Src="BVModules/Controls/ManualBreadCrumbTrail.ascx" TagName="ManualBreadCrumbTrail"
    TagPrefix="uc2" %>

<%@ Register Src="BVModules/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <uc2:ManualBreadCrumbTrail ID="ManualBreadCrumbTrail1" runat="server" />
    <h1 id="top">
        <asp:Label ID="TitleLabel" runat="server">Frequently Asked Questions</asp:Label>
    </h1>
    <uc1:MessageBox ID="msg" runat="server" Visible="false" />
    <asp:Repeater ID="dlQuestions" runat="server">
        <HeaderTemplate>
            <ol id="faquestions">
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href='<%# "#" + Eval("bvin").ToString() %>'>
                <%# Eval("name") %>
            </a></li>
        </ItemTemplate>
        <FooterTemplate>
            </ol>
        </FooterTemplate>
    </asp:Repeater>
    <h2>
        Answers...</h2>
    <asp:Repeater ID="dlPolicy" runat="server">
        <HeaderTemplate>
            <ol id="faanswers">
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <h3 id="<%# Eval("bvin") %>">
                    <%# Eval("name") %>
                </h3>
                <%#Eval("description")%>
                <p>
                    <small><a id="backToTop" href="#top">Back to Top</a></small></p>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ol>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
