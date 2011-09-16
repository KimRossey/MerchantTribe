
using System.Xml.Serialization;

namespace BVSoftware.Commerce.Catalog
{

	public enum ProductInventoryStatus : int
	{
		NotAvailable = 0,
		Available = 1,
		NotSet = -1
	}

}
