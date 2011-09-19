using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections.ObjectModel;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class InternationalResponse
    {

        public List<USPSError> Errors {get;set;}
        public List<InternationalPackage> Packages {get;set;}

        public InternationalResponse()
        {
            this.Packages = new List<InternationalPackage>();
            this.Errors = new List<USPSError>();
        }
        public InternationalResponse(string xmlData)
        {
            this.Packages = new List<InternationalPackage>();
            this.Errors = new List<USPSError>();
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

                    this.Errors.Clear();
                    XmlNodeList errorNodes;
                    errorNodes = xdoc.SelectNodes("/IntlRateV2Response/Error");
                    if (errorNodes != null)
                    {
                        foreach (XmlNode en in errorNodes)
                        {
                            USPSError e = new USPSError(en);
                            this.Errors.Add(e);
                        }
                    }

                    Packages.Clear();
                    XmlNodeList packageNodes;
                    packageNodes = xdoc.SelectNodes("/IntlRateV2Response/Package");
                    if (packageNodes != null)
                    {
                        foreach (XmlNode pn in packageNodes)
                        {
                            InternationalPackage p = new InternationalPackage(pn);
                            Packages.Add(p);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                USPSError e = new USPSError();
                e.Source = ex.StackTrace;
                e.Description = "BV Software Parsing Error: " + ex.Message;
                this.Errors.Add(e);
            }

        }
    }
}
