using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class DomesticPostage
    {

        public string MailServiceClassId { get; set; }
        public List<DomesticSpecialService> SpecialServices { get; set; }
        public string MailService {get;set;}
        public decimal Rate {get;set;}
    
        public DomesticPostage()
        {
            MailService = string.Empty;
            MailServiceClassId = string.Empty;
            Rate = 0m;
            this.SpecialServices = new List<DomesticSpecialService>();
        }
        
        public DomesticPostage(XmlNode n)
        {
            MailService = string.Empty;
            MailServiceClassId = string.Empty;
            Rate = 0m;
            this.SpecialServices = new List<DomesticSpecialService>();

            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            if (n != null)
            {
                string classId = n.Attributes["CLASSID"].Value;
                this.MailServiceClassId = classId;

                this.MailService = MerchantTribe.Web.Xml.ParseInnerText(n, "MailService");
                this.Rate = MerchantTribe.Web.Xml.ParseDecimal(n, "Rate");

                this.SpecialServices.Clear();
                foreach (XmlNode n2 in n.SelectNodes("SpecialService"))
                {
                    DomesticSpecialService svc = new DomesticSpecialService(n2);
                    this.SpecialServices.Add(svc);
                }
            }
        }

    }
}
