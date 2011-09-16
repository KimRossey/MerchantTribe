<%@ Control Language="C#" AutoEventWireup="True" Inherits="BVCommerce.BVModules_Controls_StaticAddressDisplay" Codebehind="StaticAddressDisplay.ascx.cs" %>
<table>
    <tr>
        <td>Name:</td>
        <td><asp:Label ID="NameLabel" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr id="CompanyRow" runat="server">
        <td>Company:</td>
        <td><asp:Label ID="CompanyLabel" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td>Address:</td>
        <td><asp:Label ID="AddressLineOneLabel" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr id="LineTwoRow" runat="server">
        <td></td>
        <td><asp:Label ID="AddressLineTwoLabel" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr id="LineThreeRow" runat="server">
        <td></td>
        <td><asp:Label ID="AddressLineThreeLabel" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td></td>
        <td><asp:Label ID="AddressLineFourLabel" runat="server" Text=""></asp:Label></td>
    </tr>
</table>