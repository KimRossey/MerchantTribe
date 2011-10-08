using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Datalayer;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{
    public class BaseStoreCategoryPage : BaseStorePage
    {


        private string _blockId = string.Empty;
        //private ProductSearchCriteria _productSearchCriteria = null;
        protected CategoryRepository _CategoryRepository = null;

        private Category _LocalCategory = null;
        private CategorySortOrder _sortOrder = CategorySortOrder.None;

        public Category LocalCategory
        {
            get
            {
                if (_LocalCategory == null)
                {
                    _LocalCategory = new Category();
                }
                return _LocalCategory;
            }
            set
            {
                _LocalCategory = value;
                if (!this.IsPostBack && string.IsNullOrEmpty(this.Request.QueryString["sortorder"]))
                {
                    _sortOrder = _LocalCategory.DisplaySortOrder;
                }
            }
        }

        public CategorySortOrder SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        public override bool DisplaysActiveCategoryTab
        {
            get { return true; }
        }

        protected string BlockId
        {
            get { return _blockId; }
            set
            {
                //_settingsManager.BlockId = value;
                _blockId = value;
            }
        }

    
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _CategoryRepository = base.MTApp.CatalogServices.Categories;

            string slug = (string)Page.RouteData.Values["slug"];

            if (slug != string.Empty)
            {
                LocalCategory = _CategoryRepository.FindBySlug(slug);
                SessionManager.CategoryLastId = LocalCategory.Bvin;
                if (LocalCategory == null || string.IsNullOrEmpty(LocalCategory.Bvin))
                {
                    // Check for custom URL
                    string potentialCustom = GetRouteUrl("category-route", new { slug = slug });
                    CustomUrl url = MTApp.ContentServices.CustomUrls.FindByRequestedUrl(potentialCustom);
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

                    // Check for custom page URL
                    string pageCustom = GetRouteUrl("custompage-route", new { slug = slug });
                    CustomUrl pageurl = MTApp.ContentServices.CustomUrls.FindByRequestedUrl(pageCustom);
                    if (pageurl != null)
                    {
                        if (pageurl.Bvin != string.Empty)
                        {
                            if (pageurl.IsPermanentRedirect)
                            {
                                Response.RedirectPermanent(pageurl.RedirectToUrl);
                            }
                            else
                            {
                                Response.Redirect(pageurl.RedirectToUrl);
                            }
                        }
                    }

                    // No custom found, go to error
                    EventLog.LogEvent("Category Page", "Requested Category " + slug + " was not found", MerchantTribe.Web.Logging.EventLogSeverity.Warning);
                    MerchantTribe.Commerce.Utilities.UrlRewriter.RedirectToErrorPage(MerchantTribe.Commerce.ErrorTypes.Category, Response);
                }
            }
            else
            {
                MerchantTribe.Commerce.Utilities.UrlRewriter.RedirectToErrorPage(MerchantTribe.Commerce.ErrorTypes.Category, Response);
            }
        }
    }
}