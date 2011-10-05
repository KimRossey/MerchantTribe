using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;
namespace MerchantTribe.Commerce.Content
{
    public class ContentBlockRepository: ConvertingRepositoryBase<Data.EF.bvc_ContentBlock, ContentBlock>
    {
        RequestContext context = null;

        public static ContentBlockRepository InstantiateForMemory(RequestContext c)
        {
            ContentBlockRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new ContentBlockRepository(c, new MemoryStrategy<Data.EF.bvc_ContentBlock>(PrimaryKeyType.Bvin), logger);
            return result;
        }
        public static ContentBlockRepository InstantiateForDatabase(RequestContext c)
        {            
            ContentBlockRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new ContentBlockRepository(c, new EntityFrameworkRepository<Data.EF.bvc_ContentBlock>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public ContentBlockRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_ContentBlock> strategy, ILogger log)
        {
            this.context = c;
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.bvc_ContentBlock data, ContentBlock model)
        {
            model.Bvin = data.bvin;
            model.StoreId = data.StoreId;
            model.LastUpdated = data.LastUpdated;
            model.ColumnId = data.ColumnID;
            model.ControlName = data.ControlName;
            model.SortOrder = data.SortOrder;            

            string serializedSettings = data.SerializedSettings;
            model.BaseSettings = MerchantTribe.Web.Json.ObjectFromJson<ContentBlockSettings>(serializedSettings);
            if (model.BaseSettings == null) model.BaseSettings = new ContentBlockSettings();

            string lists = data.SerializedLists;
            model.Lists = MerchantTribe.Web.Json.ObjectFromJson<ContentBlockSettingList>(lists);
            if (model.Lists == null) model.Lists = new ContentBlockSettingList();

        }
        protected override void CopyModelToData(Data.EF.bvc_ContentBlock data, ContentBlock model)
        {
            data.bvin = model.Bvin;
            data.StoreId = model.StoreId;
            data.LastUpdated = model.LastUpdated;
            data.ColumnID = model.ColumnId;
            data.ControlName = model.ControlName;
            data.SortOrder = model.SortOrder;
            data.SerializedSettings = MerchantTribe.Web.Json.ObjectToJson(model.BaseSettings);
            data.SerializedLists = MerchantTribe.Web.Json.ObjectToJson(model.Lists);

          
        }
       

        public ContentBlock Find(string bvin)
        {
            Data.EF.bvc_ContentBlock data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;
            ContentBlock result = new ContentBlock();
            CopyDataToModel(data, result);
            return result;            
        }
        public override bool Create(ContentBlock item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.LastUpdated = DateTime.UtcNow;
            item.StoreId = context.CurrentStore.Id;            
            return base.Create(item);
        }
        public bool Update(ContentBlock item)
        {
            item.LastUpdated = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Bvin));
        }
        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }
        public List<ContentBlock> FindForColumn(string columnId, long storeId)
        {
            var items = repository.Find().Where(y => y.ColumnID == columnId)
                                        .Where(y => y.StoreId == storeId)
                                        .OrderBy(y => y.SortOrder);
            return ListPoco(items);
        }
        public void DeleteForColumn(string columnId, long storeId)
        {
            List<ContentBlock> existing = FindForColumn(columnId, storeId);
            foreach (ContentBlock sub in existing)
            {
                Delete(sub.Bvin);
            }
        }
        private int FindMaxSort(List<ContentBlock> items)
        {
            int maxSort = 0;
            if (items == null) return 0;
            if (items.Count < 1) return 0;
            maxSort = items.Max(y => y.SortOrder);
            return maxSort;
        }
        public bool Resort(string columnId, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForBlock(columnId, sortedIds[i - 1], i);
                }
            }
            return true;
        }
        public bool UpdateSortOrderForBlock(string columnId, string blockId, int newSortOrder)
        {
            ContentBlock item = Find(new PrimaryKey(blockId));
            if (item == null) return false;
            if (item.ColumnId != columnId) return false;
            item.SortOrder = newSortOrder;
            return Update(item);
        }
        public void MergeList(string columnId, long storeId, List<ContentBlock> subitems)
        {
            // Set Base Key Field
            foreach (ContentBlock item in subitems)
            {
                item.ColumnId = columnId;
            }

            int maxSort = FindMaxSort(subitems);

            // Create or Update
            foreach (ContentBlock itemnew in subitems)
            {
                if (itemnew.Bvin == string.Empty)
                {
                    maxSort++;
                    itemnew.SortOrder = maxSort;
                    Create(itemnew);
                }
                else
                {
                    Update(itemnew);
                }
            }    
        
            // Delete missing
            List<ContentBlock> existing = FindForColumn(columnId, storeId);
            foreach (ContentBlock ex in existing)
            {
                var count = (from sub in subitems
                             where sub.Bvin == ex.Bvin
                             select sub).Count();
                if (count < 1)                
                {
                    Delete(ex.Bvin);
                }
            }
        }

    }
}
