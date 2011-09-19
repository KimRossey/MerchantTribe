
using System.Xml;
using System;

namespace MerchantTribe.Shipping.FedEx
{

    public class Header
    {

        private string _CustomerTransactionIdentifier = string.Empty;
        private string _AccountNumber = string.Empty;
        private string _MeterNumber = string.Empty;
        private CarrierCode _CarrierCode = FedEx.CarrierCode.FDXG;

        public CarrierCode CarrierCode
        {
            get { return _CarrierCode; }
            set { _CarrierCode = value; }
        }
        public string CustomerTransactionIdentifier
        {
            get { return _CustomerTransactionIdentifier; }
            set { _CustomerTransactionIdentifier = MerchantTribe.Web.Text.TrimToLength(value, 40); }
        }
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = MerchantTribe.Web.Text.TrimToLength(value, 12); }
        }
        public string MeterNumber
        {
            get { return _MeterNumber; }
            set { _MeterNumber = MerchantTribe.Web.Text.TrimToLength(value, 10); }
        }

        public Header()
        {

        }

        public Header(XmlNode n)
        {
            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            _CustomerTransactionIdentifier = XmlHelper.ParseInnerText(n, "CustomerTransactionIdentifier");
            _AccountNumber = XmlHelper.ParseInnerText(n, "AccountNumber");
            _MeterNumber = XmlHelper.ParseInnerText(n, "MeterNumber");
            try
            {
                string tempCode = XmlHelper.ParseInnerText(n, "CarrierCode");
                int tempCode2 = 0;
                int.TryParse(tempCode, out tempCode2);
                _CarrierCode = (CarrierCode)tempCode2;
            }
            catch 
            {

            }
        }

        public void WriteToXml(XmlTextWriter xw, string elementName)
        {
            xw.WriteStartElement(elementName);
            WriteToXml(xw);
            xw.WriteEndElement();
        }

        public void WriteToXml(XmlTextWriter xw)
        {
            XmlHelper.WriteIfNotEmpty(xw, "CustomerTransactionIdentifier", _CustomerTransactionIdentifier);
            XmlHelper.WriteIfNotEmpty(xw, "AccountNumber", _AccountNumber);
            XmlHelper.WriteIfNotEmpty(xw, "MeterNumber", _MeterNumber);
            XmlHelper.WriteIfNotEmpty(xw, "CarrierCode", _CarrierCode.ToString());
        }

    }   

}