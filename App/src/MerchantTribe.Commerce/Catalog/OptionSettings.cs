using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;

namespace MerchantTribe.Commerce.Catalog
{
    public class OptionSettings : Dictionary<string, string>
    {
        public void AddOrUpdate(string name, string value)
        {
            if (this.ContainsKey(name))
            {
                this[name] = value;
            }
            else
            {
                this.Add(name, value);
            }
        }

        public string GetSettingOrEmpty(string name)
        {
            if (this.ContainsKey(name))
            {
                return this[name];
            }
            else
            {
                return string.Empty;
            }
        }

        public bool GetBoolSetting(string name)
        {            
            if (this.ContainsKey(name))
            {
                if (this[name] == "1")
                {
                    return true;
                }
            }
            return false;
        }

        public void SetBoolSetting(string name, bool value)
        {
            if (value)
            {
                AddOrUpdate(name, "1");
            }
            else
            {
                AddOrUpdate(name, "0");
            }
        }        

        public void Merge(OptionSettings otherSettings)
        {
            foreach (KeyValuePair<string, string> kv in otherSettings)
            {
                this.AddOrUpdate(kv.Key, kv.Value);
            }
        }

        public string ToJson()
        {
            return MerchantTribe.Web.Json.ObjectToJson(this);
        }

        public static OptionSettings FromJson(string jsonValues)
        {
            OptionSettings result = 
            MerchantTribe.Web.Json.ObjectFromJson<OptionSettings>(jsonValues);
            if (result == null)
            {
                result = new OptionSettings();
            }
            return result;
        }
    }
}

