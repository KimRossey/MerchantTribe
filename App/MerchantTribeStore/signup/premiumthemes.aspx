<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="MerchantTribeStore.signup_premiumthemes" Codebehind="premiumthemes.aspx.cs" %>

<%@ Register src="SignUpMenu.ascx" tagname="SignUpMenu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
<div class="superh1">
    <h1>Supreme Themes</h1>
</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="column-main">
        <div class="block gallery">                
            <h1 class="c">Free Shopping Cart Themes</h1>
            <h2 class="c">It's easy to create custom themes too!</h2>
            &nbsp;<br />
            &nbsp;<br />
            <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                 <td>
                    <a href="/content/images/premiumthemes/medical1.jpg" target="_blank"><img src="/content/images/premiumthemes/Medical1_Small.png" border="0" alt="Medical1" /><br />
                    Medical 1</a></td>
                <td>
                    <a href="/content/images/premiumthemes/medical2.jpg" target="_blank"><img src="/content/images/premiumthemes/Medical2_Small.png" border="0" alt="Medical2" /><br />
                    Medical 2</a></td>
                    
                <td>
                    <a href="/content/images/premiumthemes/sports.jpg" target="_blank"><img src="/content/images/premiumthemes/Sports_Small.png" border="0" alt="Sports" /><br />
                    Sports</a></td>
            </tr>
             <tr>
                 <td>
                    <a href="/content/images/premiumthemes/jewelry1.jpg" target="_blank"><img src="/content/images/premiumthemes/Jewelry1_Small.jpg" border="0" alt="Jewelry1" /><br />
                    Jewelry 1</a></td>
                <td>
                    <a href="/content/images/premiumthemes/jewelry2.jpg" target="_blank"><img src="/content/images/premiumthemes/Jewelry2_Small.jpg" border="0" alt="Jewelry2" /><br />
                    Jewelry 2</a></td>
                    
                <td>
                    <a href="/content/images/premiumthemes/autotech.jpg" target="_blank"><img src="/content/images/premiumthemes/AutoTech_Small.png" border="0" alt="Auto Tech" /><br />
                    Auto Tech</a></td>
            </tr> 
                     
            <tr>
                
                <td>
                    <a href="/content/images/premiumthemes/garden1.jpg" target="_blank"><img src="/content/images/premiumthemes/Garden1_Small.jpg" border="0" alt="Garden1" /><br />
                    Garden 1</a></td>
                <td>
                    <a href="/content/images/premiumthemes/garden2.jpg" target="_blank"><img src="/content/images/premiumthemes/Garden2_Small.jpg" border="0" alt="Garden2" /><br />
                    Garden 2</a></td>
                <td>
                   <a href="/content/images/premiumthemes/tech3.jpg" target="_blank"><img src="/content/images/premiumthemes/Tech3_Small.png" border="0" alt="Tech3" /><br />
                    Tech 3</a></td>
            </tr>
            <tr>
                 <td>
                    <a href="/content/images/premiumthemes/health1.jpg" target="_blank"><img src="/content/images/premiumthemes/Health1_Small.png" border="0" alt="Health1" /><br />
                    Health 1</a></td>
                <td>
                    <a href="/content/images/premiumthemes/health3.jpg" target="_blank"><img src="/content/images/premiumthemes/Health3_Small.png" border="0" alt="Health3" /><br />
                    Health 3</a></td>
                 <td>
                    <a href="/content/images/premiumthemes/tech1.jpg" target="_blank"><img src="/content/images/premiumthemes/Tech1_Small.png" border="0" alt="Tech1" /><br />
                    Tech 1</a></td>
            </tr>
                                                 
            </table>
        </div>
        <div class="block bigtext">
            <div class="c">
            If you need help changing a theme or would like us to create a custom theme:<br />Fill out our <a href="http://www.bvsoftware.com/company/contact.aspx" alt="Contact Form"><strong>Contact Form</strong></a><br />or call us at 1-804-282-4455.
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

