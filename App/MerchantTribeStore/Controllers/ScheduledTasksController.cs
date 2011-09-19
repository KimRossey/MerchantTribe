using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using System.Configuration;
using MerchantTribe.Commerce.Scheduling;

namespace BVCommerce.Controllers
{
    public class ScheduledTasksController : Controller, IMultiStorePage
    {
        #region " Store Specific Setup Code"
        // Initialize Store Specific Request Data
        MerchantTribe.Commerce.RequestContext _BVRequestContext = new RequestContext();
        public BVApplication BVApp { get; set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            BVApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            BVApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
            {
                Response.Redirect("~/storenotavailable");
            }
            IntegrationLoader.AddIntegrations(this.BVApp.CurrentRequestContext.IntegrationEvents, this.BVApp.CurrentStore);
        }
        private void CheckFor301(string slug)
        {
            MerchantTribe.Commerce.Content.CustomUrl url = BVApp.ContentServices.CustomUrls.FindByRequestedUrl(slug);
            if (url != null)
            {
                if (url.Bvin != string.Empty)
                {
                    if (url.IsPermanentRedirect)
                    {
                        Response.RedirectPermanent(url.RedirectToUrl);
                    }
                    else
                    {
                        Response.Redirect(url.RedirectToUrl);
                    }
                }
            }
        }
        #endregion

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(string storekey)
        {
            string AcutalKey = WebAppSettings.StoreKey;
            if (AcutalKey != storekey)
            {
                EventLog.LogEvent("Scheduled Tasks", "Store Key Mismatch on Scheduled Task Call for Store " + BVApp.CurrentStore.Id, MerchantTribe.Commerce.Metrics.EventLogSeverity.Warning);
                return View();
            }

            
            QueuedTask task = BVApp.ScheduleServices.QueuedTasks.PopATaskForRun(BVApp.CurrentStore.Id);
            if (task == null)
            {
                EventLog.LogEvent("Scheduled Tasks", "No Task found on call to store " + BVApp.CurrentStore.Id, MerchantTribe.Commerce.Metrics.EventLogSeverity.Information);
                return View();
            }

            try
            {

                    // call task processor here
                    task.Status = QueuedTaskStatus.Failed;
                    task.StatusNotes = "Failed to locate the requested processor for this task.";
                    BVApp.ScheduleServices.QueuedTasks.Update(task);

            }
            catch (Exception ex)
            {
                task.Status = QueuedTaskStatus.Failed;
                task.StatusNotes = ex.Message + " " + ex.StackTrace;
                BVApp.ScheduleServices.QueuedTasks.Update(task);
            }

            return View();
        }

    }
}
