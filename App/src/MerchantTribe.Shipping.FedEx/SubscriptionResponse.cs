
using System.Xml;
using System.Collections.ObjectModel;
using System;

namespace MerchantTribe.Shipping.FedEx
{

    public class SubscriptionResponse
    {

        private Header _ReplyHeader = new Header();
        private Collection<FedExError> _Errors = new Collection<FedExError>();
        private Collection<FedExError> _SoftErrors = new Collection<FedExError>();
        private Collection<string> _SubscribedServices = new Collection<string>();

        public Header ReplyHeader
        {
            get { return _ReplyHeader; }
            set { _ReplyHeader = value; }
        }
        public Collection<FedExError> Errors
        {
            get { return _Errors; }
            set { _Errors = value; }
        }
        public Collection<FedExError> SoftErrors
        {
            get { return _SoftErrors; }
            set { _SoftErrors = value; }
        }
        public Collection<string> SubscribedServices
        {
            get { return _SubscribedServices; }
            set { _SubscribedServices = value; }
        }

        public SubscriptionResponse()
        {

        }

        public SubscriptionResponse(string xmlData)
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

                    this.Errors.Clear();
                    XmlNodeList errorNodes;
                    errorNodes = xdoc.SelectNodes("/FDXSubscriptionReply/Error");
                    if (errorNodes != null)
                    {
                        foreach (XmlNode en in errorNodes)
                        {
                            FedExError e = new FedExError(en);
                            this.Errors.Add(e);
                        }
                    }
                    errorNodes = xdoc.SelectNodes("/Error");
                    if (errorNodes != null)
                    {
                        foreach (XmlNode en in errorNodes)
                        {
                            FedExError e = new FedExError(en);
                            this.Errors.Add(e);
                        }
                    }

                    this.SoftErrors.Clear();
                    XmlNodeList softErrorNodes;
                    softErrorNodes = xdoc.SelectNodes("/FDXSubscriptionReply/SoftError");
                    if (softErrorNodes != null)
                    {
                        foreach (XmlNode en in softErrorNodes)
                        {
                            FedExError e = new FedExError(en);
                            this.SoftErrors.Add(e);
                        }
                    }

                    if (xdoc.SelectSingleNode("/FDXSubscriptionReply/ReplyHeader") != null)
                    {
                        _ReplyHeader.ParseNode(xdoc.SelectSingleNode("/FDXSubscriptionReply/ReplyHeader"));
                    }

                    if (xdoc.SelectSingleNode("/FDXSubscriptionReply/MeterNumber") != null)
                    {
                        _ReplyHeader.MeterNumber = xdoc.SelectSingleNode("/FDXSubscriptionReply/MeterNumber").InnerText;
                    }

                    foreach (XmlNode sn in xdoc.SelectNodes("/FDXSubscriptionReply/SubscribedService"))
                    {
                        this.SubscribedServices.Add(sn.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                FedExError e = new FedExError();
                e.Code = ex.StackTrace;
                e.Message = "BV Software Parsing Error: " + ex.Message;
                _Errors.Add(e);
            }

        }

    }   

}