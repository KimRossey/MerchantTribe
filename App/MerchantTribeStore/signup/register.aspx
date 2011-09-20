<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="MerchantTribeStore.signup_register" Codebehind="register.aspx.cs" %>
<%@ Import Namespace="BVCommerce.Helpers" %>
<%@ Import Namespace="BVCommerce.app" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
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

        $('#chkAgree').click(
                function () {
                    if ($('#chkAgree').is(':checked')) {
                        $('#submitbutton').removeAttr('disabled');
                    }
                    else {
                        $('#submitbutton').attr('disabled', true);
                    }
                    return;
                }
             );

    });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
<asp:Panel ID="pnlComplete" runat="server" Visible="false">
    <div class="col-1">
        <div class="block registration">
    <h1 class="c">Registration Complete!</h1>

    <div class="registration-section">
        <p>Thank you for creating your store.</p>
    </div>            
    <div class="registration-section">
                <div class="registration-form-area">
                    <fieldset>
                         <table border="0" cellspacing="0" cellpadding="3">
                        <tr>
                            <td class="r vt">Username:</td>
                            <td><asp:Literal ID="completeemail" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td class="r vt">Store Address:</td>
                            <td><asp:Literal ID="completestorelink" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td class="r vt">Store Admin Address:</td>
                            <td><asp:Literal id="completestorelinkadmin" runat="server" ></asp:Literal></td>
                        </tr>
                        </table>                       
                    </fieldset>
                </div>
   </div>
   <div class="registration-section">
        <div class="registration-form-area">
            <h2><asp:Literal ID="completebiglogin" runat="server"   ></asp:Literal>
        </h2>        
        </div>
   </div>       
    &nbsp;<br />
    <p class="clear">&nbsp;</p>
    </div>
</div>
</asp:Panel>
<asp:Panel ID="pnlMain" runat="server" Visible="true">


    <div class="col-1">
    <div class="block registration">
        <h1 class="c">Sign Up Form</h1>
        <h2 class="c">No long-term contracts. Cancel at any time risk free.</h2>
                <asp:Literal ID="litValidation" runat="server" EnableViewState="false" />                
            <div class="registration-section">
                <div class="registration-form-area">
                    <h3>
                        User Account</h3>
                    <fieldset>
                         <table border="0" cellspacing="0" cellpadding="3">
                        <tr>
                            <td class="r vt">Email:</td>
                            <td><asp:TextBox ID="EmailField" Columns="20" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="r vt">Password:</td>
                            <td><asp:TextBox ID="Password" runat="server" TextMode="Password" Columns="20" /></td>
                        </tr>
                        </table>                       
                    </fieldset>
                </div>
                <div class="registration-instructions">
                    <p class="first">
                        Create a user account or use your existing account to add more stores. If you don't
                        have an account you'll need to create your own password.</p>
                </div>
                <p class="clear">
                </p>
            </div>
            <div class="registration-section">
                <div class="registration-form-area">
                    <h3>
                        Store Name</h3>
                    <fieldset>
                        <p>
                            Every BV Commerce store needs a unique name which becomes the store's address. Later,
                            you'll be able to create a diplay name or use your own domain name.<br />
                            <strong>Use only letters and numbers - no spaces</strong></p>
                        <strong>http://
                        <asp:TextBox ID="storename" runat="server" class="" ClientIDMode="Static" Columns="20"></asp:TextBox>.bvcommerce.com</strong><br />
                        <div id="storename-results"></div>
                    </fieldset>
                </div>
                <div class="registration-instructions">
                    <p class="first">
                        Name your store. Use only letters and numbers. Try using a short name to make it
                        easier for customers to remember your store. You will have the option to upgrade
                        to your own custom domain name later.</p>
                </div>
                <p class="clear">
                </p>
            </div>           
<div class="registration-section" runat="server" id="creditcarddiv" visible="false">
                <div class="registration-form-area">
                    <h3>
                        Secure Billing Information</h3>
                    <fieldset>
                        <table border="0" cellspacing="0" cellpadding="3">
                        <tr>
                            <td class="r vt">Card Number:</td>
                            <td><asp:TextBox ID="cardnumber" ClientIDMode="Static" runat="server" Columns="20" /></td>
                        </tr>
                        <tr>
                            <td class="r vt">Expiration:</td>
                            <td><asp:DropDownList ID="expmonth" ClientIDMode="Static" runat="server">
                                <asp:ListItem Value="1">Jan (1)</asp:ListItem>
                                <asp:ListItem Value="2">Feb (1)</asp:ListItem>
                                <asp:ListItem Value="3">Mar (1)</asp:ListItem>
                                <asp:ListItem Value="4">Apr (1)</asp:ListItem>
                                <asp:ListItem Value="5">May (1)</asp:ListItem>
                                <asp:ListItem Value="6">Jun (1)</asp:ListItem>
                                <asp:ListItem Value="7">Jul (1)</asp:ListItem>
                                <asp:ListItem Value="8">Aug (1)</asp:ListItem>
                                <asp:ListItem Value="9">Sep (1)</asp:ListItem>
                                <asp:ListItem Value="10">Oct (1)</asp:ListItem>
                                <asp:ListItem Value="11">Nov (1)</asp:ListItem>
                                <asp:ListItem Value="12">Dec (1)</asp:ListItem>
                            </asp:DropDownList> /
                            <asp:DropDownList ID="expyear" runat="server" ClientIDMode="Static">
                            
                            </asp:DropDownList>                        
                           </td>
                        </tr>
                        <tr>
                            <td class="r vt">Name on Card:</td>
                            <td><asp:TextBox ID="cardholder" ClientIDMode="Static" runat="server" Columns="20" /></td>
                        </tr>
                        <tr>
                            <td class="r vt">Billing Zip Code:</td>
                            <td><asp:TextBox ID="billingzipcode" ClientIDMode="Static" runat="server" />
                            </td>
                        </tr>
                        </table>                                               
                    </fieldset>
                </div>
                <div class="registration-instructions">
                    <p class="first">
                        Enter a credit card for secure billing. Your card will be charged today and each month thereafter on this day of the month until you cancel. If you do not wish your card to be charge you can choose the free plan to test out the service.</p>
                </div>
                <p class="clear">
                </p>
            </div>

            <div class="registration-section">
                <div class="registration-form-area">
                    <h3>Terms and Conditions</h3>
                    <fieldset class="terms">
                        <p><asp:Literal ID="litPlanDetails" runat="server"></asp:Literal></p>
                        <p>
                            BV Commerce Service is billed monthly and may be cancelled at any time. Please see
                            our <a href="/signup/policies/refund" target="_blank">Refund Policy</a>, 
                            <a href="/signup/policies/privacy" target="_blank">Privacy
                                Policy</a> and <a href="/signup/policies/terms" target="_blank">Terms of Service</a> for more information.</p>
                        <p>
                            If you have questions please <a href="http://www.bvsoftware.com/company/contact.aspx"
                                target="_blank">contact us</a>.</p>
                        <p><asp:CheckBox ID="chkAgree" ClientIDMode="Static" runat="server" /> I agree to the Refund Policy, Privacy Privacy and Terms of Service.</p>
                        <asp:Button ID="submitbutton" runat="server" ClientIDMode="Static" 
                            name="submitbutton" Text="Submit Order &amp; Sign Up for BV Commerce" 
                             onclick="submitbutton_Click" />
                    </fieldset>
                </div>
                <p class="clear">
                </p>
            </div>            
    </div>
</div>

</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfForm" Runat="Server">
</asp:Content>

