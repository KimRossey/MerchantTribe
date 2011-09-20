using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore.Controllers
{
    public class PoliciesController : Shared.BaseStoreController
    {
        
        // GET: /Policies/{action}
        public ActionResult Index(string policykind)
        {
            Policy p = new Policy();

            switch (policykind.Trim().ToLowerInvariant())
            {
                case "privacy":
                    p = LoadPolicy(PolicyType.Privacy);
                    break;
                case "terms":
                    p = LoadPolicy(PolicyType.TermsAndConditions);
                    break;
                case "returns":
                    p = LoadPolicy(PolicyType.Returns);
                    break;
                case "faq":
                    p = LoadPolicy(PolicyType.Faq);
                    break;
                default:
                    p = new Policy();
                    p.Title = "Content Not Found";
                    p.Blocks.Add(new PolicyBlock() { Description = "<p>The requested policy could not be found. Please close this window and try again.</p>" });
                    break;
            }          

            ViewBag.Title = p.Title;
            return View(p);
        }

        private Policy LoadPolicy(PolicyType type)
        {
            Policy p = MTApp.ContentServices.Policies.FindOrCreateByType(type);
            if (p == null) p = new Policy();
            return p;            
        }

    }
}
