﻿<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="MerchantTribeStore.signup_home" Codebehind="home.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">    
    <script type="text/javascript">

        // Jquery Setup
        $(document).ready(function () {

            $('#storename').blur(
            function () {

                $('#storename-results').html('<img src="/content/images/system/ajax-loader.gif" alt="Loading..." />');
                $('#storename-results').show();

                var name = $('#storename').attr('value');
                var jsondata = "{storename:'" + name.replace("'", "") + "'}";
                $.ajax(
                { type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "/signup/JsonCheckStoreName.aspx",
                    data: jsondata,
                    dataType: "json",
                    success: function (data) {
                        $('#storename').attr('value', data.cleanstorename);
                        $('#storename-results').html(data.message);
                    },
                    error: function () { $('#storename-results').html(''); }
                });

                return;
            }
            );          
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="col-2w-a">
    <div class="homehero">
        <div class="h">
            <h1>
                Your Ultimate Hosted Shopping Cart</h1>
            <h2>
                BV Commerce is a hosted shopping cart solution that simplifies running online stores.
                Easy to use, easy to take orders, fully managed, free stores, free themes.</h2>
            <ul class="showbullets">
                <li>All the features you need to sell</li>
                <li>Lots of free themes that are easy to customize</li>
                <li>Secure hosting with a proven company</li>
                <li>Use your own domain name</li>
                <li>No contracts. Cancel at any time</li>
            </ul>
        </div>
    </div>
    <div class="col-2w-sub-a">
        <div class="block" style="height: 300px;">
            <h3>
                Why Choose Us?</h3>
            <ul class="showbullets">
                <li>Over 8 years in ecommerce</li>
                <li>Award winning SEO optimized software</li>
                <li>Used by top performing online businesses</li>
                <li>Built on ASP.NET Microsoft technology</li>
            </ul>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="/signup/about" title="About BV Software">Learn
                More &raquo;</a><br />
            &nbsp;
            <h3>
                Award-Winning Software</h3>
            <img class="awards" src="/content/images/system/ReadersChoice.png" alt="Reader's Choice Award Winner Best Shopping Cart 2004,2005,2006,2008"
                border="0" />
        </div>
    </div>
    <div class="col-2w-sub-b">
        <div class="block" style="height: 300px;">
            <h3>
                BV Software Customers</h3>
            <p>
                Our professionals have worked with some of the best online retailers using our toolkit
                software and hosted services. Here are a few samples:</p>
            <img class="awards" src="/content/images/system/ClientLogos.png" alt="BV Software Client Sample"
                border="0" />
        </div>
    </div>
    <div class="clear">
        &nbsp;</div>
    </div>
    <div class="col-2w-b">
        <div class="homeregistration">
            <a href="/signup" alt="Pricing and Sign Up">
                <img src="/content/images/system/freestore.png" alt="Sign up for a FREE hosted shopping cart online store"
                    border="0" width="300" height="220" /></a>
            <div style="padding: 0 15px;">
                <table border="0" cellspacing="0" cellpadding="3">
                    <tr>
                        <td colspan="2">
                            <h3>
                                Create an Account</h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="r vt">
                            Email:
                        </td>
                        <td>
                            <asp:TextBox ID="EmailField" runat="server" Columns="25"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <td class="r vt">
                            Password:
                        </td>
                        <td>
                            <asp:TextBox ID="Password" runat="server"  TextMode="Password" Columns="25" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;<br />
                            <h3>
                                Select a Store Name</h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="storename" runat="server" ClientIDMode="Static" Columns="35" />                            
                        </td>
                    </tr>
                </table>
                <div id="storename-results" style="height: 65px;">
                    <div class="flash-message-watermark">
                        a store name is required<br />
                        &nbsp;
                    </div>
                </div>
                &nbsp;<br />
                <a href="/policies/terms" target="_blank">Terms of Service</a> apply to all stores.<br />
                &nbsp;<br />
                <asp:ImageButton ID="submitbutton" ClientIDMode="Static" runat="server" 
                    ImageUrl="/content/images/System/CreateStoreGreen.png" 
                    AlternateText="Create Your Free Store"
                    PostBackUrl="/signup/register/starter" />                
                <asp:HiddenField ID="HiddenAction" runat="server" Value="freesignup" />
            </div>
        </div>
    </div>
    <div class="col-2-a">
        <div class="block" style="height: 250px;">
            <h3>
                All the Features You Need</h3>
            <p>
                If you think hosted shopping carts can’t be powerful, think again. We have all the
                features you need to succeed with your online store and we’re adding new ones all
                the time. Check out our <a href="/signup/features" title="BV Commerce Features List">full
                    feature list</a> to see all the great options we have for web merchants.
            </p>
            <h3>
                The smartest way to get started</h3>
            <p>
                It only takes a minute to <a href="/signup" title="Create a store with BV Commerce">
                    create an online store</a> with BV Commerce. Choose a name. Pick your theme.
                And populate your products. Try a free store now and start selling today. There
                are no setup fees, no long term contracts, cancel at any time. If your BV Commerce
                online web store isn't selling like you think it should just walk away risk-free.</p>
        </div>
    </div>
    <div class="col-2-b">
        <div class="block" style="height: 250px;">
            <h3>
                Create a Store as Unique as Your Business</h3>
            <p>
                Your BV Commerce hosted shopping cart includes lots of free themes and we’re adding
                new ones every day. But even better, it’s really easy to customize our themes to
                create a unique look for your online store. With a little knowledge of Photoshop
                you can literally make your own theme in a few hours.</p>
            <h3>
                Easy to Use</h3>
            <p>
                If you can check your email you can run a BV Commerce online store. We take care
                of all the technical details so that you can concentrate on selling. The pages are
                search engine optimized to drive more visitors to your site. Just fill in your product
                information, upload some pictures and you're ready to sell. Process orders right
                from the administrator panel and customers automatically receive email updates and
                tracking information.</p>
        </div>
    </div>
    <div class="clear">
        &nbsp;</div>
    </div> </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfForm" runat="Server">
</asp:Content>
