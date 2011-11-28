using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Content
{
    class PolicyBlockRepository: ConvertingRepositoryBase<Data.EF.bvc_PolicyBlock, PolicyBlock>
    {
        RequestContext context = null;

        public static PolicyBlockRepository InstantiateForMemory(RequestContext c)
        {
            PolicyBlockRepository result = null;            
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new PolicyBlockRepository(c, new MemoryStrategy<Data.EF.bvc_PolicyBlock>(PrimaryKeyType.Bvin), logger);
            return result;
        }
        public static PolicyBlockRepository InstantiateForDatabase(RequestContext c)
        {            
            PolicyBlockRepository result = null;
            ILogger logger = new MerchantTribe.Commerce.EventLog();
            result = new PolicyBlockRepository(c, new EntityFrameworkRepository<Data.EF.bvc_PolicyBlock>(
                    new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)), logger);
            return result;
        }
        public PolicyBlockRepository(RequestContext c, IRepositoryStrategy<Data.EF.bvc_PolicyBlock> strategy, ILogger log)
        {
            this.context = c;
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyModelToData(Data.EF.bvc_PolicyBlock data, PolicyBlock model)
        {
            data.bvin = model.Bvin;
            data.Description = model.Description;
            data.DescriptionPreTransform = model.DescriptionPreTransform;
            data.LastUpdated = model.LastUpdated;
            data.Name = model.Name;
            data.PolicyID = model.PolicyID;
            data.SortOrder = model.SortOrder;
            data.StoreId = model.StoreId;

           
        }
        protected override void CopyDataToModel(Data.EF.bvc_PolicyBlock data, PolicyBlock model)
        {
            model.Bvin = data.bvin;
            model.Description = data.Description;
            model.DescriptionPreTransform = data.DescriptionPreTransform;
            model.LastUpdated = data.LastUpdated;
            model.Name = data.Name;
            model.PolicyID = data.PolicyID;
            model.SortOrder = data.SortOrder;
            model.StoreId = data.StoreId;
        }

        public PolicyBlock Find(string bvin)
        {
            Data.EF.bvc_PolicyBlock data = repository.FindByPrimaryKey(new PrimaryKey(bvin));
            if (data == null) return null;
            PolicyBlock result = new PolicyBlock();
            CopyDataToModel(data, result);
            return result;            
        }
        public override bool Create(PolicyBlock item)
        {
            if (item.Bvin == string.Empty)
            {
                item.Bvin = System.Guid.NewGuid().ToString();
            }
            item.LastUpdated = DateTime.UtcNow;
            item.StoreId = context.CurrentStore.Id;            
            return base.Create(item);
        }
        public bool Update(PolicyBlock item)
        {
            item.LastUpdated = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Bvin));
        }
        public bool Delete(string bvin)
        {
            return Delete(new PrimaryKey(bvin));
        }
        public List<PolicyBlock> FindForPolicy(string policyId)
        {
            var items = repository.Find().Where(y => y.PolicyID == policyId)
                                        .OrderBy(y => y.SortOrder);
            return ListPoco(items);
        }
        public void DeleteForPolicy(string policyId)
        {
            List<PolicyBlock> existing = FindForPolicy(policyId);
            foreach (PolicyBlock sub in existing)
            {
                Delete(sub.Bvin);
            }
        }
        private int FindMaxSort(List<PolicyBlock> items)
        {
            int maxSort = 0;
            if (items == null) return 0;
            if (items.Count < 1) return 0;
            maxSort = items.Max(y => y.SortOrder);
            return maxSort;
        }
        public bool Resort(string policyId, List<string> sortedIds)
        {
            if (sortedIds != null)
            {
                for (int i = 1; i <= sortedIds.Count; i++)
                {
                    UpdateSortOrderForBlock(policyId, sortedIds[i - 1], i);
                }
            }
            return true;
        }
        public bool UpdateSortOrderForBlock(string policyId, string blockId, int newSortOrder)
        {
            PolicyBlock item = Find(new PrimaryKey(blockId));
            if (item == null) return false;
            if (item.PolicyID != policyId) return false;
            item.SortOrder = newSortOrder;
            return Update(item);
        }
        public void MergeList(string policyId, List<PolicyBlock> subitems)
        {
            // Set Base Key Field
            foreach (PolicyBlock item in subitems)
            {
                item.PolicyID = policyId;
            }

            int maxSort = FindMaxSort(subitems);

            // Create or Update
            foreach (PolicyBlock itemnew in subitems)
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
            List<PolicyBlock> existing = FindForPolicy(policyId);
            foreach (PolicyBlock ex in existing)
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
