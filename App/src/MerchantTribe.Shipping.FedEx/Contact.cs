
using System.Xml;

namespace MerchantTribe.Shipping.FedEx
{

    public class Contact
    {

        private string _PersonName = string.Empty;
        private string _CompanyName = string.Empty;
        private string _PhoneNumber = string.Empty;
        private string _PagerNumber = string.Empty;
        private string _FaxNumber = string.Empty;
        private string _EmailAddress = string.Empty;


        public string PersonName
        {
            get { return _PersonName; }
            set { _PersonName = MerchantTribe.Web.Text.TrimToLength(value, 35); }
        }
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = MerchantTribe.Web.Text.TrimToLength(value, 35); }
        }
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = MerchantTribe.Web.Text.TrimToLength(value, 16); }
        }
        public string PagerNumber
        {
            get { return _PagerNumber; }
            set { _PagerNumber = MerchantTribe.Web.Text.TrimToLength(value, 16); }
        }
        public string FaxNumber
        {
            get { return _FaxNumber; }
            set { _FaxNumber = MerchantTribe.Web.Text.TrimToLength(value, 16); }
        }
        public string EmailAddress
        {
            get { return _EmailAddress; }
            set { _EmailAddress = MerchantTribe.Web.Text.TrimToLength(value, 120); }
        }

        public void WriteToXml(XmlTextWriter xw, string elementName)
        {
            xw.WriteStartElement(elementName);
            WriteToXml(xw);
            xw.WriteEndElement();
        }

        public void WriteToXml(XmlTextWriter xw)
        {
            if (_PersonName.Trim().Length > 0)
            {
                xw.WriteElementString("PersonName", _PersonName);
            }
            if (_CompanyName.Trim().Length > 0)
            {
                xw.WriteElementString("CompanyName", _CompanyName);
            }
            //If _PhoneNumber.Trim.Length > 0 Then
            xw.WriteElementString("PhoneNumber", _PhoneNumber);
            //End If
            if (_PagerNumber.Trim().Length > 0)
            {
                xw.WriteElementString("PagerNumber", _PagerNumber);
            }
            if (_FaxNumber.Trim().Length > 0)
            {
                xw.WriteElementString("FaxNumber", _FaxNumber);
            }
            if (_EmailAddress.Trim().Length > 0)
            {
                xw.WriteElementString("EmailAddress", _EmailAddress);
            }
        }

    }

}
