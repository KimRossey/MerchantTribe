using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Storage;
using MerchantTribeStore.Areas.AdminContent.Models;
using MerchantTribeStore.Models;

namespace MerchantTribeStore.Areas.AdminContent.Controllers
{
    public class FileManagerController : MerchantTribeStore.Controllers.Shared.BaseAdminController
    {

        private BreadCrumbViewModel BuildBreadCrumbs(string path)
        {
            BreadCrumbViewModel result = new BreadCrumbViewModel();
            result.HideHomeLink = true;

            List<BreadCrumbItem> items = new List<BreadCrumbItem>();

            string workingPath = path.TrimEnd('\\');

            bool finished = false;
            while (finished == false)
            {
                workingPath = workingPath.TrimEnd('\\');
                if (workingPath.Length < 1)
                {
                    finished = true;
                    break;
                }                
                string dir = System.IO.Path.GetFileName(workingPath);

                items.Add(new BreadCrumbItem()
                {
                    Link = Url.Content("~/bvadmin/content/filemanager?path=" + workingPath),
                    Name = dir,
                    Title = dir
                });
                workingPath = System.IO.Path.GetDirectoryName(workingPath);
            }
            items.Add(new BreadCrumbItem()
            {
                Link = Url.Content("~/bvadmin/content/filemanager"),
                Name = "Root",
                Title = "Root"
            });

            for (int i = items.Count - 1; i >= 0; i--)
            {
                result.Items.Enqueue(items[i]);
            }
            return result;
        }

        //
        // GET: /AdminContent/FileManager/        
        public ActionResult Index()
        {
            string path = Request.QueryString["path"] ?? string.Empty;
            string cleanPath = DiskStorage.FileManagerCleanPath(path);

            FileManagerViewModel model = new FileManagerViewModel(cleanPath);
            model.Directories = DiskStorage.FileManagerListDirectories(MTApp.CurrentStore.Id, cleanPath);
            model.Files = DiskStorage.FileManagerListFiles(MTApp.CurrentStore.Id, cleanPath);
            model.BreadCrumbs = BuildBreadCrumbs(cleanPath);           
            return View(model);
        }

    }
}
