using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using MerchantTribe.Shipping.USPostal.v4;
using MerchantTribe.Shipping;

namespace MerchantTribe.Shipping.USPostal
{
        public class USPostalServiceSettings : ServiceSettings
        {
            public DomesticPackageType PackageType
            {
                get { return (DomesticPackageType)GetIntegerSetting("PackageType"); }
                set { this.SetIntegerSetting("PackageType", (int)value); }
            }
           
            public List<IServiceCode> ServiceCodeFilter
            {
                get
                {
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

                    if (result.Count < 1) result.Add(new ServiceCode() { Code = "-1", DisplayName = "All Available Services" });
                    return result;
                }
                set
                {
                    string s = "";
                    foreach (IServiceCode code in value)
                    {
                        s += code.Code + "|" + code.DisplayName + ",";
                    }
                    s = s.TrimEnd(',');
                    this.AddOrUpdate("ServiceCodeFilter", s);
                }
            }

            public USPostalServiceSettings()
            {
                PackageType = DomesticPackageType.Ignore;
            }

            public bool ReturnAllServices()
            {
                int x = (from svc in ServiceCodeFilter
                         where svc.Code == "-1"
                         select svc).Count();

                if (x > 0) return true;

                return false;
            }

            public DomesticServiceType GetServiceForProcessing()
            {                
                // if a single service is selected, return that strategy code
                if (ServiceCodeFilter.Count == 1)
                {
                    int _code = -1;
                    if (int.TryParse(ServiceCodeFilter[0].Code, out _code))
                    {
                        return (DomesticServiceType)_code;                        
                    }
                }

                // otherwise return multi-code
                return DomesticServiceType.All;
            }
            
        }
}
