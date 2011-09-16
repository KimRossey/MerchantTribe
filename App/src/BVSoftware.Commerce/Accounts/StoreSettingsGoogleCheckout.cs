using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace BVSoftware.Commerce.Accounts
{
    public class StoreSettingsGoogleCheckout
    {
        private StoreSettings parent = null;

        public StoreSettingsGoogleCheckout(StoreSettings s)
        {
            parent = s;
        }

        // Google Checkout 
        public string ButtonSize
        {
            get { return parent.GetProp("GoogleCheckoutButtonSize"); }
            set { parent.SetProp("GoogleCheckoutButtonSize", value); }
        }
        public string ButtonBackground
        {
            get { return parent.GetProp("GoogleCheckoutButtonBackground"); }
            set { parent.SetProp("GoogleCheckoutButtonBackground", value); }
        }
        public bool DebugMode
        {
            get { return parent.GetPropBool("GoogleDebugMode"); }
            set { parent.SetProp("GoogleDebugMode", value); }
        }
        public decimal DefaultShippingAmount
        {
            get { return parent.GetPropDecimal("GoogleDefaultShippingAmount"); }
            set { parent.SetProp("GoogleDefaultShippingAmount", value); }
        }
        public Controls.GoogleDefaultShippingTypes DefaultShippingType
        {
            get { return (Controls.GoogleDefaultShippingTypes)parent.GetPropInt("GoogleDefaultShippingType"); }
            set { parent.SetProp("GoogleDefaultShippingType", (int)value); }
        }
        public BVSoftware.Commerce.Utilities.SerializableDictionary<string, decimal> DefaultShippingValues
        {
            get
            {
                Utilities.SerializableDictionary<string, decimal> result = new Utilities.SerializableDictionary<string, decimal>();
                string value = parent.GetProp("GoogleDefaultShippingValues");
                if (!string.IsNullOrEmpty(value))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(Utilities.SerializableDictionary<string, decimal>));
                    XmlTextReader xmlReader = new XmlTextReader(new StringReader(value));
                    try
                    {
                        if (deserializer.CanDeserialize(xmlReader))
                        {
                            result = (Utilities.SerializableDictionary<string, decimal>)deserializer.Deserialize(xmlReader);
                        }
                    }
                    finally
                    {
                        xmlReader.Close();
                    }
                }
                return result;
            }
            set
            {
                if (value == null)
                {
                    value = new Utilities.SerializableDictionary<string, decimal>();
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Utilities.SerializableDictionary<string, decimal>));
                StringWriter sr = new StringWriter();
                try
                {
                    serializer.Serialize(sr, value);
                    parent.SetProp("GoogleDefaultShippingValues", sr.ToString());
                }
                finally
                {
                    sr.Close();
                }
            }
        }
        public string MerchantId
        {
            get { return parent.GetProp("GoogleMerchantId"); }
            set { parent.SetProp("GoogleMerchantId", value); }
        }
        public string MerchantKey
        {
            get { return parent.GetProp("GoogleMerchantKey"); }
            set { parent.SetProp("GoogleMerchantKey", value); }
        }
        public string Mode
        {
            get { return parent.GetProp("GoogleMode"); }
            set { parent.SetProp("GoogleMode", value); }
        }
        public string Currency
        {
            get { return parent.GetProp("GoogleCurrency"); }
            set { parent.SetProp("GoogleCurrency", value); }
        }
        public int CartMinutes
        {
            get { return parent.GetPropInt("GoogleCartMinutes"); }
            set { parent.SetProp("GoogleCartMinutes", value); }
        }
        public int MinimumAccountDaysOld
        {
            get { return parent.GetPropInt("GoogleMinimumAccountDaysOld"); }
            set { parent.SetProp("GoogleMinimumAccountDaysOld", value); }
        }
        public bool AVSFailsPutHold
        {
            get { return parent.GetPropBool("GoogleAVSFailsPutHold"); }
            set { parent.SetProp("GoogleAVSFailsPutHold", value); }
        }
        public bool AVSPartialMatchPutHold
        {
            get { return parent.GetPropBool("GoogleAVSPartialMatchPutHold"); }
            set { parent.SetProp("GoogleAVSPartialMatchPutHold", value); }
        }
        public bool AVSNotSupportedPutHold
        {
            get { return parent.GetPropBool("GoogleAVSNotSupportedPutHold"); }
            set { parent.SetProp("GoogleAVSNotSupportedPutHold", value); }
        }
        public bool AVSErrorPutHold
        {
            get { return parent.GetPropBool("GoogleAVSErrorPutHold"); }
            set { parent.SetProp("GoogleAVSErrorPutHold", value); }
        }
        public bool CVNNoMatchPutHold
        {
            get { return parent.GetPropBool("GoogleCVNNoMatchPutHold"); }
            set { parent.SetProp("GoogleCVNNoMatchPutHold", value); }
        }
        public bool CVNNotAvailablePutHold
        {
            get { return parent.GetPropBool("GoogleCVNNotAvailablePutHold"); }
            set { parent.SetProp("GoogleCVNNotAvailablePutHold", value); }
        }
        public bool CVNErrorPutHold
        {
            get { return parent.GetPropBool("GoogleCVNErrorPutHold"); }
            set { parent.SetProp("GoogleCVNErrorPutHold", value); }
        }
        public bool PaymentProtectionEligiblePutHold
        {
            get { return parent.GetPropBool("GooglePaymentProtectionEligiblePutHold"); }
            set { parent.SetProp("GooglePaymentProtectionEligiblePutHold", value); }
        }
    }
}
