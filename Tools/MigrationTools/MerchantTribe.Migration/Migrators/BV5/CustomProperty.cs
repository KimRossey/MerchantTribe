using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
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
    [Serializable]
    public class CustomProperty
    {

        private string _DeveloperId = string.Empty;
        private string _Key = string.Empty;
        private string _Value = string.Empty;

        public string DeveloperId
        {
            get { return _DeveloperId; }
            set { _DeveloperId = value; }
        }
        public string Key
        {
            get { return _Key; }
            set { _Key = value; }
        }
        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public CustomProperty()
        {

        }

        public CustomProperty(string devId, string propertyKey, string propertyValue)
        {
            _DeveloperId = devId;
            _Key = propertyKey;
            _Value = propertyValue;
        }
     
        public CustomPropertyDTO ToDto()
        {
            CustomPropertyDTO dto = new CustomPropertyDTO();
            dto.Value = this.Value;
            dto.Key = this.Key;
            dto.DeveloperId = this.DeveloperId;

            return dto;
        }


    }
}
