using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MerchantTribe.Shipping.USPostal.v4
{
    public class InternationalExtraService
    {
            private string _ServiceId = string.Empty;

            public InternationalExtraServiceType ServiceType { get; set; }
            public decimal Price { get; set; }
            public string ServiceName { get; set; }
            public bool Available { get; set; }

            public InternationalExtraService()
            {
                this.ServiceName = "";
                this.ServiceType = InternationalExtraServiceType.NotSet;
                this.Price = 0m;
                this.Available = false;
            }
            public InternationalExtraService(XmlNode node)
            {
                this.ServiceName = "";
                this.ServiceType = InternationalExtraServiceType.NotSet;
                this.Price = 0m;
                this.Available = false;

                this.ParseNode(node);
            }

            public void ParseNode(XmlNode n)
            {
                if (n != null)
                {

                    this._ServiceId = MerchantTribe.Web.Xml.ParseInnerText(n, "ServiceID");
                    this.ServiceName = MerchantTribe.Web.Xml.ParseInnerText(n, "ServiceName");
                    this.Price = MerchantTribe.Web.Xml.ParseDecimal(n, "Price");
                    this.Available = MerchantTribe.Web.Xml.ParseBoolean(n, "Available");

                    try
                    {
                        int temp = -1;
                        if (int.TryParse(_ServiceId, out temp))
                        {
                            this.ServiceType = (InternationalExtraServiceType)temp;
                        }
                    }
                    catch
                    {
                        this.ServiceType = InternationalExtraServiceType.NotSet;
                    }
                }
            }

        
    }
}
