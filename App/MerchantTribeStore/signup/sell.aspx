﻿<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="MerchantTribeStore.signup_sell" Codebehind="sell.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
<div class="superh1">
        <h1>
            Selling Your Products</h1>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="column-main">
        <div class="block tour-sell">
            <h1 class="c">
                We make it easy to sell.</h1>
            <h2 class="c">
                A lot.</h2>
                <div class="tour-sell-0">
                    
            <div class="feature">
                <p>
                    BV Commerce Hosted has all the features you need to sell online. Your dashboard
                    sends you news and updates. Adding products and categories is easy. Our one page
                    checkout keeps your customers happy. And a variety of shipping options helps you
                    keep costs down.</p>
                </div>
                </div>
            <div class="tour-sell-1">
                <div class="feature">
                    
                    <h3>
                        Dashboard</h3>
                    <p>
                        Your Home Base. Get a quick summary of your store's success and receive information about the lastest updates to BV Commerce every time you login to your store.
                    </p>
                </div>
            </div>
            <div class="tour-sell-2">
                <div class="feature">
                    <h3>
                        Products and Categories</h3>
                    <p>
                        BV Commerce simplifies the set up process with intuitive product and category pages
                        that let you decide what options you need.<br />
                        <a href="features.aspx" alt="BV Commerce Feature List">Full Feature List</a></p>
                    <h4>
                        Adding Products is Easy
                    </h4>
                    <p>
                        Adding and editing your products is simple and powerful with our basic and advanced
                        options, including unlimited choices and options for product variations like sizes or colors.</p>
                    <h4>
                        Resize Images Automatically</h4>
                    <p>
                        Upload a high quality image and BV Commerce will generate smaller images optimized for faster web browsing. No
                        need to manually process images ahead of time.</p>
                    <h4>
                        Unlimited Categories and Sub Categories</h4>
                    <p>
                        You get to decide how many categories you need to best merchandize your products. Create as many levels as you need to sell effectively.</p>
                    <h4>
                        Product Reviews For SEO</h4>
                    <p>
                        Product reviews are a great way to generate fresh content for your online store. Reviews add credibility to your products and the user generated HTML is indexed by Google offer another chance to attract customers.</p>
                </div>
            </div>
            <div class="tour-sell-3">
                <div class="feature">
                    <h3>
                        Checkout and Payment</h3>
                    <p>
                        Our streamlined checkout process and intuitive dropdown lists encourage browsers
                        to become your buyers. And lots of payment options make it easy for them to purchase.<br />
                        <a href="features.aspx" alt="BV Commerce Feature List">Full Feature List</a>
                    </p>
                    <h4>
                        Keep Conversion Rates High</h4>
                    <p>
                        BV Commerce Hosted has a super simple one-page AJAX enabled checkout process. This
                        keeps your conversion rates high with fewer slow page loads.</p>
                    <h4>
                        Less Typos with Dropdown Lists</h4>
                    <p>
                        State/region dropdown lists mean fewer typing mistakes when customers enter addresses.</p>
                    <h4>
                        Send to Friends</h4>
                    <p>
                        Customers can select separate shipping and billing addresses. This is great for
                        gift giving.</p>
                    <h4>
                        Many Payment Options</h4>
                    <p>
                        Customers can pay with Paypal, Credit Cards, Purchase Orders, Cash on Delivery and more. Payment gateways include BV Secure Gateway and Authorize.net, Payflow Pro and PayPal Website Payments Pro.</p>
                    <h4>
                        Tax Rates for Every Location</h4>
                    <p>
                        Create tax rates by country, state, and postal codes. Optionally, the tax can also
                        apply to shipping charges, as some states require.</p>
                </div>
            </div>
            <div class="tour-sell-4">
                <div class="feature">
                    <h3>
                        Managing Orders</h3>
                    <p>
                        Track orders from newly arrived through payment, shipping and completed statuses. Our batch processing buttons allow you to quickly collect payment from multiple orders when they're ready to ship. 
                    </p>
                    <h4>
                        Keep Track of Your Sales Process</h4>
                    <p>
                        Quick tabs help you easily spot where orders are in your workflow. Automatically send email updates with tracking information to customers when items ship.</p>
                    <h4>
                        Manage The Unexpected</h4>
                    <p>
                        Moving orders to on-hold status when you need to follow up with customers lets you keep the rest of the orders processing flowing through your store without losing track of special cases. </p>
                    <p><a href="features.aspx" alt="BV Commerce Feature List">Full Feature List</a></p>
                </div>
            </div>
            <div class="tour-sell-5">
                <div class="feature">
                    <h3>
                        So Many Shipping Options</h3>
                    <p>
                        Choose from a variety of shipping methods that best meet your business needs.</p>
                    <h4>
                        Lots of Ways to Ship Your Products</h4>
                    <p>
                        Options include flat rate per shipment and flat rate by item, and rate table by
                        total price, total weight or item count.</p>
                    <h4>
                        Save Money on Overseas Orders</h4>
                    <p>
                        Create shipping zones in the US and internationally so you don’t lose money on shipping
                        costs on overseas orders.</p>
                    <p><a href="features.aspx" alt="BV Commerce Feature List">Full Feature List</a></p>
                </div>
            </div>
        </div>
        
         <div class="block">
            <div class="pager">
                <div class="last"><a href="/signup/design">&laquo; Creating Your Theme</a></div>
                <div class="next"><a href="/signup/promote">Tracking and Promoting &raquo;</a></div>
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

