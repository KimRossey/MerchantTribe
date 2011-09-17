using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVCommerce.Filters;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Catalog;
using MerchantTribe.Web;
using BVSoftware.Commerce.Content;

namespace BVCommerce.Areas.account.Controllers
{
    [CustomerSignedInFilter]
    public class OrderHistoryController : BVCommerce.Controllers.Shared.BaseStoreController
    {
        //
        // GET: /account/OrderHistory/
        public ActionResult Index()
        {            
            ViewBag.MetaDescription = "Order History | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountpage";
            ViewBag.ViewButtonImageUrl = BVApp.ThemeManager().ButtonUrl("view", Request.IsSecureConnection); ;

            List<OrderSnapshot> orders = LoadOrders();
            return View(orders);
        }

        private List<OrderSnapshot> LoadOrders()
        {
            
            int totalCount = 0;
            List<OrderSnapshot> orders = new List<OrderSnapshot>();

            // pull all BV Orders
            orders = BVApp.OrderServices.Orders.FindByUserId(SessionManager.GetCurrentUserId(), 1, 100, ref totalCount);

            if (orders == null) return new List<OrderSnapshot>();

            return orders;
        }

        //
        // GET: /account/OrderHistory/Details/{id}
        public ActionResult Details(string id)
        {
            ViewBag.Title = "View Order";
            ViewBag.MetaDescription = "View Order | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountorderdetailspage";
            
            PrepBVOrder(id);
            
            return View();           
        }
        
        private void PrepBVOrder(string bvin)
        {                        
            Order o = BVApp.OrderServices.Orders.FindForCurrentStore(bvin);
            ViewBag.Order = o;
            ViewBag.AcumaticaOrder = null;
            
            OrderPaymentSummary paySummary = BVApp.OrderServices.PaymentSummary(o);
            ViewBag.OrderPaymentSummary = paySummary;

            // File Downloads
            List<ProductFile> fileDownloads = new List<ProductFile>();            
            if ((o.PaymentStatus == OrderPaymentStatus.Paid) && (o.StatusCode != OrderStatusCode.OnHold))
            {                
                foreach (LineItem item in o.Items)
                {
                    if (item.ProductId != string.Empty)
                    {
                        List<ProductFile> productFiles = BVApp.CatalogServices.ProductFiles.FindByProductId(item.ProductId);
                        foreach (ProductFile file in productFiles)
                        {
                            fileDownloads.Add(file);
                        }
                    }
                }            
            }
            ViewBag.FileDownloads = fileDownloads;
            ViewBag.FileDownloadsButtonUrl = BVApp.ThemeManager().ButtonUrl("download", Request.IsSecureConnection);
        }

    }
}
