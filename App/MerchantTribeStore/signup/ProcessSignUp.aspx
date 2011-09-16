<%@ Page Title="" Language="C#" MasterPageFile="~/signup/SignUp.master" AutoEventWireup="True" Inherits="BVCommerce.signup_ProcessSignUp" Codebehind="ProcessSignUp.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeroPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">
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

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfForm" Runat="Server">

<!-- Google Code for Hosted Store SignUp Conversion Page -->
<script type="text/javascript">
<!--
    var google_conversion_id = 1062989762;
    var google_conversion_language = "en";
    var google_conversion_format = "2";
    var google_conversion_color = "ffffff";
    var google_conversion_label = "eXreCO64qgEQwt_v-gM";
    var google_conversion_value = 0;
    if (0.01) {
        google_conversion_value = 0.01;
    }
//-->
</script>
<script type="text/javascript" src="https://www.googleadservices.com/pagead/conversion.js">
</script>
<noscript>
<div style="display:inline;">
<img height="1" width="1" style="border-style:none;" alt="" src="https://www.googleadservices.com/pagead/conversion/1062989762/?value=0.01&amp;label=eXreCO64qgEQwt_v-gM&amp;guid=ON&amp;script=0"/>
</div>
</noscript>

</asp:Content>

