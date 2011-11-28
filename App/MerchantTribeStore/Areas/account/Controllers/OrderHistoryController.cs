using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribeStore.Filters;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Web;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class OrderHistoryController : MerchantTribeStore.Controllers.Shared.BaseStoreController
    {
        //
        // GET: /account/OrderHistory/
        public ActionResult Index()
        {            
            ViewBag.MetaDescription = "Order History | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountpage";
            ViewBag.ViewButtonImageUrl = MTApp.ThemeManager().ButtonUrl("view", Request.IsSecureConnection); ;

            List<OrderSnapshot> orders = LoadOrders();
            return View(orders);
        }

        private List<OrderSnapshot> LoadOrders()
        {
            
            int totalCount = 0;
            List<OrderSnapshot> orders = new List<OrderSnapshot>();

            // pull all BV Orders
            orders = MTApp.OrderServices.Orders.FindByUserId(SessionManager.GetCurrentUserId(MTApp.CurrentStore), 1, 100, ref totalCount);

            if (orders == null) return new List<OrderSnapshot>();

            return orders;
        }

        //
        // GET: /account/OrderHistory/Details/{id}
        public ActionResult Details(string id)
        {
            ViewBag.Title = "View Order";
            ViewBag.MetaDescription = "View Order | " + MTApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = MTApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountorderdetailspage";
            
            PrepBVOrder(id);
            
            return View();           
        }
        
        private void PrepBVOrder(string bvin)
        {                        
            Order o = MTApp.OrderServices.Orders.FindForCurrentStore(bvin);
            ViewBag.Order = o;
            ViewBag.AcumaticaOrder = null;
            
            OrderPaymentSummary paySummary = MTApp.OrderServices.PaymentSummary(o);
            ViewBag.OrderPaymentSummary = paySummary;

            // File Downloads
            List<ProductFile> fileDownloads = new List<ProductFile>();            
            if ((o.PaymentStatus == OrderPaymentStatus.Paid) && (o.StatusCode != OrderStatusCode.OnHold))
            {                
                foreach (LineItem item in o.Items)
                {
                    if (item.ProductId != string.Empty)
                    {
                        List<ProductFile> productFiles = MTApp.CatalogServices.ProductFiles.FindByProductId(item.ProductId);
                        foreach (ProductFile file in productFiles)
                        {
                            fileDownloads.Add(file);
                        }
                    }
                }            
            }
            ViewBag.FileDownloads = fileDownloads;
            ViewBag.FileDownloadsButtonUrl = MTApp.ThemeManager().ButtonUrl("download", Request.IsSecureConnection);
        }

    }
}
