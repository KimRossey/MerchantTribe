using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVCommerce.Models;
using System.Text;
using BVSoftware.Commerce.Content;

namespace BVCommerce.Controllers
{
    public class ContentColumnController : Shared.BaseStoreController
    {
        [ChildActionOnly]
        public ActionResult Index(string id)
        {                                    
            ContentColumn col = null;            
            col = BVApp.ContentServices.Columns.Find(id);            
            if (col == null)
            {
                col = BVApp.ContentServices.Columns.FindByDisplayName(id);
            }
            if (col == null) return Content("Column Not Found");
            
            return View(col);
        }
    }
}
