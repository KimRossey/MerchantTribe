using System;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductStoreSearchCriteria
	{

		public class CustomPropertyValue
		{
			private string _PropertyBvin = string.Empty;
			private string _PropertyValue = string.Empty;

			public string PropertyBvin {
				get { return _PropertyBvin; }
				set { _PropertyBvin = value; }
			}
			public string PropertyValue {
				get { return _PropertyValue; }
				set { _PropertyValue = value; }
			}

			public CustomPropertyValue()
			{

			}

			public CustomPropertyValue(string bvin, string value)
			{
				_PropertyBvin = bvin;
				_PropertyValue = value;
			}

			public CustomPropertyValue Clone()
			{
				CustomPropertyValue result = new CustomPropertyValue(this.PropertyBvin, this.PropertyValue);
				return result;
			}

		}

		private string _Keyword = string.Empty;
		private string _ManufacturerId = string.Empty;
		private string _VendorId = string.Empty;
		private string _CategoryId = string.Empty;
		private decimal _MinPrice = -1m;
		private decimal _MaxPrice = -1m;
		private ProductSearchCriteriaSortOrder _sortOrder = ProductSearchCriteriaSortOrder.NotSet;
		private ProductStoreSearchSortBy _sortBy = ProductStoreSearchSortBy.NotSet;
		private Collection<CustomPropertyValue> _CustomProperties = new Collection<CustomPropertyValue>();

		public string Keyword {
			get { return _Keyword; }
			set { _Keyword = value; }
		}
		public string ManufacturerId {
			get { return _ManufacturerId; }
			set { _ManufacturerId = value; }
		}
		public string VendorId {
			get { return _VendorId; }
			set { _VendorId = value; }
		}
		public string CategoryId {
			get { return _CategoryId; }
			set { _CategoryId = value; }
		}
		public decimal MinPrice {
			get { return _MinPrice; }
			set { _MinPrice = value; }
		}
		public decimal MaxPrice {
			get { return _MaxPrice; }
			set { _MaxPrice = value; }
		}
		public ProductSearchCriteriaSortOrder SortOrder {
			get { return _sortOrder; }
			set { _sortOrder = value; }
		}
		public ProductStoreSearchSortBy SortBy {
			get { return _sortBy; }
			set { _sortBy = value; }
		}
		public Collection<CustomPropertyValue> CustomProperties {
			get { return _CustomProperties; }
			set { _CustomProperties = value; }
		}

		public ProductStoreSearchCriteria()
		{

		}

		public ProductStoreSearchCriteria Clone()
		{
			ProductStoreSearchCriteria result = new ProductStoreSearchCriteria();

			result.CategoryId = this.CategoryId;
			foreach (CustomPropertyValue cpv in this.CustomProperties) {
				CustomPropertyValue cp = cpv.Clone();
				result.CustomProperties.Add(cp);
			}
			result.Keyword = this.Keyword;
			result.ManufacturerId = this.ManufacturerId;
			result.MaxPrice = this.MaxPrice;
			result.MinPrice = this.MinPrice;
			result.SortBy = this.SortBy;
			result.SortOrder = this.SortOrder;
			result.VendorId = this.VendorId;

			return result;
		}

	}
}
