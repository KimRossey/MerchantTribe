using System;
using System.Collections.ObjectModel;
using System.Web.UI;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using System.Collections.Generic;
using System.Text;

namespace BVCommerce
{

    partial class BVModules_Controls_RelatedItems : MerchantTribe.Commerce.Content.BVUserControl
    {

        public string ProductID
        {
            get { return this.bvinField.Value; }
            set { this.bvinField.Value = value; }
        }
        public int MaxItemsToShow
        {
            get { return int.Parse(this.MaxItemsToShowField.Value); }
            set { this.MaxItemsToShowField.Value = value.ToString(); }
        }
        public bool IncludeAutoSuggestions
        {
            get { return bool.Parse(this.IncludeAutoSuggestionsField.Value); }
            set { this.IncludeAutoSuggestionsField.Value = value.ToString(); }
        }
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);                 
        }

        public void LoadRelatedItems(string productBvin)
        {
            this.pnlMain.Visible = false;
            this.bvinField.Value = productBvin;

            CatalogService catalogServices = MyPage.MTApp.CatalogServices;
            
            List<ProductRelationship> relatedItems = catalogServices.ProductRelationships.FindForProduct(productBvin);

            if (relatedItems == null) return;
            
            this.pnlMain.Visible = true;

            int toDisplay = MaxItemsToShow;


            // we have fewer available than max to show
            if (relatedItems.Count < MaxItemsToShow)
            {
                if (IncludeAutoSuggestions)
                {
                    // try to fill in auto suggestions
                    int toAuto = MaxItemsToShow - relatedItems.Count;
                    List<ProductRelationship> autos = MyPage.MTApp.GetAutoSuggestedRelatedItems(productBvin, toAuto);
                    if (autos != null)
                    {
                        foreach (ProductRelationship r in autos)
                        {
                            relatedItems.Add(r);
                        }
                    }
                }
                
                toDisplay = relatedItems.Count;
            }


            if (relatedItems.Count < 1) { this.pnlMain.Visible = false; return; }


            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < toDisplay; i++)
            {
                string relatedBvin = relatedItems[i].RelatedProductId;
                Product related = MyPage.MTApp.CatalogServices.Products.Find(relatedBvin);
                if (related != null)
                {
                    bool isFirst = false;
                    if (i == 0) isFirst = true;

                    bool isLast = false;
                    if (i == (toDisplay - 1)) isLast = true;

                    UserSpecificPrice price = MyPage.MTApp.PriceProduct(related, MyPage.MTApp.CurrentCustomer, null);
                    MerchantTribe.Commerce.Utilities.HtmlRendering.RenderSingleProduct(ref sb, related, isLast, isFirst, this.Page, price);
                }
            }

            sb.Append("<div class=\"clear\"></div>");
            this.litRelatedItems.Text = sb.ToString();
        }

    }
}