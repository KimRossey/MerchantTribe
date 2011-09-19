using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class CategoryFacet
    {
        private long _Id = 0;
        private string _CategoryId = string.Empty;
        private long _PropertyId = 0;
        private long _ParentPropertyId = 0;
        private int _SortOrder = 0;
        private string _FilterName = string.Empty;
        private CategoryFacetDisplayMode _DisplayMode = CategoryFacetDisplayMode.Single;
        private long _StoreId = 0;

        public CategoryFacet()
        {
        }

        public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        public string CategoryId
        {
            get { return _CategoryId; }
            set { _CategoryId = value; }
        }
        public long PropertyId
        {
            get { return _PropertyId; }
            set { _PropertyId = value; }
        }
        public long ParentPropertyId
        {
            get { return _ParentPropertyId; }
            set { _ParentPropertyId = value; }
        }
        public int SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }
        public string FilterName
        {
            get { return _FilterName; }
            set { _FilterName = value; }
        }
        public CategoryFacetDisplayMode DisplayMode
        {
            get { return _DisplayMode; }
            set { _DisplayMode = value; }
        }
        public long StoreId
        {
            get { return _StoreId; }
            set { _StoreId = value; }
        }

    }
}
