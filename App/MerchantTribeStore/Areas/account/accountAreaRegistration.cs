﻿using System.Web.Mvc;

namespace MerchantTribeStore.Areas.account
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
            context.MapRoute("customerlogin", "signin",
                new { controller = "Authentication", action="SignIn"});
            context.MapRoute("customerlogout", "signout", 
                new { controller = "Authentication", action = "SignOut" });

            context.MapRoute("forgotpassword-route", "account/forgotpassword/{email}/{returnmode}",
                new { controller = "Authentication", action = "ForgotPassword", email = "", returnmode = "" });            

            context.MapRoute(
                "account_default",
                "account/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, controller="OrderHistory" }
            );
            
        }
    }
}
