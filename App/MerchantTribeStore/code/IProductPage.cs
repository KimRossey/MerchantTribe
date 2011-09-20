using System;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;

namespace MerchantTribeStore
{
    public interface IProductPage
    {

        Product LocalProduct { get; set; }
        int ModuleProductQuantity { get; set; }
        IMessageBox MessageBox { get; set; }
        bool DisplaysActiveCategoryTab { get; }

        void PopulateProductInfo(bool isValidCombination);

    }

}