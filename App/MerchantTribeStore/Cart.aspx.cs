using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.BusinessRules;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.Catalog;
using System.Linq;
using MerchantTribe.Commerce.Marketing;

namespace BVCommerce
{

    partial class Cart : BaseStorePage
    {

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.Title = SiteTerms.GetTerm(SiteTermIds.ShoppingCart);

            this.PaypalExpressCheckoutButton1.CheckoutButtonClicked += this.PaypalExpressCheckoutClicked;
            this.GoogleCheckoutButton1.CheckoutButtonClicked += this.GoogleCheckoutClicked;
            this.GoogleCheckoutButton1.WorkflowFailed += this.GoogleWorkflowFailed;
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.AddBodyClass("store-cart-page");
        }

        ThemeManager tm = null;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            WebForms.MakePageNonCacheable(this);

            tm = MTApp.ThemeManager();

            this.btnContinueShopping.ImageUrl = tm.ButtonUrl("keepshopping", Request.IsSecureConnection);
            this.btnUpdateQuantities.ImageUrl = tm.ButtonUrl("update", Request.IsSecureConnection);
            this.btnCheckout.ImageUrl = tm.ButtonUrl("securecheckout", Request.IsSecureConnection);
            this.btnAddCoupon.ImageUrl = tm.ButtonUrl("new", Request.IsSecureConnection);

            MessageBox1.ClearMessage();

            if (!Page.IsPostBack)
            {
                CheckForQuickAdd();
            }

            LoadCart();
        }

        public void CheckForQuickAdd()
        {
            if (this.Request.QueryString["quickaddid"] != null)
            {
                string bvin = Request.QueryString["quickaddid"];
                Product prod = MTApp.CatalogServices.Products.Find(bvin);
                if (prod != null)
                {
                    int quantity = 1;
                    if (this.Request.QueryString["quickaddqty"] != null)
                    {
                        int val = 0;
                        if (int.TryParse(Request.QueryString["quickaddqty"], out val))
                        {
                            quantity = val;
                        }
                    }
                    Order o = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                    LineItem li = MTApp.CatalogServices.ConvertProductToLineItem(prod, new OptionSelectionList(), quantity, MTApp);
                    MTApp.AddToOrderWithCalculateAndSave(o, li);
                    SessionManager.SaveOrderCookies(o);
                }
            }
            else if (this.Request.QueryString["quickaddsku"] != null)
            {
                string sku = Request.QueryString["quickaddsku"];
                Product prod = MTApp.CatalogServices.Products.FindBySku(sku);
                if (prod != null)
                {
                    int quantity = 1;
                    if (this.Request.QueryString["quickaddqty"] != null)
                    {
                        int val = 0;
                        if (int.TryParse(Request.QueryString["quickaddqty"], out val))
                        {
                            quantity = val;
                        }
                    }
                    Order o = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                    LineItem li = MTApp.CatalogServices.ConvertProductToLineItem(prod, new OptionSelectionList(), quantity, MTApp);
                    li.Quantity = quantity;
                    MTApp.AddToOrderWithCalculateAndSave(o, li);
                    SessionManager.SaveOrderCookies(o);
                }
            }
            else if (this.Request.QueryString["multi"] != null)
            {
                string[] skus = Request.QueryString["multi"].Split(';');
                Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                bool addedParts = false;

                foreach (string s in skus)
                {
                    string[] skuparts = s.Split(':');
                    string newsku = skuparts[0];
                    string bvin = string.Empty;
                    Product p = MTApp.CatalogServices.Products.FindBySku(newsku);
                    if (p != null)
                    {
                        if (p.Bvin.Trim().Length > 0)
                        {
                                int qty = 1;
                                if (skuparts.Length > 1)
                                {
                                    int.TryParse(skuparts[1], out qty);
                                }
                                if (qty < 1)
                                {
                                    qty = 1;
                                }
                                LineItem li = MTApp.CatalogServices.ConvertProductToLineItem(p, new OptionSelectionList(), qty, MTApp);
                                li.Quantity = qty;
                                Basket.Items.Add(li);
                                addedParts = true;                            
                        }
                    }
                }
                if (addedParts)
                {
                    MTApp.CalculateOrderAndSave(Basket);
                    SessionManager.SaveOrderCookies(Basket);
                }
            }
        }

        private void LoadCart()
        {
            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);

            if (Basket != null)
            {

                if ((Basket.Items == null) || ((Basket.Items != null) && (Basket.Items.Count <= 0)))
                {
                    this.lblcart.Text = "Your shopping cart is empty! <a href=\"" + Page.ResolveUrl(GetKeepShoppingLocation()) + "\">Keep Shopping</a> &gt;&gt;";
                    this.pnlWholeCart.Visible = false;
                }
                else
                {
                    if (Basket.TotalQuantity > 1)
                    {
                        this.lblcart.Text = Basket.TotalQuantity.ToString("#") + " Items in Cart";
                    }
                    else
                    {
                        this.lblcart.Text = Basket.TotalQuantity.ToString("#") + " Item in Cart";
                    }

                    this.pnlWholeCart.Visible = true;

                    //this.litTotals.Text = Basket.TotalsAsTable();

                    if (Basket.OrderDiscountDetails.Count > 0)
                    {
                        this.litSubTotalName.Text = "Before Discounts:";
                        this.lblSubTotal.Text = string.Format("{0:c}", Basket.TotalOrderBeforeDiscounts);

                        this.litDiscounts.Visible = true;
                        StringBuilder sb = new StringBuilder();
                        foreach (DiscountDetail d in Basket.OrderDiscountDetails)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td class=\"formlabel2\">");
                            sb.Append(d.Description + ":");
                            sb.Append("</td>");
                            sb.Append("<td class=\"formfield2\">");
                            sb.Append(string.Format("{0:c}", d.Amount));
                            sb.Append("</td>");
                            sb.Append("</tr>");
                        }

                        sb.Append("<tr>");
                        sb.Append("<td class=\"formlabel\">");
                        sb.Append("Sub Total:</td>");
                        sb.Append("<td class=\"formfield\">");
                        sb.Append(string.Format("{0:c}", Basket.TotalOrderAfterDiscounts));
                        sb.Append("</td>");
                        sb.Append("</tr>");

                        this.litDiscounts.Text = sb.ToString();
                    }
                    else
                    {
                        this.litDiscounts.Visible = false;
                        this.litSubTotalName.Text = "Sub Total:";
                        this.lblSubTotal.Text = string.Format("{0:c}", Basket.TotalOrderBeforeDiscounts);
                    }
                    //Me.lblGrandTotal.Text = String.Format("{0:c}", Basket.GrandTotal)

                    this.ItemGridView.DataSource = Basket.Items;
                    this.ItemGridView.DataBind();

                    if (Basket.TotalQuantity <= 0)
                    {
                        cartactioncheckout.Visible = false;
                    }

                    this.CouponGrid.DataSource = Basket.Coupons;
                    this.CouponGrid.DataBind();
                  
                }
            }
        }

        protected void btnContinueShopping_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MessageBox1.ClearMessage();
            string destination = GetKeepShoppingLocation();
            Response.Redirect(destination);
        }

        private string GetKeepShoppingLocation()
        {
            string result = "~";
            if (SessionManager.CategoryLastId != string.Empty)
            {
                Category c = this.MTApp.CatalogServices.Categories.Find(SessionManager.CategoryLastId);
                if (c != null)
                {
                    if (c.Bvin != string.Empty)
                    {
                        result = UrlRewriter.BuildUrlForCategory(new CategorySnapshot(c), MTApp.CurrentRequestContext.RoutingContext);
                    }
                }
            }
            return result;
        }

        protected void ItemGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LineItem lineItem = (LineItem)e.Row.DataItem;
                if (lineItem != null)
                {                    
                    
                    System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgProduct");
                    if (img != null)
                    {
                        Product associatedProduct = lineItem.GetAssociatedProduct(MTApp);
                        if (associatedProduct != null)
                        {
                            img.Visible = true;
                            img.ImageUrl = MerchantTribe.Commerce.Storage.DiskStorage.ProductVariantImageUrlMedium(MTApp.CurrentStore.Id, lineItem.ProductId, associatedProduct.ImageFileSmall, lineItem.VariantId, Request.IsSecureConnection);
                            img.AlternateText = lineItem.ProductName;
                        }
                    }

                    HyperLink description = (HyperLink)e.Row.FindControl("DescriptionLink");
                    if (description != null)
                    {
                        description.Text = "<div class=\"cartsku\">" + lineItem.ProductSku + "</div><div class=\"cartproductname\">" + lineItem.ProductName + "</div>";
                        description.Text += lineItem.ProductShortDescription;
                        description.NavigateUrl = "#";

                        Product product = MTApp.CatalogServices.Products.Find(lineItem.ProductId);
                        if (product != null)
                        {
                            description.NavigateUrl = UrlRewriter.BuildUrlForProduct(product, this, "OrderBvin=" + lineItem.OrderBvin + "&LineItemId=" + lineItem.Id);
                        }
                    }

                    
                        //if (asociatedProduct != null)
                        //{
                        //    if associatedProduct.IsOutOfStock)
                        //    {
                        //        description.Text = description.Text + "<div class=\"outofstock\">" + SiteTerms.GetTerm("OutOfStockNoPurchase") + "</div>";
                        //        MessageBox1.ShowInformation(SiteTerms.ReplaceTermVariable(SiteTerms.GetTerm("CartOutOfStock"), "ProductName", lineItem.ProductName));
                        //    }
                        //    else if (associatedProduct.IsBackordered)
                        //    {
                        //        description.Text = description.Text + "<div class=\"backordered\">" + SiteTerms.GetTerm("OutOfStockAllowPurchase") + "</div>";
                        //    }
                        //    else if (associatedProduct.IsLowStock((int)lineItem.Quantity))
                        //    {
                        //        description.Text = description.Text + "<div class=\"lowstock\">" + SiteTerms.GetTerm("LowStockLineItem") + " Only " + lineItem.AssociatedProduct.QuantityAvailableForSale.ToString("0") + " are available.</div>";
                        //        MessageBox1.ShowInformation(SiteTerms.ReplaceTermVariable(SiteTerms.GetTerm("CartNotEnoughQuantity"), "ProductName", lineItem.ProductName));
                        //    }
                        //}                    

                    //CustomProperty custproperty = null;
                    //foreach (CustomProperty item in lineItem.CustomProperties)
                    //{
                    //    if (item.DeveloperId == "bvsoftware" && item.Key == "quantitychanged")
                    //    {
                    //        description.Text = description.Text + "<div class=\"quantitychanged\">" + SiteTerms.GetTerm("QuantityChanged") + "</div>";
                    //        custproperty = item;
                    //        MessageBox1.ShowInformation(SiteTerms.GetTerm("LineItemsChanged"));
                    //        break;
                    //    }
                    //}
                    //if (custproperty != null)
                    //{
                    //    lineItem.CustomProperties.Remove(custproperty);
                    //    LineItem.Update(lineItem);
                    //}


                    ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                    if (btnDelete != null)
                    {
                        btnDelete.ImageUrl = tm.ButtonUrl("x", Request.IsSecureConnection);
                    }
                    
                    Literal totalLabel = (Literal)e.Row.FindControl("TotalLabel");
                    totalLabel.Text = lineItem.LineTotal.ToString("c");

                    Literal totalWithoutDiscountsLabel = (Literal)e.Row.FindControl("TotalWithoutDiscountsLabel");
                    if (lineItem.LineTotal != lineItem.LineTotalWithoutDiscounts)
                    {
                        totalWithoutDiscountsLabel.Visible = true;
                        totalWithoutDiscountsLabel.Text = lineItem.LineTotalWithoutDiscounts.ToString("c");

                        Literal litDiscounts = (Literal)e.Row.FindControl("litDiscounts");
                        if (litDiscounts != null)
                        {
                            litDiscounts.Text = "<div class=\"discounts\">" + lineItem.DiscountDetailsAsHtml() + "</div>";
                        }                        
                    }
                    else
                    {
                        totalWithoutDiscountsLabel.Visible = false;
                    }
                }
            }
        }

        protected void ItemGridView_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            MessageBox1.ClearMessage();
            long Id = (long)ItemGridView.DataKeys[e.RowIndex].Value;
            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            if (Basket != null)
            {
                var li = Basket.Items.Where(y => y.Id == Id).SingleOrDefault();
                if (li != null)
                {
                    Basket.Items.Remove(li);
                    MTApp.CalculateOrderAndSave(Basket);
                    SessionManager.SaveOrderCookies(Basket);
                }
                LoadCart();
            }
        }

        protected void btnUpdateQuantities_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                MessageBox1.ClearMessage();
                GetCurrentLineItemQuantities();
                LoadCart();
            }
        }

        public bool GetCurrentLineItemQuantities()
        {
            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            if (Basket != null)
            {
                for (int i = 0; i <= ItemGridView.Rows.Count - 1; i++)
                {
                    if (ItemGridView.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        TextBox qty = (TextBox)ItemGridView.Rows[i].FindControl("QtyField");
                        if (qty != null)
                        {
                            long itemId = (long)ItemGridView.DataKeys[ItemGridView.Rows[i].RowIndex].Value;
                            SystemOperationResult opResult = MTApp.OrderServices.OrdersUpdateItemQuantity(itemId, int.Parse(qty.Text), Basket);                            
                            if (opResult.Message != string.Empty)
                            {
                                MessageBox1.ShowInformation(opResult.Message);
                            }
                            if (!opResult.Success)
                            {
                                return false;
                            }
                        }
                    }
                }

                Dictionary<string, int> productQuantities = new Dictionary<string, int>(Basket.Items.Count);
                foreach (LineItem item in Basket.Items)
                {
                    if (productQuantities.ContainsKey(item.ProductId))
                    {
                        productQuantities[item.ProductId] = productQuantities[item.ProductId] + item.Quantity;
                    }
                    else
                    {
                        productQuantities.Add(item.ProductId, item.Quantity);
                    }
                }

                //if this product is a child product, then we need to compare the minimum quantity against its parent
                foreach (LineItem item in Basket.Items)
                {
                    Product associatedProduct = item.GetAssociatedProduct(MTApp);
                    if (associatedProduct != null)
                    {
                        Product itemToCompare = null;
                        itemToCompare = associatedProduct;

                        if (itemToCompare.MinimumQty > productQuantities[item.ProductId])
                        {
                            string message = SiteTerms.GetTerm(SiteTermIds.CartPageMinimumQuantityError);
                            message = SiteTerms.ReplaceTermVariable(message, "productName", item.ProductName);
                            message = SiteTerms.ReplaceTermVariable(message, "quantity", itemToCompare.MinimumQty.ToString());
                            MessageBox1.ShowError(message);
                            int difference = itemToCompare.MinimumQty - productQuantities[item.ProductId];
                            item.Quantity += difference;
                            return false;
                        }
                    }
                }
            }

            MTApp.CalculateOrderAndSave(Basket);
            SessionManager.SaveOrderCookies(Basket);
            return true;
        }

        void ForwardToCheckout()
        {
            OrderTaskContext c = new OrderTaskContext(MTApp);
            c.UserId = SessionManager.GetCurrentUserId();
            c.Order = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            if (Workflow.RunByName(c, WorkflowNames.CheckoutSelected))
            {
                Response.Redirect(MTApp.CurrentStore.RootUrlSecure() + "checkout");
            }
            else
            {
                bool customerMessageFound = false;
                foreach (WorkflowMessage msg in c.Errors)
                {
                    EventLog.LogEvent(msg.Name, msg.Description, MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
                    if (msg.CustomerVisible)
                    {
                        customerMessageFound = true;
                        MessageBox1.ShowError(msg.Description);
                    }
                }
                if (!customerMessageFound)
                {
                    EventLog.LogEvent("Checkout Selected Workflow", "Checkout failed but no errors were recorded.", MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
                    MessageBox1.ShowError("Checkout Failed. If problem continues, please contact customer support.");
                }
            }
        }

        protected void btnCheckout_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if (Page.IsValid)
            {
                if (GetCurrentLineItemQuantities())
                {
                    if (CheckForStockOnItems())
                    {
                        MessageBox1.ClearMessage();
                        ForwardToCheckout();
                    }
                }
                else
                {
                    LoadCart();
                }
            }
        }

        protected bool CheckForStockOnItems()
        {
            
                Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                SystemOperationResult result = MTApp.CheckForStockOnItems(Basket);
                if (result.Success)
                {
                    return true;
                }
                else
                {
                    MessageBox1.ShowError(result.Message);
                    return false;
                }                        
        }

        protected void btnAddCoupon_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            MessageBox1.ClearMessage();
            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            Basket.AddCouponCode(this.CouponField.Text.Trim());
            MTApp.CalculateOrderAndSave(Basket);
            SessionManager.SaveOrderCookies(Basket);
            LoadCart();
        }

        protected void CouponGrid_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btnDeleteCoupon = (ImageButton)e.Row.FindControl("btnDeleteCoupon");
                if (btnDeleteCoupon != null)
                {
                    btnDeleteCoupon.ImageUrl = tm.ButtonUrl("x", Request.IsSecureConnection);
                }
            }
        }

        protected void CouponGrid_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            this.MessageBox1.ClearMessage();
            long tempid = (long)e.Keys[0];
            
                Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
                Basket.RemoveCouponCode(tempid);
                MTApp.CalculateOrderAndSave(Basket);
                SessionManager.SaveOrderCookies(Basket);
            
            LoadCart();
        }

        protected void AddToCartClicked(string productId)
        {
            LoadCart();
        }

        protected void ItemGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GiftWrap")
            {
                string lineItemBvin = (string)e.CommandArgument;
                Response.Redirect("GiftWrap.aspx?id=" + lineItemBvin);
            }
        }

        public void PaypalExpressCheckoutClicked(PaypalExpressCheckoutArgs args)
        {
            GetCurrentLineItemQuantities();
            if (!CheckForStockOnItems())
            {
                args.Failed = true;
            }
            else
            {
                args.Failed = false;
            }
        }

        public void WorkflowFailed(string Message)
        {
            MessageBox1.ShowError(Message);
        }

        public void GoogleCheckoutClicked(GoogleCheckoutArgs args)
        {
            GetCurrentLineItemQuantities();
            if (!CheckForStockOnItems())
            {
                args.Failed = true;
            }
            else
            {
                args.Failed = false;
            }
        }

        public void GoogleWorkflowFailed(string Message)
        {
            MessageBox1.ShowError(Message);
        }

    }
}