using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class InternationalPostage
    {
        public string ServiceId { get; set; }
        public string ServiceDescription { get; set; }
        public decimal Rate { get; set; }
        public List<InternationalExtraService> ExtraServices { get; set; }
                
        public InternationalPostage()
        {
            ServiceDescription = string.Empty;
            ServiceId = string.Empty;
            Rate = 0m;
            this.ExtraServices = new List<InternationalExtraService>();
        }
        
        public InternationalPostage(XmlNode n)
        {
            ServiceDescription = string.Empty;
            ServiceId = string.Empty;
            Rate = 0m;
            this.ExtraServices = new List<InternationalExtraService>();

            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            if (n != null)
            {
                string classId = n.Attributes["ID"].Value;
                this.ServiceId = classId;

                this.ServiceDescription = MerchantTribe.Web.Xml.ParseInnerText(n, "SvcDescription");
                this.Rate = MerchantTribe.Web.Xml.ParseDecimal(n, "Postage");

                XmlNode ExtraServicesNode = n.SelectSingleNode("ExtraServices");

                this.ExtraServices.Clear();
                foreach (XmlNode n2 in ExtraServicesNode.SelectNodes("ExtraService"))
                {
                    InternationalExtraService svc = new InternationalExtraService(n2);
                    this.ExtraServices.Add(svc);
                }
            }
        }

    }
}
