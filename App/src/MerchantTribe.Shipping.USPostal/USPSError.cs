using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MerchantTribe.Shipping.USPostal
{
    public class USPSError
    {

        private string _Number = string.Empty;
        private string _Source = string.Empty;
        private string _Description = string.Empty;
        private string _HelpFile = string.Empty;
        private string _HelpContext = string.Empty;

        public string Number
        {
            get { return _Number; }
            set { _Number = value; }
        }
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string HelpFile
        {
            get { return _HelpFile; }
            set { _HelpFile = value; }
        }
        public string HelpContext
        {
            get { return _HelpContext; }
            set { _HelpContext = value; }
        }

        public USPSError()
        {
        }
        public USPSError(XmlNode n)
        {
            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            if (n == null) return;

            this._Number = MerchantTribe.Web.Xml.ParseInnerText(n, "Number");
            this._Source = MerchantTribe.Web.Xml.ParseInnerText(n, "Source");
            this._Description = MerchantTribe.Web.Xml.ParseInnerText(n, "Description");
            this._HelpFile = MerchantTribe.Web.Xml.ParseInnerText(n, "HelpFile");
            this._HelpContext = MerchantTribe.Web.Xml.ParseInnerText(n, "HelpContext");          
        }


    }
}
