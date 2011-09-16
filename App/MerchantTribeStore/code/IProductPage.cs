using System;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce
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