
using System.Xml;
using System.Collections.ObjectModel;
using System;

namespace MerchantTribe.Shipping.FedEx
{

    public class RateResponse
    {

        private Header _ReplyHeader = new Header();
        private Collection<FedExError> _Errors = new Collection<FedExError>();
        private Collection<FedExError> _SoftErrors = new Collection<FedExError>();
        private RateEstimatedCharges _EstimatedCharges = new RateEstimatedCharges();

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
        public RateEstimatedCharges EstimatedCharges
        {
            get { return _EstimatedCharges; }
            set { _EstimatedCharges = value; }
        }

        public RateResponse()
        {

        }

        public RateResponse(string xmlData)
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
                    errorNodes = xdoc.SelectNodes("/FDXRateReply/Error");
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
                    softErrorNodes = xdoc.SelectNodes("/FDXRateReply/SoftError");
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
                        _ReplyHeader.ParseNode(xdoc.SelectSingleNode("/FDXRateReply/ReplyHeader"));
                    }

                    if (xdoc.SelectSingleNode("/FDXRateReply/EstimatedCharges") != null)
                    {
                        _EstimatedCharges.ParseNode(xdoc.SelectSingleNode("/FDXRateReply/EstimatedCharges"));
                    }

                    //For Each sn As XmlNode In xdoc.SelectNodes("/FDXSubscriptionReply/SubscribedService")
                    //    Me.SubscribedServices.Add(sn.InnerText)
                    //Next
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