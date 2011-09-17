using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using MerchantTribe.Web.Search;

namespace BVCommerce.Controllers
{
    public class SuperController : Shared.BaseSuperController
    {
        //
        // GET: /Super/
        public ActionResult Index()
        {

            ViewData["ActiveStores"] = BVApp.AccountServices.Stores.CountOfActive().ToString();
            ViewData["FreeStores"] = BVApp.AccountServices.Stores.CountOfFree().ToString();
            ViewData["PaidStores"] = BVApp.AccountServices.Stores.CountOfPaid().ToString();
            ViewData["Products"] = BVApp.CatalogServices.Products.FindAllForAllStoresCount().ToString();
            ViewData["SalesVolume"] = BVApp.OrderServices.Transactions.FindTotalTransactionsForever().ToString("C");

            return View();
        }

        // GET: /Super/Search
        public ActionResult Search()
        {
            TempData["results"] = string.Empty;
            return View();
        }
        // POST: /Super/Search
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("Search")]
        public ActionResult Search(string keyword)
        {
            string r = "No Results Found";

            SearchManager searcher = new SearchManager();
            int total = 0;
            List<SearchObject> results = searcher.DoSearchForAllStores(keyword.Trim(), 1, 100, ref total);

            if (results != null)
            {
                r = "Found " + total.ToString() + " matches";
                r += "<ul>";

                foreach (SearchObject obj in results)
                {
                    r += "<li>";
                    r += obj.Title;
                    r += "</li>";
                }
                r += "</ul>";
            }

            TempData["results"] = r;
            return View();
        }

        // Get: /Super/RebuildSearch
        public ActionResult RebuildSearch()
        {
            TempData["message"] = string.Empty;
            return View();
        }

        // POST: /Super/RebuildSearch
        [AcceptVerbs(HttpVerbs.Post)]
        [ActionName("RebuildSearch")]
        public ActionResult RebuildSearchPost()
        {
            SearchManager manager = new SearchManager();
            manager.RebuildProductSearchIndex(BVApp);
            TempData["message"] = "Finished rebuild at " + DateTime.Now.ToLocalTime();                        
            return View();
        }
    }
}
