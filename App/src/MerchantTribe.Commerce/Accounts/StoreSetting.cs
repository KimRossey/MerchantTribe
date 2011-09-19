using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSetting
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public bool SettingValueAsBool
        {
            get
            {
                if (SettingValue == "1")
                    return true;
                else
                    return false;
            }
            set
            {
                if (value == true)
                    SettingValue = "1";
                else
                    SettingValue = "0";
            }
        }

        public StoreSetting()
        {
            Id = -1;
            StoreId = -1;
            SettingName = string.Empty;
            SettingValue = string.Empty;
        }

        public int ValueAsInteger
        {
            get
            {
                int result = -1;
                int.TryParse(SettingValue, out result);
                return result;
            }
            set
            {
                SettingValue = value.ToString();
            }
        }

        public long ValueAsLong
        {
            get
            {
                long result = -1;
                long.TryParse(SettingValue, out result);
                return result;
            }
            set
            {
                SettingValue = value.ToString();
            }
        }

        public decimal ValueAsDecimal
        {
            get
            {
                decimal result = -1;
                if (String.IsNullOrEmpty(SettingValue)) return result;
                decimal.TryParse(SettingValue, out result);
                return result;
            }
            set
            {
                SettingValue = value.ToString();
            }
        }

        public bool ValueAsBool
        {
            get
            {
                bool result = false;
                if (SettingValue.Trim().ToUpperInvariant() == "1")
                {
                    result = true;
                }
                if (SettingValue.Trim().ToUpperInvariant() == "T")
                {
                    result = true;
                }
                if (SettingValue.Trim().ToUpperInvariant() == "TRUE")
                {
                    result = true;
                }
                return result;
            }
            set
            {
                if (value)
                {
                    SettingValue = "1";
                }
                else
                {
                    SettingValue = "0";
                }
            }
        }

    }
}
