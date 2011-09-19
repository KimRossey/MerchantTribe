

using System.IO;
using System.Xml;

namespace MerchantTribe.Shipping.Ups
{

    public class ShipAcceptRequest
    {

        private UpsSettings _settings = new UpsSettings();
        private string _XmlAcceptConfirmRequest = string.Empty;
        private string _XmlAcceptConfirmResponse = string.Empty;
        private string _ShipDigest = string.Empty;

        public UpsSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
        public string XmlAcceptRequest
        {
            get { return _XmlAcceptConfirmRequest; }
            set { _XmlAcceptConfirmRequest = value; }
        }
        public string XmlAcceptResponse
        {
            get { return _XmlAcceptConfirmResponse; }
            set { _XmlAcceptConfirmResponse = value; }
        }
        public string ShipDigest
        {
            get { return _ShipDigest; }
            set { _ShipDigest = value; }
        }

        public ShipAcceptRequest()
        {

        }

    }
 
}