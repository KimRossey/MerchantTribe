using System;
using System.Collections.ObjectModel;

namespace BVSoftware.Commerce.Controls
{
	public class BVValidationController
	{
		public static string GetRegularExpression(string key)
		{
			switch (key.ToLower()) {
				case "catalog.product.productname":
					return ".{1," + Catalog.Product.ProductNameMaxLength + "}";
				case "catalog.product.sku":
					return ".{1," + Catalog.Product.SkuMaxLength + "}";
				default:
					throw new ApplicationException("Regular Expression Key Not Found: " + key);
			}
		}

		public static string GetErrorMessage(string key)
		{
			switch (key.ToLower()) {
				case "catalog.product.productname.lengthexceeded":
					return "Product Name must be between 1 and " + Catalog.Product.ProductNameMaxLength.ToString() + " characters long.";
				case "catalog.product.sku.lengthexceeded":
					return "Product Sku must be between 1 and " + Catalog.Product.SkuMaxLength.ToString() + " characters long.";
				default:
					throw new ApplicationException("Error Key Not Found: " + key);
			}
		}
	}
}
