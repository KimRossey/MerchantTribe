<%@ Page Language="C#" MasterPageFile="~/BVAdmin/BVAdmin.master" AutoEventWireup="True" Inherits="MerchantTribeStore.BVAdmin_Content_CategoryTemplates" title="Untitled Page" Codebehind="CategoryTemplates.aspx.cs" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <h1>Category Templates</h1>
    <uc1:MessageBox ID="msg" runat="server" />
    <table border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td align="left" valign="middle">
                <h2>
                    <asp:Label ID="lblResults" runat="server">No Results</asp:Label></h2>
            </td>
            <td class="formlabel" align="right" valign="middle">
                </td>
        </tr>
    </table>
    &nbsp;
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
        BorderColor="#CCCCCC" CellPadding="3" GridLines="None" Width="100%" 
        onrowcommand="GridView1_RowCommand" onrowdatabound="GridView1_RowDataBound">
        <Columns>
            <asp:TemplateField HeaderText="Template Name">
                <ItemTemplate>
                    <asp:Label runat="server" Id="TemplateNameLabel"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton ID="EditLinkButton" runat="server" CausesValidation="False" CommandName="Edit"
                        Text="Edit"></asp:LinkButton>                    
                </ItemTemplate>                
            </asp:TemplateField>            
        </Columns>
        <RowStyle CssClass="row" />
        <HeaderStyle CssClass="rowheader" />
        <AlternatingRowStyle CssClass="alternaterow" />
    </asp:GridView>
</asp:Content>


