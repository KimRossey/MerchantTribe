using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{    
    public class OrderHasProducts: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdOrderHasProducts + "}"); }
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

        public QualificationHasMode HasMode
        {
            get { QualificationHasMode result = QualificationHasMode.HasAtLeast;
                  int temp = GetSettingAsInt("HasMode");
                  if (temp < 0) temp = 0;
                  result = (QualificationHasMode)temp;
                  return result;
                }
            set { SetSetting("HasMode", (int)value);}
        }
        public QualificationSetMode SetMode
        {
            get { QualificationSetMode result = QualificationSetMode.AnyOfTheseItems;
                  int temp = GetSettingAsInt("SetMode");
                  if (temp < 0) temp = 0;
                  result = (QualificationSetMode)temp;
                  return result;
                }
            set { SetSetting("SetMode", (int)value);}
        }
        public int Quantity
        {
            get { return GetSettingAsInt("Quantity");}
            set { SetSetting("Quantity", value);}
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "";
            
            switch(HasMode)
            {
                case QualificationHasMode.HasAtLeast:
                    result += "When order has AT LEAST ";
                    break;
            }
            result += this.Quantity.ToString();
            switch(SetMode)
            {
                case QualificationSetMode.AllOfTheseItems:
                    result += " of ALL of these products";
                    break;
                case QualificationSetMode.AnyOfTheseItems:
                    result += " of ANY of these products";
                    break;
            }
            result += ":<ul>";
            
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

        public OrderHasProducts()
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lowest;
            // Only supporting "Has At Least" mode for now
            this.HasMode = QualificationHasMode.HasAtLeast;
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
            if (context.Mode != PromotionType.Offer) return false;
            if (context.Order == null) return false;
            if (context.Order.Items == null) return false;
            
            switch (SetMode)
            {
                case QualificationSetMode.AnyOfTheseItems:
                    return MatchAny(this.Quantity, context.Order.Items, this.CurrentProductIds());                    
                case QualificationSetMode.AllOfTheseItems:
                    return MatchAll(this.Quantity, context.Order.Items, this.CurrentProductIds());                    
            }                                 

            return false;            
        }

        private bool MatchAny(int qty, List<LineItem> items, List<string> productIds)
        {
            int QuantityLeftToMatch = qty;

            foreach (LineItem li in items)
            {
                if (this.CurrentProductIds().Contains(li.ProductId.Trim().ToLowerInvariant()))
                {
                    QuantityLeftToMatch -= li.Quantity;
                    if (QuantityLeftToMatch <= 0) return true;
                }
            }

            return false;
        }

        private bool MatchAll(int qty, List<LineItem> items, List<string> productIds)
        {
            // Build up dictionary of items to match with quantities
            Dictionary<string, int> ItemsToFind = new Dictionary<string, int>();
            foreach (string bvin in productIds)
            {
                ItemsToFind.Add(bvin, qty);
            }

            // Subtract each quantity found for items
            foreach (LineItem li in items)
            {
                string lid = li.ProductId.Trim().ToLowerInvariant();
                if (ItemsToFind.ContainsKey(lid))
                {
                    ItemsToFind[lid] -= li.Quantity;
                }
            }

            foreach (string bvin2 in productIds)
            {
                // If we didn't get enough quantity found, return false;
                if (ItemsToFind[bvin2] > 0)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}