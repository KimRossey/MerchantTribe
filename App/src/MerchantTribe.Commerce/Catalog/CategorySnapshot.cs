using System;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using MerchantTribe.Web;
using System.Data.Entity;
using MerchantTribe.Commerce.Content.Parts;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{

    public class CategorySnapshot
    {
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public CategorySourceType SourceType { get; set; }
        public string ImageUrl { get; set; }
        public int LatestProductCount { get; set; }
        public string CustomPageUrl { get; set; }
        public bool CustomPageOpenInNewWindow { get; set; }
        public bool ShowInTopMenu { get; set; }
        public bool Hidden { get; set; }
        public string CustomPageId { get; set; }
        public CustomPageLayoutType CustomPageLayout { get; set; }
        private string _RewriteUrl = string.Empty;
        public string RewriteUrl
        {
            get { return _RewriteUrl; }
            set { _RewriteUrl = value.Trim().ToLowerInvariant(); }
        }
        public int SortOrder { get; set; }
        public string MetaTitle { get; set; }

        public CategorySnapshot()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.ParentId = "0";
            this.Name = string.Empty;
            this.SourceType = CategorySourceType.Manual;
            this.ImageUrl = string.Empty;
            this.LatestProductCount = 0;
            this.CustomPageUrl = string.Empty;
            this.ShowInTopMenu = false;
            this.Hidden = false;
            this.CustomPageId = string.Empty;
            this.CustomPageLayout = CustomPageLayoutType.Empty;
            this.SortOrder = 0;
            this.MetaTitle = string.Empty;
        }
        public CategorySnapshot(Category cat)
        {
            this.Bvin = cat.Bvin;
            this.StoreId = cat.StoreId;
            this.ParentId = cat.ParentId;
            this.Name = cat.Name;
            this.SourceType = cat.SourceType;
            this.ImageUrl = cat.ImageUrl;
            this.LatestProductCount = cat.LatestProductCount;
            this.CustomPageUrl = cat.CustomPageUrl;
            this.ShowInTopMenu = cat.ShowInTopMenu;
            this.Hidden = cat.Hidden;
            this.CustomPageId = cat.CustomPageId;
            this.CustomPageLayout = cat.CustomPageLayout;
            this.SortOrder = cat.SortOrder;
            this.MetaTitle = cat.MetaTitle;
            this.RewriteUrl = cat.RewriteUrl;
        }


        // DTO
        public CategorySnapshotDTO ToDto()
        {
            CategorySnapshotDTO dto = new CategorySnapshotDTO();

            dto.Bvin = this.Bvin;
            dto.CustomPageId = this.CustomPageId;
            dto.CustomPageLayout = (CustomPageLayoutTypeDTO)((int)this.CustomPageLayout);
            dto.CustomPageOpenInNewWindow = this.CustomPageOpenInNewWindow;
            dto.CustomPageUrl = this.CustomPageUrl;
            dto.Hidden = this.Hidden;
            dto.ImageUrl = this.ImageUrl;
            dto.LatestProductCount = this.LatestProductCount;
            dto.Name = this.Name;
            dto.ParentId = this.ParentId;
            dto.RewriteUrl = this.RewriteUrl;
            dto.ShowInTopMenu = this.ShowInTopMenu;
            dto.SourceType = (CategorySourceTypeDTO)((int)this.SourceType);
            dto.StoreId = this.StoreId;
            dto.SortOrder = this.SortOrder;
            dto.MetaTitle = this.MetaTitle;
            return dto;
        }

    }

}
