<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="BVCommerce.Receipt" Codebehind="Receipt.aspx.cs" %>

<%@ Register Src="BVModules/Controls/ViewOrder.ascx" TagName="ViewOrder" TagPrefix="uc1" %>
<%@ Register src="BVModules/Controls/MessageBox.ascx" tagname="MessageBox" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<div class="content-wrapper">
<uc2:MessageBox ID="MessageBox1" runat="server" />
<h1>Thank You! Order Received.</h1>    
<asp:Label ID="lblOrderNumber" runat="server"></asp:Label>
    <asp:Panel ID="DownloadsPanel" runat="server" Visible="false">
        <h2>Your Order Contains File Downloads</h2>
        <asp:GridView ID="FilesGridView" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="bvin" 
            onrowdatabound="FilesGridView_RowDataBound" Width="100%" 
            onrowediting="FilesGridView_RowEditing">
            <Columns>
                <asp:BoundField DataField="ShortDescription" HeaderText="File Name" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="DownloadImageButton" runat="server" AlternateText="Download" CommandName="Edit" ImageUrl="~/BVModules/Themes/Print Book/images/Buttons/Download.png" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView><br />
        &nbsp;<br />
    </asp:Panel>
    <asp:HyperLink ID="lnkDownloadAgain" runat="Server" Visible="false" NavigateUrl="~/MyAccount_Orders.aspx" Text="You can download files at anytime from the My Account section of the store."></asp:HyperLink>    
    <br />&nbsp;<br />    
<uc1:ViewOrder DisableReturns="true" ID="ViewOrder1" runat="server" DisableNotesAndPayment="true" DisableStatus="true" />
</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="EndOfForm" runat="server">    
</asp:Content>

