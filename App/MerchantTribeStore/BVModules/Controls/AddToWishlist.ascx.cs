using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Metrics;
using MerchantTribe.Commerce.Orders;

namespace BVCommerce
{

    partial class BVModules_Controls_AddToWishlist : MerchantTribe.Commerce.Content.BVUserControl
    {
        public delegate void ClickedDelegate(AddToWishlistClickedEventArgs args);
        public event ClickedDelegate Clicked;

        protected void AddToWishlist_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddToWishlistClickedEventArgs args = new AddToWishlistClickedEventArgs();
            if (Clicked != null)
            {
                Clicked(args);
            }

            if (args.IsValid)
            {
                if (Page.IsValid)
                {
                    Product selectedProduct;
                    if (args.VariantsDisplay != null)
                    {
                        if (!args.VariantsDisplay.IsValid())
                        {
                            return;
                        }
                        selectedProduct = args.VariantsDisplay.GetSelectedProduct(null);
                        if (selectedProduct == null)
                        {
                            EventLog.LogEvent("AddToWishlist.aspx.cs", "Product could not be found from Variants display. Current product: " + args.Page.LocalProduct.Bvin + " " + args.Page.LocalProduct.Sku, EventLogSeverity.Error);
                            return;
                        }
                        else
                        {
                            if (args.VariantsDisplay.IsValidCombination)
                            {
                                args.Page.LocalProduct = selectedProduct;
                            }
                            else
                            {
                                args.MessageBox.ShowError("Cannot Add To " + SiteTerms.GetTerm(SiteTermIds.WishList) + ". Current Selection Is Not Available.");
                            }
                        }
                    }
                    else
                    {
                        selectedProduct = args.Page.LocalProduct;
                    }

                    args.Page.ModuleProductQuantity = args.Quantity;

                    LineItem li = new LineItem();
                    li.ProductId = selectedProduct.Bvin;
                    li.Quantity = args.Quantity;
                    if (args.VariantsDisplay != null)
                    {
                        args.VariantsDisplay.WriteValuesToLineItem(li);
                    }

                    //if (!string.IsNullOrEmpty(SessionManager.GetCurrentUserId()))
                    //{
                    //    WishList.AddItemToList(SessionManager.GetCurrentUserId(), li);
                    //}
                    //else
                    //{
                    //    Session.Add(WebAppSettings.WishlistItemSessionKey, li);
                    //}
                    //Response.Redirect("~/MyAccount_WishList.aspx");
                }
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                ThemeManager themes = MyPage.BVApp.ThemeManager();
                this.AddToWishlist.ImageUrl = themes.ButtonUrl("AddToWishList", Request.IsSecureConnection);
            }
        }
    }
}