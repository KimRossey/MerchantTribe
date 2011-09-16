﻿<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_secure" Codebehind="secure.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
    <div class="superh1">
        <h1>
            All About Hosting</h1>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="column-main">
        <div class="block">
            <h1 class="c">Hosting</h1>
            <h2 class="c"> Reliable. Secure. Trustworthy.</h2>
            <div class="tour-hosting-1">
                <div class="feature">
                    <h3>
                        You Can Concentrate on Sales</h3>
                    <p>
                        Because we take care of all the technical stuff, so you can focus on your business.</p>
                     <h3>
                    We Don’t Punish You for Being Popular</h3>
                <p>
                    With unlimited bandwith your charges don’t increase with your web traffic.</p>
                    <h3>
                    Servers That Adjust to Your Web Traffic</h3>
                <p>
                    Our scalable servers ensure that your web site will always be fast—even if Oprah
                    comes knocking. (You could only hope.)</p> 
                    
                     <h3>
                    SSL Encryption Keeps Data Safe</h3>
                <p>
                    We provide free 128 bit encryption. Sensitive information like credit card numbers
                    is transmitted securely.
                </p>
                
                </div>
            </div>
            &nbsp;<br />
           <div class="clear"></div>            
            &nbsp;<br />
            
            <div>
                <div class="col-3sub-a">
                    <h3>
                        Automatic Nightly Backups</h3>
                    <p>
                        In the unlikely event of a server crash your information will still be secure.
                    </p>
                </div>
                <div class="col-3sub-b">
                    <h3>
                        Automatically Get Free Updates</h3>
                    <p>
                        You automatically receive every new feature added to BV Commerce Hosted.
                    </p>
                </div>
                <div class="col-3sub-c">
                    <h3>
                        Use Your Own Domain Name</h3>
                    <p>
                        Use your unique domain name! Or use a bvcommerce.com domain name provided to you
                        when you sign up.
                    </p>
                </div>
                <div class="clear">
                </div>
            </div>
            
            <div >
            <div class="col-3sub-a">
                <h3>
                    Built on ASP.NET MVC</h3>
                <p>
                    Built on Microsoft’s newest fastest technology, used by developers all over the
                    world.</p>
            </div>
            <div class="col-3sub-b">
                <h3>
                    About Peak 10</h3>
                <p>
                    Peak 10 owns and operates multiple high performance data centers that are supported
                    by highly skilled technical personnel 24 hours a day, seven days a week.</p>
            </div>
            <div class="col-3sub-c">
                <h3>&nbsp;</h3>
                <p> All of
                    Peak 10’s state-of-the-art data centers are engineered with multiple levels of security,
                    uninterruptible power, redundant HVAC systems, fire suppression and around-the-clock
                    monitoring and management.</p>
            </div>
            <div class="clear">
            </div>
        </div>
            
        </div>    
                       
    <div class="block">
        <div class="pager">
            <div class="last">
                <a href="/signup/promote">&laquo; Tracking and Promoting</a></div>
            <div class="next">
                <a href="/signup/about">About BV &raquo;</a></div>
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

