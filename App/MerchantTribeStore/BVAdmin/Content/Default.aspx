<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_Default" Title="Untitled Page" Codebehind="Default.aspx.cs" %>

<%@ Register Src="../Controls/ContentColumnEditor.ascx" TagName="ContentColumnEditor"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>
        Home Page</h1>
    <table border="0" cellspacing="0" cellpadding="0" width="910">
    <tr>
        <td valign="top" align="left" width="225"><div style="margin: 0px;" class="smallcolumneditor">
            <uc1:ContentColumnEditor ID="ContentColumnEditor1" runat="server" ColumnId="1" />
        </div></td>
        <td valign="top" align="left"><div style="margin: 0px;">
            <uc1:ContentColumnEditor ID="ContentColumnEditor2" runat="server" ColumnId="2" />
        </div></td>
        <td valign="top" align="left" width="225"> <div style="margin: 0px;" class="smallcolumneditor">
            <uc1:ContentColumnEditor ID="ContentColumnEditor3" runat="server" ColumnId="3" />
        </div></td>
    </tr>
    </table>       
</asp:Content>
