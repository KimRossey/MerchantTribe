using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce;

namespace BVCommerce.Filters
{
    public class CustomerSignedInFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // do base action first to ensure we have our context objects like bvapp
            base.OnActionExecuting(filterContext);

            if (filterContext.Controller is Controllers.Shared.BaseStoreController)
            {
                BVApplication bvapp = ((Controllers.Shared.BaseStoreController)filterContext.Controller).BVApp;
                if (bvapp != null)
                {
                    if (!SessionManager.IsUserAuthenticated(bvapp))
                    {
                        filterContext.HttpContext.Response.Redirect("~/signin");
                    }
                }
            }
        }            
    }
}