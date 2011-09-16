<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Acumatica2.aspx.cs" Inherits="BVCommerce.BVAdmin.Configuration.Acumatica2" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Acumatica ERP Integration - Mappings</h1>        
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
        <table border="0" cellspacing="0" cellpadding="3">       
        <tr>
            <td class="formlabel">ERP Tax Class for New Items:</td>
            <td class="formfield">
                 <asp:DropDownList ID="lstNewItemTaxClass" runat="server"></asp:DropDownList></td>
        </tr>     
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td class="formlabel">Warehouse for New Items:</td>
            <td class="formfield">
                 <asp:DropDownList ID="lstWarehouses" runat="server"></asp:DropDownList></td>
        </tr>     
        <tr>
            <td class="formlabel">Warehouse for Line Items:</td>
            <td class="formfield">
                 <asp:DropDownList ID="lstWarehouses2" runat="server"></asp:DropDownList></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>     
        <tr>
            <td class="formlabel">&nbsp;</td>
            <td class="formfield"><asp:CheckBox ID="chkCustomerIdString" runat="server" Text="Use text ID for customers instead of auto numbers (not recommended)" /></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>        
        <tr>
            <td class="formlabel">Default Customer Payment Method:</td>
            <td class="formfield">
                 <asp:DropDownList ID="lstPayments" runat="server"></asp:DropDownList></td>
        </tr>     
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td class="formlabel">Payment Mappings:</td>
            <td class="formfield"><asp:PlaceHolder runat="server" ID="phPayment"></asp:PlaceHolder></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
            <td class="formlabel">Shipping Mappings:</td>
            <td class="formfield"><asp:PlaceHolder runat="server" ID="phShipping"></asp:PlaceHolder></td>
        </tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
            </tr>            
        </table>
        </asp:Panel>
        
</asp:Content>



