using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Routing;
using BVSoftware.Commerce;
using System.Web.Mvc;
using System.Web.Caching;

namespace BVCommerce
{
    public class Global : System.Web.HttpApplication
    {

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        #region " Scheduled Tasks "

        static void ScheduleTaskTrigger()
        {
            HttpRuntime.Cache.Add("ScheduledTaskTrigger",
                                  "42", // Magic string that really could be anything but 42 is nice.
                                  null, 
                                  Cache.NoAbsoluteExpiration, 
                                  TimeSpan.FromMinutes(2), 
                                  CacheItemPriority.NotRemovable, 
                                  new CacheItemRemovedCallback(PerformScheduledTasks));
        }        
        static void PerformScheduledTasks(string key, Object value, CacheItemRemovedReason reason)
        {
            try
            {
                BVSoftware.Commerce.RequestContext context = new BVSoftware.Commerce.RequestContext();
                BVApplication bvapp = BVSoftware.Commerce.BVApplication.InstantiateForDataBase(context);

                List<long> storeIds = bvapp.ScheduleServices.QueuedTasks.ListStoresWithTasksToRun();
                if (storeIds != null)
                {
                    List<BVSoftware.Commerce.Accounts.StoreDomainSnapshot> stores = bvapp.AccountServices.Stores.FindDomainSnapshotsByIds(storeIds);
                    if (stores != null)
                    {
                        System.Threading.Tasks.Parallel.ForEach(stores, CallTasksOnStore);
                        //foreach (BVSoftware.Commerce.Accounts.StoreDomainSnapshot snap in stores)
                        //{
                        //    string storekey = System.Configuration.ConfigurationManager.AppSettings["storekey"];
                        //    string rootUrl = snap.RootUrl();
                        //    string destination = rootUrl + "scheduledtasks/" + storekey;
                        //    BVSoftware.Commerce.Utilities.WebForms.SendRequestByPost(destination, string.Empty);
                        //}
                    }
                }
                
            }
            catch
            {
                // suppress error and schedule next run
            }

            ScheduleTaskTrigger();
        }

        private static void CallTasksOnStore(BVSoftware.Commerce.Accounts.StoreDomainSnapshot snap)
        {
            string storekey = System.Configuration.ConfigurationManager.AppSettings["storekey"];
            string rootUrl = snap.RootUrl();
            string destination = rootUrl + "scheduledtasks/" + storekey;
            BVSoftware.Commerce.Utilities.WebForms.SendRequestByPost(destination, string.Empty);
        }

#endregion

        void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("bvc.js");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("images/{*imagedata}");
            //routes.IgnoreRoute("bvadmin/*");
            //routes.IgnoreRoute("{resource}.aspx/{*pathInfo}");

            routes.MapPageRoute("adminlogin", "account/login", "~/BVAdmin/Login.aspx");
            routes.MapPageRoute("adminresetpassword", "account/resetpassword", "~/BVAdmin/ResetPassword.aspx");
            routes.MapPageRoute("adminresetpassword2", "account/resetpassword2", "~/BVAdmin/ResetPassword2.aspx");
            routes.MapPageRoute("adminlogout", "account/logout", "~/BVAdmin/logout.aspx");
            
            routes.MapPageRoute("customerlogin", "signin", "~/Login.aspx");
            routes.MapPageRoute("customlogout", "signout", "~/Logout.aspx"); 
           
            routes.MapPageRoute("forgotpassword-route", "forgotpassword/{email}/{checkout}", "~/ForgotPassword.aspx", 
                                false, new RouteValueDictionary(new {email="",checkout="0"}));

            routes.MapPageRoute("cart-route", "cart", "~/cart.aspx");
            routes.MapPageRoute("contact-route", "contact", "~/ContactUs.aspx");
            routes.MapPageRoute("giftcards-route", "giftcards", "~/GiftCards.aspx");
            routes.MapPageRoute("emailsignup-route", "emailsignup", "~/EmailSignUp.aspx");
                                                                        
            routes.MapPageRoute("productreview-route", "productreviews/{slug}", "~/productreviews.aspx");
                                                  
            // policies
            routes.MapRoute("policy-route", "policies/{policykind}",
                                new { controller = "Policies", action="Index" });

            // estimate shipping
            routes.MapRoute("estimate-shipping", "estimateshipping/{action}/{id}",
                                new { controller = "EstimateShipping", action = "index", id = "0" });
            // checkouts
            routes.MapPageRoute("checkout-route", "checkout", "~/checkout.aspx");
            routes.MapPageRoute("checkout-paypal-route", "paypalexpresscheckout", "~/CheckoutPayPalExpress.aspx");
            routes.MapPageRoute("paypal-ipn-route", "paypalipn", "~/IPNHandler.aspx");
            routes.MapPageRoute("checkout-google-route", "checkout/google", "~/checkout-google.aspx");

            // Search
            routes.MapPageRoute("search-route", "search", "~/search.aspx");

            routes.MapRoute("notfound", "storenotfound", new { controller = "Store", action = "NotFound" });
            routes.MapRoute("notavailable", "storenotavailable", new { controller = "Store", action = "NotAvailable" });
            routes.MapRoute("storeclosed", "storeclosed", new { controller = "Store", action = "Closed" });

            // Admin
            routes.MapPageRoute("admin", "admin", "~/bvadmin/default.aspx");


            // Third Party Checkouts
            routes.MapPageRoute("googlenotify", "googlenotify", "~/googlenotify.aspx");

            // Signup Routes
            routes.MapPageRoute("signup-paypal", "paypaloffer", "~/signup/paypaloffer.aspx");
            routes.MapPageRoute("signup-tour", "signup/tour", "~/signup/tour.aspx");
            routes.MapPageRoute("signup-design", "signup/design", "~/signup/design.aspx");
            routes.MapPageRoute("signup-about", "signup/about", "~/signup/about.aspx");
            routes.MapPageRoute("signup-pricing", "signup", "~/signup/default.aspx");
            routes.MapPageRoute("signup-featured", "signup/features", "~/signup/features.aspx");
            routes.MapPageRoute("signup-themes", "signup/premiumthemes", "~/signup/premiumthemes.aspx");
            routes.MapPageRoute("signup-promote", "signup/promote", "~/signup/promote.aspx");
            routes.MapPageRoute("signup-register", "signup/register/{id}", "~/signup/register.aspx");
            routes.MapPageRoute("signup-processsignup", "signup/processsignup", "~/signup/processsignup.aspx");
            routes.MapPageRoute("signup-sell", "signup/sell", "~/signup/sell.aspx");
            routes.MapPageRoute("signup-secure", "signup/secure", "~/signup/secure.aspx");
            routes.MapPageRoute("signup-privacy", "signup/policies/privacy", "~/signup/policies/privacy.aspx");
            routes.MapPageRoute("signup-refund", "signup/policies/refund", "~/signup/policies/refund.aspx");
            routes.MapPageRoute("signup-terms", "signup/policies/terms", "~/signup/policies/terms.aspx");

            // Home page
            routes.MapRoute("homepage", "", new { controller = "Home", action = "Index" });
            routes.MapRoute("oldhomepage", "default.aspx", new { controller = "Home", action = "ToIndex" });            

            // MVC Page Routes
            routes.MapRoute("fileuploadhandler", "fileuploadhandler/{typecode}/{*details}", new { controller = "FileUpload", action = "Index" });
            routes.MapRoute("flexpartjson", "flexpartjson/{pageid}/{partid}", new { controller = "FlexPartJson", action = "Index" });
            routes.MapRoute("todo", "todo/{action}/{id}/{*params}", new { controller = "ToDo", Action = "Index", id = 0 });
            routes.MapRoute("css", "css/{themename}/styles.css", new { controller = "StyleSheets", action = "Index" });
            routes.MapRoute("js", "js/{filename}", new { controller = "Javascript", action = "Index" });
            routes.MapRoute("scheduledtasks", "scheduledtasks/{storekey}", new { controller = "ScheduledTasks", action = "Index" });

            routes.MapRoute("sitemap-route", "sitemap/{*params}", new { controller = "SiteMap", action = "Index" });
            routes.MapRoute("sitemapxml", "sitemap.xml", new { controller = "SiteMap", action = "Xml"});
            
            routes.MapRoute("apirest", "api/rest/v{version}/{modelname}/{*parameters}", new { controller = "ApiRest", action = "Index", version=1 });
            
            // MVC Super             
            routes.MapRoute("superstores", "super/stores/{action}/{*params}", new { controller = "SuperStores", Action = "Index" });
            routes.MapRoute("superhome", "super/{action}/{*params}", new { controller = "Super", Action = "Index" });

            // Custom Router
            // This should stop anything
            routes.Add("bvroute", new Route("{*slug}", new CustomRouter()));            
                        
        }
        
        void Application_Start(object sender, EventArgs e)
        {            
            // routing
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);                        

            // Scheduled Tasks Launcher
            string disableCacheTimers = System.Configuration.ConfigurationManager.AppSettings["disablecachetimers"];
            if (disableCacheTimers != "1")
            {
                ScheduleTaskTrigger();
            }
        }

        void Application_End(object sender, EventArgs e)
        {            
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Exception ex = Server.GetLastError().GetBaseException();
            BVSoftware.Commerce.EventLog.LogEvent("Error", StringUtils.SessionInfo(), BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
            BVSoftware.Commerce.EventLog.LogEvent(ex);
            while (ex.InnerException != null)
            {
                BVSoftware.Commerce.EventLog.LogEvent(ex);
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started
            /*
                'Try
                'If BVSoftware.Commerce.WebAppSettings.AutomaticallyRegenerateDynamicCategories Then
                'If ((DateTime.Now() - BVSoftware.Commerce.WebAppSettings.AutomaticallyRegenerateDynamicCategoriesLastDateTimeRun).Ticks / TimeSpan.TicksPerHour) > _
                'BVSoftware.Commerce.WebAppSettings.AutomaticallyRegenerateDynamicCategoriesIntervalInHours Then
                'BVSoftware.Commerce.WebAppSettings.AutomaticallyRegenerateDynamicCategoriesLastDateTimeRun = DateTime.Now()
                'System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf BVSoftware.Commerce.Catalog.Category.RegenerateDynamicCategories))
                'End If
                'End If
                'Catch ex As Exception
                'BVSoftware.Commerce.EventLog.LogEvent(ex)
                'End Try
        
                'Try
                'If ((DateTime.Now() - BVSoftware.Commerce.WebAppSettings.CartCleanupLastTimeRun).Ticks / TimeSpan.TicksPerHour) > _
                'BVSoftware.Commerce.WebAppSettings.CartCleanupIntervalInHours Then
                'BVSoftware.Commerce.WebAppSettings.CartCleanupLastTimeRun = DateTime.Now()
                'System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf BVSoftware.Commerce.Orders.Order.CleanupCarts))
                'End If
                'Catch ex As Exception
                'BVSoftware.Commerce.EventLog.LogEvent(ex)
                'End Try
        
                'Try
                'If (BVSoftware.Commerce.WebAppSettings.InventoryLowHours > 0) AndAlso (Not BVSoftware.Commerce.WebAppSettings.DisableInventory) Then
                'If ((DateTime.Now() - BVSoftware.Commerce.WebAppSettings.InventoryLowLastTimeRun).Ticks / TimeSpan.TicksPerHour) > _
                'BVSoftware.Commerce.WebAppSettings.InventoryLowHours Then
                'BVSoftware.Commerce.WebAppSettings.InventoryLowLastTimeRun = DateTime.Now()
                'System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf BVSoftware.Commerce.Catalog.ProductInventory.EmailLowStockReport))
                'End If
                'End If
                'Catch ex As Exception
                'BVSoftware.Commerce.EventLog.LogEvent(ex)
                'End Try
        
                'Try
                'If (BVSoftware.Commerce.WebAppSettings.CCSHours > 0) Then
                'If ((DateTime.Now() - BVSoftware.Commerce.WebAppSettings.CCSLastTimeRun).Ticks / TimeSpan.TicksPerHour) > 24 Then
                'Dim lastTimeRun As DateTime = BVSoftware.Commerce.WebAppSettings.CCSLastTimeRun
                'BVSoftware.Commerce.WebAppSettings.CCSLastTimeRun = DateTime.Now()
                'System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf BVSoftware.Commerce.Orders.Order.CleanUpCCNumbers), lastTimeRun)
                'End If
                'End If
                'Catch ex As Exception
                'BVSoftware.Commerce.EventLog.LogEvent(ex)
                'End Try
            */


        }

        void Session_End(object sender, EventArgs e)
        {           
        }

        void Application_BeginRequest(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(BVSoftware.Commerce.WebAppSettings.SiteCultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(BVSoftware.Commerce.WebAppSettings.SiteCultureCode);
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom.Equals("disablecsscaching", StringComparison.CurrentCultureIgnoreCase))
            {
                if (System.Configuration.ConfigurationManager.AppSettings["disablecsscaching"] == "1")
                {
                    return System.Guid.NewGuid().ToString();
                }
                return "0";
            }
            else
                return base.GetVaryByCustomString(context, custom);
        }
    }
}
