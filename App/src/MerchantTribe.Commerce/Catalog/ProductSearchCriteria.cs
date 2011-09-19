using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductSearchCriteria
	{
       	private string _Keyword = string.Empty;
		private string _ManufacturerId = string.Empty;
		private string _VendorId = string.Empty;
		private ProductStatus _Status = ProductStatus.NotSet;
		private ProductInventoryStatus _InventoryStatus = ProductInventoryStatus.NotSet;
		private string _ProductTypeId = string.Empty;
		private string _CategoryId = string.Empty;
		private string _NotCategoryId = string.Empty;				
		private bool _displayInactiveProducts = false;
        private Catalog.CategorySortOrder _CategorySort = CategorySortOrder.None;

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
		public ProductStatus Status {
			get { return _Status; }
			set { _Status = value; }
		}
		public ProductInventoryStatus InventoryStatus {
			get { return _InventoryStatus; }
			set { _InventoryStatus = value; }
		}
		public string ProductTypeId {
			get { return _ProductTypeId; }
			set { _ProductTypeId = value; }
		}
		public string CategoryId {
			get { return _CategoryId; }
			set { _CategoryId = value; }
		}
		public string NotCategoryId {
			get { return _NotCategoryId; }
			set { _NotCategoryId = value; }
		}
		public bool DisplayInactiveProducts {
			get { return _displayInactiveProducts; }
			set { _displayInactiveProducts = value; }
		}
        public CategorySortOrder CategorySort
        {
            get { return _CategorySort; }
            set { _CategorySort = value; }
        }

		public ProductSearchCriteria()
		{

		}
				
		public ProductSearchCriteria Clone()
		{
			ProductSearchCriteria result = new ProductSearchCriteria();

			result.CategoryId = this.CategoryId;
			result.InventoryStatus = this.InventoryStatus;
			result.Keyword = this.Keyword;
			result.ManufacturerId = this.ManufacturerId;
			result.NotCategoryId = this.NotCategoryId;
			result.ProductTypeId = this.ProductTypeId;
			result.Status = this.Status;
			result.VendorId = this.VendorId;
			result.DisplayInactiveProducts = this.DisplayInactiveProducts;

			return result;
		}

	}
}

