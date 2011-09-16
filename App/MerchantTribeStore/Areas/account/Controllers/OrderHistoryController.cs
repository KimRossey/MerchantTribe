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
using BVSoftware.AcumaticaTools;

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

            // Merge Acumatica Orders if Available
            if (BVApp.CurrentStore.Settings.Acumatica.IntegrationEnabled)
            {
                orders = MergeAcumaticaOrders(orders);
            }

            if (orders == null) return new List<OrderSnapshot>();

            return orders;
        }
        private List<OrderSnapshot> MergeAcumaticaOrders(List<OrderSnapshot> bvorders)
        {
            List<OrderSnapshot> result = new List<OrderSnapshot>();

            //AcumaticaIntegration acumatica = AcumaticaIntegration.Factory(BVApp);
            List<BVSoftware.AcumaticaTools.OrderSummaryData> acumaticaOrders = new List<BVSoftware.AcumaticaTools.OrderSummaryData>();
            BVSoftware.Commerce.Membership.CustomerAccount currentCustomer = BVApp.CurrentCustomer;
            if (currentCustomer != null && currentCustomer.Bvin != string.Empty)
            {
                AcumaticaIntegration acumatica = AcumaticaIntegration.Factory(BVApp);
                acumaticaOrders = acumatica.FindAllOrdersForCustomer(currentCustomer.Email);
            }
            if (acumaticaOrders != null)
            {
                foreach (OrderSnapshot bvorder in bvorders)
                {
                    if (bvorder.AcumaticaId == string.Empty)
                    {
                        result.Add(bvorder);
                        continue;
                    }

                    var acu = acumaticaOrders.Where(y => y.Number == bvorder.AcumaticaId).FirstOrDefault();
                    if (acu != null)
                    {
                        bvorder.bvin = acu.Number;
                        bvorder.TotalGrand = acu.Amount;
                        result.Add(bvorder);

                        // Mark this acumatica order as matched
                        acu.Number = string.Empty;
                        acu.Amount = -1;
                    }
                    else
                    {
                        result.Add(bvorder);
                    }
                }

                // Now add any acumatica orders not matched
                foreach (BVSoftware.AcumaticaTools.OrderSummaryData ao in acumaticaOrders.OrderByDescending(y => y.TimeOfOrder).Where(y => y.Amount >= 0))
                {
                    OrderSnapshot s = new OrderSnapshot();
                    s.bvin = ao.Number;
                    s.OrderNumber = "A-" + ao.Number;
                    s.TotalGrand = ao.Amount;
                    s.AcumaticaId = ao.Number;
                    s.TimeOfOrderUtc = ao.TimeOfOrder.ToUniversalTime();
                    result.Add(s);
                }
            }

            return result;
        }

        //
        // GET: /account/OrderHistory/Details/{id}
        public ActionResult Details(string id)
        {
            ViewBag.Title = "View Order";
            ViewBag.MetaDescription = "View Order | " + BVApp.CurrentStore.Settings.MetaDescription;
            ViewBag.MetaKeywords = BVApp.CurrentStore.Settings.MetaKeywords;
            ViewBag.BodyClass = "myaccountorderdetailspage";
            
            if (BVApp.CurrentStore.Settings.Acumatica.IntegrationEnabled && Request.QueryString["acumaticamode"] == "1")
            {
                PrepAcumaticaOrder(id);
            }
            else
            {
                PrepBVOrder(id);
            }
            
            return View();           
        }

        private void PrepAcumaticaOrder(string bvin)
        {
            ViewBag.AcumaticaOrder = null;
            ViewBag.Order = null;
            ViewBag.FileDownloads = new List<ProductFile>();

            AcumaticaIntegration acumatica = AcumaticaIntegration.Factory(BVApp);
            try
            {
                if (acumatica.CustomerOwnsOrder(BVApp.CurrentCustomer.Email, bvin))
                {
                    bool ownsOrder = acumatica.CustomerOwnsOrder(BVApp.CurrentCustomer.Email, bvin);

                    if (ownsOrder)
                    {
                        BVSoftware.AcumaticaTools.OrderData data = null;                        
                        data = acumatica.GetSingleOrder(bvin);
                        ViewBag.AcumaticaOrder = data;                        
                    }
                    else
                    {
                        FlashFailure("That order number is not assigned to your account");                        
                    }
                }
            }
            catch (RemoteIntegrationException)
            {
                string term = BVSoftware.Commerce.Content.SiteTerms.GetTerm(SiteTermIds.AcumaticaUnavailable);
                FlashInfo(term);                
            }

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
