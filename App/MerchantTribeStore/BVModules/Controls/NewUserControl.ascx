<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="MerchantTribeStore.BVModules_Controls_NewUserControl" Codebehind="NewUserControl.ascx.cs" %>
<asp:ValidationSummary ID="valNewUserSummary" CssClass="validationmessage" EnableClientScript="True"
    runat="server" ValidationGroup="NewUser"></asp:ValidationSummary>
<asp:Label ID="lblError" runat="server" CssClass="errormessage" EnableViewState="False" Visible="false"></asp:Label>
<asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSaveChanges">
<table id="NewUserTable" runat="server" border="0" cellspacing="0" cellpadding="3">
    <tr>
        <td class="formlabel">
            Email:</td>
        <td class="formfield">
            <asp:TextBox ID="EmailField" CssClass="forminput required" runat="server" Columns="30"
                MaxLength="100" TabIndex="2001"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewUser" ID="val2Username"  ForeColor=" " CssClass="errormessage"
                EnableClientScript="True" runat="server" ControlToValidate="EmailField" Display="Dynamic"
                ErrorMessage="Please enter an email address" Visible="True">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ValidationGroup="NewUser" ID="valUsername"  ForeColor=" " CssClass="errormessage"
                runat="server" ControlToValidate="EmailField" Display="Dynamic" ErrorMessage="Please enter a valid email address"
                ValidationExpression="^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td class="formlabel">
            <asp:Label ID="FirstNameLabel" CssClass="required" runat="server" Text="First Name:"
                AssociatedControlID="FirstNameField"></asp:Label></td>
        <td class="formfield">
            <asp:TextBox ID="FirstNameField" CssClass="forminput required" TabIndex="2002" runat="server"
                Columns="30" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewUser" ID="valFirstName"  ForeColor=" " CssClass="errormessage"
                EnableClientScript="True" runat="server" ControlToValidate="FirstNameField" Display="Dynamic"
                ErrorMessage="Please enter a first name or nickname" Visible="True">*</asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="formlabel">
            <asp:Label ID="LastNameLabel" CssClass="required" runat="server" Text="Last Name:"
                AssociatedControlID="LastNameField"></asp:Label></td>
        <td class="formfield">
            <asp:TextBox ID="LastNameField" CssClass="forminput required" TabIndex="2003" runat="server"
                Columns="30" MaxLength="50"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewUser" ID="RequiredFieldValidator1"
                ForeColor=" " CssClass="errormessage" EnableClientScript="True" runat="server" ControlToValidate="LastNameField"
                Display="Dynamic" ErrorMessage="Please enter a last name" Visible="True">*</asp:RequiredFieldValidator></td>
    </tr>   
    <tr>
        <td class="formlabel">            
            <asp:Label ID="PasswordLabel" CssClass="required" runat="server" Text="Password:"
                AssociatedControlID="PasswordField"></asp:Label></td>
        <td class="formfield">            
            <asp:TextBox ID="PasswordField" CssClass="forminput required" runat="server" Columns="30"
                TabIndex="2006" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewUser" ID="valPassword" runat="server"
                ControlToValidate="PasswordField" ErrorMessage="A password is required" ForeColor=" " CssClass="errormessage">*</asp:RequiredFieldValidator></td>
    </tr>
    <tr>
        <td class="formlabel">
            <asp:Label ID="PasswordConfirmLabel" CssClass="required" runat="server" Text="Confirm Password:"
                AssociatedControlID="PasswordConfirmField"></asp:Label></td>
        <td class="formfield">
            <asp:TextBox ID="PasswordConfirmField" CssClass="forminput required" runat="server"
                Columns="30" TabIndex="2007" TextMode="Password"></asp:TextBox>
            <asp:RequiredFieldValidator ValidationGroup="NewUser" ID="RequiredFieldValidator2"
                runat="server" ControlToValidate="PasswordConfirmField" ErrorMessage="A password confirmation is required" ForeColor=" " CssClass="errormessage">*</asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="PasswordField"
                ControlToValidate="PasswordConfirmField" Display="Dynamic" ErrorMessage="Password and Confirmation Must Match."
                ValidationGroup="NewUser" ForeColor=" " CssClass="errormessage">*</asp:CompareValidator></td>
    </tr>
    <tr>
        <td>&nbsp;</td>
        <td class="formfield"><asp:ImageButton ID="btnSaveChanges" TabIndex="2050" runat="server" ValidationGroup="NewUser" onclick="btnSaveChanges_Click"></asp:ImageButton>
        </td>
    </tr>
</table>
</asp:Panel>
<asp:HiddenField ID="BvinField" runat="server" />
