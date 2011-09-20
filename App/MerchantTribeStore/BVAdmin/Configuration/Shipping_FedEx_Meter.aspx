<%@ Page Title="FedEx Meter Number Registration" Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="true" CodeBehind="Shipping_FedEx_Meter.aspx.cs" Inherits="MerchantTribeStore.BVAdmin.Configuration.Shipping_FedEx_Meter" %>
<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headcontent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<uc1:MessageBox ID="MessageBox1" runat="server" />
    <h1>
        FedEx Meter Number Registration</h1>
    <i>Current Meter Number =
        <asp:Label runat="server" ID="lblCurrentMeterNumber" /></i><br />
    &nbsp;<br />
    <table border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="formlabel">
                Fed Ex Account Number</td>
            <td class="formfield">
                <asp:TextBox ID="AccountNumberField" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                Your Name</td>
            <td class="formfield">
                <asp:TextBox ID="PersonNameField" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                Company Name (optional)</td>
            <td class="formfield">
                <asp:TextBox ID="CompanyNameField" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                Country Code</td>
            <td class="formfield">
                <asp:DropDownList AutoCallBack="true" ID="CountryCodeField" runat="server" 
                    ondatabinding="CountryCodeField_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>      
        <tr>
            <td class="formlabel">
                Address Line 1</td>
            <td class="formfield">
                <asp:TextBox ID="Line1Field" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                Address Line 2</td>
            <td class="formfield">
                <asp:TextBox ID="Line2Field" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                City Name</td>
            <td class="formfield">
                <asp:TextBox ID="CityNameField" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                State Code</td>
            <td class="formfield"><asp:DropDownList ID="lstState" runat="server">
            </asp:DropDownList>
                <asp:TextBox ID="StateCodeField" Text="ZZ" runat="server" Columns="3" MaxLength="2" /></td>
        </tr>
        <tr>
            <td class="formlabel">
                Postal Code</td>
            <td class="formfield">
                <asp:TextBox ID="PostalCodeField" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td align="right" class="formlabel">
                Phone Number</td>
            <td align="left" class="formfield">
                <asp:TextBox ID="PhoneNumber" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td align="right" class="formlabel">
                Email Address (optional)</td>
            <td align="left" class="formfield">
                <asp:TextBox ID="EmailAddress" runat="server" Columns="30" /></td>
        </tr>
        <tr>
            <td align="right" class="formlabel">
                <asp:ImageButton ID="btnCancel" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
            <td align="left" class="formfield">
                <asp:ImageButton ID="btnSubmit" runat="server" 
                    ImageUrl="~/BVAdmin/Images/Buttons/Submit.png" onclick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PreBodyCloseContent" runat="server">
</asp:Content>
