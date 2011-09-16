﻿<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_promote" Codebehind="promote.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
<div class="superh1">
    <h1>Tracking and Promoting</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="column-main">
        <div class="block">
            <h1 class="c">Built-In Tools to Advertise Your Online Store</h1>
            <h2 class="c">And see how it's performing</h2>
            &nbsp;<br />
            &nbsp;<br />    
<div class="feature">
<p>BV Commerce includes several powerful tools that can help you track and test how your web store is performing. And our search engine friendly web pages help with organic search rankings.</p>    
</div>
<div class="tour-tracking-1">
    <div class="feature">
    <h3>Track Customers and Sales With Google Analytics</h3>            
            <p>Use built in Google Analytics to track what people are doing on your website. Analytics can tell you:</p>
            <ul class="showbullets">
<li>What country your customers are coming from</li>
<li>How they found you</li>
<li>What search terms they used to find you</li>
<li>What URLs are referring them to your store</li>
<li>The number of visits your store is receiving by page hits and unique visitors</li>
</ul>
    </div>
</div>                        
<div class="tour-tracking-2">
    <div class="feature">
        <h3>Track Online Advertising Campaigns</h3>
            <p>Google Adwords Tracking and Yahoo Sales Tracking help you monitor and test your PPC campaigns (Pay Per Click) on Google and Yahoo. You can use PPC campaigns to advertise on search terms and drive traffic to your website.</p>
            <ul class="showbullets">
                <li>Track Ad Conversion Rates</li>
                <li>Find out if your ads are cost-effective</li>
                <li>Track your top performing search keywords</li>
                <li>Get ideas for new marketing terms</li>
            </ul>
    </div>
</div>                        
<div class="tour-tracking-3">
    <div class="feature">
         <h3>Get to the Top of Google</h3>            
            <p>BV Commerce has search engine friendly web pages, including the use of meta tags, page titles, heading tags, and SEO friendly URLs. This dramatically increases your chances of high organic search rankings. And the more people who see your site, the more potential customers you have. </p>
            
            <h3>See How You’re Performing Over Time</h3>
            <p>Right now you can see how your sales are performing over time with our built in graphical reports by date. Stay tuned for more reporting tools. They’re on the way.</p>
            
    </div>
</div>                                                                                                                                 
        </div>
        
         <div class="block">
            <div class="pager">
                <div class="last"><a href="/signup/sell">&laquo; Selling Your Products</a></div>
                <div class="next"><a href="/signup/secure">All About Hosting &raquo;</a></div>
            </div>
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

