<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_tour" Codebehind="tour.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
<div class="superh1">
    <h1>Setting Up Your Store</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="column-main">
        <div class="block">            
            <h1 class="c">Finish Your To Do List in Just 5 Steps</h1>
<a href="/SignUp"><img src="/content/images/system/tour1.jpg" 
                            alt="Just 5 Steps to Create Your Store" 
                            border="0" width="680" height="335" /></a>
</div>
<div class="block">
    <div class="pager">
        <div class="last"><a href="/">&laquo; Home</a></div>
        <div class="next"><a href="/signup/design">Creating Your Theme &raquo;</a></div>
    </div>
</div>
<div class="block">
<div class="col-3sub-a">
<h3>Master of Your Domain</h3>
<p>Use your own domain name or use our provided bvcommerce.com domain name.<br />
<a href="/signup/features" title="BV Commerce Hosted Shopping Cart Features">Feature List &raquo;</a>
</p>
</div>                            

<div class="col-3sub-b">
<h3>Always Up-To-Date</h3>
<p>Whenever new features and upgrades are added to the software, they will be automatically added to your store.<br />
<a href="/signup/features" title="BV Commerce Hosted Shopping Cart Features">Feature List &raquo;</a>
</p>
</div>
<div class="col-3sub-c">
<h3>Search Engine Love</h3>
<p>Get to the top of Google with built-in SEO—including meta tags, page titles, heading tags and SEO friendly URLs. <br />
<a href="/signup/features" title="BV Commerce Hosted Shopping Cart Features">Feature List &raquo;</a></p>
</div>
  <div class="clear"></div>
        </div>
    </div>
    <div class="column-side">
        <uc1:SignUpMenu ID="SignUpMenu1" runat="server" />        
    </div>
    <div class="clear">
        &nbsp;</div>


</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfForm" Runat="Server">
</asp:Content>

