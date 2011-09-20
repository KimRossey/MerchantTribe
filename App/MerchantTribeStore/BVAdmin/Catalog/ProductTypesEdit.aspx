<%@ Page MasterPageFile="~/BVAdmin/BVAdmin.master"  ValidateRequest="False" Language="C#"
    AutoEventWireup="True" Inherits="MerchantTribeStore.Product_ProductTypes_Edit" Codebehind="ProductTypesEdit.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        Edit Product Type</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <table class="FormTable">
        <tr>
            <td class="FormLabel" align="right">
                Product Type&nbsp;Name&nbsp;
            </td>
            <td class="FormLabel" align="left">
                <asp:TextBox ID="ProductTypeNameField" runat="server" CssClass="FormInput" Columns="40"></asp:TextBox><bvc5:BVRequiredFieldValidator
                    ID="RequiredFieldValidator1" runat="server" ErrorMessage="Product Type Name is Required"
                    ControlToValidate="ProductTypeNameField">*</bvc5:BVRequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="FormLabel" colspan="2">
                <table cellspacing="0" cellpadding="3" border="0">
                    <tr>
                        <td class="FormLabel" valign="middle" align="right">
                            <asp:ImageButton ID="btnMovePropertyUp" runat="server" AlternateText="Move Up" 
                                ImageUrl="~/BVAdmin/images/buttons/up.png" onclick="btnMovePropertyUp_Click">
                            </asp:ImageButton><br />
                            <br />
                            <asp:ImageButton ID="btnMovePropertyDown" runat="server" AlternateText="Move Down"
                                ImageUrl="~/BVAdmin/images/buttons/down.png" 
                                onclick="btnMovePropertyDown_Click"></asp:ImageButton></td>
                        <td class="FormLabel" valign="top" align="left">
                            Selected&nbsp;Properties<br />
                            <asp:ListBox ID="lstProperties" runat="server" Rows="10"></asp:ListBox></td>
                        <td class="FormLabel" valign="middle" align="center">
                            <asp:ImageButton ID="btnAddProperty" runat="server" 
                                ImageUrl="~/BVAdmin/images/buttons/Add.png" onclick="btnAddProperty_Click">
                            </asp:ImageButton><br />
                            <br />
                            <asp:ImageButton ID="btnRemoveProperty" runat="server" 
                                ImageUrl="~/BVAdmin/images/buttons/Remove.png" 
                                onclick="btnRemoveProperty_Click">
                            </asp:ImageButton></td>
                        <td class="FormLabel" valign="top" align="left">
                            Available Properties<br />
                            <asp:ListBox ID="lstAvailableProperties" runat="server" Rows="10" SelectionMode="Multiple">
                            </asp:ListBox></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="FormLabel" align="left">
                <asp:ImageButton ID="btnCancel" runat="server" CausesValidation="False" AlternateText="Cancel"
                    ImageUrl="~/BVAdmin/images/buttons/Cancel.png" onclick="btnCancel_Click"></asp:ImageButton></td>
            <td class="FormLabel" align="right">
                <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save Changes" 
                    ImageUrl="~/BVAdmin/images/buttons/SaveChanges.png" onclick="btnSave_Click">
                </asp:ImageButton></td>
        </tr>
    </table>
</asp:Content>
