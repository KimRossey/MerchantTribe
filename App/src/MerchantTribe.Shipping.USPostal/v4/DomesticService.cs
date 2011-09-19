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
    public class DomesticService
    {
        public string LastRequest { get; set; }
        public string LastResponse { get; set; }

        public DomesticService()
        {
            this.LastResponse = string.Empty;
            this.LastRequest = string.Empty;
        }

        public DomesticResponse ProcessRequest(DomesticRequest request)
        {
            this.LastResponse = string.Empty;
            this.LastRequest = string.Empty;

            // Validate Request First
            DomesticResponse result = ValidateRequest(request);
            if (result.Errors.Count > 0) return result;

            try
            {

                string sURL = request.ApiUrl;
                sURL += "?API=RateV4&XML=";

                // Build XML
                string requestXml = string.Empty;

                StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
                XmlTextWriter xw = new XmlTextWriter(sw);

                xw.Formatting = Formatting.None;

                // Start Request
                xw.WriteStartElement("RateV4Request");
                xw.WriteAttributeString("USERID", request.UserId);
                xw.WriteElementString("Revision", request.Revision);

                foreach (DomesticPackage pak in request.Packages)
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

                result = new DomesticResponse(sResponse);
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

        public DomesticResponse ValidateRequest(DomesticRequest request)
        {
            DomesticResponse result = new DomesticResponse();

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
    }
}
