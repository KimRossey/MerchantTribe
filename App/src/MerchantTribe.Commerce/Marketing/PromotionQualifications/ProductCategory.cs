using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class ProductCategory : PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdProductCategory + "}"); }
        }
        public List<String> CurrentCategoryIds()
        {
            List<String> result = new List<string>();
            string all = GetSetting("CategoryIds");
            string[] parts = all.Split(',');
            foreach (string s in parts)
            {
                if (s != string.Empty)
                {
                    result.Add(s);
                }
            }
            return result;
        }
        private void SaveCategoryIdsToSettings(List<String> typeIds)
        {
            string all = string.Empty;
            foreach (string s in typeIds)
            {
                if (s != string.Empty)
                {
                    all += s + ",";
                }
            }
            all = all.TrimEnd(',');
            SetSetting("CategoryIds", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "When Product is in Category:<ul>";
            foreach (string bvin in this.CurrentCategoryIds())
            {
                Catalog.Category c = app.CatalogServices.Categories.Find(bvin);
                if (c != null)
                {
                    result += "<li>" + c.Name + "<br />";
                    result += "<em>" + c.RewriteUrl + "</em></li>";
                }
            }
            result += "</ul>";
            return result;
        }

        public ProductCategory() : this(string.Empty)
        {

        }
        public ProductCategory(string categoryId):base()
        {
            this.ProcessingCost = RelativeProcessingCost.Highest;
            AddCategoryId(categoryId);
        }

        public void AddCategoryId(string id)
        {
            List<String> _Ids = CurrentCategoryIds();

            string possible = id.Trim().ToLowerInvariant();
            if (possible == string.Empty) return;
            if (_Ids.Contains(possible)) return;
            _Ids.Add(possible);
            SaveCategoryIdsToSettings(_Ids);
        }
        public void RemoveCategoryId(string id)
        {
            List<String> _Ids = this.CurrentCategoryIds();
            if (_Ids.Contains(id))
            {
                _Ids.Remove(id);
                SaveCategoryIdsToSettings(_Ids);
            }
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Mode != PromotionType.Sale) return false;
            if (context.Product == null) return false;
            if (context.UserPrice == null) return false;
            if (context.MTApp == null) return false;

            // Note: this only checks the first 100 categories. You're pretty much insane if you're
            // running a promotion on a product by category and it's in more than 100 categories.
            List<Catalog.CategoryProductAssociation> assignments
                = context.MTApp.CatalogServices.CategoriesXProducts.FindForProduct(context.Product.Bvin, 1, 100);

            foreach (Catalog.CategoryProductAssociation cross in assignments)
            {
                string match = cross.CategoryId.Trim().ToLowerInvariant();
                if (this.CurrentCategoryIds().Contains(match)) return true;
            }
            return false;
        }
    }
}