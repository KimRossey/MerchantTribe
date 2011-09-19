using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;
using MerchantTribe.Web;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class InternationalService
    {
        public string LastRequest { get; set; }
        public string LastResponse { get; set; }

        public InternationalService()
        {
            this.LastResponse = string.Empty;
            this.LastRequest = string.Empty;
        }

        public InternationalResponse ProcessRequest(InternationalRequest request)
        {
            this.LastResponse = string.Empty;
            this.LastRequest = string.Empty;

            // Validate Request First
            InternationalResponse result = ValidateRequest(request);
            if (result.Errors.Count > 0) return result;

            try
            {

                string sURL = request.ApiUrl;
                sURL += "?API=IntlRateV2&XML=";

                // Build XML
                string requestXml = string.Empty;

                StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
                XmlTextWriter xw = new XmlTextWriter(sw);

                xw.Formatting = Formatting.None;

                // Start Request
                xw.WriteStartElement("IntlRateV2Request");
                xw.WriteAttributeString("USERID", request.UserId);
                xw.WriteElementString("Revision", request.Revision);

                foreach (InternationalPackage pak in request.Packages)
                {
                    pak.WriteToXml(ref xw);
                }

                //End Rate Request
                xw.WriteEndElement();
                xw.Flush();
                xw.Close();
                requestXml = sw.GetStringBuilder().ToString();
                if (!requestXml.StartsWith("<"))
                {
                    requestXml = requestXml.Substring(1, requestXml.Length - 1);
                }

                // Diagnostics
                this.LastRequest = requestXml;
                
                string sResponse = string.Empty;
                string dataToSend = sURL + System.Web.HttpUtility.UrlEncode(requestXml);

                sResponse = readHtmlPage(dataToSend);

                // Diagnostics
                this.LastResponse = sResponse;                

                result = new InternationalResponse(sResponse);
            }
            catch (Exception ex)
            {
                result.Errors.Add(new USPSError() { Description = ex.Message + " " + ex.StackTrace, Source = "BV Exception" });
            }
            return result;
        }

        private string readHtmlPage(string url)
        {
            url = url.Replace("\n", "");
            url = url.Replace("\r", "");
            url = url.Replace("\t", "");

            WebResponse objResponse;
            WebRequest objRequest;
            string result = string.Empty;
            objRequest = System.Net.HttpWebRequest.Create(url);
            objResponse = objRequest.GetResponse();
            StreamReader sr = new StreamReader(objResponse.GetResponseStream());
            result += sr.ReadToEnd();
            sr.Close();
            return result;
        }

        public InternationalResponse ValidateRequest(InternationalRequest request)
        {
            InternationalResponse result = new InternationalResponse();

            if (request == null)
            {
                result.Errors.Add(new USPSError() { Description = "Request was null" });
                return result;
            }

            if (request.Packages.Count < 1)
            {
                result.Errors.Add(new USPSError() { Description = "Request requires at least one package." });            
            }

            if (request.UserId.Trim().Length < 1)
            {
                result.Errors.Add(new USPSError() { Description = "UserId is Required for Requests" });
            }            

            return result;
        }

        public static decimal GetInternationalWeightLimit(string countryName)
        {
            decimal result = 44m;

            switch (countryName)
            {
                case "Albania":
                case "Pakistan":
                case "Romania":
                case "Tanzania":
                case "Uganda":
                case "Vanuatu":
                    result = 22m;
                    break;
                case "Chile":
                case "El Salvador":
                case "Israel":
                case "Taiwan":
                    result = 33m;
                    break;
                case "Brazil":
                    result = 50m;
                    break;
                case "Andorra":
                case "Austria":
                case "Belgium":
                case "Canada":
                case "China":
                case "Czech Republic":
                case "Denmark":
                case "Eritrea":
                case "Finland":
                case "France":
                case "French Guiana":
                case "Georgia":
                case "Germany":
                case "Great Britain":
                case "Greece":
                case "Guadeloupe":
                case "Hong Kong":
                case "Ireland":
                case "Italy":
                case "Japan":
                case "Jordan":
                case "Korea":
                case "Liechtenstein":
                case "Luxembourg":
                case "Macao":
                case "Macedonia":
                case "Malaysia":
                case "Malta":
                case "Martinique":
                case "Mexico":
                case "Morocco":
                case "Netherlands":
                case "Norway":
                case "Portugal":
                case "San Marino":
                case "Saudi Arabia":
                case "Singapore":
                case "Slovakia":
                case "Spain":
                case "Sweden":
                case "Switzerland":
                case "Vatican City":
                case "Yemen":
                    result = 66m;
                    break;
                case "Faroe Islands":
                case "Haiti":
                case "Serbia":
                    result = 70m;
                    break;
                default:
                    result = 44m;
                    break;
            }

            return result;
        }
    }
}
