<%@ Page Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.nopermissions" Codebehind="NoPermissions.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Pemissions Error</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:50px;margin-left:auto;margin-right:auto;margin-bottom:50px;">
    <table cellpadding="3" cellspacing="0" border="0">
        <tr>
            <td align="left" valign="top" class="errorMessage">
                You do not have permission to view this page.<br />
                &nbsp;<br />
                Try logging in as a different user or contact the site administrator for more details.</td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:HyperLink ID="lnkStore" NavigateUrl="~/default.aspx" runat="Server">Return to the Store</asp:HyperLink>
                </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:HyperLink runat="server" ID="lnkLogin" NavigateUrl="~/login.aspx">Login as a different user.</asp:HyperLink>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
