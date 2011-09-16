<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="BVCommerce.BVModules_Controls_AddressBilling" Codebehind="AddressBilling.ascx.cs" %>
<table border="0" cellspacing="0" cellpadding="3" class="address-table">
<tr>
    <td class="formlabel">&nbsp;</td>
    <td class="formfield"><asp:DropDownList ID="billingcountryname" runat="server" ClientIDMode="Static" 
                            TabIndex="401" /></td>
</tr>
<tr>
    <td class="formlabel">First Name:</td>
    <td class="formfield"><asp:TextBox ID="billingfirstname" runat="server" ClientIDMode="Static" 
                            MaxLength="255" Columns="20" TabIndex="402" /></td>
</tr>
<tr>
    <td class="formlabel">Last Name:</td>
    <td class="formfield"><asp:TextBox ID="billinglastname" runat="server" ClientIDMode="Static" 
                            MaxLength="255" Columns="20" TabIndex="403" /></td>
</tr>
<tr>
    <td class="formlabel">Company:</td>
    <td class="formfield"><asp:TextBox ID="billingcompany" runat="server" ClientIDMode="Static" 
                            MaxLength="255" Columns="20" TabIndex="404" /></td>
</tr>
<tr>
    <td class="formlabel">Address:</td>
    <td class="formfield"><asp:TextBox ID="billingaddress" runat="server" ClientIDMode="Static"
                            MaxLength="255" size="20" TabIndex="405" /></td>
</tr>
<tr>
    <td class="formlabel">City:</td>
    <td class="formfield"><asp:TextBox ID="billingcity" runat="server" ClientIDMode="Static"
                            MaxLength="50" size="20" TabIndex="406" /></td>
</tr>
<tr>
    <td class="formlabel">State:</td>
    <td class="formfield"><select ID="billingstate" EnableViewState="false" runat="Server" ClientIDMode="Static" TabIndex="407" /></td>
</tr>
<tr>
    <td class="formlabel">Postal Code:</td>
    <td class="formfield"><asp:TextBox ID="billingzip" runat="server" ClientIDMode="Static"
                            MaxLength="20" size="10" TabIndex="408" /></td>
</tr>
</table><asp:HiddenField ID="billingaddressbvin" ClientIDMode="Static" runat="server" />
