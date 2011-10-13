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

	public class Category
	{
        public string Bvin { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public long StoreId { get; set; }        										
		public string ParentId {get;set;}
		public string Name {get;set;}
		public string Description {get;set;}
		public CategorySortOrder DisplaySortOrder {get;set;}
		public CategorySourceType SourceType {get;set;}
		public int SortOrder {get;set;}
		public string MetaKeywords {get;set;}
		public string MetaDescription {get;set;}
		public string MetaTitle {get;set;}
		public string ImageUrl {get;set;}
		public string BannerImageUrl {get;set;}
		public int LatestProductCount {get;set;}
		public string CustomPageUrl {get;set;}
		public bool CustomPageOpenInNewWindow {get;set;}
		public bool ShowInTopMenu {get;set;}
		public bool Hidden {get;set;}
		public string TemplateName {get;set;}
		public string PreContentColumnId {get;set;}
		public string PostContentColumnId {get;set;}        
		public bool ShowTitle {get;set;}
		public string Criteria {get;set;}		
		public string CustomPageId {get;set;}
		public string PreTransformDescription {get;set;}
		public string Keywords {get;set;}
		public bool CustomerChangeableSortOrder {get;set;}
        public CustomPageLayoutType CustomPageLayout {get;set;}
        private string _RewriteUrl = string.Empty;
        public string RewriteUrl
        {
            get { return _RewriteUrl; }
            set { _RewriteUrl = value.Trim().ToLowerInvariant(); }
        }

        public List<CategoryPageVersion> Versions { get; set; }

        public Category()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.ParentId = "0";
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.DisplaySortOrder = CategorySortOrder.ManualOrder;
            this.SourceType = CategorySourceType.Manual;
            this.SortOrder = 0;
            this.MetaKeywords = string.Empty;
            this.MetaDescription = string.Empty;
            this.MetaTitle = string.Empty;
            this.ImageUrl = string.Empty;
            this.BannerImageUrl = string.Empty;
            this.LatestProductCount = 0;
            this.CustomPageUrl = string.Empty;
            this.ShowInTopMenu = false;
            this.Hidden = false;
            this.TemplateName = "BV Grid";
            this.PreContentColumnId = string.Empty;
            this.PostContentColumnId = string.Empty;
            this.ShowTitle = true;
            this.Criteria = string.Empty;
            this.CustomPageId = string.Empty;
            this.PreTransformDescription = string.Empty;
            this.Keywords = string.Empty;
            this.CustomerChangeableSortOrder = true;
            this.CustomPageLayout = CustomPageLayoutType.Empty;
            Versions = new List<CategoryPageVersion>();
        }

        // Search Within List Functions                                          
        public static List<CategorySnapshot> FindChildrenInList(List<CategorySnapshot> allCats, string parentId)
        {
            return FindChildrenInList(allCats, parentId, false);
        }
        public static List<CategorySnapshot> FindChildrenInList(List<CategorySnapshot> allCats, string parentId, bool showHidden)
        {
            List<CategorySnapshot> results = new List<CategorySnapshot>();

            foreach (CategorySnapshot c in allCats)
            {
                if (c.ParentId == parentId)
                {
                    if (c.Hidden == false || showHidden == true)
                    {
                        results.Add(c);
                    }
                }
            }

            results = results.OrderBy(y => y.SortOrder).ToList();
            return results;
        }
        public static CategorySnapshot FindInList(List<CategorySnapshot> allCats, string bvin)
        {
            foreach (CategorySnapshot cat in allCats)
            {
                if (cat.Bvin == bvin)
                {
                    return cat;
                }
            }
            return null;
        }

        public static Collection<System.Web.UI.WebControls.ListItem> ListFullTreeWithIndents(RequestContext context)
        {
            CategoryRepository repo = CategoryRepository.InstantiateForDatabase(context);
            List<CategorySnapshot> allCats = repo.FindAllPaged(1, int.MaxValue);
            return ListFullTreeWithIndents(allCats);
        }
        public static Collection<System.Web.UI.WebControls.ListItem> ListFullTreeWithIndents(List<CategorySnapshot> allCats)
        {
            return ListFullTreeWithIndents(allCats, false);
        }
        public static Collection<System.Web.UI.WebControls.ListItem> ListFullTreeWithIndents(List<CategorySnapshot> allCats, bool showHidden)
        {
            Collection<System.Web.UI.WebControls.ListItem> result
                = new Collection<System.Web.UI.WebControls.ListItem>();            
            AddIndentedChildren(ref result, "0", 0, ref allCats, showHidden);
            return result;
        }
        private static void AddIndentedChildren(ref Collection<System.Web.UI.WebControls.ListItem> result, string parentId, int currentDepth, ref List<CategorySnapshot> allCats, bool showHidden)
        {

            List<Catalog.CategorySnapshot> children = FindChildrenInList(allCats, parentId, showHidden);
            if (children != null)
            {
                foreach (Catalog.CategorySnapshot c in children)
                {

                    StringBuilder spacer = new StringBuilder();
                    for (int i = 0; i <= currentDepth - 1; i++)
                    {
                        spacer.Append("_");
                    }
                    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                    li.Value = c.Bvin;
                    li.Text = spacer.ToString() + c.Name;
                    result.Add(li);

                    AddIndentedChildren(ref result, c.Bvin, currentDepth + 1, ref allCats, showHidden);
                }
            }
        }

        public static List<CategorySnapshot> BuildTrailToRoot(string categoryId, RequestContext context)
        {
            CategoryRepository repo = CategoryRepository.InstantiateForDatabase(context);

            List<CategorySnapshot> result = new List<CategorySnapshot>();

            List<CategorySnapshot> allCats = repo.FindAllPaged(1, int.MaxValue);

            BuildParentTrail(ref allCats, categoryId, ref result);

            return result;
        }

        public static void BuildParentTrail(ref List<CategorySnapshot> allCats, string currentId, ref List<CategorySnapshot> trail)
        {
            if (currentId == "0" || currentId == string.Empty)
            {
                return;
            }

            Catalog.CategorySnapshot current = FindInList(allCats, currentId);

            if (current != null)
            {
                trail.Add(current);
                if (current.ParentId == "0")
                {
                    return;
                }

                if (current.ParentId != null)
                {
                    if (current.ParentId != string.Empty)
                    {
                        BuildParentTrail(ref allCats, current.ParentId, ref trail);
                    }
                }

            }
        }
                                                                                                                  
        // DTO
        public void FromDto(CategoryDTO dto)
        {
            if (dto == null) return;

            this.BannerImageUrl = dto.BannerImageUrl ?? string.Empty;
            this.Bvin = dto.Bvin ?? string.Empty;
            this.Criteria = dto.Criteria ?? string.Empty;
            this.CustomerChangeableSortOrder = this.CustomerChangeableSortOrder;
            this.CustomPageId = dto.CustomPageId ?? string.Empty;
            this.CustomPageLayout = (CustomPageLayoutType)((int)dto.CustomPageLayout);
            this.CustomPageOpenInNewWindow = dto.CustomPageOpenInNewWindow;
            this.CustomPageUrl = dto.CustomPageUrl ?? string.Empty;
            this.Description = dto.Description ?? string.Empty;
            this.DisplaySortOrder = (CategorySortOrder)((int)dto.DisplaySortOrder);
            this.Hidden = dto.Hidden;
            this.ImageUrl = dto.ImageUrl ?? string.Empty;
            this.Keywords = dto.Keywords ?? string.Empty;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.LatestProductCount = dto.LatestProductCount;
            this.MetaDescription = dto.MetaDescription ?? string.Empty;
            this.MetaKeywords = dto.MetaKeywords ?? string.Empty;
            this.MetaTitle = dto.MetaTitle ?? string.Empty;
            this.Name = dto.Name ?? string.Empty;
            this.ParentId = dto.ParentId ?? string.Empty;
            this.PostContentColumnId = dto.PostContentColumnId ?? string.Empty;
            this.PreContentColumnId = dto.PreContentColumnId ?? string.Empty;
            this.PreTransformDescription = dto.PreTransformDescription ?? string.Empty;
            this.RewriteUrl = dto.RewriteUrl ?? string.Empty;
            this.ShowInTopMenu = dto.ShowInTopMenu;
            this.ShowTitle = dto.ShowTitle;
            this.SortOrder = dto.SortOrder;
            this.SourceType = (CategorySourceType)((int)dto.SourceType);
            this.StoreId = dto.StoreId;
            this.TemplateName = dto.TemplateName ?? string.Empty;            
        }
        public CategoryDTO ToDto()
        {
            CategoryDTO dto = new CategoryDTO();

            dto.BannerImageUrl = this.BannerImageUrl;
            dto.Bvin = this.Bvin;
            dto.Criteria = this.Criteria;
            dto.CustomerChangeableSortOrder = this.CustomerChangeableSortOrder;
            dto.CustomPageId = this.CustomPageId;
            dto.CustomPageLayout = (CustomPageLayoutTypeDTO)((int)this.CustomPageLayout);
            dto.CustomPageOpenInNewWindow = this.CustomPageOpenInNewWindow;
            dto.CustomPageUrl = this.CustomPageUrl;
            dto.Description = this.Description;
            dto.DisplaySortOrder = (CategorySortOrderDTO)((int)this.DisplaySortOrder);
            dto.Hidden = this.Hidden;
            dto.ImageUrl = this.ImageUrl;
            dto.Keywords = this.Keywords;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.LatestProductCount = this.LatestProductCount;
            dto.MetaDescription = this.MetaDescription;
            dto.MetaKeywords = this.MetaKeywords;
            dto.MetaTitle = this.MetaTitle;
            dto.Name = this.Name;
            dto.ParentId = this.ParentId;
            dto.PostContentColumnId = this.PostContentColumnId;
            dto.PreContentColumnId = this.PreContentColumnId;
            dto.PreTransformDescription = this.PreTransformDescription;
            dto.RewriteUrl = this.RewriteUrl;
            dto.ShowInTopMenu = this.ShowInTopMenu;
            dto.ShowTitle = this.ShowTitle;
            dto.SortOrder = this.SortOrder;
            dto.SourceType = (CategorySourceTypeDTO)((int)this.SourceType);
            dto.StoreId = this.StoreId;
            dto.TemplateName = this.TemplateName;            

            return dto;
        }
        
        // Flex Page Stuff
        public IContentPart FindFlexPart(string partId)
        {
            IContentPart result = null;
            if (Versions.Count > 0)
            {
                if (partId == "0")
                {
                    result = Versions[0].Root;
                }
                else
                {
                    result = Versions[0].Root.FindPart(partId);
                }
            }
            return result;
        }

        public RootColumn GetSimpleSample()
        {
            RootColumn root = new RootColumn();

            

            // Sidebar + Main Column
            ColumnContainer col1 = new ColumnContainer(root);
            col1.SetColumns("3,9");
            root.Parts.Add(col1);

            col1.Columns[0].AddPart(new Html());
            col1.Columns[1].AddPart(new Html() { RawHtml = "<h1>Heading</h1>" });
            col1.Columns[1].AddPart(new Html());
            col1.Columns[1].AddPart(new Html() { RawHtml = "<h2>Sub Heading</h2>" });
            col1.Columns[1].AddPart(new Html());

            ColumnContainer col2 = new ColumnContainer(col1.Columns[1]);
            col2.SetColumns("2,5,2");
            col2.Columns[0].AddPart(new Html() { RawHtml = "<p>Left Side</p>" });
            col2.Columns[1].AddPart(new Html());
            col2.Columns[2].AddPart(new Html(){ RawHtml="<ul><li>One</li><li>Two</li><li>Three</li></ul>"});
            col1.Columns[1].AddPart(col2);

            ColumnContainer small = new ColumnContainer(col2.Columns[2]);
            small.SetColumns("1,1");
            small.Columns[0].AddPart(new Html() { RawHtml = "Small text goes here" });
            small.Columns[1].AddPart(new Html() { RawHtml = "More small text!" });
            col2.Columns[2].AddPart(small);

            root.Parts.Add(new Html());

            return root;
        }
        public RootColumn GetSampleContent()
        {
            RootColumn root = new RootColumn();

            Html part1 = new Html() { RawHtml = "Wide Area Here - Col 12" };
            root.Parts.Add(part1);

            // Sidebar + Main Column
            ColumnContainer col1 = new ColumnContainer(root);
            col1.SetColumns("3w,9");
            root.Parts.Add(col1);

            // First 3 in sidebar
            ColumnContainer colside1 = new ColumnContainer(col1.Columns[0]);
            colside1.SetColumns("1,1,1w");
            colside1.Columns[0].Parts.Add(new Html() { RawHtml = "1" });
            colside1.Columns[1].Parts.Add(new Html() { RawHtml = "1" });
            colside1.Columns[2].Parts.Add(new Html() { RawHtml = "1w" });
            col1.Columns[0].Parts.Add(colside1);

            // Second group in side
            ColumnContainer colside2 = new ColumnContainer(col1.Columns[0]);
            colside2.SetColumns("2w,1w");
            colside2.Columns[0].Parts.Add(new Html() { RawHtml = "2w" });
            colside2.Columns[1].Parts.Add(new Html() { RawHtml = "1w" });
            col1.Columns[0].Parts.Add(colside2);

            // Third group in side
            ColumnContainer colside3 = new ColumnContainer(col1.Columns[0]);
            colside3.SetColumns("1,2");
            colside3.Columns[0].Parts.Add(new Html() { RawHtml = "1" });            
            colside3.Columns[1].Parts.Add(new Html() { RawHtml = "2l" });
            col1.Columns[0].Parts.Add(colside3);

            // Html in bottom of sidebar
            col1.Columns[0].Parts.Add(new Html() { RawHtml = "This is some content that will span the entire sidebar column and should be 3 columns wide." });



            // Start Grid 9 Area
            col1.Columns[1].AddPart(new Html() { RawHtml = "Grid 9 - should be bumped up against wide column" });


            // 4 column in 9
            ColumnContainer nine1 = new ColumnContainer(col1.Columns[1]);
            nine1.SetColumns("2,2,2,3");
            nine1.Columns[0].Parts.Add(new Html() { RawHtml = "2" });
            nine1.Columns[1].Parts.Add(new Html() { RawHtml = "2" });            
            nine1.Columns[2].Parts.Add(new Html() { RawHtml = "2" });
            nine1.Columns[3].Parts.Add(new Html() { RawHtml = "3l" });            
            col1.Columns[1].Parts.Add(nine1);

            col1.Columns[1].AddPart(new Html() { RawHtml = "And Another Section" });


            // 5 column in 9
            ColumnContainer nine2 = new ColumnContainer(col1.Columns[1]);
            nine2.SetColumns("4,2,1,1,1");
            nine2.Columns[0].Parts.Add(new Html() { RawHtml = "4" });
            nine2.Columns[1].Parts.Add(new Html() { RawHtml = "2" });
            nine2.Columns[2].Parts.Add(new Html() { RawHtml = "1" });
            nine2.Columns[3].Parts.Add(new Html() { RawHtml = "1" });
            nine2.Columns[4].Parts.Add(new Html() { RawHtml = "1l" });
            col1.Columns[1].Parts.Add(nine2);

            col1.Columns[1].AddPart(new Html() { RawHtml = "And Same Section (no gaps)" });

            // 5wide column in 9
            ColumnContainer nine3 = new ColumnContainer(col1.Columns[1]);
            nine3.SetColumns("4w,2w,1w,1w,1w");
            nine3.Columns[0].Parts.Add(new Html() { RawHtml = "4w" });
            nine3.Columns[1].Parts.Add(new Html() { RawHtml = "2w" });
            nine3.Columns[2].Parts.Add(new Html() { RawHtml = "1w" });
            nine3.Columns[3].Parts.Add(new Html() { RawHtml = "1w" });
            nine3.Columns[4].Parts.Add(new Html() { RawHtml = "1l" });
            col1.Columns[1].Parts.Add(nine3);

            // 12 columns
            root.AddPart(new Html() { RawHtml = "After Others" });
            ColumnContainer r1 = new ColumnContainer(root);
            r1.SetColumns("1,1,1,1,1,1,1,1,1,1,1,1");
            for (int i = 0; i < 12; i++)
            {
                r1.Columns[i].Parts.Add(new Html() { RawHtml = "1" });
            }
            root.AddPart(r1);


            // 12 columns
            root.AddPart(new Html() { RawHtml = "Smallest Columns (no gaps)" });
            ColumnContainer r2 = new ColumnContainer(root);
            r2.SetColumns("1w,1w,1w,1w,1w,1w,1w,1w,1w,1w,1w,1w");
            for (int i = 0; i < 12; i++)
            {
                r2.Columns[i].Parts.Add(new Html() { RawHtml = "1" });
            }
            root.AddPart(r2);


            // 2 even columns
            root.AddPart(new Html() { RawHtml = "2 even columns" });
            ColumnContainer r3 = new ColumnContainer(root);
            r3.SetColumns("6,6");            
            r3.Columns[0].Parts.Add(new Html() { RawHtml = "6" });
            r3.Columns[1].Parts.Add(new Html() { RawHtml = "6l" });
            root.AddPart(r3);

            // 3 even columns
            root.AddPart(new Html() { RawHtml = "3 even columns" });
            ColumnContainer r4 = new ColumnContainer(root);
            r4.SetColumns("4,4,4");
            r4.Columns[0].Parts.Add(new Html() { RawHtml = "4" });
            r4.Columns[1].Parts.Add(new Html() { RawHtml = "4" });
            r4.Columns[2].Parts.Add(new Html() { RawHtml = "4l" });
            root.AddPart(r4);


            // 4 even columns
            root.AddPart(new Html() { RawHtml = "4 even columns" });
            ColumnContainer r4e = new ColumnContainer(root);
            r4e.SetColumns("3,3,3,3");
            r4e.Columns[0].Parts.Add(new Html() { RawHtml = "3" });
            r4e.Columns[1].Parts.Add(new Html() { RawHtml = "3" });
            r4e.Columns[2].Parts.Add(new Html() { RawHtml = "3" });
            r4e.Columns[3].Parts.Add(new Html() { RawHtml = "3l" });
            root.AddPart(r4e);


            // 2 sides + middle
            root.AddPart(new Html() { RawHtml = "2 sides + middle" });
            ColumnContainer r5 = new ColumnContainer(root);
            r5.SetColumns("2,8,2");
            r5.Columns[0].Parts.Add(new Html() { RawHtml = "2" });
            r5.Columns[1].Parts.Add(new Html() { RawHtml = "8" });
            r5.Columns[2].Parts.Add(new Html() { RawHtml = "2l" });
            root.AddPart(r5);

            // 2 sides wide + middle
            root.AddPart(new Html() { RawHtml = "2 sides wide + middle" });
            ColumnContainer r6 = new ColumnContainer(root);
            r6.SetColumns("3,6,3");
            r6.Columns[0].Parts.Add(new Html() { RawHtml = "3" });
            r6.Columns[1].Parts.Add(new Html() { RawHtml = "6" });
            r6.Columns[2].Parts.Add(new Html() { RawHtml = "3l" });
            root.AddPart(r6);

            // Side Left
            root.AddPart(new Html() { RawHtml = "Side Left" });
            ColumnContainer sl = new ColumnContainer(root);
            sl.SetColumns("3,9");            
            sl.Columns[0].Parts.Add(new Html() { RawHtml = "3" });
            sl.Columns[1].Parts.Add(new Html() { RawHtml = "9l" });
            root.AddPart(sl);

            // Side Right
            root.AddPart(new Html() { RawHtml = "Side Right" });
            ColumnContainer sr = new ColumnContainer(root);
            sr.SetColumns("9,3");            
            sr.Columns[0].Parts.Add(new Html() { RawHtml = "9" });
            sr.Columns[1].Parts.Add(new Html() { RawHtml = "3l" });
            root.AddPart(sr);


             // Off Balance Right
            root.AddPart(new Html() { RawHtml = "Off Balance Right" });
            ColumnContainer sr2 = new ColumnContainer(root);
            sr2.SetColumns("5,7");            
            sr2.Columns[0].Parts.Add(new Html() { RawHtml = "5" });
            sr2.Columns[1].Parts.Add(new Html() { RawHtml = "7l" });
            root.AddPart(sr2);


            // Off Balance Left
            root.AddPart(new Html() { RawHtml = "Off Balance Left" });
            ColumnContainer sl2 = new ColumnContainer(root);
            sl2.SetColumns("7,5");            
            sl2.Columns[0].Parts.Add(new Html() { RawHtml = "7" });
            sl2.Columns[1].Parts.Add(new Html() { RawHtml = "5l" });
            root.AddPart(sl2);

            root.AddPart(new Html() { RawHtml = "<p>End of Samples</p>" });


            return root;
        }

        public CategoryPageVersion GetCurrentVersion()
        {
            return GetCurrentVersion(DateTime.UtcNow);
        }
        public CategoryPageVersion GetCurrentVersion(DateTime currentUtcTime)
        {
            CategoryPageVersion current = new CategoryPageVersion();

            if (Versions.Count > 0)
            {
                return (Versions[0]);
            }

            return current;
        }
        
    }



}
