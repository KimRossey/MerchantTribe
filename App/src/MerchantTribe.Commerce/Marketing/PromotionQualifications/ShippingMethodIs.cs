using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class ShippingMethodIs : PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdShippingMethodIs + "}"); }
        }
        public List<String> ItemIds()
        {
            List<String> result = new List<string>();
            string all = GetSetting("itemids");
            string[] parts = all.Split(',');
            foreach (string s in parts)
            {
                if (s != string.Empty)
                {
                    result.Add(s.Trim().ToUpperInvariant());
                }
            }
            return result;
        }
        private void SaveItemIdsToSettings(List<String> coupons)
        {
            string all = string.Empty;
            foreach (string s in coupons)
            {
                if (s != string.Empty)
                {
                    all += s.Trim().ToUpperInvariant() + ",";
                }
            }
            all = all.TrimEnd(',');
            SetSetting("itemids", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            List<Shipping.ShippingMethod> methods = app.OrderServices.ShippingMethods.FindAll(app.CurrentStore.Id);

            string result = "When Order Has Shipping Method Of:<ul>";
            foreach (string itemid in this.ItemIds())
            {           
                string displayName = itemid;

                if (methods != null)
                {
                    var m = methods.Where(y => y.Bvin == itemid).SingleOrDefault();
                    if (m != null)
                    {
                        displayName = m.Name;
                    }
                }
                result += "<li>" + displayName + "</li>";
            }
            result += "</ul>";
            return result;
        }
        public ShippingMethodIs()
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Normal;            
        }

        public void AddItemId(string itemid)
        {
            List<String> _ItemIds = this.ItemIds();

            string possible = itemid.Trim().ToLowerInvariant();
            if (possible == string.Empty) return;
            if (_ItemIds.Contains(possible)) return;
            _ItemIds.Add(possible);
            SaveItemIdsToSettings(_ItemIds);
        }
        public void RemoveItemId(string itemid)
        {
            List<String> _ItemIds = this.ItemIds();
            if (_ItemIds.Contains(itemid.Trim().ToLowerInvariant()))
            {
                _ItemIds.Remove(itemid.Trim().ToLowerInvariant());
                SaveItemIdsToSettings(_ItemIds);
            }
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Mode != PromotionType.Offer) return false;
            if (context.Order == null) return false;                        
            if (context.Order.ShippingMethodId.Trim().Length < 1) return false;

            foreach (string itemid in this.ItemIds())
            {
                if (context.Order.ShippingMethodId == itemid) return true;
            }

            return false;            
        }
    }
}