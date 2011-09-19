using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MerchantTribe.Commerce.Contacts
{
    [DataContract]
    public class AddressList: List<Address>
    {
        public string ToJson()
        {
            return MerchantTribe.Web.Json.ObjectToJson(this);
        }

        public static AddressList FromJson(string jsonValues)
        {
            List<Address> result =
            MerchantTribe.Web.Json.ObjectFromJson<List<Address>>(jsonValues);
            if (result == null)
            {
                result = new List<Address>();
            }

            AddressList trueResult = new AddressList();
            trueResult.AddRange(result);
            return trueResult;
        }
        
    }
}
