<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_about" Codebehind="about.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
    <div class="superh1">
    <h1>About BV Software</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="column-main">
        <div class="block">
            <h1 class="c">About BV Software</h1>
            <h2 class="c">Powerful Online Stores that are Easy to Use</h2>
            &nbsp;<br />
            <img src="/content/images/system/RichmondMap.jpg" style="float:right;border:solid 1px #666;margin:0 0 20px 20px;" />            
            <p>
                BV Commerce Hosted is a new hosted shopping cart solution from BV Software, makers of BV Software Toolkit, a robust shopping cart platform with over 1,000 features. Five versions of BV Software's award winning ecommerce software have been released over the last eight years and it continues to win awards and strong praise from small businesses. BV Commerce is used by large and small businesses including: <em>Pebble Beach Resorts</em>, <em>Chesapeake Energy (CHK)</em> and <em>Susan G. Komen for the Cure.</em></p>
            <p>
                BV Commerce Hosted is designed to be both easy to use and powerful. It takes just minutes to set up an online store with BV Commerce. You can choose from our selection of free online store themes, but it’s also really easy to create your own unique theme. Or we can make one for you. And we have tons of features that online merchants need, from unlimited products and categories to sales and discounts, the ability to add product variations and more. The best part is that we manage the store for you, so you can concentrate on selling.</p>

            <p>BV Commerce Hosted and BV Commerce Toolkit are the brainchild of Marcus McConnell, an eCommerce specialist with experience building shopping cart solutions for international retailers such as EddieBauer.com, Godiva.com and 1-800-Flowers.com. Marcus created BV Software in 2001 after frustrating experiences working with existing shopping cart software for small businesses. He saw an opportunity to deliver a world-class shopping cart solution to small businesses on the Microsoft platform at a price they could afford.</p>
            
            <p>Learn more about BV Software and see our <a href="http://www.bvsoftware.com">
                company home page</a>. Or call us at 1-804-282-4455 and talk to us directly.</p>            
        </div>
        
         <div class="block">
            <div class="pager">
                <div class="last"><a href="/signup/secure">&laquo; All About Hosting</a></div>
                <div class="next"><a href="/signup/">Pricing &amp; Sign Up &raquo;</a></div>
            </div>
        </div>
        
        
        <div class="block" style="text-align:center;">
            <div class="col-3sub-a">
                <a href="http://blog.bvsoftware.com" target="_blank"><img src="/content/images/system/feed_48x48.png" alt="BV Blog" border="0" /><br />Read Our Blog</a>
            </div>                            

            <div class="col-3sub-b">
                <a href="http://twitter.com/MarcusMcConnell" target="_blank"><img src="/content/images/system/twitter_48x48.png" alt="Follow BV Software on Twitter" border="0" /><br />Follow Us on Twitter</a>
            </div>
            
            <div class="col-3sub-c">
                <a href="http://www.facebook.com/home.php#/pages/BV-Software-Shopping-Carts-and-Ecommerce-Solutions/129473117862?ref=nf" target="_blank"><img src="/content/images/system/facebook_48x48.png" alt="facebook" border="0" /><br />Become Our Facebook Friend</a>
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

