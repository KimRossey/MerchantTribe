<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Controls_Payment" Codebehind="Payment.ascx.cs" %>
<%@ Register Src="CreditCardInput.ascx" TagName="CreditCardInput" TagPrefix="uc1" %>
<table border="0" cellspacing="0" cellpadding="3" width="100%">
    <tr runat="server" id="rowNoPaymentNeeded" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbNoPayment" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><asp:Label ID="lblNoPaymentNeeded" runat="server"></asp:Label></td>
    </tr>
    <tr runat="server" id="rowCreditCard" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbCreditCard" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td>Credit Card<uc1:CreditCardInput ID="CreditCardInput1" runat="server" />            
        </td>
    </tr>
    <tr runat="server" id="trPaypal" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbPaypal" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><img src="https://www.paypal.com/en_US/i/logo/PayPal_mark_37x23.gif" align="left" style="margin-right:7px;"><span style="font-size:11px; font-family: Arial, Verdana;">Save time.  Checkout securely.<br /> Pay without sharing your financial information.</span></td>
    </tr>
    <tr runat="server" id="trPurchaseOrder" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbPurchaseOrder" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><asp:Label ID="lblPurchaseOrderDescription" runat="server"></asp:Label> #: <asp:textbox ID="ponumber" ClientIDMode="Static" runat="server" Columns="10"></asp:textbox></td>
    </tr>
    <tr runat="server" id="trCompanyAccount" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbCompanyAccount" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><asp:Label ID="lblCompanyAccountDescription" runat="server"></asp:Label> #: <asp:textbox ID="accountnumber" ClientIDMode="Static" runat="server" Columns="10"></asp:textbox></td>
    </tr>
    <tr runat="server" id="rowCheck" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbCheck" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><asp:Label ID="lblCheckDescription" runat="server"></asp:Label></td>
    </tr>
    <tr runat="server" id="rowTelephone" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbTelephone" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><asp:Label ID="lblTelephoneDescription" runat="server"></asp:Label></td>
    </tr>
    <tr runat="server" id="trCOD" visible="false">
        <td valign="top" class="radiobuttoncol"><asp:RadioButton ID="rbCOD" GroupName="PaymentGroup" runat="server" Checked="false" /></td>
        <td><asp:Label ID="lblCOD" runat="server"></asp:Label></td>
    </tr>
</table>