using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Catalog
{
    class CategoryPageVersionRepository: ConvertingRepositoryBase<Data.EF.PageVersion, CategoryPageVersion>
    {

        protected override void CopyModelToData(Data.EF.PageVersion data, CategoryPageVersion model)
        {
            data.AdminName = model.AdminName;
            data.AvailableEndDateUtc = model.AvailableEndDateUtc;
            data.AvailableScheduleId = model.AvailableScheduleId;
            data.AvailableStartDateUtc = model.AvailableStartDateUtc;
            data.Id = model.Id;
            data.PageId = model.PageId;
            data.PublishedStatus = (int)model.PublishedStatus;
            data.SerializedContent = model.Root.SerializeToString();
        }

        protected override void CopyDataToModel(Data.EF.PageVersion data, CategoryPageVersion model)
        {
            model.AdminName = data.AdminName;
            model.AvailableEndDateUtc = data.AvailableEndDateUtc;
            model.AvailableScheduleId = data.AvailableScheduleId;
            model.AvailableStartDateUtc = data.AvailableStartDateUtc;
            model.Id = data.Id;
            model.PageId = data.PageId;
            model.PublishedStatus = (PublishStatus)data.PublishedStatus;
            model.Root.DeserializeFromXml(data.SerializedContent);
        }

        public CategoryPageVersionRepository(IRepositoryStrategy<Data.EF.PageVersion> strategy, ILogger log)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;            
        }
        
        public bool Update(CategoryPageVersion item)
        {            
            return base.Update(item, new PrimaryKey(item.Id));
        }

        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<CategoryPageVersion> FindForPage(string pageId)
        {
            var items = repository.Find().Where(y => y.PageId == pageId)
                                        .OrderByDescending(y => y.Id);
            return ListPoco(items);
        }

        public void DeleteForPage(string pageBvin)
        {
            List<CategoryPageVersion> existing = FindForPage(pageBvin);
            foreach (CategoryPageVersion sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(string pageBvin, List<CategoryPageVersion> subitems)
        {
            // Set Base Key Field
            foreach (CategoryPageVersion item in subitems)
            {
                item.PageId = pageBvin;
            }

            // Create or Update
            foreach (CategoryPageVersion itemnew in subitems)
            {
                if (itemnew.Id < 1)
                {
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }    
        
            // Delete missing
            List<CategoryPageVersion> existing = FindForPage(pageBvin);
            foreach (CategoryPageVersion ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Id == ex.Id
                             select sub).Count();
                if (count < 1)                
                {
                    Delete(ex.Id);
                }
            }
        }

    }
}
