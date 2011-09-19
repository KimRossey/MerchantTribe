using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Content.Parts;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;

namespace BVCommerce.Controllers
{
    public class FlexPartJsonController : Controller
    {

        // Initialize Store Specific Request Data
        MerchantTribe.Commerce.RequestContext _BVRequestContext = new RequestContext();
        public MerchantTribe.Commerce.RequestContext CurrentRequestContext
        {
            get { return _BVRequestContext; }
            set { _BVRequestContext = value; }
        }
        public MerchantTribe.Commerce.Accounts.Store CurrentStore
        {
            get { return _BVRequestContext.CurrentStore; }
            set { _BVRequestContext.CurrentStore = value; }
        }

        private AccountService _AccountService = null;
        public AccountService AccountServices
        {
            get
            {
                if (_AccountService == null)
                {
                    _AccountService = AccountService.InstantiateForDatabase(_BVRequestContext);
                }
                return _AccountService;
            }
        }
        private CatalogService _CatalogService = null;
        public CatalogService CatalogServices
        {
            get
            {
                if (_CatalogService == null)
                {
                    _CatalogService = CatalogService.InstantiateForDatabase(_BVRequestContext);
                }
                return _CatalogService;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Store routing context for URL Rewriting
            CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, AccountServices);
            if (CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }
        }
        private CategoryRepository _CategoryRepository = null;

        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Index(string pageid, string partid)        
        {

            JsonResult result = new JsonResult();

            CurrentRequestContext.FlexPageId = pageid;
            CurrentRequestContext.UrlHelper = this.Url;

            Category flexpage = CatalogServices.Categories.Find(pageid);
            if (flexpage != null)
            {
                IContentPart part = flexpage.FindFlexPart(partid);                                
                result.Data = part.ProcessJsonRequest(Request.Form, this.CurrentRequestContext, flexpage);                                
            }

            return result;
        }

    }
}
