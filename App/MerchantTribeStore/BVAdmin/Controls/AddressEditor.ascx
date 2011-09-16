<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="BVCommerce.BVAdmin_Controls_AddressEditor" Codebehind="AddressEditor.ascx.cs" %>
     <table width="300" cellpadding="1" cellspacing="0">
        <tr>
            <td class="formlabel">
                Country:
            </td>
            <td class="formfield">
                <asp:DropDownList ID="lstCountry" TabIndex="101" runat="server" 
                    AutoPostBack="true" onselectedindexchanged="lstCountry_SelectedIndexChanged">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td class="formlabel">
                First:&nbsp;</td>
            <td class="formfield">
                <asp:TextBox ID="firstNameField" TabIndex="103" runat="server"
                    Columns="10" Width="100px"></asp:TextBox>
                <bvc5:BVRequiredFieldValidator ID="valFirstName"
                        runat="server" ErrorMessage="First Name is Required" ForeColor=" " ControlToValidate="firstNameField" CssClass="errormessage">*</bvc5:BVRequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="formlabel">
                Last:
            </td>
            <td class="formfield">
                <asp:TextBox ID="lastNameField" TabIndex="105" runat="server"
                    Columns="20" Width="150px"></asp:TextBox><bvc5:BVRequiredFieldValidator ID="valLastName" runat="server"
                        ErrorMessage="Last Name is Required" ForeColor=" " ControlToValidate="lastNameField" CssClass="errormessage">*</bvc5:BVRequiredFieldValidator></td>
        </tr>
        <tr id="CompanyNameRow" runat="server">
            <td class="formlabel">
                Company:</td>
            <td class="formfield">
                <asp:TextBox ID="CompanyField" TabIndex="106" runat="server"
                    Columns="20" Width="150px"></asp:TextBox>
                <bvc5:BVRequiredFieldValidator ID="valCompany" runat="server" ControlToValidate="CompanyField"
                    ForeColor=" " ErrorMessage="Company is Required" Enabled="False" CssClass="errormessage">
				    *</bvc5:BVRequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="formlabel">
                Address:
            </td>
            <td class="formfield">
                <asp:TextBox ID="address1Field" TabIndex="107" runat="server"
                    Columns="20" Width="150px"></asp:TextBox><bvc5:BVRequiredFieldValidator ID="valAddress" runat="server"
                        ErrorMessage="Address is Required" ForeColor=" " ControlToValidate="address1Field" CssClass="errormessage">*</bvc5:BVRequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="formlabel">
                &nbsp;
            </td>
            <td class="formfield">
                <asp:TextBox ID="address2Field" TabIndex="108" runat="server"
                    Columns="20" Width="150px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">
                &nbsp;
            </td>
            <td class="formfield">
                <asp:TextBox ID="address3Field" TabIndex="108" runat="server"
                    Columns="20" Width="150px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">
                City:
            </td>
            <td class="formfield">
                <asp:TextBox ID="cityField" TabIndex="109" runat="server" Columns="20" Width="150px"></asp:TextBox><bvc5:BVRequiredFieldValidator
                    ID="valCity" runat="server" ErrorMessage="City is Required" ForeColor=" " ControlToValidate="cityField" CssClass="errormessage">*</bvc5:BVRequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="formlabel">
                State, Zip:&nbsp;</td>
            <td class="formfield">
               <asp:DropDownList ID="lstState" TabIndex="110" runat="server">
                </asp:DropDownList> <asp:TextBox ID="stateField" TabIndex="111"
                    runat="server" Columns="10" Visible="False"></asp:TextBox>&nbsp;
                <asp:TextBox ID="postalCodeField" TabIndex="112" runat="server"
                    Columns="10"></asp:TextBox><bvc5:BVRequiredFieldValidator ID="valPostalCode" runat="server"
                        ErrorMessage="Postal Code is Required" ForeColor=" " ControlToValidate="postalCodeField" CssClass="errormessage">*</bvc5:BVRequiredFieldValidator>
                <asp:Label ID="lblStateError" runat="server" Visible="False">
				    *</asp:Label>
            </td>
        </tr>        
        <tr id="PhoneRow" runat="server">
            <td class="formlabel">
                Phone:
            </td>
            <td class="formfield">
                <asp:TextBox ID="PhoneNumberField" TabIndex="114" runat="server"
                    Columns="20" Width="150px"></asp:TextBox>
                <bvc5:BVRequiredFieldValidator ID="valPhone" runat="server" ControlToValidate="PhoneNumberField"
                    ForeColor=" " ErrorMessage="Phone Number is Required" Enabled="False" CssClass="errormessage">
				    *</bvc5:BVRequiredFieldValidator></td>
        </tr>
    </table>
    <asp:HiddenField ID="AddressBvin" runat="server" />
    <asp:HiddenField ID="AddressTypeField" runat="server" />
    <asp:HiddenField ID="StoreId" runat="server" />