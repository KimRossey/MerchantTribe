using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public abstract class PromotionQualificationBase: IPromotionQualification
    {
        public const string TypeIdAnyProduct = "47B2F15C-137E-4A1C-BAE9-88D0DC1DFF64";
        public const string TypeIdProductBvin = "6B39C94F-1A33-4939-9EC8-BF3EDA744A07";
        public const string TypeIdProductCategory = "3C3132F3-CA5B-4FDE-BF0E-38C82E428096";
        public const string TypeIdProductType = "6BB6ADA9-E296-4C53-9DF7-57C7ABCFE98A";
        public const string TypeIdOrderHasCoupon = "B8B1BF8A-EEB5-4A74-8CCE-7D3C9282BD3D";
        public const string TypeIdAnyOrder = "C8DD095E-F0F4-4A91-9870-823233A2D92B";
        public const string TypeIdOrderHasProducts = "489F961C-8E97-4B78-A5AA-92EAC47BD6F9";
        public const string TypeIdOrderSubTotalIs = "25E02AEA-2FD3-469D-85E5-A6FA0756DDAD";
        public const string TypeIdUserIs = "18A31B99-49E7-43E5-80CA-E1EE64371E1B";
        public const string TypeIdUserIsInGroup = "43A4A2B8-8ECE-4CD2-AA71-BBECC57392A7";
        public const string TypeIdShippingMethodIs = "D9E6B675-1784-4CD2-8041-4D776FE213A7";
        public const string TypeIdAnyShippingMethod = "6453763A-75EA-4FB7-854D-364A5455A68F";

        public static PromotionQualificationBase Factory(string typeId)
        {
            switch (typeId)
            {
                case TypeIdAnyProduct:
                    return new AnyProduct();
                case TypeIdProductBvin:
                    return new ProductBvin();
                case TypeIdProductCategory:
                    return new ProductCategory();
                case TypeIdProductType:
                    return new ProductType();
                case TypeIdOrderHasCoupon:
                    return new OrderHasCoupon();
                case TypeIdAnyOrder:
                    return new AnyOrder();
                case TypeIdOrderHasProducts:
                    return new OrderHasProducts();
                case TypeIdOrderSubTotalIs:
                    return new OrderSubTotalIs();
                case TypeIdUserIs:
                    return new UserIs();
                case TypeIdUserIsInGroup:
                    return new UserIsInGroup();
                case TypeIdShippingMethodIs:
                    return new ShippingMethodIs();
                case TypeIdAnyShippingMethod:
                    return new AnyShippingMethod();
            }
            return null;
        }

        public long Id { get; set; }
        public RelativeProcessingCost ProcessingCost { get; set; }
        public abstract Guid TypeId { get; }

        private Dictionary<string, string> _Settings = new Dictionary<string, string>();
        public Dictionary<string, string> Settings
        {
            get
            {
                return _Settings;
            }
            set
            {
                _Settings = value;
            }
        }

        public abstract string FriendlyDescription(MerchantTribeApplication app);
        
        public PromotionQualificationBase()
        {
            Id = 0;
            ProcessingCost = RelativeProcessingCost.Normal;
        }

        public virtual bool MeetsQualification(PromotionContext context)
        {
            return false;
        }

        protected string GetSetting(string key)
        {
            if (Settings == null) return string.Empty;
            if (!Settings.ContainsKey(key)) return string.Empty;            
            string result = Settings[key];            
            return result;
        }
        protected int GetSettingAsInt(string key)
        {
            if (Settings == null) return -1;
            string result = GetSetting(key);
            if (result == null) return -1;
            int temp = -1;
            int.TryParse(result, out temp);
            return temp;
        }
        protected decimal GetSettingAsDecimal(string key)
        {
            if (Settings == null) return -1;
            string result = GetSetting(key);
            if (result == null) return -1;
            decimal temp = -1;
            decimal.TryParse(result, out temp);
            return temp;
        }
        protected bool GetSettingAsBool(string key)
        {
            if (Settings == null) return false;
            string result = GetSetting(key);
            if (result == null) return false;
            if (result == "1") return true;
            return false;
        }
        protected void SetSetting(string key, string value)
        {
            if (Settings == null) return;
            Settings[key] = value;
        }
        protected void SetSetting(string key, int value)
        {
            if (Settings == null) return;
            Settings[key] = value.ToString();
        }
        protected void SetSetting(string key, decimal value)
        {
            if (Settings == null) return;
            Settings[key] = value.ToString();
        }
        protected void SetSetting(string key, bool value)
        {
            if (Settings == null) return;
            Settings[key] = value ? "1" : "0";
        }


      
    }
}
