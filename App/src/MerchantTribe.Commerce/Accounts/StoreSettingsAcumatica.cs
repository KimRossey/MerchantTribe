using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class StoreSettingsAcumatica
    {
        private StoreSettings parent = null;

        public StoreSettingsAcumatica(StoreSettings s)
        {
            parent = s;
        }

        public bool IntegrationEnabled
        {
            get { return parent.GetPropBool("AcumaticaIntegrationEnabled"); }
            set { parent.SetProp("AcumaticaIntegrationEnabled", value); }
        }
        public string Username
        {
            get { return parent.GetProp("AcumaticaUsername"); }
            set { parent.SetProp("AcumaticaUsername", value); }
        }
        public string Password
        {
            get { return parent.GetProp("AcumaticaPassword"); }
            set { parent.SetProp("AcumaticaPassword", value); }
        }
        public string SiteUrl
        {
            get { return parent.GetProp("AcumaticaSiteUrl"); }
            set { parent.SetProp("AcumaticaSiteUrl", value); }
        }
        public string NewItemTaxClassId
        {
            get { return parent.GetProp("AcumaticaNewItemTaxClassId"); }
            set { parent.SetProp("AcumaticaNewItemTaxClassId", value); }
        }
        public string NewItemWarehouseId
        {
            get { return parent.GetProp("AcumaticaNewItemWarehouseId"); }
            set { parent.SetProp("AcumaticaNewItemWarehouseId", value); }
        }
        public string OrderLineItemWarehouseId
        {
            get { return parent.GetProp("AcumaticaOrderLineItemWarehouseId"); }
            set { parent.SetProp("AcumaticaOrderLineItemWarehouseId", value); }
        }
        public string PaymentCCId
        {
            get { return parent.GetProp("AcumaticaPaymentCCId"); }
            set { parent.SetProp("AcumaticaPaymentCCId", value); }
        }
        public bool CustomerIdIsString
        {
            get { return parent.GetPropBool("AcumaticaCustomerIdIsString"); }
            set { parent.SetProp("AcumaticaCustomerIdIsString", value); }
        }
        public int MinutesBetweenDataPulls
        {
            get { return parent.GetPropInt("AcumaticaTaskTimer1"); }
            set { parent.SetProp("AcumaticaTaskTimer1", value); }
        }

        public DateTime LastCustomerPullUtc
        {
            get
            {
                DateTime result = DateTime.Now.AddYears(-1);
                string prop = parent.GetProp("AcumaticaLastPulledCustomers");
                DateTime temp = DateTime.UtcNow.AddYears(-1);
                if (DateTime.TryParse(prop, out temp))
                {
                    return temp.ToUniversalTime();
                }
                return result;
            }
            set
            {
                string temp = value.ToString("u");
                parent.SetProp("AcumaticaLastPulledCustomers", temp);
            }
        }

        public List<StoreIntegrationPaymentMapping> PaymentMappings
        {
            get
            {
                List<StoreIntegrationPaymentMapping> result = new List<StoreIntegrationPaymentMapping>();

                string data = parent.GetProp("AcumaticaPaymentMappings");
                string[] parts = data.Split('|');

                for (int i = 0; i < parts.Length; i++)
                {
                    StoreIntegrationPaymentMapping temp = new StoreIntegrationPaymentMapping();
                    if (temp.Deserialize(parts[i]))
                    {
                        result.Add(temp);
                    }
                }
                return result;
            }
            set
            {
                string data = string.Empty;
                foreach (StoreIntegrationPaymentMapping c in value)
                {
                    data += c.Serialize() + "|";
                }
                data = data.TrimEnd('|');
                parent.SetProp("AcumaticaPaymentMappings", data);
            }
        }
        public List<StoreIntegrationShippingMapping> ShippingMappings
        {
            get
            {
                List<StoreIntegrationShippingMapping> result = new List<StoreIntegrationShippingMapping>();

                string data = parent.GetProp("AcumaticaShippingMappings");
                string[] parts = data.Split('|');

                for (int i = 0; i < parts.Length; i++)
                {
                    StoreIntegrationShippingMapping temp = new StoreIntegrationShippingMapping();
                    if (temp.Deserialize(parts[i]))
                    {
                        result.Add(temp);
                    }
                }
                return result;
            }
            set
            {
                string data = string.Empty;
                foreach (StoreIntegrationShippingMapping c in value)
                {
                    data += c.Serialize() + "|";
                }
                data = data.TrimEnd('|');
                parent.SetProp("AcumaticaShippingMappings", data);
            }
        }
    }
}
