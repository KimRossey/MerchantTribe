
namespace MerchantTribe.Shipping.Ups
{

    public class VoidShipmentRequest
    {


        private UpsSettings _settings = new UpsSettings();
        private string _XmlAcceptConfirmRequest = string.Empty;
        private string _XmlAcceptConfirmResponse = string.Empty;
        private string _ShipmentIdentificationNumber = string.Empty;

        public UpsSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }
        public string XmlRequest
        {
            get { return _XmlAcceptConfirmRequest; }
            set { _XmlAcceptConfirmRequest = value; }
        }
        public string XmlResponse
        {
            get { return _XmlAcceptConfirmResponse; }
            set { _XmlAcceptConfirmResponse = value; }
        }
        public string ShipmentIdentificationNumber
        {
            get { return _ShipmentIdentificationNumber; }
            set { _ShipmentIdentificationNumber = value; }
        }

        public VoidShipmentRequest()
        {

        }
    }
    
}