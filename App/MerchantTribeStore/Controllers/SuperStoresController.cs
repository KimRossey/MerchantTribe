using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using System.Text;
using BVCommerce.Models;

namespace BVCommerce.Controllers
{
    public class SuperStoresController : Shared.BaseSuperController
    {
        //
        // GET: /Super/Stores/

        public ActionResult Index()
        {
            return View();
        }

        // Get: /Super/Stores/NewStoreReport
        public ActionResult NewStoreReport()
        {
            List<Store> stores = BVApp.AccountServices.Stores.FindStoresCreatedAfterDateForSuper(DateTime.UtcNow.AddDays(-30));

            List<Models.SuperStoreViewModel> viewmodel = new List<Models.SuperStoreViewModel>();
            foreach (Store s in stores)
            {
                SuperStoreViewModel m = new SuperStoreViewModel(s);
                m.Users = BVApp.AccountServices.FindAdminUsersByStoreId(s.Id);
                viewmodel.Add(m);
            }

            return View(viewmodel);
        }
                                 
    }
}
