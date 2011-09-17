using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MerchantTribe.Payment.Methods
{
    public class PayLeap: Method
    {
                
        private const string LiveUrl = "https://secure.payleap.com/SmartPayments/transact.asmx/ProcessCreditCard";
        private const string DeveloperUrl = "http://test.payleap.com/SmartPayments/transact.asmx/ProcessCreditCard";
                 
        public override string Name
        {
            get { return "BV Secure Gateway"; }
        }

        public override string Id
        {
            get { return "6FC76AD8-66BF-47b0-8982-1C4118F01645"; }
        }

        public PayLeapSettings Settings { get; set; }

        public PayLeap()
        {
            Settings = new PayLeapSettings();
        }

        public override void ProcessTransaction(Transaction t)
        {
            bool result = false;

            try
            {
                string url = LiveUrl;                
                if (Settings.DeveloperMode)
                {
                    url = DeveloperUrl;
                }
                
                // Build Data String
                // Card Number and Expiration
                string expDate = string.Empty;
                if (t.Card.ExpirationMonth < 10)
                {
                    expDate = "0" + t.Card.ExpirationMonth.ToString();
                }
                else
                {
                    expDate = t.Card.ExpirationMonth.ToString();
                }
                if (t.Card.ExpirationYear > 99)
                {
                    expDate += t.Card.ExpirationYear.ToString().Substring(2, 2);
                }
                else
                {
                    expDate += t.Card.ExpirationYear.ToString();
                }


                // Set Parameters
                StringBuilder sb = new StringBuilder();
                string postData = string.Empty;

                sb.Append("UserName=");
                sb.Append(SafeWriteString(Settings.Username.Trim()));
                sb.Append("&Password=");
                sb.Append(SafeWriteString(Settings.Password.Trim()));
                sb.Append("&Amount=");
                sb.Append(SafeWriteString(t.Amount.ToString()));
                sb.Append("&InvNum=");
                sb.Append(SafeWriteString(t.MerchantInvoiceNumber));                
                sb.Append("&Street=");
                sb.Append(SafeWriteString(t.Customer.Street));                                
                sb.Append("&Zip=");
                sb.Append(SafeWriteString(t.Customer.PostalCode));
                sb.Append("&NameOnCard=");
                sb.Append(SafeWriteString(t.Card.CardHolderName));

                sb.Append("&MagData=");

                // Extra Tags
                StringBuilder sbextra = new StringBuilder();
                sbextra.Append("<CustomerId>" + TextHelper.XmlEncode(t.Customer.Email) + "</CustomerId>");
                sbextra.Append("<City>" + TextHelper.XmlEncode(t.Customer.City) + "</City>");                
                if (t.Customer.Region != string.Empty)
                {
                    sbextra.Append("<BillToState>" + TextHelper.XmlEncode(t.Customer.Region) + "</BillToState>");                                        
                }                
                if (Settings.TrainingMode)
                {
                    sbextra.Append("<TrainingMode>T</TrainingMode>");
                }
                sbextra.Append("<EntryMode>MANUAL</EntryMode>");
                               
                switch (t.Action)
                {
                    case ActionType.CreditCardCharge:
                        // Charge
                        sb.Append("&TransType=");
                        sb.Append(SafeWriteString("Sale"));
                        sb.Append("&PNRef=");
                        break;
                    case ActionType.CreditCardHold:
                        // Authorize
                        sb.Append("&TransType=");
                        sb.Append(SafeWriteString("Auth"));
                        sb.Append("&PNRef=");
                        break;
                    case ActionType.CreditCardCapture:
                        // Capture, Post Authorize
                        sb.Append("&TransType=");
                        sb.Append(SafeWriteString("Force"));
                        sb.Append("&PNRef=");
                        sb.Append(SafeWriteString(t.PreviousTransactionNumber));
                        sbextra.Append("<AuthCode>" + t.PreviousTransactionAuthCode + "</AuthCode>");
                        break;
                    case ActionType.CreditCardVoid:
                        // Void
                        sb.Append("&TransType=");
                        sb.Append(SafeWriteString("Void"));
                        sb.Append("&PNRef=");
                        sb.Append(SafeWriteString(t.PreviousTransactionNumber));                        
                        break;
                    case ActionType.CreditCardRefund:
                        // Refund, Credit
                        sb.Append("&TransType=");
                        sb.Append(SafeWriteString("Return"));
                        sb.Append("&PNRef=");
                        sb.Append(SafeWriteString(t.PreviousTransactionNumber));                        
                        break;
                }

                // Add Card Number, CVV Code and Expiration Date
                sb.Append("&CardNum=");
                sb.Append(SafeWriteString(t.Card.CardNumber));
                
                    sb.Append("&CVNum=");
                    if (t.Card.SecurityCode.Length > 0)
                    {
                    sb.Append(SafeWriteString(t.Card.SecurityCode));
                    }

                    sb.Append("&ExpDate=");
                    if (t.Action != ActionType.CreditCardVoid)
                    {
                    sb.Append(SafeWriteString(expDate));
                    }

                // Write Extra Tags
                sb.Append("&ExtData=");
                sb.Append(SafeWriteString(sbextra.ToString()));
               

                // Dump string builder to string to send to Authorize.Net
                postData = sb.ToString();

                string xmlresponse = string.Empty;
                try
                {
                    xmlresponse = NetworkUtilities.SendRequestByPost(url, postData);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Error: URL|" + url + "  POST|" + postData + " RESPONSE|" + xmlresponse + " :: " + ex.Message); 
                }

                if (Settings.EnableDebugTracing)
                {
                   t.Result.Messages.Add(new Message(postData,"TRACE-POST:", MessageType.Error));
                    t.Result.Messages.Add(new Message(xmlresponse,"TRACE-REPLY:", MessageType.Error));
                }

                XDocument response = XDocument.Parse(xmlresponse);
                XNamespace ns = response.Root.Attribute("xmlns").Value ?? "";
                PayLeapResponse r = new PayLeapResponse();
                r.Parse(response);                

                if (r != null)
                {
                    t.Result.CvvCode = CvnResponseType.Unavailable;
                    t.Result.ResponseCode = r.AuthCode;
                    t.Result.ResponseCodeDescription = r.Message;
                    t.Result.ReferenceNumber = r.PNRef;
                    t.Result.ReferenceNumber2 = r.AuthCode;
                    t.Result.AvsCode = ParseAvsCode(r.GetAVSResult);

                    if (r.Result == "0")
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        t.Result.Messages.Add(new Message(r.RespMSG, r.AuthCode, MessageType.Warning));
                    }
                }                                
                                                      
            }
            catch (Exception ex)
            {
                result = false;
                t.Result.Messages.Add(new Message("Unknown Payment Error: " + ex.Message, "BVP_PL_1001", MessageType.Error));
                t.Result.Messages.Add(new Message("Stack Trace " + ex.StackTrace, "STACKTRACE", MessageType.Error));
            }

            t.Result.Succeeded = result;
         
        }
                
        public override MethodSettings BaseSettings
        {
            get { return Settings; }
        }

        #region Helper Methods

        private string SafeWriteBool(bool input)
        {
            if (input)
            {
                return System.Web.HttpUtility.UrlEncode("TRUE");
            }
            else
            {
                return System.Web.HttpUtility.UrlEncode("FALSE");
            }
        }
        private string SafeWriteString(string input)
        {
            return System.Web.HttpUtility.UrlEncode(input);
        }        
        private AvsResponseType ParseAvsCode(string code)
        {
            AvsResponseType result = AvsResponseType.Unavailable;

            switch (code.ToUpper())
            {
                case "A":
                    result = AvsResponseType.PartialMatchAddress;
                    break;
                case "B":
                    result = AvsResponseType.Unavailable;
                    break;
                case "E":
                    result = AvsResponseType.Error;
                    break;
                case "G":
                    result = AvsResponseType.Unavailable;
                    break;
                case "N":
                    result = AvsResponseType.NoMatch;
                    break;
                case "P":
                    result = AvsResponseType.Unavailable;
                    break;
                case "R":
                    result = AvsResponseType.Unavailable;
                    break;
                case "S":
                    result = AvsResponseType.Unavailable;
                    break;
                case "U":
                    result = AvsResponseType.Unavailable;
                    break;
                case "W":
                    result = AvsResponseType.PartialMatchPostalCode;
                    break;
                case "X":
                    result = AvsResponseType.FullMatch;
                    break;
                case "Y":
                    result = AvsResponseType.FullMatch;
                    break;
                case "Z":
                    result = AvsResponseType.PartialMatchPostalCode;
                    break;
            }

            return result;
        }
        private CvnResponseType ParseSecurityCode(string code)
        {
            CvnResponseType result = CvnResponseType.Unavailable;

            switch (code.ToUpper())
            {
                case "M":
                    result = CvnResponseType.Match;
                    break;
                case "N":
                    result = CvnResponseType.NoMatch;
                    break;
                case "P":
                    result = CvnResponseType.Unavailable;
                    break;
                case "S":
                    result = CvnResponseType.Error;
                    break;
                case "U":
                    result = CvnResponseType.Unavailable;
                    break;
                case "X":
                    result = CvnResponseType.Unavailable;
                    break;
            }
            return result;
        }      

        #endregion

    }

    public class PayLeapResponse
    {

        public string Result { get; set; }
        public string Message { get; set; }
        public string RespMSG { get; set; }
        public string AuthCode { get; set; }
        public string PNRef { get; set; }
        public string GetAVSResult { get; set; }
        public string GetCVResult { get; set; }

        public PayLeapResponse()
                  {
                      Result = string.Empty;
                      RespMSG = string.Empty;
                      Message = string.Empty;
                      AuthCode = string.Empty;
                      PNRef = string.Empty;
                      GetAVSResult = string.Empty;
                      GetCVResult = string.Empty;
                  }

        public void Parse(XDocument response)
        {
            if (response != null)
            {
                foreach (XElement e in response.Root.Elements())
                {
                    switch (e.Name.LocalName.ToString())
                    {
                        case "Result":
                            Result = (string)e.Value;
                            break;
                        case "RespMSG":
                            RespMSG = (string)e.Value;
                            break;
                        case "Message":
                            Message = (string)e.Value;
                            break;
                        case "AuthCode":
                            AuthCode = (string)e.Value;
                            break;
                        case "GetAVSResult":
                            GetAVSResult = (string)e.Value;
                            break;
                        case "GetCVResult":
                            GetCVResult = (string)e.Value;
                            break;
                        case "PNRef":
                            PNRef = (string)e.Value;
                            break;
                    }
                }
            }
        }
    }
}