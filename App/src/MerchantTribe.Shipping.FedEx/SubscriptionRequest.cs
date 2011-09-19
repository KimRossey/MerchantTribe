
using System.Xml;
using System.IO;

namespace MerchantTribe.Shipping.FedEx
{

    public class SubscriptionRequest
    {

        private Header _RequestHeader = new Header();
        private Contact _RequestContact = new Contact();
        private Address _RequestAddress = new Address();

        public Header RequestHeader
        {
            get { return _RequestHeader; }
            set { _RequestHeader = value; }
        }
        public Contact RequestContact
        {
            get { return _RequestContact; }
            set { _RequestContact = value; }
        }
        public Address RequestAddress
        {
            get { return _RequestAddress; }
            set { _RequestAddress = value; }
        }

        public SubscriptionResponse Send()
        {
            return Send(FedExConstants.LiveServiceUrl);
        }
        public SubscriptionResponse Send(string serviceUrl)
        {
            SubscriptionResponse result = new SubscriptionResponse();

            string xmlToSend = BuildXml();
            string responseXml = RateService.SendRequest(serviceUrl, xmlToSend);
            result.Parse(responseXml);

            return result;
        }

        public string BuildXml()
        {
            string result = string.Empty;

            StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            xw.Indentation = 2;

            xw.WriteStartDocument();

            //Preamble
            xw.WriteStartElement("FDXSubscriptionRequest");
            xw.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xw.WriteAttributeString("xsi:noNamespaceSchemaLocation", "FDXSubscriptionRequest.xsd");

            _RequestHeader.WriteToXml(xw, "RequestHeader");
            _RequestContact.WriteToXml(xw, "Contact");
            _RequestAddress.WriteToXml(xw, "Address");

            xw.WriteEndDocument();

            xw.Flush();
            xw.Close();

            result = sw.GetStringBuilder().ToString();

            return result;
        }

    }    

}