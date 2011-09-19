using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class DomesticResponse
    {

        private Collection<USPSError> _Errors = new Collection<USPSError>();
        private Collection<DomesticPackage> _Packages = new Collection<DomesticPackage>();

        public Collection<USPSError> Errors
        {
            get { return _Errors; }
            set { _Errors = value; }
        }
        public Collection<DomesticPackage> Packages
        {
            get { return _Packages; }
            set { _Packages = value; }
        }

        public DomesticResponse()
        {
        }
        public DomesticResponse(string xmlData)
        {
            Parse(xmlData);
        }

        public void Parse(string xmlData)
        {
            try
            {
                if (xmlData.Trim().Length > 0)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xmlData);

                    _Errors.Clear();
                    XmlNodeList errorNodes;
                    errorNodes = xdoc.SelectNodes("/RateV4Response/Error");
                    if (errorNodes != null)
                    {
                        foreach (XmlNode en in errorNodes)
                        {
                            USPSError e = new USPSError(en);
                            _Errors.Add(e);
                        }
                    }

                    _Packages.Clear();
                    XmlNodeList packageNodes;
                    packageNodes = xdoc.SelectNodes("/RateV4Response/Package");
                    if (packageNodes != null)
                    {
                        foreach (XmlNode pn in packageNodes)
                        {
                            DomesticPackage p = new DomesticPackage(pn);
                            _Packages.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                USPSError e = new USPSError();
                e.Source = ex.StackTrace;
                e.Description = "BV Software Parsing Error: " + ex.Message;
                _Errors.Add(e);
            }

        }


    }
}
