<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="BVCommerce.BVModules_Controls_LoginControl" Codebehind="LoginControl.ascx.cs" %>
<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:ValidationSummary ID="valLoginSummary" CssClass="errormessage" EnableClientScript="True"
    runat="server" ValidationGroup="Login"></asp:ValidationSummary>
<uc1:MessageBox ID="MessageBox1" runat="server" />
<asp:Panel ID="pnlMain" runat="server" defaultButton="btnLogin">
<table id="CurrentUserTable" cellspacing="0" cellpadding="3">
    <tr>
        <td class="formlabel">
            Email:</td>
        <td class="formfield">
            <asp:TextBox ID="UsernameField" CssClass="forminput required logincontrolemailfield" runat="server" ToolTip="Enter Your Username" TabIndex="1"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="formlabel">
            <asp:Label ID="PasswordLabel" CssClass="required" runat="server" Text="Password:"
                AssociatedControlID="PasswordField"></asp:Label></td>
        <td class="formfield">
            <asp:TextBox ID="PasswordField" CssClass="forminput required" runat="server" TextMode="Password"
                ToolTip="Enter Your Password" TabIndex="2"></asp:TextBox></td>
    </tr>
    <tr id="trRememberMe" runat="server">
        <td class="formlabel">
        </td>
        <td class="formfield">
            <asp:CheckBox ID="RememberMeCheckBox" runat="server" Text="Remember Me" Checked="true" TabIndex="3" />
        </td>
    </tr>
    <tr>
        <td class="formlabel">
        </td>
        <td class="formfield">
            <asp:ImageButton ID="btnLogin" runat="server" AlternateText="Login" ImageUrl=""
                ToolTip="Login" ValidationGroup="Login" TabIndex="4" 
                onclick="btnLogin_Click" /></td>
    </tr>
    <tr>
        <td class="formlabel">
        </td>
        <td>
            <asp:LinkButton ID="btnForgotPassword" TabIndex="5" runat="server" 
                CssClass="BVSmallText forgot" onclick="btnForgotPassword_Click">Forgot Your Password? Click Here.</asp:LinkButton>
            </td>
    </tr>
</table>
</asp:Panel>
<asp:HiddenField ID="RedirectToField" runat="server" /><asp:HiddenField ID="CheckoutModeField" runat="server" Value="false" />
