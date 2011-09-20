<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_VolumeDiscounts" Codebehind="VolumeDiscounts.ascx.cs" %>
<asp:Panel ID="pnlVolumeDiscounts" runat="server">
    <div id="VolumeDiscounts">
        <h3><asp:Label ID="lblVolumeDiscounts" runat="server" CssClass="ProductPropertyLabel">Volume Discounts</asp:Label></h3>
        <asp:DataGrid ID="dgVolumeDiscounts" runat="server" CellPadding="3" BorderWidth="0"
            CellSpacing="0" AutoGenerateColumns="False" GridLines="None">
            <AlternatingItemStyle CssClass="VolumePricingText"></AlternatingItemStyle>
            <ItemStyle CssClass="VolumePricingText"></ItemStyle>
            <HeaderStyle CssClass="VolumePricingHeader"></HeaderStyle>
            <Columns>
                <asp:BoundColumn DataField="Qty" HeaderText="Qty"></asp:BoundColumn>
                <asp:BoundColumn DataField="Amount" HeaderText="Price" DataFormatString="{0:c}"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid></div>
</asp:Panel>