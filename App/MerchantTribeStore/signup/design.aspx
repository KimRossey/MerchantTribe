<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_design" Codebehind="design.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
    <div class="superh1">
    <h1>Creating Your Theme</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="column-main">
        <div class="block">
            <h1 class="c">You’re Only Limited by Your Imagination</h1>
            <h2 class="c">Pick a Theme. Build a Theme. Buy a Theme.</h2>
        
<div class="tour-design-2">
    <div class="premium">
        <a href="/signup/premiumthemes">
        <img src="/content/images/system/TourDesign2.png" alt="Buy Premium Themes for BV Commerce" border="0" /></a>
    </div>
    <div class="feature">
        <h3>Choose from FREE Themes</h3>
        <p>BV Commerce includes lots of free online store themes that you can easily customize.<br />
        <a href="premiumthemes.aspx" title="BV Commerce Themes">See Some Example Themes</a></p>
        
    </div>
</div>

<div class="tour-design-3">
    
        <div class="feature">
        <h3>Create Your Own Themes!</h3>
        <p>You can create a unique online store theme in a few hours with basic knowledge of Photoshop and a little HTML. Not only is it fast, it’s fun. <a href="http://help.bvcommerce.com/pages/creating-themes-a-basic-guide">Learn How</a></p>        

        <h3>Buy a Theme</h3>
        <p>We can make you a totally unique online store theme or create a low-cost semi-customized theme based on one of our included layouts. Call us at 1-804-282-4455 to find out more.</p>        
        </div>        
</div>

            <div class="clear"></div>
        </div>
        
        <div class="block">
            <div class="pager">
                <div class="last"><a href="/signup/tour">&laquo; Setting Up Your Store</a></div>
                <div class="next"><a href="/signup/sell">Sell Your Products &raquo;</a></div>
            </div>
        </div>
        
        <div class="block">
            <div class="col-3sub-a">
                <h3>Need a Custom Theme?</h3>
                <p>Want a custom theme? Call 1-804-282-4455 or e-mail us and we’ll create an original theme for your store!</p>
            </div>                            

            <div class="col-3sub-b">
                <h3>See Our Theme Tutorial</h3>
                <p>See our quick Create a <a href="http://help.bvcommerce.com/pages/creating-themes-a-basic-guide">Theme Tutorial</a> to learn how to create themes for BV Commerce Hosted.</p>
            </div>
            
            <div class="col-3sub-c">
                <h3>Browse Example Themes</h3>
                <p>It's easy to create your own theme or modify one of our <a href="/signup/premiumthemes" title="Free Shopping Cart Themes">FREE Shopping Cart Themes</a> or call 1-804-282-4455. </p>
            </div>        
            <div class="clear"></div>            
            <p>*There is a 3-5 five day processing period for semi-custom themes, but you can start selling immediately and simply update your theme later with the click of a button. </p>
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

