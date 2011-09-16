using System;
using System.Text;
using System.Web.UI;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Content;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Utilities;
using System.Linq;
using System.Collections.Generic;

namespace BVCommerce
{

    partial class ProductPage : BaseStorePage, IProductPage
    {

        private Product _LocalProduct = new Product();
        private int _ModuleProductQuantity = 1;
        private IMessageBox _errorMessage = null;

        protected string TabScriptSource = string.Empty;

        public Product LocalProduct
        {
            get { return _LocalProduct; }
            set { _LocalProduct = value; }
        }
        public int ModuleProductQuantity
        {
            get { return _ModuleProductQuantity; }
            set { _ModuleProductQuantity = value; }
        }
        public IMessageBox MessageBox
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        public bool DisplaysActiveCategoryTab1
        {
            get { return true; }
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            this.AddBodyClass("store-product-page");

            string slug = (string)Page.RouteData.Values["slug"];

            if (slug != string.Empty)
            {
                _LocalProduct = BVApp.CatalogServices.Products.FindBySlug(slug);
                bool possibleError = false;
                if (_LocalProduct == null)
                {
                    possibleError = true;
                }
                else if (_LocalProduct.Status == ProductStatus.Disabled)
                {
                    possibleError = true;                    
                }

                if (possibleError)
                {
                    // Check for custom URL
                    string potentialCustom = GetRouteUrl("bvroute", new { slug = slug });
                    CustomUrl url = BVApp.ContentServices.CustomUrls.FindByRequestedUrl(potentialCustom);
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

                    UrlRewriter.RedirectToErrorPage(BVSoftware.Commerce.ErrorTypes.Product, Response);
                }



            }
            else
            {                
                UrlRewriter.RedirectToErrorPage(BVSoftware.Commerce.ErrorTypes.Product, Response);
            }

            if (LocalProduct.Bvin == string.Empty)
            {                
                EventLog.LogEvent("Product Page", "Requested Product slug " + slug + " was not found", BVSoftware.Commerce.Metrics.EventLogSeverity.Error);
            }
            else
            {
                if (LocalProduct.PreContentColumnId != string.Empty)
                {
                    this.PreContentColumn.ColumnID = LocalProduct.PreContentColumnId;
                    this.PreContentColumn.LoadColumn();
                }
                if (LocalProduct.PostContentColumnId != string.Empty)
                {
                    this.PostContentColumn.ColumnID = LocalProduct.PostContentColumnId;
                    this.PostContentColumn.LoadColumn();
                }

                // Render Options
                if (LocalProduct.HasOptions())
                {
                    this.phOptions.Controls.Add(new LiteralControl("<div class=\"options\">"));
                    BVSoftware.Commerce.Utilities.HtmlRendering.ProductOptionsAsControls(LocalProduct.Options, phOptions);
                    this.phOptions.Controls.Add(new LiteralControl("<div class=\"clear\"></div></div>"));
                }

            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            TabScriptSource = Page.ResolveUrl("~/Tabs.js");

            Page.ClientScript.RegisterClientScriptBlock(typeof(System.Web.UI.Page), "productpagescripts", RenderJQuery(), true);

            this.litRelatedItemsTitle.Text = SiteTerms.GetTerm(SiteTermIds.RelatedItems);
            this.litRelatedItemsTitle2.Text = this.litRelatedItemsTitle.Text;

            if (LocalProduct != null)
            {
                // store in hidden form field for ajax posts backs
                this.productbvin.Value = LocalProduct.Bvin;

                // Page Title
                if (LocalProduct.MetaTitle.Trim().Length > 0)
                {
                    this.Title = LocalProduct.MetaTitle;
                }
                else
                {
                    this.Title = LocalProduct.ProductName;
                }

                // Meta Keywords
                if (LocalProduct.MetaKeywords.Trim().Length > 0)
                {
                    Page.MetaKeywords = LocalProduct.MetaKeywords;
                }

                // Meta Description
                if (LocalProduct.MetaDescription.Trim().Length > 0)
                {
                    Page.MetaDescription = LocalProduct.MetaDescription;
                }
            }

            this.btnAddToCart.ImageUrl = this.BVApp.ThemeManager().ButtonUrl("addtocart", Request.IsSecureConnection);

            CheckForBackOrder();


            if (!Page.IsPostBack)
            {
                if (LocalProduct != null)
                {

                    OptionSelectionList selections = new OptionSelectionList();

                    decimal lineitemQty = 0;
                    if (Request.QueryString["LineItemId"] != null)
                    {
                        string orderBvin = Request.QueryString["OrderBvin"];
                        string lineItemString = Request.QueryString["LineItemId"];
                        long lineItemId = 0;
                        long.TryParse(lineItemString, out lineItemId);

                        Order o = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
                        if (o != null)
                        {
                            var li = o.Items.Where(y => y.Id == lineItemId).SingleOrDefault();
                            if (li != null)
                            {
                                lineitemQty = li.Quantity;
                                selections = li.SelectionData;
                            }
                        }                                                                            
                    }

                    // Load Values from line Item if there are any
                    SetOptionsFromSelections(selections);

                    //Me.litMessage.Text = WebAppSettings.ItemAddedToCartText

                    PopulateProductInfo(true);
                    //Record Last 10 Products Viewed
                    PersonalizationServices.RecordProductViews(LocalProduct.Bvin);

                    // Minimum Quantity Settings
                    int min = 1;
                    if (lineitemQty > LocalProduct.MinimumQty)
                    {
                        min = (int)Math.Round(lineitemQty, 0);
                    }
                    else
                    {
                        min = LocalProduct.MinimumQty;
                    }
                    if (min < 1) min = 1;
                    QuantityField.Text = min.ToString();
                }
            }

            this.ProductReviewDisplay1.LoadReviews(LocalProduct);
            this.RelatedItems1.LoadRelatedItems(LocalProduct.Bvin);
            RenderAdditionalImages(LocalProduct.Bvin);
        }

        private void SetOptionsFromSelections(OptionSelectionList selections)
        {
            if ((LocalProduct.HasOptions()))
            {
                foreach (Option opt in LocalProduct.Options)
                {
                    opt.SetSelectionsInPlaceholder(phOptions, selections);
                }
            }
        }

        private string RenderJQuery()
        {
            StringBuilder sb = new StringBuilder();

            

            sb.Append(" var currentmediumimage; ");


            // Modal Popup Code
            sb.Append("function CloseDialog() {");
            sb.Append("$('.overlay').remove();");
            sb.Append("$('.modal2').hide();");
            sb.Append("}");

            sb.Append("function OpenDialog(lnk) {");
            sb.Append("$('<div />').addClass('overlay').appendTo('body').show();");
            sb.Append("$('.modal2').show();");
            sb.Append("var loadid = $(lnk).attr('href');");            
            sb.Append("$('#popoverpage2').attr('src', loadid);");
            sb.Append("}");

            sb.Append("$(document).keyup(function(e) {");
            sb.Append(" if (e.keyCode == 27) { CloseDialog(); } ");
            sb.Append("});");

            if ((LocalProduct.HasOptions()))
            {
                // Evaluate Selections
                sb.Append("function EvaluateSelections() {" + System.Environment.NewLine);
                sb.Append(" var loadingUrl = '" + Page.ResolveUrl("~/images/system/ajax-loader.gif") + "';" + System.Environment.NewLine);
                sb.Append(" var loadingTag = '<img src=\"' + loadingUrl + '\" border=\"0\" alt=\"loading...\" />';" + System.Environment.NewLine);
                //sb.Append(" $('#sku').html(loadingTag);")
                //sb.Append(" $('#pricewrapper').html(loadingTag);")
                sb.Append(" $('.buttons').hide();" + System.Environment.NewLine);
                sb.Append(" $('#localmessage').html('<label>&nbsp;</label><span class=\"choice\">' + loadingTag + '</span>');" + System.Environment.NewLine);
                //sb.Append(" $('#imgMain').attr('src',loadingUrl);"+ System.Environment.NewLine)

                // Write each option to temp varible so we can get it's value as a string
                // This ensures the JSON will get correctly quoted by JQuery
                foreach (Option opt in LocalProduct.Options)
                {
                    if (opt.OptionType == OptionTypes.CheckBoxes | opt.OptionType == OptionTypes.DropDownList | opt.OptionType == OptionTypes.RadioButtonList)
                    {
                        sb.Append("var opt" + opt.Bvin.Replace("-", "") + "=");
                        if (opt.OptionType == OptionTypes.RadioButtonList)
                        {
                            sb.Append("$('.radio" + opt.Bvin.Replace("-", "") + ":checked').val();" + System.Environment.NewLine);

                        }
                        else if (opt.OptionType == OptionTypes.CheckBoxes)
                        {
                            sb.Append("$('.check" + opt.Bvin.Replace("-", "") + ":checked').val();" + System.Environment.NewLine);
                        }
                        else
                        {
                            sb.Append("$('#opt" + opt.Bvin.Replace("-", "") + "').val();" + System.Environment.NewLine);
                        }
                        sb.Append("opt" + opt.Bvin.Replace("-", "") + "+='';" + System.Environment.NewLine);
                        //sb.Append("alert(opt" + opt.Bvin.Replace("-", "") + ");" + System.Environment.NewLine);                            
                    }
                }

                sb.Append("$.post('" + Page.ResolveUrl("~/product_validate.aspx") + "'," + System.Environment.NewLine);
                sb.Append(" {\"productbvin\":\"" + LocalProduct.Bvin + "\"" + System.Environment.NewLine);
                foreach (Option opt in LocalProduct.Options)
                {
                    if (opt.OptionType == OptionTypes.CheckBoxes | opt.OptionType == OptionTypes.DropDownList | opt.OptionType == OptionTypes.RadioButtonList)
                    {
                        sb.Append(", \"opt" + opt.Bvin.Replace("-", "") + "\": opt" + opt.Bvin.Replace("-", ""));
                    }
                }
                sb.Append("}, " + System.Environment.NewLine);
                sb.Append("  function(data) {" + System.Environment.NewLine);
                //sb.Append(" alert(data.Message);" + System.Environment.NewLine);
                
                sb.Append(" if (data.Message === null || data.Message.Length < 1) { $('#localmessage').html('');} else {");                
                sb.Append("$('#localmessage').html('<label>&nbsp;</label><span class=\"choice\">' + data.Message + '</span>');");
                sb.Append("}" + System.Environment.NewLine);

                sb.Append(" $('#imgMain').attr('src',data.ImageUrl);" + System.Environment.NewLine);
                sb.Append(" $('#sku').html(data.Sku);" + System.Environment.NewLine);
                sb.Append(" $('.stockdisplay').html(data.StockMessage);" + System.Environment.NewLine);
                sb.Append(" $('#pricewrapper').html(data.Price);" + System.Environment.NewLine);
                //sb.Append(" alert(data.IsValid); "+ System.Environment.NewLine)
                sb.Append(" if (data.IsValid === false) { $('.buttons').hide();} else { $('.buttons').show();}" + System.Environment.NewLine);
                sb.Append("  }, 'json');" + System.Environment.NewLine);
                // end post function            
                sb.Append("}" + System.Environment.NewLine);
                // end evaluate function
            }


            // Document Ready Function
            sb.Append("$(document).ready(function() {" + System.Environment.NewLine);
           
            if ((LocalProduct.HasOptions()))
            {
                sb.Append("$(\".isoption\").change(function() {" + System.Environment.NewLine);
                sb.Append(" EvaluateSelections();" + System.Environment.NewLine);
                sb.Append(" return true;" + System.Environment.NewLine);
                sb.Append("});" + System.Environment.NewLine);

                sb.Append(" EvaluateSelections(); " + System.Environment.NewLine);
            }


            // AdditionalProductImage
            sb.Append(" $('.additionalimages a').mouseover( function() {");
            sb.Append(" var newurl = $(this).attr('alt'); ");                        
            sb.Append(" var currentimg = $('#imgMain').attr('src'); ");
            sb.Append(" $('#imgMainLast').val(currentimg); ");            
            sb.Append(" $('#imgMain').attr('src',newurl); ");
            sb.Append(" });");

            sb.Append(" $('.additionalimages a').mouseout( function() {");
            sb.Append(" var originalimg = $('#imgMainLast').val();");
            sb.Append(" $('#imgMain').attr('src',originalimg); ");
            sb.Append(" });");


            // Popup Close
            sb.Append("$('#dialogclose').click(function () {");
            sb.Append("CloseDialog();");
            sb.Append(" return false;");
            sb.Append(" });");

            sb.Append("$('#dialogclose2').click(function () {");
            sb.Append("CloseDialog();");
            sb.Append(" return false;");
            sb.Append(" });");

            // Popup Open
            sb.Append("$('.popover').click(function () {");
            sb.Append("OpenDialog($(this));");
            sb.Append("return false;");
            sb.Append("});");

            sb.Append("CloseDialog();");
               
            sb.Append("});" + System.Environment.NewLine + System.Environment.NewLine);
            // End of Document Ready



            

            return sb.ToString();
        }

        public string RenderPrices(string userId)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"prices\">");

            UserSpecificPrice productDisplay = BVApp.PriceProduct(LocalProduct,BVApp.CurrentCustomer, null);
            if (productDisplay.ListPriceGreaterThanCurrentPrice)
            {
                sb.Append("<label>" + SiteTerms.GetTerm(SiteTermIds.ListPrice) + "</label>");
                sb.Append("<span class=\"choice\"><strike>");
                sb.Append(LocalProduct.ListPrice.ToString("C"));
                sb.Append("</strike></span>");
            }


            sb.Append("<label>" + SiteTerms.GetTerm(SiteTermIds.SitePrice) + "</label>");
            sb.Append("<span class=\"choice\">");
            sb.Append(productDisplay.DisplayPrice());
            sb.Append("</span>");

            if ((productDisplay.BasePrice < productDisplay.ListPrice) && (productDisplay.OverrideText.Trim().Length < 1))
            {
                sb.Append("<label>" + SiteTerms.GetTerm(SiteTermIds.YouSave) + "</label>");
                sb.Append("<span class=\"choice\">");
                sb.Append(productDisplay.Savings.ToString("c") + " - " + productDisplay.SavingsPercent + System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.PercentSymbol);
                sb.Append("</span>");
            }

            sb.Append("<div class=\"clear\"></div></div>");
            return sb.ToString();
        }
        public void CheckForBackOrder()
        {
            InventoryCheckData data = BVApp.CatalogServices.InventoryCheck(LocalProduct, string.Empty);
            this.litStockDisplay.Text = data.InventoryMessage;
            this.QuantityField.Enabled = data.IsAvailableForSale;
            this.btnAddToCart.Visible = data.IsAvailableForSale;
        }

        private void RenderAdditionalImages(string productBvin)
        {
            List<ProductImage> images = BVApp.CatalogServices.ProductImages.FindByProductId(productBvin);

            if (images.Count < 1) return;

            StringBuilder sb= new StringBuilder();
            sb.Append("<div class=\"additionalimages\">");
            foreach (ProductImage img in images)
            {
                RenderSingleAdditionalImage(sb, img);
            }
            sb.Append("</div>");
            this.litAdditionalImages.Text = sb.ToString();
        }
        private void RenderSingleAdditionalImage(StringBuilder sb, ProductImage img)
        {
            string mediumUrl = BVSoftware.Commerce.Storage.DiskStorage.ProductAdditionalImageUrlMedium(BVApp.CurrentStore.Id,
                                                                                                       img.ProductId,
                                                                                                       img.Bvin,
                                                                                                       img.FileName,
                                                                                                       false);
            string largeUrl = BVSoftware.Commerce.Storage.DiskStorage.ProductAdditionalImageUrlOriginal(BVApp.CurrentStore.Id,
                                                                                                       img.ProductId,
                                                                                                       img.Bvin,
                                                                                                       img.FileName,
                                                                                                       false);
            sb.Append("<a href=\"" + largeUrl + "\" alt=\"" + mediumUrl + "\" class=\"popover\">");
            sb.Append("<img src=\"");
            sb.Append(BVSoftware.Commerce.Storage.DiskStorage.ProductAdditionalImageUrlTiny(BVApp.CurrentStore.Id,
                                                                                                      img.ProductId,
                                                                                                      img.Bvin,
                                                                                                      img.FileName,
                                                                                                      false));
            sb.Append("\" border=\"0\" alt=\"" + img.AlternateText + "\" />");
            sb.Append("</a>");
        }
    
        public void PopulateProductInfo(bool IsValidCombination)
        {
            // Name Fields
            this.lblName.Text = this.LocalProduct.ProductName;
            this.lblSku.Text = this.LocalProduct.Sku;
            this.lblDescription.Text = this.LocalProduct.LongDescription;

            this.imgMain.ImageUrl = BVSoftware.Commerce.Storage.DiskStorage.ProductImageUrlMedium(BVApp.CurrentStore.Id, LocalProduct.Bvin, LocalProduct.ImageFileSmall, Request.IsSecureConnection);
            this.imgMain.AlternateText = LocalProduct.ImageFileSmallAlternateText;

            // Cross Sell
            //Me.CrossSellDisplay.Product = Me.LocalProduct
            //Me.CrossSellDisplay.DataBind()

            if (IsValidCombination)
            {
                if (!ProductControlsPanel.Visible)
                {
                    ProductControlsPanel.Visible = true;
                }

                string userId = SessionManager.GetCurrentUserId();

                // Prices            
                this.litPrices.Text = RenderPrices(userId);
            }

            else
            {
                ProductControlsPanel.Visible = false;
            }


            this.TabNavReviews.Visible = LocalProduct.AllowReviews;
            this.pnlReviews.Visible = LocalProduct.AllowReviews;
            //this.TabNavSuggested.Visible = false;
            //this.pnlSuggested.Visible = false;

            // Render Other Tabs Here
            RenderTabs(LocalProduct);

        }

        private void RenderTabs(BVSoftware.Commerce.Catalog.Product p)
        {
            this.litOtherTabsNav.Text = string.Empty;
            this.litOtherTabs.Text = string.Empty;

            if (p == null) return;
            if (p.Tabs == null) return;

            foreach (BVSoftware.Commerce.Catalog.ProductDescriptionTab t in p.Tabs.OrderBy(y => y.SortOrder))
            {
                this.litOtherTabsNav.Text += "<li id=\"TabNav" + t.Bvin + "\"><a href=\"#Tab" + t.Bvin + "\">" + t.TabTitle + "</a></li>";
                this.litOtherTabs.Text += "<div><div id=\"Tab" + t.Bvin + "\"><div class=\"padded\"><h2>"
                                            + t.TabTitle + "</h2>" + t.HtmlData + "</div></div></div>";
            }
        }


        private OptionSelectionList ParseSelections(Product p)
        {
            OptionSelectionList result = new OptionSelectionList();

            foreach (Option opt in p.Options)
            {
                OptionSelection selected = opt.ParseFromPlaceholder(phOptions);
                if (selected != null)
                {
                    result.Add(selected);
                }
            }

            return result;
        }

        private bool ValidateSelections(Product p, OptionSelectionList selections)
        {
            bool result = false;

            if ((p.HasOptions()))
            {
                if ((p.HasVariants()))
                {
                    Variant v = p.Variants.FindBySelectionData(selections, p.Options);
                    if ((v != null))
                    {
                        result = true;
                    }
                    else
                    {
                        this.litMessage.Text = "<div class=\"flash-message-warning\">The options you've selected aren't available at the moment. Please select different options.</div>";
                    }
                }
                else
                {
                    result = true;
                }

                // Make sure no "labels" are selected
                if (selections.HasLabelsSelected())
                {
                    result = false;
                    this.litMessage.Text = "<div class=\"flash-message-warning\">Please make all selections before adding to cart.</div>";
                }

            }
            else
            {
                result = true;
            }

            return result;
        }

        protected void btnAddToCart_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            string destination = string.Empty;
            OptionSelectionList selections = ParseSelections(LocalProduct);

            bool IsPurchasable = ValidateSelections(LocalProduct, selections);

            if ((IsPurchasable))
            {

                int quantity = DetermineQuantityToAdd();
                if (quantity < 1) return;

                LineItem li = BVApp.CatalogServices.ConvertProductToLineItem(LocalProduct, 
                                                                                selections, 
                                                                                quantity, 
                                                                                BVApp);

                Order Basket = SessionManager.CurrentShoppingCart(BVApp.OrderServices);
                if (Basket.UserID != SessionManager.GetCurrentUserId())
                {
                    Basket.UserID = SessionManager.GetCurrentUserId();
                }

                if (Request.QueryString["LineItemId"] != null)
                {
                    string lineItemString = Request.QueryString["LineItemId"];
                    long lineItemId = 0;
                    long.TryParse(lineItemString, out lineItemId);
                    var toRemove = Basket.Items.Where(y => y.Id == lineItemId).SingleOrDefault();
                    if (toRemove != null) Basket.Items.Remove(toRemove);
                }

                BVApp.AddToOrderWithCalculateAndSave(Basket, li);
                SessionManager.SaveOrderCookies(Basket);

                destination = LocalProduct.GetCartDestinationUrl(Basket.Items.Last());                
                Response.Redirect(destination);
            }

        }

        private int DetermineQuantityToAdd()
        {
            int result = -1;

            int quantity = 0;
            if (int.TryParse(this.QuantityField.Text.Trim(), out quantity))
            {
                if (LocalProduct.MinimumQty > 0)
                {
                    if (quantity < LocalProduct.MinimumQty)
                    {
                        MessageBox.ShowError(SiteTerms.ReplaceTermVariable(SiteTerms.GetTerm(SiteTermIds.ProductPageMinimumQuantityError), "quantity", LocalProduct.MinimumQty.ToString()));
                        return -1;
                    }
                }
            }
            else
            {
                if (LocalProduct.MinimumQty > 0)
                {
                    quantity = LocalProduct.MinimumQty;
                }
                else
                {
                    quantity = 1;
                }
            }
                        
            result = quantity;

            return result;
        }

    }
}