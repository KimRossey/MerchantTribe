using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Storage;
using MerchantTribeStore.Areas.AdminContent.Models;

namespace MerchantTribeStore.Areas.AdminContent.Controllers
{
    public class FileManagerController : MerchantTribeStore.Controllers.Shared.BaseAdminController
    {
        //
        // GET: /AdminContent/FileManager/

        public ActionResult Index()
        {
            string path = Request.QueryString["path"] ?? string.Empty;
            string cleanPath = DiskStorage.FileManagerCleanPath(path);

            FileManagerViewModel model = new FileManagerViewModel(cleanPath);
            model.Directories = DiskStorage.FileManagerListDirectories(MTApp.CurrentStore.Id, cleanPath);
            model.Files = DiskStorage.FileManagerListFiles(MTApp.CurrentStore.Id, cleanPath);
                        
            return View(model);
        }

    }
}
