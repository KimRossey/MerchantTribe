
namespace MerchantTribe.Commerce.Catalog
{
	public enum ProductInventoryMode
	{
		NotSet = -1,
        AlwayInStock = 100,
        WhenOutOfStockHide = 101,
        WhenOutOfStockShow = 102,
        WhenOutOfStockAllowBackorders = 103
	}
}

