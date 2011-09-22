<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.ForgotPassword" title="Forgot Password" Codebehind="ForgotPassword.aspx.cs" %>

<%@ Register Src="BVModules/Controls/ManualBreadCrumbTrail.ascx" TagName="ManualBreadCrumbTrail"
    TagPrefix="uc2" %>
<%@ Register Src="~/BVModules/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:ManualBreadCrumbTrail ID="ManualBreadCrumbTrail1" runat="server" />
    <h1>
        <span>
            <asp:Label ID="TitleLabel" runat="server">Forgot Password</asp:Label></span></h1>
    <asp:ValidationSummary ID="valSummary" runat="server" EnableClientScript="True" CssClass="errormessage"
        ForeColor=" "></asp:ValidationSummary>
<uc1:MessageBox ID="msg" runat="server" />
    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSend" runat="server">
    <p>
        This form will generate a new random password for your account. You will receive
        an email with your new password.</p>
    <asp:Label ID="lblUsername" CssClass="FormLabel" runat="server">Email:</asp:Label><asp:TextBox
        CssClass="FormInput" ID="inUsername" TabIndex="3000" runat="server" Columns="30"></asp:TextBox>&nbsp;
        <asp:RequiredFieldValidator
            ID="val2Username" runat="server" EnableClientScript="True" CssClass="errormessage"
            Visible="True" ErrorMessage="Please enter an email address" Display="Dynamic"
            ControlToValidate="inUsername" ForeColor=" ">*</asp:RequiredFieldValidator>&nbsp;<br/>
    &nbsp;<br/>
    <asp:LinkButton ID="lnkClose" TabIndex="3002" runat="server" 
        CausesValidation="False" runat="server" onclick="lnkClose_Click">Continue</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:ImageButton ID="btnSend" TabIndex="3001" runat="server" 
        ImageUrl="" 
        onclick="btnSend_Click" style="width: 14px">
    </asp:ImageButton>
    </asp:Panel>
</asp:Content>

