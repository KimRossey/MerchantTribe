using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Cryptography;

namespace MerchantTribe.Commerce.Content
{
    public class ContentBlockSettings : Dictionary<string, string>
    {
     
        private string Encrypt(string input)
        {
            return AesEncryption.Encode(input, KeyManager.GetKey(0));
        }
        private string Decrypt(string input)
        {
            return AesEncryption.Decode(input, KeyManager.GetKey(0));
        }

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

        public void AddOrUpdateEncrypted(string name, string unencryptedValue)
        {
            string encrypted = Encrypt(unencryptedValue);
            AddOrUpdate(name, encrypted);
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

        public string GetSettingOrEmptyEncrypted(string name)
        {
            if (this.ContainsKey(name))
            {
                return Decrypt(this[name]);
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

        public decimal GetDecimalSetting(string name)
        {
            decimal result = -1;
            if (this.ContainsKey(name))
            {
                decimal temp = 0;
                if (decimal.TryParse(this[name], out temp))
                {
                    result = temp;
                }
            }
            return result;
        }

        public void SetDecimalSetting(string name, decimal value)
        {
            AddOrUpdate(name, value.ToString());
        }

        public int GetIntegerSetting(string name)
        {
            int result = -1;
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

        public void SetIntegerSetting(string name, int value)
        {
            AddOrUpdate(name, value.ToString());
        }

        public void Merge(ContentBlockSettings otherSettings)
        {
            if (otherSettings == null) return;

            foreach (KeyValuePair<string, string> kv in otherSettings)
            {
                this.AddOrUpdate(kv.Key, kv.Value);
            }
        }

    }
}
