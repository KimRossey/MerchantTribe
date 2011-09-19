using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MerchantTribe.CommerceDTO.v1.Marketing;

namespace MerchantTribe.Commerce.Marketing
{
    public class DiscountDetail
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public DiscountDetail()
        {
            Id = System.Guid.NewGuid();
            Description = string.Empty;
            Amount = 0;
        }
      
        public static string ListToXml(List<DiscountDetail> details)
        {
            var x = new XElement("DiscountDetails",
                        from detail in details
                        select new XElement("DiscountDetail",
                                                new XElement("Id", detail.Id),
                                                new XElement("Description", detail.Description),
                                                new XElement("Amount", detail.Amount)
                                            )
                        );
            return x.ToString(SaveOptions.None);                    
        }

        public static List<DiscountDetail> ListFromXml(string xml)
        {
            if (xml.Trim().Length < 1) return new List<DiscountDetail>();

            XDocument doc = XDocument.Parse(xml, LoadOptions.None);

            var query = from xElem in doc.Descendants("DiscountDetail")
                        select new DiscountDetail
                        {
                            Id = Guid.Parse(xElem.Element("Id").Value),
                            Description = xElem.Element("Description").Value,
                            Amount = Decimal.Parse(xElem.Element("Amount").Value)
                        };
            List<DiscountDetail> result = new List<DiscountDetail>();
            result.AddRange(query);
            return result;
        }

        //DTO
        public DiscountDetailDTO ToDto()
        {
            DiscountDetailDTO dto = new DiscountDetailDTO();

            dto.Id = this.Id;
            dto.Description = this.Description ?? string.Empty;
            dto.Amount = this.Amount;

            return dto;
        }
        public void FromDto(DiscountDetailDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.Description = dto.Description ?? string.Empty;
            this.Amount = dto.Amount;
        }
    }
}
