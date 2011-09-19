using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.Generic;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductProperty
	{        				
        public long Id {get;set;}
        public long StoreId { get; set; }
		public string PropertyName {get;set;}
		public string DisplayName {get;set;}
		public bool DisplayOnSite {get;set;}
		public bool DisplayToDropShipper {get;set;}
		public string FriendlyTypeName {
			get {
				string result = "Unknown";
				switch (TypeCode) {
					case Catalog.ProductPropertyType.CurrencyField:
						result = "Currency";
                        break;
					case Catalog.ProductPropertyType.DateField:
						result = "Date";
                        break;
					case Catalog.ProductPropertyType.MultipleChoiceField:
						result = "Multiple Choice";
                        break;
					case Catalog.ProductPropertyType.TextField:
						result = "Text Block";
                        break;
					case Catalog.ProductPropertyType.HyperLink:
						result = "Hyperlink";
                        break;
					default:
						result = "Unknown";
                        break;
				}
				return result;
			}
				// do nothing
			set { }
		}
		public ProductPropertyType TypeCode {get;set;}
		public string DefaultValue {get;set;}
		public string CultureCode {get;set;}
		public List<ProductPropertyChoice> Choices {get;set;}		
        public string TypeCodeDisplayName {
			get {
				string result = "Uknown";
				switch (TypeCode) {
					case ProductPropertyType.CurrencyField:
						result = "Currency Field";
                        break;
					case ProductPropertyType.DateField:
						result = "Date Field";
                        break;
					case ProductPropertyType.HyperLink:
						result = "Hyperlink";
                        break;
					case ProductPropertyType.MultipleChoiceField:
						result = "Multiple Choice";
                        break;
					case ProductPropertyType.None:
						result = "Uknown";
                        break;
					case ProductPropertyType.TextField:
						result = "Text Field";
                        break;
					default:
						result = "Unknown";
                        break;
				}
				return result;
			}
				// do nothing
			set { }
		}
        public DateTime LastUpdatedUtc { get; set; }

        public ProductProperty()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.PropertyName = string.Empty;
            this.DisplayName = string.Empty;
            this.DisplayOnSite = true;
            this.DisplayToDropShipper = false;
            this.TypeCode = ProductPropertyType.TextField;
            this.DefaultValue = string.Empty;
            this.CultureCode = "en-US";
            this.Choices = new List<ProductPropertyChoice>();
            this.LastUpdatedUtc = DateTime.UtcNow;
        }       

        //DTO
        public ProductPropertyDTO ToDto()
        {
            ProductPropertyDTO dto = new ProductPropertyDTO();

            foreach (ProductPropertyChoice c in this.Choices)
            {
                dto.Choices.Add(c.ToDto());
            }
            dto.CultureCode = this.CultureCode;
            dto.DefaultValue = this.DefaultValue;
            dto.DisplayName = this.DisplayName;
            dto.DisplayOnSite = this.DisplayOnSite;
            dto.DisplayToDropShipper = this.DisplayToDropShipper;
            dto.Id = this.Id;
            dto.PropertyName = this.PropertyName;
            dto.StoreId = this.StoreId;
            dto.TypeCode = (ProductPropertyTypeDTO)((int)this.TypeCode);
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            return dto;
        }
        public void FromDto(ProductPropertyDTO dto)
        {
            if (dto == null) return;

            this.Choices.Clear();
            if (dto.Choices != null)
            {
                foreach (ProductPropertyChoiceDTO c in dto.Choices)
                {
                    ProductPropertyChoice pc = new ProductPropertyChoice();
                    pc.FromDto(c);
                    this.Choices.Add(pc);
                }
            }
            this.CultureCode = dto.CultureCode;
            this.DefaultValue = dto.DefaultValue;
            this.DisplayName = dto.DisplayName;
            this.DisplayOnSite = dto.DisplayOnSite;
            this.DisplayToDropShipper = dto.DisplayToDropShipper;
            this.Id = dto.Id;
            this.PropertyName = dto.PropertyName;
            this.StoreId = dto.StoreId;
            this.TypeCode = (ProductPropertyType)((int)dto.TypeCode);
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
        }

	}
}
