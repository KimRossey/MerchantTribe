using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionActions
{
    public abstract class PromotionActionBase: IPromotionAction
    {
        public const string TypeIdProductPriceAdjustment = "A07AFF02-BA28-42E0-B334-324DE467B2D7";
        public const string TypeIdOrderTotalAdjustment = "6574B0B9-9C65-4968-8605-EECF6F7A0407";
        public const string TypeIdOrderShippingAdjustment = "608C118E-CF72-4703-B4CB-DAB1D579C53E";

        public static PromotionActionBase Factory(string typeId)
        {
            switch (typeId)
            {
                case TypeIdProductPriceAdjustment:
                    return new ProductPriceAdjustment();
                case TypeIdOrderTotalAdjustment:
                    return new OrderTotalAdjustment();
                case TypeIdOrderShippingAdjustment:
                    return new OrderShippingAdjustment();
            }
            return null;
        }

        public long Id {get;set;}
        public abstract Guid TypeId {get;}
        
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


        public PromotionActionBase()
        {
            this.Id = 0;
        }

        public abstract string FriendlyDescription { get; }

        public virtual bool ApplyAction(PromotionContext context, PromotionActionMode mode)
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
            Settings[key] = value ? "1":"0";
        }



        
    }
}
