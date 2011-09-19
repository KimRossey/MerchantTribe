using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class ProductType: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdProductType + "}"); }
        }

        public List<String> CurrentTypeIds()
        {
            List<String> result = new List<string>();
            string all = GetSetting("TypeIds");
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
        private void SaveTypeIdsToSettings(List<String> typeIds)
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
            SetSetting("TypeIds", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            List<Catalog.ProductType> allTypes = app.CatalogServices.ProductTypes.FindAll();
            allTypes.Insert(0, new Catalog.ProductType() { Bvin = "0", ProductTypeName = "Generic" });

            string result = "When Product Type is:<ul>";
            foreach (string bvin in this.CurrentTypeIds())
            {
                Catalog.ProductType p = allTypes.Where(y => y.Bvin == bvin).FirstOrDefault();
                if (p != null)
                {
                    result += "<li>" + p.ProductTypeName + "</li>";
                }
            }
            result += "</ul>";
            return result;
        }

        public ProductType() : this(string.Empty)
        {

        }
        public ProductType(string typeId):base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lowest;
            AddProductType(typeId);            
        }

        public void AddProductType(string typeId)
        {
            List<String> _Ids = CurrentTypeIds();

            string possible = typeId.Trim().ToLowerInvariant();
            if (possible == string.Empty) return;
            if (_Ids.Contains(possible)) return;
            _Ids.Add(possible);
            SaveTypeIdsToSettings(_Ids);
        }
        public void RemoveProductType(string typeId)
        {
            List<String> _Ids = this.CurrentTypeIds();
            if (_Ids.Contains(typeId))
            {
                _Ids.Remove(typeId);
                SaveTypeIdsToSettings(_Ids);
            }
        }
        
        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Product == null) return false;
            if (context.UserPrice == null) return false;

            string match = context.Product.ProductTypeId.Trim().ToLowerInvariant();
            
            // for "generic" type we need to match 0 instead of empty string
            if (match == string.Empty) match = "0";

            return this.CurrentTypeIds().Contains(match);            
        }
    }
}
