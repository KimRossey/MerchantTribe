using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.Contacts
{
    class MailingListMemberRepository: ConvertingRepositoryBase<Data.EF.bvc_MailingListMember, MailingListMember>
    {

        protected override void CopyModelToData(Data.EF.bvc_MailingListMember data, MailingListMember model)
        {
            data.Id = model.Id;
            data.EmailAddress = model.EmailAddress;
            data.FirstName = model.FirstName;
            data.LastName = model.LastName;
            data.LastUpdatedUtc = model.LastUpdatedUtc;
            data.ListID = model.ListId;
            data.StoreId = model.StoreId;            
        }
        protected override void CopyDataToModel(Data.EF.bvc_MailingListMember data, MailingListMember model)
        {
            model.Id = data.Id;
            model.EmailAddress = data.EmailAddress;
            model.FirstName = data.FirstName;
            model.LastName = data.LastName;
            model.LastUpdatedUtc = data.LastUpdatedUtc;
            model.ListId = data.ListID;
            model.StoreId = data.StoreId;            
        }

        public MailingListMemberRepository(IRepositoryStrategy<Data.EF.bvc_MailingListMember> strategy, ILogger log)
        {
            repository = strategy;
            this.logger = log;
            repository.Logger = this.logger;            
        }

        public MailingListMember Find(long id, long storeId)
        {
            MailingListMember result = this.Find(new PrimaryKey(id));
            if (result != null)
            {
                if (result.StoreId == storeId)
                {
                    return result;
                }
            }
            return null;
        }
        public MailingListMember FindByEmailForList(long listId, string email, long storeId)
        {
            var items = repository.Find().Where(y => y.ListID == listId)
                                         .Where(y => y.StoreId == storeId)
                                         .Where(y => y.EmailAddress == email);                                         
            return SinglePoco(items);                        
        }
        public bool Update(MailingListMember item)
        {
            item.LastUpdatedUtc = DateTime.UtcNow;
            return base.Update(item, new PrimaryKey(item.Id));
        }
        public bool Delete(long id)
        {
            return Delete(new PrimaryKey(id));
        }

        public List<MailingListMember> FindForList(long listId)
        {
            var items = repository.Find().Where(y => y.ListID == listId)
                                        .OrderBy(y => y.EmailAddress);
            return ListPoco(items);
        }

        public void DeleteForList(long listId)
        {
            List<MailingListMember> existing = FindForList(listId);
            foreach (MailingListMember sub in existing)
            {
                Delete(sub.Id);
            }
        }

        public void MergeList(long listId, List<MailingListMember> subitems)
        {
            // Set Base Key Field
            foreach (MailingListMember item in subitems)
            {
                item.ListId = listId;
            }

            // Create or Update
            foreach (MailingListMember itemnew in subitems)
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
            List<MailingListMember> existing = FindForList(listId);
            foreach (MailingListMember ex in existing)
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
