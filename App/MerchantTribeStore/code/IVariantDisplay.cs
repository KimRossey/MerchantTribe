using System;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Orders;

namespace BVCommerce
{
    public interface IVariantDisplay
    {
        bool IsValidCombination { get; set; }
        Product GetSelectedProduct(Product currentProduct);
        bool IsValid();
        void WriteValuesToLineItem(LineItem lineItem);
        void Initialize(bool clear);
    }
}