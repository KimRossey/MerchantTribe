
using System.Xml;

namespace MerchantTribe.Shipping.FedEx
{

    public class RateSpecialServices
    {

        private bool _ResidentialDelivery = false;
        private bool _InsideDelivery = false;
        private bool _SaturdayDelivery = false;
        private bool _NonstandardContainer = false;

        public bool ResidentialDelivery
        {
            get { return _ResidentialDelivery; }
            set { _ResidentialDelivery = value; }
        }
        public bool InsideDelivery
        {
            get { return _InsideDelivery; }
            set { _InsideDelivery = value; }
        }
        public bool SaturdayDelivery
        {
            get { return _SaturdayDelivery; }
            set { _SaturdayDelivery = value; }
        }
        public bool NonstandardContainer
        {
            get { return _NonstandardContainer; }
            set { _NonstandardContainer = value; }
        }

        public void WriteToXml(XmlTextWriter xw, string elementName)
        {
            xw.WriteStartElement(elementName);
            WriteToXml(xw);
            xw.WriteEndElement();
        }

        public void WriteToXml(XmlTextWriter xw)
        {
            if (_ResidentialDelivery)
            {
                xw.WriteElementString("ResidentialDelivery", "1");
            }
            if (_InsideDelivery)
            {
                xw.WriteElementString("InsideDelivery", "1");
            }
            if (_SaturdayDelivery)
            {
                xw.WriteElementString("SaturdayDelivery", "1");
            }
            if (_NonstandardContainer)
            {
                xw.WriteElementString("NonstandardContainer", "1");
            }
        }

    }

}