<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Controls_AddressShipping" Codebehind="AddressShipping.ascx.cs" %>
<table border="0" cellspacing="0" cellpadding="3" class="address-table">
<tr>
    <td class="formlabel">&nbsp;</td>
    <td class="formfield"><asp:DropDownList ID="shippingcountryname" runat="server" ClientIDMode="Static" 
                            TabIndex="201" /></td>
</tr>
<tr>
    <td class="formlabel">First Name:</td>
    <td class="formfield"><asp:TextBox ID="shippingfirstname" runat="server" ClientIDMode="Static" 
                            MaxLength="255" Columns="20" TabIndex="202" /><span class="requiredfield">*</span></td>
</tr>
<tr>
    <td class="formlabel">Last Name:</td>
    <td class="formfield"><asp:TextBox ID="shippinglastname" runat="server" ClientIDMode="Static" 
                            MaxLength="255" Columns="20" TabIndex="203" /><span class="requiredfield">*</span></td>
</tr>
<tr>
    <td class="formlabel">Company:</td>
    <td class="formfield"><asp:TextBox ID="shippingcompany" runat="server" ClientIDMode="Static" 
                            MaxLength="255" Columns="20" TabIndex="204" /></td>
</tr>
<tr>
    <td class="formlabel">Address:</td>
    <td class="formfield"><asp:TextBox ID="shippingaddress" runat="server" ClientIDMode="Static"
                            MaxLength="255" size="20" TabIndex="205" /><span class="requiredfield">*</span></td>
</tr>
<tr>
    <td class="formlabel">City:</td>
    <td class="formfield"><asp:TextBox ID="shippingcity" runat="server" ClientIDMode="Static"
                            MaxLength="50" size="20" TabIndex="206" /><span class="requiredfield">*</span></td>
</tr>
<tr>
    <td class="formlabel">State:</td>
    <td class="formfield"><select ID="shippingstate" EnableViewState="false" runat="Server" ClientIDMode="Static" TabIndex="207" /></td>
</tr>
<tr>
    <td class="formlabel">Postal Code:</td>
    <td class="formfield"><asp:TextBox ID="shippingzip" runat="server" ClientIDMode="Static"
                            MaxLength="20" size="10" TabIndex="208" /><span class="requiredfield">*</span></td>
</tr>
<tr>
    <td class="formlabel">Phone:</td>
    <td class="formfield"><asp:TextBox ID="shippingphone" runat="server" ClientIDMode="Static"
                            MaxLength="20" size="20" TabIndex="209" /></td>
</tr>
</table><asp:HiddenField ID="shippingaddressbvin" ClientIDMode="Static" runat="server" />
