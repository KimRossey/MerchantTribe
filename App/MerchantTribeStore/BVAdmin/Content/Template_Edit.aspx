<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Template_Edit" title="Untitled Page" Codebehind="Template_Edit.aspx.cs" %>

<%@ Register Src="../Controls/HtmlEditor.ascx" TagName="HtmlEditor" TagPrefix="uc1" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc2:MessageBox ID="MessageBox1" runat="server" />
<h1>Edit Template</h1>
<table border="0" cellspacing="0" cellpadding="5">
<tr>
    <td class="formlabel">Template:</td>
    <td class="formfield"><uc1:HtmlEditor ID="TemplateField" EditorHeight="300" EditorWidth="700"
                        EditorWrap="true" runat="server" /></td>
</tr>
<tr>
    <td class="formlabel"></td>
    <td class="formfield"><asp:ImageButton ID="btnSave" runat="server" 
            imageurl="~/BVAdmin/Images/Buttons/SaveChanges.png" 
            AlternateText="Save Changes" onclick="btnSave_Click" /></td>
</tr>
</table>
</asp:Content>



