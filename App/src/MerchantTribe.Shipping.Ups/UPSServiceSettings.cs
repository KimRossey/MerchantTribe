using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Shipping;

namespace MerchantTribe.Shipping.Ups
{
    public class UPSServiceSettings: ServiceSettings
    {        
        public bool NegotiatedRates
        {
            get { return GetBoolSetting("UpsNegotiatedRates"); }
            set { SetBoolSetting("UpsNegotiatedRates", value); }
        }
        public bool GetAllRates
        {
            get { return GetBoolSetting("GetAllRates"); }
            set { SetBoolSetting("GetAllRates", value); }
        }
        public List<IServiceCode> ServiceCodeFilter
        {
            get {
                List<IServiceCode> result = new List<IServiceCode>();
                string serialized = GetSettingOrEmpty("ServiceCodeFilter");
                string[] codes = serialized.Split(',');
                foreach (string code in codes)
                {
                    string[] parts = code.Split('|');
                    
                    if (parts.Length > 0)
                    {
                        ServiceCode c = new ServiceCode();
                        c.Code = parts[0];
                        if (parts.Length > 1)
                        {
                            c.DisplayName = parts[1];
                        }
                        result.Add(c);
                    }
                }
                return result;
            }
            set {
                string s = "";
                foreach (IServiceCode code in value)
                {
                    s += code.Code + "|" + code.DisplayName + ",";
                }
                s = s.TrimEnd(',');
                this.AddOrUpdate("ServiceCodeFilter",s);
            }
        }        
    }
}
