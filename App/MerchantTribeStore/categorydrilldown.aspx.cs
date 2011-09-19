using System.Collections.ObjectModel;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Utilities;
using System.Collections.Generic;
using System.Linq;
using MerchantTribe.Web;

namespace BVCommerce
{
    public partial class categorydrilldown : BaseStoreCategoryPage
    {
        private bool _HideZeroCountChoices = true;

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            this.AddBodyClass("store-categorydrilldown-page");
       }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            if (Pager1.Visible)
            {
                Pager2.Visible = true;
            }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            if (LocalCategory != null)
            {
                int itemsPerPage = 9;

                Pager1.ItemsPerPage = itemsPerPage;
                Pager2.ItemsPerPage = itemsPerPage;


                string key = Request.QueryString["node"] ?? string.Empty;                
                if (key == "-") key = string.Empty;
                string pageString = Request.QueryString["page"] ?? "1";

                PopulateCategoryInfo(key);
                RenderSelections(key, LocalCategory);
                
                RecordCategoryView();
            }
        }

        private void RecordCategoryView()
        {
            SessionManager.CategoryLastId = LocalCategory.Bvin;
        }
    
        public void PopulateCategoryInfo(string key)
        {

            // Page Title
            if (LocalCategory.MetaTitle.Trim().Length > 0)
            {
                this.Title = LocalCategory.MetaTitle;
            }
            else
            {
                this.Title = LocalCategory.Name;
            }

            // Meta Keywords
            if (LocalCategory.MetaKeywords.Trim().Length > 0)
            {
                Page.MetaKeywords = LocalCategory.MetaKeywords;
            }

            // Meta Description
            if (LocalCategory.MetaDescription.Trim().Length > 0)
            {
                Page.MetaDescription = LocalCategory.MetaDescription;
            }

            // Title
            if (LocalCategory.ShowTitle == false)
            {
                this.lblTitle.Visible = false;
            }
            else
            {
                this.lblTitle.Visible = true;
                this.lblTitle.Text = "<h1>" + LocalCategory.Name + "</h1>";
            }

            //Description
            this.DescriptionLiteral.Text = string.Empty;
            if (LocalCategory.Description.Trim().Length > 0)
            {
                if (key == string.Empty)
                {
                    this.DescriptionLiteral.Text = LocalCategory.Description;
                }
            }            
        }

        private void RenderSelections(string key, Category category)
        {
            CategoryFacetManager manager = CategoryFacetManager.InstantiateForDatabase(MTApp.CurrentRequestContext);
            List<CategoryFacet> facets = manager.FindByCategory(category.Bvin);
            List<ProductProperty> properties = LoadProperties(facets);

            if (key == string.Empty)
            {
                key = CategoryFacetKeyHelper.BuildEmptyKey(facets.Count);
            }


            List<long> parsedKey = CategoryFacetKeyHelper.ParseKeyToList(key);
            if (parsedKey.Count != facets.Count)
            {
                return;
            }

            LoadProductsForKey(key, manager);


            List<ProductFacetCount> productCounts = manager.FindCountsOfVisibleFacets(key, facets, properties);
            List<long> visibleIds = manager.FindVisibleFacetsIdsForKey(key, facets);


            StringBuilder sbNotSelected = new StringBuilder();
         
            for (int i = 0; i < facets.Count; i++)
            {
                if (visibleIds.Contains(facets[i].Id))
                {
                    // Find the property that matches the facet
                    var p = (from pr in properties
                             where pr.Id == facets[i].PropertyId
                             select pr).SingleOrDefault();
                    if (p != null)
                    {
                        if (parsedKey[i] < 1)
                        {
                            // not selected
                            RenderNonSelection(sbNotSelected, i, parsedKey[i], key, facets[i], p, productCounts);
                        }
                        else
                        {
                            // selected
                            RenderSelection(sbNotSelected, i, parsedKey[i], key, facets[i], p, facets);
                        }
                    }
                }
            }


            this.litFilters.Text = sbNotSelected.ToString();
        }
        
        private List<ProductProperty> LoadProperties(List<CategoryFacet> facets)
        {

            List<ProductProperty> result = new List<ProductProperty>();

            // Collect ids
            List<long> ids = new List<long>();
            foreach (CategoryFacet facet in facets)
            {
                ids.Add(facet.PropertyId);            
            }

            // Get unsorted
            List<ProductProperty> unsorted = MTApp.CatalogServices.ProductProperties.FindMany(ids);

            // sort
            foreach(CategoryFacet facet in facets)
            {
                ProductProperty temp = unsorted.Where(y => y.Id == facet.PropertyId).FirstOrDefault();
                if (temp != null)
                {
                    result.Add(temp);
                }
            }
                
            return result;
        }

        private void RenderSelection(StringBuilder sb, int index, long selectedValue, string key,
                                   CategoryFacet facet, ProductProperty property, List<CategoryFacet> allFacets)
        {            
            string selectedName = string.Empty;
            var sn = (from c in property.Choices
                      where c.Id == selectedValue
                      select c.ChoiceName).SingleOrDefault();
            if (sn != null)
            {
                selectedName = sn;
                this.CategoryBreadCrumbTrail1.AddExtra(sn);
                //this.Title = this.Title + " | " + sn;
                this.MetaDescription = this.MetaDescription + " | " + sn;
            }
            sb.Append("<h6 class=\"filter\">" + property.DisplayName + "</h6> ");

            string baseUrl = LocalCategory.RewriteUrl + "?node=" + CategoryFacetKeyHelper.ClearSelfAndChildrenFromKey(key, allFacets, facet.Id);
            sb.Append("<ul class=\"filterselected\"><li>");
            sb.Append(" <a href=\"" + baseUrl + "\">");
            sb.Append(selectedName);
            sb.Append(" [clear]</a></li></ul>");
        }


        private void RenderNonSelection(StringBuilder sb, int index, long selectedValue, string key,
                                    CategoryFacet facet, ProductProperty property, List<ProductFacetCount> counts)
        {
            
            sb.Append("<h6 class=\"filter\">" + property.DisplayName + "</h6>");
            sb.Append("<ul class=\"filterselections\">");
            foreach (ProductPropertyChoice c in property.Choices)
            {
                string newKey = CategoryFacetKeyHelper.ReplaceKeyValue(key, index, c.Id);
                string baseUrl = LocalCategory.RewriteUrl + "?node=" + newKey;
                string sqlKey = CategoryFacetKeyHelper.ParseKeyToSqlList(newKey);
                ProductFacetCount fc = new ProductFacetCount();
                foreach (ProductFacetCount f in counts)
                {
                    if (f.Key == sqlKey)
                    {
                        fc = f;
                        break;
                    }
                }

                if (fc.ProductCount > 0 || _HideZeroCountChoices == false)
                {
                    sb.Append("<li><a href=\"" + baseUrl + "\">");
                    sb.Append(c.ChoiceName);
                    sb.Append(" (" + fc.ProductCount.ToString() + ")");
                    sb.Append("</a></li>");
                }
            }
            sb.Append("</ul>");
        }

        private void LoadProductsForKey(string key, CategoryFacetManager manager)
        {
            
            int rowCount = 0;

            List<Product> displayProducts = MTApp.CatalogServices.FindProductsMatchingKey(key, 
                                                                            Pager1.CurrentRow, 
                                                                            Pager1.ItemsPerPage, 
                                                                            ref rowCount);
            RenderProducts(displayProducts);

            Pager1.RowCount = rowCount;
            Pager2.RowCount = rowCount;
        }

        private void RenderProducts(List<Product> displayProducts)
        {
            if (displayProducts == null) return;

            StringBuilder sb = new StringBuilder();

            int columnCount = 1;

            foreach (Product p in displayProducts)
            {

                bool isLastInRow = false;
                bool isFirstInRow = false;
                if ((columnCount == 1))
                {
                    isFirstInRow = true;
                }

                if ((columnCount == 3))
                {
                    isLastInRow = true;
                    columnCount = 1;
                }
                else
                {
                    columnCount += 1;
                }
                UserSpecificPrice price = MTApp.PriceProduct(p, MTApp.CurrentCustomer, null);
                MerchantTribe.Commerce.Utilities.HtmlRendering.RenderSingleProduct(ref sb, p, isLastInRow, isFirstInRow, this.Page, price);
            }

            this.categoryitems.Text = sb.ToString();
        }
               
    }
}