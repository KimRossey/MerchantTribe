using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Contacts
{
    public class PhoneNumberList
    {
        public string ToJson()
        {
            return MerchantTribe.Web.Json.ObjectToJson(this);
        }

        public static PhoneNumberList FromJson(string jsonValues)
        {
            PhoneNumberList result =
            MerchantTribe.Web.Json.ObjectFromJson<PhoneNumberList>(jsonValues);
            if (result == null)
            {
                result = new PhoneNumberList();
            }
            return result;
        }
    }
}
