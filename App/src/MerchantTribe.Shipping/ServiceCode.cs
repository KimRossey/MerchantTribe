using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public class ServiceCode: IServiceCode
    {

        public string Code {get;set;}        
        public string DisplayName {get;set;}

        public ServiceCode()
        {
            Code = string.Empty;
            DisplayName = string.Empty;
        }
        public ServiceCode(string name, string code)
        {
            Code = code;
            DisplayName = name;
        }
    }
}
