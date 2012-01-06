using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Client;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.CommerceDTO.v1.Content;
using MerchantTribe.CommerceDTO.v1.Marketing;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1.Taxes;

namespace MerchantTribe.Migration.Migrators.BV5
{
    public class PackageItem
    {

        public string ProductBvin { get; set; }
        public long LineItemId { get; set; }
        public int Quantity { get; set; }

        public static Collection<PackageItem> FromXml(string data)
        {
            Collection<PackageItem> result = new Collection<PackageItem>();

            try
            {
                StringReader tr = new StringReader(data);
                XmlSerializer xs = new XmlSerializer(result.GetType());                
                result = (Collection<PackageItem>)xs.Deserialize(tr);
                if (result == null) result = new Collection<PackageItem>();                
            }
            catch
            {
                result = new Collection<PackageItem>();
            }

            return result;
        }
    }
}
