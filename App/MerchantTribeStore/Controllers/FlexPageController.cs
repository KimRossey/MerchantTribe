using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using System.Text;
using MerchantTribe.Commerce.Content.Parts;

namespace MerchantTribeStore.Controllers
{
    public class FlexPageController : Shared.BaseStoreController
    {


        private void CheckFor301(string slug)
        {
            MerchantTribe.Commerce.Content.CustomUrl url = MTApp.ContentServices.CustomUrls.FindByRequestedUrl(slug);
            if (url != null)
            {
                if (url.Bvin != string.Empty)
                {
                    if (url.IsPermanentRedirect)
                    {
                        Response.RedirectPermanent(url.RedirectToUrl);
                    }
                    else
                    {
                        Response.Redirect(url.RedirectToUrl);
                    }
                }
            }
        }

        //
        // GET: /FlexPage/

        [HandleError]
        public ActionResult Index(string slug)
        {            
            Category cat = MTApp.CatalogServices.Categories.FindBySlugForStore(slug, MTApp.CurrentRequestContext.CurrentStore.Id);
            if (cat == null) cat = new Category();
            
            if (cat.SourceType != CategorySourceType.FlexPage)
            {
                CheckFor301("/" + slug);                
                return Redirect("~/Error?type=category");
            }

            ViewBag.Title = cat.MetaTitle;            
            ViewBag.MetaKeywords = cat.MetaKeywords;
            ViewBag.MetaDescription = cat.MetaDescription;
            ViewData["basecss"] = Url.Content("~/content/FlexBase.css");
            ViewData["slug"] = slug;
            MTApp.CurrentRequestContext.FlexPageId = cat.Bvin;
            MTApp.CurrentRequestContext.UrlHelper = this.Url;
            
            if (MTApp.IsEditMode)
            {
                ViewData["EditCss"] = "<link href=\"" + Url.Content("~/css/flexedit/styles.css") + "\" rel=\"stylesheet\" type=\"text/css\" />";
                string editJS = "<script type=\"text/javascript\" src=\"" + Url.Content("~/content/FlexEdit.js") + "\"></script>";                
                editJS += "<script type=\"text/javascript\" src=\"" + Url.Content("~/scripts/Silverlight.js") + "\"></script>";
              
                ViewData["EditJs"] = editJS;
                ViewData["EditorPanel"] = GetEditorPanel(cat.Bvin);
                ViewData["EditPopup"] = GetEditPopup();
            }

            if (cat.Versions.Count < 1)
            {
                cat.Versions.Add(new CategoryPageVersion() { AdminName = "First Version", PublishedStatus = PublishStatus.Published, Root = cat.GetSimpleSample() });
            }

            // Load Content Parts for Page        
            try
            {
                if (MTApp.IsEditMode)
                {
                    if (Request["preview"] == "1")
                    {
                        ViewData["ContentParts"] = cat.Versions[0].Root.RenderForDisplay(MTApp.CurrentRequestContext, cat);
                    }
                    else
                    {
                        ViewData["ContentParts"] = cat.Versions[0].Root.RenderForEdit(MTApp.CurrentRequestContext, cat);
                    }
                }
                else
                {
                    ViewData["ContentParts"] = cat.Versions[0].Root.RenderForDisplay(MTApp.CurrentRequestContext, cat);
                }
            }
            catch (Exception ex)
            {
                ViewData["ContentParts"] = ex.Message + ex.StackTrace;
            }

            return View();
        }

        private string GetEditorPanel(string bvin)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=\"flexedit\">");

            sb.Append("<div class=\"flexbuttonright\"><a href=\"" + Url.Content("~/bvadmin/catalog/Categories_FinishedEditing.aspx?id=" + bvin) + "\"><img src=\"" + Url.Content("~/images/system/flexedit/btnClose.png") + "\" alt=\"Close Editor\" /></a></div>");

            if (Request["preview"] == "1")
            {
                sb.Append("<div class=\"flexbuttonright\"><a href=\"" + Url.Content(Request.AppRelativeCurrentExecutionFilePath) + "\"><img src=\"" + Url.Content("~/images/system/flexedit/btnPreviewOn.png") + "\" alt=\"Preview Is On\" /></a></div>");
            }
            else
            {
                sb.Append("<div class=\"flexbuttonright\"><a href=\"" + Url.Content(Request.AppRelativeCurrentExecutionFilePath + "?preview=1") + "\"><img src=\"" + Url.Content("~/images/system/flexedit/btnPreviewOff.png") + "\" alt=\"Preview Is Off\" /></a></div>");
            }

            sb.Append("<div class=\"dragpart dragbutton\" id=\"columncontainer\" ><img src=\"" + Url.Content("~/images/system/flexedit/btnColumns.png") + "\" alt=\"Columns\" /></div>");            
            sb.Append("<div class=\"dragpart dragbutton\" id=\"htmlpart\" ><img src=\"" + Url.Content("~/images/system/flexedit/btnHtml.png") + "\" alt=\"HTML\" /></div>");
            sb.Append("<div class=\"dragpart dragbutton\" id=\"image\" ><img src=\"" + Url.Content("~/images/system/flexedit/btnImage.png") + "\" alt=\"Image\" /></div>");
            
            sb.Append("<div class=\"hidden\" id=\"flexpageid\">" + bvin + "</div>");
            sb.Append("<div class=\"hidden\" id=\"flexjsonurl\">" + Url.Content("~/flexpartjson/" + bvin) + "</div>");
            sb.Append("<div class=\"hidden\" id=\"flexpageediting\"></div>");
            sb.Append("</div>");
            return sb.ToString();
        }

        private string GetEditPopup()
        {
            StringBuilder sb = new StringBuilder();            
            sb.Append("<div class=\"editormodal\">");
            sb.Append("<div class=\"editorpopover\">");
            sb.Append("<a id=\"editorclose\" href=\"#\">Close</a>");
            sb.Append("<form id=\"editorform\" action=\"\" method=\"post\"></form><br />");            
            sb.Append("</div>");
            sb.Append("</div>");            
                                               
            return sb.ToString();
        }


       
    }
}
