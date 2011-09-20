using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using System.Text;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Controllers
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
            List<Store> stores = MTApp.AccountServices.Stores.FindStoresCreatedAfterDateForSuper(DateTime.UtcNow.AddDays(-30));

            List<Models.SuperStoreViewModel> viewmodel = new List<Models.SuperStoreViewModel>();
            foreach (Store s in stores)
            {
                SuperStoreViewModel m = new SuperStoreViewModel(s);
                m.Users = MTApp.AccountServices.FindAdminUsersByStoreId(s.Id);
                viewmodel.Add(m);
            }

            return View(viewmodel);
        }
                                 
    }
}
