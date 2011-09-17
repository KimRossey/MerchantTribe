using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class MethodSettings: Dictionary<string,string>
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
        public int GetIntSetting(string name)
        {
            int result = 0;
            if (this.ContainsKey(name))
            {                
                int temp = 0;
                if (int.TryParse(this[name], out temp))
                {
                    result = temp;
                }                
            }
            return result;
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
        public void SetIntSetting(string name, int value)
        {
                AddOrUpdate(name, value.ToString());
        }

        public void Merge(MethodSettings otherSettings)
        {
            foreach (KeyValuePair<string, string> kv in otherSettings)
            {
                this.AddOrUpdate(kv.Key, kv.Value);
            }
        }
    }
}
