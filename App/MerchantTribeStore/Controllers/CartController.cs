﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Content;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Commerce.Utilities;
using MerchantTribe.Commerce.BusinessRules;

namespace MerchantTribeStore.Controllers
{
    public class CartController : BaseStoreController
    {

        private CartViewModel IndexSetup()
        {
            ViewBag.Title = SiteTerms.GetTerm(SiteTermIds.ShoppingCart);
            ViewBag.BodyClass = "store-cart-page";

            CartViewModel model = new CartViewModel();
            LoadButtonImages(model);
            model.KeepShoppingUrl = GetKeepShoppingLocation();
            SetPayPalVisibility(model);
            return model;
        }
        private void LoadButtonImages(CartViewModel model)
        {
            ThemeManager tm = MTApp.ThemeManager();
            model.KeepShoppingButtonUrl = tm.ButtonUrl("keepshopping", Request.IsSecureConnection);
            model.CheckoutButtonUrl = tm.ButtonUrl("securecheckout", Request.IsSecureConnection);
            model.AddCouponButtonUrl = tm.ButtonUrl("new", Request.IsSecureConnection);
            model.DeleteButtonUrl = tm.ButtonUrl("x", Request.IsSecureConnection);
            model.EstimateShippingButtonUrl = tm.ButtonUrl("estimateshipping", Request.IsSecureConnection);
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
        private void SetPayPalVisibility(CartViewModel model)
        {
            AvailablePayments availablePayments = new AvailablePayments();
            Collection<DisplayPaymentMethod> enabledMethods;
            enabledMethods = availablePayments.EnabledMethods(MTApp.CurrentStore);

            model.PayPalExpressAvailable = false;
            foreach (DisplayPaymentMethod m in enabledMethods)
            {
                switch (m.MethodId)
                {
                    case WebAppSettings.PaymentIdPaypalExpress:
                        model.PayPalExpressAvailable = true;
                        break;
                    default:
                        // do nothing
                        break;
                }
            }
        }

        // GET: /Cart/        
        [NonCacheableResponseFilter]
        public ActionResult Index()
        {
            CartViewModel model = IndexSetup();
            CheckForQuickAdd();
            LoadCart(model);

            return View(model);
        }

        // POST: /Cart/
        [ActionName("Index")]
        [HttpPost]
        public ActionResult IndexPost()
        {
            CartViewModel model = IndexSetup();
            LoadCart(model);

            if (CheckForStockOnItems(model))
            {
                ForwardToCheckout(model);
            }
            return View(model);
        }

        private void CheckForQuickAdd()
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
        private void LoadCart(CartViewModel model)
        {
            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            if (Basket == null) return;
            model.CurrentOrder = Basket;

            if ((Basket.Items == null) || ((Basket.Items != null) && (Basket.Items.Count <= 0)))
            {
                model.CartEmpty = true;
                return;
            }

            foreach (LineItem li in model.CurrentOrder.Items)
            {
                CartLineItemViewModel ci = new CartLineItemViewModel();
                ci.Item = li;

                Product associatedProduct = li.GetAssociatedProduct(MTApp);
                if (associatedProduct != null)
                {
                    ci.ShowImage = true;
                    ci.ImageUrl = MerchantTribe.Commerce.Storage
                                  .DiskStorage.ProductVariantImageUrlMedium(
                                  MTApp.CurrentStore.Id, li.ProductId,
                                  associatedProduct.ImageFileSmall,
                                  li.VariantId, Request.IsSecureConnection);

                    ci.LinkUrl = UrlRewriter.BuildUrlForProduct(associatedProduct,
                                   MTApp.CurrentRequestContext.RoutingContext,
                                   "OrderBvin=" + li.OrderBvin + "&LineItemId=" + li.Id);
                }


                if (li.LineTotal != li.LineTotalWithoutDiscounts)
                {
                    ci.HasDiscounts = true;
                }

                model.LineItems.Add(ci);
            }
        }
        private bool CheckForStockOnItems(CartViewModel model)
        {
            Order Basket = model.CurrentOrder;
            SystemOperationResult result = MTApp.CheckForStockOnItems(Basket);
            if (result.Success)
            {
                return true;
            }
            else
            {
                this.FlashFailure(result.Message);
                return false;
            }
        }
        public void ForwardToCheckout(CartViewModel model)
        {
            if (Request["paypalexpress"] != null && Request["paypalexpress"] == "true")
            {
                ForwardToPayPalExpress(model);
            }

            OrderTaskContext c = new OrderTaskContext(MTApp);
            c.UserId = SessionManager.GetCurrentUserId();
            c.Order = model.CurrentOrder;
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
                        this.FlashFailure(msg.Description);
                    }
                }
                if (!customerMessageFound)
                {
                    EventLog.LogEvent("Checkout Selected Workflow", "Checkout failed but no errors were recorded.", MerchantTribe.Commerce.Metrics.EventLogSeverity.Error);
                    this.FlashFailure("Checkout Failed. If problem continues, please contact customer support.");
                }
            }
        }
        public void ForwardToPayPalExpress(CartViewModel model)
        {
            Order Basket = model.CurrentOrder;
            // Save as Order
            MerchantTribe.Commerce.BusinessRules.OrderTaskContext c
                = new MerchantTribe.Commerce.BusinessRules.OrderTaskContext(MTApp);
            c.UserId = SessionManager.GetCurrentUserId();
            c.Order = Basket;
            bool checkoutFailed = false;
            if (!MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.CheckoutSelected))
            {
                checkoutFailed = true;
                bool customerMessageFound = false;
                foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage msg in c.Errors)
                {
                    EventLog.LogEvent(msg.Name, msg.Description, EventLogSeverity.Error);
                    if (msg.CustomerVisible)
                    {
                        customerMessageFound = true;                        
                        FlashWarning(msg.Description);                        
                    }
                }
                if (!customerMessageFound)
                {
                    EventLog.LogEvent("Checkout Selected Workflow", "Checkout failed but no errors were recorded.", EventLogSeverity.Error);
                    FlashWarning("Checkout Failed. If problem continues, please contact customer support.");
                }
            }

            if (!checkoutFailed)
            {
                c.Inputs.Add("bvsoftware", "Mode", "PaypalExpress");
                if (!MerchantTribe.Commerce.BusinessRules.Workflow.RunByName(c, MerchantTribe.Commerce.BusinessRules.WorkflowNames.ThirdPartyCheckoutSelected))
                {
                    bool customerMessageFound = false;
                    EventLog.LogEvent("Paypal Express Checkout Failed", "Specific Errors to follow", EventLogSeverity.Error);
                    foreach (MerchantTribe.Commerce.BusinessRules.WorkflowMessage item in c.Errors)
                    {
                        EventLog.LogEvent("Paypal Express Checkout Failed", item.Name + ": " + item.Description, EventLogSeverity.Error);
                        if (item.CustomerVisible)
                        {
                            FlashWarning(item.Description);
                            customerMessageFound = true;
                        }
                    }
                    if (!customerMessageFound)
                    {
                        FlashWarning("Paypal Express Checkout Failed. If this problem persists please notify customer support.");
                    }
                }
            }

        }

        [HttpPost]
        public ActionResult RemoveLineItem()
        {
            string ids = Request["lineitemid"] ?? string.Empty;
            long Id = 0;
            long.TryParse(ids, out Id);

            string orderBvin = Request["orderbvin"] ?? string.Empty;

            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            if (Basket != null)
            {
                if (Basket.bvin == orderBvin)
                {
                    var li = Basket.Items.Where(y => y.Id == Id).SingleOrDefault();
                    if (li != null)
                    {
                        Basket.Items.Remove(li);
                        MTApp.CalculateOrderAndSave(Basket);
                        SessionManager.SaveOrderCookies(Basket);
                    }
                }
            }
            return Redirect("~/cart");
        }

        [HttpPost]
        public ActionResult AddCoupon()
        {
            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            string code = Request["couponcode"] ?? string.Empty;
            Basket.AddCouponCode(code.Trim());
            MTApp.CalculateOrderAndSave(Basket);
            SessionManager.SaveOrderCookies(Basket);
            return Redirect("~/cart");
        }

        [HttpPost]
        public ActionResult RemoveCoupon()
        {
            string couponid = Request["couponid"] ?? string.Empty;
            long tempid = 0;
            long.TryParse(couponid, out tempid);

            Order Basket = SessionManager.CurrentShoppingCart(MTApp.OrderServices);
            Basket.RemoveCouponCode(tempid);
            MTApp.CalculateOrderAndSave(Basket);
            SessionManager.SaveOrderCookies(Basket);

            return Redirect("~/cart");
        }
    }
}
