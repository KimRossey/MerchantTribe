using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Content;

namespace BVCommerce
{

    partial class ErrorPage : BaseStorePage
    {


        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.Page.Title = MTApp.CurrentStore.StoreName;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            HttpContext.Current.Response.StatusCode = 404;
            string type = Request.QueryString["type"];
            if (string.Compare(type, "product", true) == 0)
            {
                HeaderLiteral.Text = SiteTerms.GetTerm(SiteTermIds.ErrorPageHeaderTextProduct);
                ErrorContentLiteral.Text = SiteTerms.GetTerm(SiteTermIds.ErrorPageContentTextProduct);

                if (HeaderLiteral.Text == string.Empty)
                {
                    HeaderLiteral.Text = "Error finding product";
                }

                if (ErrorContentLiteral.Text == string.Empty)
                {
                    ErrorContentLiteral.Text = "An error occurred while trying to find the specified product.";
                }
                HttpContext.Current.Response.StatusDescription = "Product Not Found";
            }
            else if (string.Compare(type, "category", true) == 0)
            {
                HeaderLiteral.Text = SiteTerms.GetTerm(SiteTermIds.ErrorPageHeaderTextCategory);
                ErrorContentLiteral.Text = SiteTerms.GetTerm(SiteTermIds.ErrorPageContentTextCategory);

                if (HeaderLiteral.Text == string.Empty)
                {
                    HeaderLiteral.Text = "Error finding category";
                }

                if (ErrorContentLiteral.Text == string.Empty)
                {
                    ErrorContentLiteral.Text = "An error occurred while trying to find the specified category.";
                }
                HttpContext.Current.Response.StatusDescription = "Category Not Found";
            }
            else
            {
                string requested = string.Empty;
                if (Request.QueryString["aspxerrorpath"] != null)
                {
                    requested = Request.QueryString["aspxerrorpath"];
                }
                else
                {
                    requested = Request.RawUrl;
                }
                                
                    MerchantTribe.Commerce.Content.CustomUrl url = MTApp.ContentServices.CustomUrls.FindByRequestedUrl(requested);
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
                
               

                HeaderLiteral.Text = SiteTerms.GetTerm(SiteTermIds.ErrorPageHeaderTextGeneric);
                ErrorContentLiteral.Text = SiteTerms.GetTerm(SiteTermIds.ErrorPageContentTextGeneric);

                if (HeaderLiteral.Text == string.Empty)
                {
                    HeaderLiteral.Text = "Error finding page";
                }

                if (ErrorContentLiteral.Text == string.Empty)
                {
                    ErrorContentLiteral.Text = "An error occurred while processing your request.";
                }
            }
        }

    }
}