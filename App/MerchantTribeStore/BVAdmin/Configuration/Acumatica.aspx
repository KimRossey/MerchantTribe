<%@ Page Title="" Language="C#" MasterPageFile="~/BVAdmin/BVAdminNav.master" AutoEventWireup="true" CodeBehind="Acumatica.aspx.cs" Inherits="BVCommerce.BVAdmin.Configuration.Acumatica" %>

<%@ Register Src="../Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<%@ Register src="NavMenu.ascx" tagname="NavMenu" tagprefix="uc2" %>
<asp:Content ID="nav" ContentPlaceHolderID="NavContent" runat="server">
    <uc2:NavMenu ID="NavMenu1" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Acumatica ERP Integration</h1>        
        <uc1:MessageBox ID="MessageBox1" runat="server" />
        <asp:Label id="lblError" runat="server" CssClass="errormessage"></asp:Label>
        <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnSave">
        <table border="0" cellspacing="0" cellpadding="3">
        <tr>
            <td class="formlabel">Enable Acumatica Integration:</td>
            <td class="formfield">
                 <asp:CheckBox ID="chkEnableAcumatica" runat="server" /></td>
        </tr>     
        <tr>
            <td class="formlabel">Username:</td>
            <td class="formfield">
                <asp:TextBox ID="AcumaticaUsername" Columns="20" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Password:</td>
            <td class="formfield">
                <asp:TextBox ID="AcumaticaPassword" Columns="20" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="formlabel">Acumatica URL:</td>
            <td class="formfield">
                <asp:TextBox ID="AcumaticaSiteUrl" Columns="40" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;<br /></td>
        </tr>
        <tr>
            <td class="formlabel">Pull Data Frequency:</td>
            <td class="formfield">
                <asp:DropDownList ID="lstFrequency" runat="server">        
                    <asp:ListItem Text="Never" Value="-1"></asp:ListItem>         
                    <asp:ListItem Text="Every 5 Minutes" Value="5"></asp:ListItem>   
                    <asp:ListItem Text="Every 30 Minutes" Value="30"></asp:ListItem>
                    <asp:ListItem Text="Every Hour" Value="60"></asp:ListItem>
                    <asp:ListItem Text="Every 6 Hours" Value="360"></asp:ListItem>
                    <asp:ListItem Text="Every 12 Hours" Value="720"></asp:ListItem>
                    <asp:ListItem Text="Once A Day" Value="1440"></asp:ListItem>
                    <asp:ListItem Text="Once A Week" Value="10080"></asp:ListItem>
                </asp:DropDownList> <asp:LinkButton runat="server" ID="btnPullData" 
                    Text="<b>Pull Data Now</b>" CssClass="btn" onclick="btnPullData_Click"></asp:LinkButton><br />
                    <span class="tiny">Last Pulled Data at: <asp:Literal ID="litLastPulled" runat="server" /><br />
                    Next Pull At: <asp:Literal id="litNextPull" runat="server"></asp:Literal></span>
            </td>
        </tr>        
        <tr>
            <td colspan="2">&nbsp;<br /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="formfield"><asp:LinkButton ID="lnkTest" runat="server" CssClass="btn" 
                    Text="<b>Test Connection</b>" onclick="lnkTest_Click"></asp:LinkButton></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td class="formfield"><a href="Acumatica2.aspx">Configure Mapping Settings</a></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
                <td class="formlabel">
                    <asp:ImageButton ID="btnCancel" runat="server" ImageUrl="../images/buttons/Cancel.png"
                        CausesValidation="False" onclick="btnCancel_Click"></asp:ImageButton></td>
                <td class="formfield"><asp:ImageButton ID="btnSave" CausesValidation="true"
                            runat="server" ImageUrl="../images/buttons/SaveChanges.png" 
                        onclick="btnSave_Click"></asp:ImageButton></td>
            </tr>            
        </table>
        </asp:Panel>
        
</asp:Content>


