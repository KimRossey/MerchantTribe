using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Billing.Data
{
    public class BillingAccountRepository: IBillingAccountRepository
    {

        private string _ConnectionString = string.Empty;

        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value;
            context = new DbDataContext(value);
            }
        }

        private DbDataContext context = new DbDataContext();

        public BillingAccountRepository()
        {
            ConnectionString = string.Empty;
        }
        public BillingAccountRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private static void CopyModelToData(bvb_BillingAccount data, BillingAccount model)
        {
            data.Email = model.Email;
            data.Id = model.Id;
            data.BillingZipCode = model.BillingZipCode;
            string json = MerchantTribe.Web.Json.ObjectToJson(model.CreditCard);
            string key = MerchantTribe.Web.Cryptography.KeyManager.GetKey(0);
            data.CreditCard = MerchantTribe.Web.Cryptography.AesEncryption.Encode(json, key);
        }
        private static void CopyDataToModel(bvb_BillingAccount data, BillingAccount model)
        {
            model.Email = data.Email;
            model.Id = data.Id;
            model.BillingZipCode = data.BillingZipCode;
            string key = MerchantTribe.Web.Cryptography.KeyManager.GetKey(0);
            if (data.CreditCard.Trim().Length > 2)
            {
                string json = MerchantTribe.Web.Cryptography.AesEncryption.Decode(data.CreditCard, key);
                model.CreditCard = MerchantTribe.Web.Json.ObjectFromJson<MerchantTribe.Payment.CardData>(json);
            }
        }
                    
        #region IBillingAccountRepository Members

        public bool Create(BillingAccount b)
        {
            bool result = false;

            if (b != null)
            {
                bvb_BillingAccount x = new bvb_BillingAccount();
                CopyModelToData(x, b);
               
                try
                {
                    if (context != null)
                    {
                        context.bvb_BillingAccounts.InsertOnSubmit(x);
                        context.SubmitChanges();
                        result = true;
                        // Copy back to model so we get new ID number
                        CopyDataToModel(x, b);
                    }
                }
                catch
                {
                    result = false;                    
                }
            }

            return result;
        }

        public BillingAccount FindById(long id)
        {
            BillingAccount result = null;

            try
            {
                var x = (from n in context.bvb_BillingAccounts
                         where n.Id == id
                         select n).SingleOrDefault();

                if (x != null)
                {
                    result = new BillingAccount();
                    CopyDataToModel(x, result);
                }

            }
            catch
            {
                result = null;
            }

            return result;
        }

        public BillingAccount FindByEmail(string email)
        {
            BillingAccount result = null;
            
            try
            {
                var x = (from n in context.bvb_BillingAccounts
                         where n.Email == email.Trim().ToLowerInvariant()
                         select n).SingleOrDefault();

                if (x != null)
                {
                    result = new BillingAccount();
                    CopyDataToModel(x, result);
                }

            }
            catch
            {
                result = null;
            }

            return result;
        }

        public BillingAccount FindOrCreate(string email)
        {
            BillingAccount b = FindByEmail(email);
            if (b != null)
            {
                if (b.Id > 0)
                {
                    return b;
                }
            }
            b = new BillingAccount();
            b.Email = email.Trim().ToLowerInvariant();
            if (Create(b))
            {
                return b;
            }
            return null;
        }

        public BillingAccount FindOrCreate(BillingAccount a)
        {
            BillingAccount b = FindByEmail(a.Email);
            if (b != null)
            {
                if (b.Id > 0)
                {                    
                    return b;
                }
            }            
            if (Create(a))
            {
                return a;
            }
            return null;
        }

        public bool Update(BillingAccount a)
        {
            bool result = false;

            if (a != null)
            {
                try
                {
                    var x = (from n in context.bvb_BillingAccounts
                             where n.Id == a.Id
                             select n).SingleOrDefault();

                    if (x != null)
                    {
                        CopyModelToData(x, a);
                        context.SubmitChanges();
                        result = true;
                    }

                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public List<BillingAccount> FindAll()
        {
            List<BillingAccount> result = new List<BillingAccount>();

            try
            {
                var x = context.bvb_BillingAccounts.ToList();
                if (x != null)
                {
                    foreach (bvb_BillingAccount n in x)
                    {
                        BillingAccount m = new BillingAccount();
                        CopyDataToModel(n, m);
                        result.Add(m);
                    }
                }
            }
            catch
            {
                result = new List<BillingAccount>();
            }

            return result;
        }

        #endregion
    }
}
