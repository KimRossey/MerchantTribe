using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductSearchResultGroup
	{
		private string _GroupName = string.Empty;
		private Collection<Catalog.Product> _Products = new Collection<Catalog.Product>();
		private string _InfoMessage = string.Empty;

		public string GroupName {
			get { return _GroupName; }
			set { _GroupName = value; }
		}
		public Collection<Catalog.Product> Products {
			get { return _Products; }
			set { _Products = value; }
		}
		public string InfoMessage {
			get { return _InfoMessage; }
			set { _InfoMessage = value; }
		}

		public ProductSearchResultGroup()
		{

		}

		public ProductSearchResultGroup(string name, Collection<Catalog.Product> productResults)
		{
			_GroupName = name;
			_Products = productResults;
		}

	}
}
