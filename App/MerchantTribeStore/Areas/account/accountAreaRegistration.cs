using System.Web.Mvc;

namespace BVCommerce.Areas.account
{
    public class accountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {            
            context.MapRoute(
                "account_default",
                "account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, controller="OrderHistory" }
            );
        }
    }
}
