﻿<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_default" Codebehind="default.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
    <div class="superh1">
    <h1>Pricing &amp; Sign Up</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="column-main">
        <div class="block">
            <h1 class="c">
                Start Selling Now! Try a FREE store!</h1>
            <h2 class="c">
                Cancel at any time at no risk. (Free version-No credit card required.)</h2>
            <table class="priceplans">           
            <tr class="planname">
                <td class="pnone">&nbsp;</td>
                <td><a href="/signup/register/starter"><img src="/content/images/system/PlanFree2.png" alt="Sign Up for a Trial Store" /></a>                
                </td>
                <td><a href="/signup/register/basic">
                        <img src="/content/images/system/PlanBasic2.png" alt="Sign Up for Basic Plan" /></a></td>
                <td class="featured">
                    <a href="/signup/register/plus">
                        <img src="/content/images/system/PlanPlus2.png" alt="Sign Up for Plus Plan" /></a>
                    </td>
                <td><a href="/signup/register/premium">
                        <% if ((bool)ViewData["IsPayPalLead"] == true)
                           { %><img src="/content/images/system/PlanPayPal2.png" alt="Sign Up for Premium Plan" /><% } else { %><img src="/content/images/system/PlanPremium2.png" alt="Sign Up for Premium Plan" /><% } %></a>
                           </td>
                <td><a href="/signup/register/max">
                        <img src="/content/images/system/PlanMax2.png" alt="Sign Up for Max Plan" /></a></td>
            </tr>            
            <tr class="rowalt">
                <td class="desc">Sales Cap:</td>
                <td class="p1">$1000/mo</td>
                <td class="p2">Unlimited</td>
                <td class="p3">Unlimited</td>
                <td class="p4">Unlimited</td>
                <td class="p5">Unlimited</td>
            </tr>
            <tr class="row">
                <td class="desc">Max Products:</td>
                <td class="p1">10</td>
                <td class="p2">100</td>
                <td class="p3">5,000</td>
                <td class="p4">10,000</td>
                <td class="p5">50,000</td>
            </tr>
            <tr class="rowalt">
                <td class="desc">Transfer:</td>
                <td class="p1">1GB</td>
                <td class="p2">10GB</td>
                <td class="p3">100GB</td>
                <td class="p4">250GB</td>
                <td class="p5">1,000GB</td>
            </tr>
            <tr class="row">
                <td class="desc">Monthly:</td>
                <td class="p1">$0</td>
                <td class="p2">$49</td>
                <td class="p3">$99</td>
                <td class="p4">$199</td>
                <td class="p5">$499</td>
            </tr>                              
            <tr class="rowalt">
                <td class="desc">PayPal:</td>
                <td class="p1">Yes</td>
                <td class="p2">Yes</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr id="trCC" class="row" runat="server">
                <td class="desc">Credit Cards:</td>
                <td class="p1">&nbsp;</td>
                <td class="p2">Yes</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr id="trPO" class="rowalt" runat="server">
                <td class="desc">Purchase Orders:</td>
                <td class="p1">&nbsp;</td>
                <td class="p2">&nbsp;</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr id="trCOD" class="row" runat="server">
                <td class="desc">COD:</td>
                <td class="p1">&nbsp;</td>
                <td class="p2">&nbsp;</td>
                <td class="p3">Yes</td>
                <td class="p4">Yes</td>
                <td class="p5">Yes</td>
            </tr>
            <tr class="planchoose">
                <td class="pnone">&nbsp;</td>
                <td><a href="/signup/register/starter">
                        <img src="/content/images/system/SignUpTiny.png" alt="Sign Up for a Trial Store" /></a></td>
                <td><a href="/signup/register/basic">
                        <img src="/content/images/system/SignUpTiny.png" alt="Sign Up for Basic Plan" /></a></td>
                <td class="featured"><a href="/signup/register/plus">
                        <img src="/content/images/system/SignUpTiny.png" alt="Sign Up for Plus Plan" /></a></td>
                <td><a href="/signup/register/premium">
                        <img src="/content/images/system/SignUpTiny.png" alt="Sign Up for Premium Plan" /></a></td>
                <td><a href="/signup/register/max">
                        <img src="/content/images/system/SignUpTiny.png" alt="Sign Up for Max Plan" /></a></td>
            </tr>
            </table><br />
            &nbsp;<br />
            <div class="col-main-left">
                <h3>
                    BV Commerce comes complete with:</h3>
                <ul class="showbullets">
                    <li>Automatic software upgrades</li>
                    <li>Security alerts</li>
                    <li>Quick, one page checkout</li>
                    <li>Design API for creating your own themes</li>
                    <li>Search engine optimization</li>
                    <li>Guaranteed 99.9% uptime</li>
                    <li>Intuitive, easy to use admin</li>
                    <li>Access to community forums</li>
                    <li>Paypal integration</li>
                </ul>
                <a href="/signup/features" title="BV Commerce Hosted Shopping Cart Features">Full Feature List &raquo;</a>
            </div>
            <div class="col-main-right">
                <h3>Not Sure Which Plan is Right for You?</h3>
                <p>Call 1-804-282-4455 or <a href="http://www.bvsoftware.com/company/contact.aspx">e-mail.</a></p>

                <h3>Need a custom plan?</h3>
                <p>Contact us at 1-804-282-4455 or by <a href="http://www.bvsoftware.com/company/contact.aspx">e-mail</a> and we can help you put together a plan that meets your needs. </p>

                <h3>Questions About Signing Up?</h3>
                <ul class="showbullets">
                <li>There are no setup fees.</li>
                <li>You don&#8217;t need a credit card to sign up for a free store. You will need a credit card number for paid plans. </li>
                <li>You can upgrade, downgrade or cancel a plan at any time from the administration panel of your store.</li>
                </ul>
            </div>
            <div class="clear">
                &nbsp;</div>
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

