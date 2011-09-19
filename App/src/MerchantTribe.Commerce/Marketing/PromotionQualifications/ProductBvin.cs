using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class ProductBvin : PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdProductBvin + "}"); }
        }

        public List<String> CurrentProductIds()
        {
            List<String> result = new List<string>();
            string all = GetSetting("ProductIds");
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
        private void SaveProductIdsToSettings(List<String> productIds)
        {
            string all = string.Empty;
            foreach (string s in productIds)
            {
                if (s != string.Empty)
                {
                    all += s + ",";
                }
            }
            all = all.TrimEnd(',');
            SetSetting("ProductIds", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "When Product is:<ul>";
            foreach (string bvin in this.CurrentProductIds())
            {
                Catalog.Product p = app.CatalogServices.Products.Find(bvin);
                if (p != null)
                {
                    result += "<li>[" + p.Sku + "] " + p.ProductName + "</li>";
                }
            }
            result += "</ul>";
            return result;
        }

        public ProductBvin()
            : this(string.Empty)
        {

        }
        public ProductBvin(string bvin)
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lowest;
            AddProductBvin(bvin);
        }

        public void AddProductBvin(string bvin)
        {
            List<String> _ProductIds = CurrentProductIds();

            string possible = bvin.Trim().ToLowerInvariant();
            if (possible == string.Empty) return;
            if (_ProductIds.Contains(possible)) return;
            _ProductIds.Add(possible);
            SaveProductIdsToSettings(_ProductIds);
        }
        public void RemoveProductBvin(string bvin)
        {
            List<String> _ProductIds = CurrentProductIds();
            if (_ProductIds.Contains(bvin))
            {
                _ProductIds.Remove(bvin);
                SaveProductIdsToSettings(_ProductIds);
            }
        }
        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Product == null) return false;
            if (context.UserPrice == null) return false;

            string match = context.Product.Bvin.Trim().ToLowerInvariant();

            return this.CurrentProductIds().Contains(match);
        }
    }
}
