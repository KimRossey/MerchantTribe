<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_PaymentMethods_Google_Checkout_Edit" Codebehind="Edit.ascx.cs" %>
<h1>Google Checkout Options</h1>
<asp:Panel ID="pnlMain" DefaultButton="btnSave" runat="server">
<asp:ValidationSummary ID="ValidationSummary1" runat="server" />
<table border="0" cellspacing="0" cellpadding="3">
<tr>
    <td colspan="2">
        <h2>General Settings</h2>
    </td>
</tr>
<tr>
    <td class="formlabel">Merchant Id:</td>
    <td class="formfield">
        <asp:TextBox ID="MerchantIdTextBox" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td class="formlabel">Merchant Key:</td>
    <td class="formfield">
        <asp:TextBox ID="MerchantKeyTextBox" runat="server"></asp:TextBox>
    </td>
</tr>
<tr>
    <td class="formlabel">Google Checkout Environment:</td>
    <td class="formfield"><asp:RadioButtonList runat="server" ID="ModeRadioButtonList">
    <asp:ListItem Selected="true" Text="Test" Value="Sandbox"></asp:ListItem>
    <asp:ListItem Text="Production" Value="Production"></asp:ListItem>    
    </asp:RadioButtonList></td>
</tr>
<tr>
    <td class="formlabel">Google Monetary Format: </td>
    <td class="formfield"><asp:RadioButtonList runat="server" ID="GoogleMonetaryFormatRadioButtonList">
        <asp:ListItem Selected="true" Text="U.S. Dollars" Value="USD"></asp:ListItem>    
    </asp:RadioButtonList></td>
</tr>
<tr>
    <td class="formlabel">Google Checkout Button Size: </td>
    <td class="formfield">
        <asp:DropDownList ID="CheckoutButtonSizeDropDownList" runat="server">
            <asp:ListItem Selected="true" Text="Small (160x43)" Value="Small"></asp:ListItem>    
            <asp:ListItem Text="Medium (168x44)" Value="Medium"></asp:ListItem>    
            <asp:ListItem Text="Large (180x46)" Value="Large"></asp:ListItem>    
        </asp:DropDownList>        
    </td>
</tr>
<tr>
    <td class="formlabel">Google Checkout Button Background: </td>
    <td class="formfield">
        <asp:DropDownList ID="CheckoutButtonBackgroundDropDownList" runat="server">
            <asp:ListItem Selected="true" Text="Transparent" Value="Transparent"></asp:ListItem>    
            <asp:ListItem Text="White" Value="White"></asp:ListItem>    
        </asp:DropDownList>        
    </td>
</tr>
<tr>
    <td class="formlabel">Debug Mode:</td>
    <td class="formfield">
        <asp:CheckBox ID="DebugModeCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td class="formlabel">Number Of Minutes For A User's Google Cart To Stay valid.<br /> (0 = does not expire)</td>
    <td class="formfield">        
        <asp:TextBox ID="CartValidMinutesTextBox" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CartValidMinutesTextBox" Text="*" ErrorMessage="Cart Valid Minutes Is Required."></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="CartValidMinutesTextBox" Text="*" ErrorMessage="Cart Valid Minutes Must Be Numeric" ValidationExpression="\d+"></asp:RegularExpressionValidator>
        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="CartValidMinutesTextBox" Text="*" ErrorMessage="Cart Valid Minutes Must Be A Positive Numeric Value" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When Google Account Is Less Than</td>
    <td class="formfield">        
        <asp:TextBox ID="DaysOldTextBox" runat="server"></asp:TextBox> days old.
        <asp:RequiredFieldValidator ID="DaysOldRequiredFieldValidator" runat="server" ControlToValidate="DaysOldTextBox" Text="*" ErrorMessage="Account Days Old Is Required."></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="DaysOldRegularExpressionValidator" runat="server" ControlToValidate="DaysOldTextBox" Text="*" ErrorMessage="Account Days Old Must Be Numeric" ValidationExpression="\d+"></asp:RegularExpressionValidator>
        <asp:RangeValidator ID="DaysOldRangeValidator" runat="server" ControlToValidate="DaysOldTextBox" Text="*" ErrorMessage="Account Days Old Must Be A Positive Numeric Value" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
    </td>
</tr>
<tr>
    <td colspan="2">
        <h2>AVS (Address Verification) Settings</h2>
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When AVS Does Not Receive A Match:</td>
    <td class="formfield">
        <asp:CheckBox ID="AVSFailsCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When AVS Receives Only A Partial Match:</td>
    <td class="formfield">
        <asp:CheckBox ID="AVSPartialMatchCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When AVS Is Not Supported By Card Issuer:</td>
    <td class="formfield">
        <asp:CheckBox ID="AVSNotSupportedCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When AVS Returns An Error:</td>
    <td class="formfield">
        <asp:CheckBox ID="AVSErrorCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td colspan="2">
        <h2>CVN (Credit Verification Number) Settings</h2>
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When CVN Does Not Return A Match:</td>
    <td class="formfield">
        <asp:CheckBox ID="CVNNoMatchCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When CVN Is Not Supported By Card Issuer:</td>
    <td class="formfield">
        <asp:CheckBox ID="CVNNotAvailableCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When CVN Returns An Error:</td>
    <td class="formfield">
        <asp:CheckBox ID="CVNErrorCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td colspan="2">
        <h2>Google Payment Guarantee Settings</h2>
    </td>
</tr>
<tr>
    <td class="formlabel">Put Orders On Hold When Order Is Not <br />Eligible For Google Payment Guarantee:</td>
    <td class="formfield">
        <asp:CheckBox ID="GoogleProtectionEligibleCheckBox" runat="server" /> 
    </td>
</tr>
<tr>
    <td colspan="2">
        <h2>Shipping Settings</h2>
    </td>
</tr>
<tr>
    <td class="formlabel">Default Shipping Type</td>
    <td class="formfield">
        <asp:RadioButtonList runat="server" ID="DefaultShippingTypeRadioButtonList">
            <asp:ListItem Selected="true" Text="Amount Per Weight Unit" Value="0"></asp:ListItem>
            <asp:ListItem Text="Amount Per Monetary Unit" Value="1"></asp:ListItem>
            <asp:ListItem Text="Flat Rate" Value="2"></asp:ListItem>
        </asp:RadioButtonList>
    </td>
</tr>
<tr>
    <td class="formlabel">Base Default Shipping Amount: <br />(Used if a value is not supplied below)</td>
    <td class="formfield">
      <asp:TextBox ID="BaseDefaultShippingTextBox" runat="server"></asp:TextBox>
      <asp:CustomValidator ID="BaseDefaultShippingCustomValidator" runat="server" ControlToValidate="BaseDefaultShippingTextBox"
          ErrorMessage="Base Default Shipping Amount Must Be A Valid Monetary Amount." Text="*"></asp:CustomValidator></td>
</tr>
<asp:PlaceHolder ID="ShippingSettingsPlaceHolder" runat="server"></asp:PlaceHolder>
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td class="formlabel">
        <asp:ImageButton ID="btnCancel" CausesValidation="false" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/Cancel.png" onclick="btnCancel_Click" /></td>
    <td class="formfield">
        <asp:ImageButton ID="btnSave" runat="server" 
            ImageUrl="~/BVAdmin/Images/Buttons/SaveChanges.png" onclick="btnSave_Click" /></td>
</tr>
</table></asp:Panel>
