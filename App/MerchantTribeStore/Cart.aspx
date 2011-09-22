<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="True" Inherits="MerchantTribeStore.Cart" Title="Shopping Cart" Codebehind="Cart.aspx.cs" %>

<%@ Register Src="BVModules/Controls/GoogleCheckoutButton.ascx" TagName="GoogleCheckoutButton"
    TagPrefix="uc5" %>
<%@ Register Src="BVModules/Controls/PaypalExpressCheckoutButton.ascx" TagName="PaypalExpressCheckoutButton"
    TagPrefix="uc4" %>
<%@ Register Src="BVModules/Controls/EstimateShipping.ascx" TagName="EstimateShipping"
    TagPrefix="uc2" %>
<%@ Register Src="BVModules/Controls/MessageBox.ascx" TagName="MessageBox" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
   
    <asp:Panel ID="pnlAll" DefaultButton="btnUpdateQuantities" runat="server">
        <div id="carttitle">
            <h1>
                <asp:Label ID="TitleLabel" runat="server">Shopping Cart</asp:Label>
            </h1>
            <uc1:MessageBox ID="MessageBox1" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
            <div id="cartsubtitle">
                <h3>
                    <asp:Label ID="lblcart" runat="server">Your Cart is Empty</asp:Label></h3>
            </div>
        </div>
        <asp:Panel ID="pnlWholeCart" runat="server" Visible="true">
            <div id="cartcontainer">
                &nbsp;<asp:Literal ID="ItemListLiteral" runat="server"></asp:Literal>
                <asp:GridView GridLines="None" ID="ItemGridView" runat="server" AutoGenerateColumns="False"
                    Width="100%" DataKeyNames="Id" CssClass="cartproductgrid" 
                    EnableViewState="false" onrowdatabound="ItemGridView_RowDataBound" 
                    onrowdeleting="ItemGridView_RowDeleting" >
                    <Columns>
                        <asp:TemplateField HeaderText="Product" HeaderStyle-HorizontalAlign="left" ItemStyle-CssClass="productimagecolumn" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <div class="cartitemimage">
                                    <asp:Image ID="imgProduct" runat="server" AlternateText="" />
                                </div>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false" ItemStyle-CssClass="productdetailscolumn" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <div id="cartitemdescription" runat="server" class="cartitemdescription">                                    
                                    <asp:Hyperlink ID="DescriptionLink" runat="server"></asp:Hyperlink>
                                    <asp:PlaceHolder ID="CartInputModifiersPlaceHolder" runat="server"></asp:PlaceHolder>                                    
                                    <asp:PlaceHolder ID="KitDisplayPlaceHolder" runat="server"></asp:PlaceHolder>
                                    <asp:Literal ID="litDiscounts" runat="server"></asp:Literal>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price" HeaderStyle-HorizontalAlign="right" ItemStyle-CssClass="productpricecolumn" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <span class="cartproductprice"><asp:Literal ID="Label1" runat="server" Text='<%# Bind("BasePricePerItem", "{0:c}") %>'></asp:Literal></span>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="right" ItemStyle-CssClass="productquantitycolumn" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <div>
                                    <asp:TextBox ID="QtyField" Width="50" runat="server" Text='<%# Bind("Quantity","{0:#}") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Quantity Must Be Entered"
                                    ControlToValidate="QtyField" Text="*" ForeColor=" " CssClass="errormessage"></asp:RequiredFieldValidator>                                    
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Quantity Must Be Numeric"
                                        ControlToValidate="QtyField" ValidationExpression="\d{1,6}" ForeColor=" " CssClass="errormessage"></asp:RegularExpressionValidator>
                                </div>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" HeaderStyle-HorizontalAlign="right" ItemStyle-CssClass="producttotalcolumn" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <span class="lineitemnodiscounts"><asp:Literal ID="TotalWithoutDiscountsLabel" runat="server" Text='' Visible="false"></asp:Literal></span>
                                <span class="totallabel"><asp:Literal ID="TotalLabel" runat="server" Text=''></asp:Literal></span>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="productdeletecolumn" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                                    ImageUrl="~/content/themes/theme-914951ab-fc9b-46aa-8e99-ac76be3f8a5b/buttons/x.png" AlternateText="Delete" />                                
                            </ItemTemplate> 
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="cartfooter">
                <div id="carttotals">
                    <asp:Literal ID="litTotals" runat="server"></asp:Literal>
                    <table border="0" cellspacing="0" cellpadding="3">
                        <tr>
                            <td class="formlabel">
                                <asp:Literal ID="litSubTotalName" runat="server"></asp:Literal></td>
                            <td class="formfield">
                                <asp:Label ID="lblSubTotal" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <asp:Literal ID="litDiscounts" runat="server"></asp:Literal>                        
                    </table>
                </div>
                <div id="cartupdates">
                    <span>
                        <asp:Label ID="lblMakeChanges" runat="server">Make any changes above? </asp:Label>&nbsp;</span>
                    <asp:ImageButton ID="btnUpdateQuantities" runat="server" ImageUrl="~/content/themes/theme-914951ab-fc9b-46aa-8e99-ac76be3f8a5b/buttons/Update.png"
                        AlternateText="Update" onclick="btnUpdateQuantities_Click" />
                </div>
                <uc2:EstimateShipping ID="EstimateShipping1" runat="server" />
            </div>
            <div id="cartactions">
                <div id="cartactioncontinue">
                    <asp:ImageButton ID="btnContinueShopping" runat="server" ImageUrl="~/content/themes/theme-914951ab-fc9b-46aa-8e99-ac76be3f8a5b/buttons/keepshopping.png"
                        AlternateText="Continue Shopping" onclick="btnContinueShopping_Click" />
                </div>
                <div id="cartactioncheckout" runat="server">
                    <asp:ImageButton ID="btnCheckout" runat="server" ImageUrl="~/content/themes/theme-914951ab-fc9b-46aa-8e99-ac76be3f8a5b/buttons/placeorder.png"
                        AlternateText="Checkout" onclick="btnCheckout_Click" />
                    <uc5:GoogleCheckoutButton ID="GoogleCheckoutButton1" runat="server" />
                    <uc4:PaypalExpressCheckoutButton ID="PaypalExpressCheckoutButton1" runat="server"
                        DisplayText="false" />
                </div>
            </div>
            <div id="cartcoupons">
                <asp:Panel ID="pnlCoupons" runat="server" DefaultButton="btnAddCoupon">
                    Add a Promotional Code:
                    <asp:TextBox ID="CouponField" runat="server"></asp:TextBox>
                    <asp:ImageButton ID="btnAddCoupon" runat="server" ImageUrl="~/content/themes/theme-914951ab-fc9b-46aa-8e99-ac76be3f8a5b/buttons/submit.png"
                        AlternateText="Add Coupon to Cart" onclick="btnAddCoupon_Click" /><br />
                    <asp:GridView EnableViewState="false" CellPadding="3" CellSpacing="0" 
                        GridLines="none" ID="CouponGrid" runat="server"
                        AutoGenerateColumns="False" DataKeyNames="Id" ShowHeader="False" 
                        onrowdatabound="CouponGrid_RowDataBound" onrowdeleting="CouponGrid_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="CouponCode" ShowHeader="False" />
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDeleteCoupon" ImageUrl="~/content/themes/theme-914951ab-fc9b-46aa-8e99-ac76be3f8a5b/buttons/x.png"
                                        runat="server" CausesValidation="false" CommandName="Delete" AlternateText="Delete Coupon" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>                        
        </asp:Panel>
    </asp:Panel>
</asp:Content>
