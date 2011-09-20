<%@ Control Language="C#" AutoEventWireup="True" Inherits="MerchantTribeStore.BVModules_Controls_ViewOrderItems" Codebehind="ViewOrderItems.ascx.cs" %>
<asp:GridView Style="margin: 20px 0px 20px 0px; border-bottom: solid 1px #666;" GridLines="None"
                ID="ItemsGridView" runat="server" AutoGenerateColumns="False" 
    Width="100%" DataKeyNames="Id" onrowdatabound="ItemsGridView_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-VerticalAlign="top">
                        <ItemTemplate>
                            <asp:CheckBox ID="SelectedCheckBox" runat="server" Checked="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item" ItemStyle-VerticalAlign="top">
                        <ItemTemplate>                            
                                <asp:Label ID="SKUField" runat="server"></asp:Label>
                                <asp:Label ID="DescriptionField" runat="server"></asp:Label>                                
                                <asp:Literal ID="litDiscounts" runat="server"></asp:Literal>&nbsp;
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Shipping" ItemStyle-VerticalAlign="top">
                        <ItemTemplate>
                            <asp:Label ID="ShippingStatusField" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price" ItemStyle-VerticalAlign="top">
                        <ItemTemplate>
                            <asp:Label ID="AdjustedPriceField" runat="server" Text='<%# Bind("BasePricePerItem", "{0:c}") %>'></asp:Label><br />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty" ItemStyle-VerticalAlign="top">
                        <ItemTemplate>
                            <asp:Label ID="QuantityField" runat="server" Text='<%# Bind("Quantity","{0:#}") %>'></asp:Label><br />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Line Total" ItemStyle-VerticalAlign="top">
                        <ItemTemplate>
                            <asp:Literal ID="LineTotalField" runat="server"></asp:Literal>                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle CssClass="row" />
                <HeaderStyle CssClass="rowheader" />
                <AlternatingRowStyle CssClass="alternaterow" />
            </asp:GridView>
            <asp:Panel ID="pnlReturn" runat="server">
            <asp:ImageButton ID="ReturnItemsImageButton" runat="server" 
                    ImageUrl="~/BVModules/Themes/BVC5/Images/Buttons/ReturnItems.png" 
                    AlternateText="Return selected items" onclick="ReturnItemsImageButton_Click" /><br />
            <div class="errorDiv">
                <asp:Label ID="returnErrorLabel" runat="server" Text="" EnableViewState="False"></asp:Label>
            </div>
            (Please select the items to return and press the "Return Items" button.)
            </asp:Panel>