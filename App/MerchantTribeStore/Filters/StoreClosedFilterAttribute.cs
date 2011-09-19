using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;

namespace BVCommerce.Filters
{
    public class StoreClosedFilterAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // do base action first to ensure we have our context objects like bvapp
            base.OnActionExecuting(filterContext);

            // skip everything if we're a store controller with closed actions, etc.
            if (filterContext.Controller is Controllers.StoreController) return;

            // otherwise check for a store closed page
            if (filterContext.Controller is Controllers.Shared.BaseStoreController)
            {
                BVApplication bvapp = ((Controllers.Shared.BaseStoreController)filterContext.Controller).BVApp;
                if (bvapp != null)
                {
                    if (bvapp.CurrentRequestContext.CurrentStore.Settings.StoreClosed)
                    {
                        bool hasPass = false;
                        string guestPass = SessionManager.StoreClosedGuestPasswordForCurrentUser;
                        if (guestPass.Trim().Length > 0)
                        {
                            if (guestPass == bvapp.CurrentStore.Settings.StoreClosedGuestPassword)
                            {
                                hasPass = true;
                            }
                        }
                        if (bvapp.CurrentRequestContext.IsAdmin() == false && hasPass == false)
                        {
                            filterContext.Result = new RedirectResult("~/storeclosed");
                        }
                    }
                }
            }            
        }
    }
}