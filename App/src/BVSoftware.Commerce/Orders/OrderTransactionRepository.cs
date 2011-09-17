using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Data;
using System.Data.Objects;
using MerchantTribe.Web.Logging;

namespace BVSoftware.Commerce.Orders
{
    public class OrderTransactionRepository: ConvertingRepositoryBase<Data.EF.ecommrc_OrderTransactions, OrderTransaction>
    {
        private RequestContext context = null;

        public static OrderTransactionRepository InstantiateForMemory(RequestContext c)
        {
            OrderTransactionRepository result = null;
            result = new OrderTransactionRepository(c,
                        new MemoryStrategy<Data.EF.ecommrc_OrderTransactions>(PrimaryKeyType.Long), new TextLogger());
            return result;
        }
        public static OrderTransactionRepository InstantiateForDatabase(RequestContext c)
        {
            OrderTransactionRepository result = null;
            result = new OrderTransactionRepository(c,
                     new EntityFrameworkRepository<Data.EF.ecommrc_OrderTransactions>(new Data.EF.EntityFrameworkDevConnectionString(c.ConnectionStringForEntityFramework)),
                     new EventLog());            
            return result;
        }
        public OrderTransactionRepository(RequestContext c, IRepositoryStrategy<Data.EF.ecommrc_OrderTransactions> r, ILogger log)
        {
            context = c;
            repository = r;
            this.logger = log;
            repository.Logger = this.logger;
        }

        protected override void CopyDataToModel(Data.EF.ecommrc_OrderTransactions data, OrderTransaction model)
        {
            model.Action = (BVSoftware.Payment.ActionType)data.Action;
            model.Amount = data.Amount;

            string key = MerchantTribe.Web.Cryptography.KeyManager.GetKey(0);
            if (data.CreditCard.Trim().Length > 2)
            {
                string json = MerchantTribe.Web.Cryptography.AesEncryption.Decode(data.CreditCard, key);
                model.CreditCard = MerchantTribe.Web.Json.ObjectFromJson<BVSoftware.Payment.CardData>(json);
            }

            model.Id = data.Id;
            model.LinkedToTransaction = data.LinkedToTransaction;
            model.OrderId = data.OrderId;
            model.OrderNumber = data.OrderNumber;
            model.RefNum1 = data.RefNum1;
            model.RefNum2 = data.RefNum2;
            model.StoreId = data.StoreId;
            model.Success = data.Success;
            model.TimeStampUtc = data.Timestamp;
            model.Voided = data.Voided;
            model.Messages = data.Messages;
            model.CheckNumber = data.CheckNumber;
            model.GiftCardNumber = data.GiftCardNumber;
            model.PurchaseOrderNumber = data.PurchaseOrderNumber;
            model.CompanyAccountNumber = data.CompanyAccountNumber;
        }
        protected override void CopyModelToData(Data.EF.ecommrc_OrderTransactions data, OrderTransaction model)
        {
            data.Action = (int)model.Action;
            data.Amount = model.Amount;

            string json = MerchantTribe.Web.Json.ObjectToJson(model.CreditCard);
            string key = MerchantTribe.Web.Cryptography.KeyManager.GetKey(0);
            data.CreditCard = MerchantTribe.Web.Cryptography.AesEncryption.Encode(json, key);

            data.Id = model.Id;
            data.LinkedToTransaction = model.LinkedToTransaction;
            data.OrderId = model.OrderId;
            data.OrderNumber = model.OrderNumber;
            data.RefNum1 = model.RefNum1;
            data.RefNum2 = model.RefNum2;
            data.StoreId = model.StoreId;
            data.Success = model.Success;
            data.Timestamp = model.TimeStampUtc;
            data.Voided = model.Voided;
            data.Messages = model.Messages;
            data.GiftCardNumber = model.GiftCardNumber;
            data.CheckNumber = model.CheckNumber;
            data.PurchaseOrderNumber = model.PurchaseOrderNumber;
            data.CompanyAccountNumber = model.CompanyAccountNumber;
        }

        public override bool Create(OrderTransaction item)
        {
            item.StoreId = context.CurrentStore.Id;
            return base.Create(item);
        }
        public bool Update(OrderTransaction c)
        {
            return this.Update(c, new PrimaryKey(c.Id));
        }
        public bool Delete(Guid id)
        {
            long storeId = context.CurrentStore.Id;
            OrderTransaction item = Find(id);
            if (item == null) return false;
            if (item.StoreId != storeId) return false;
            return Delete(new PrimaryKey(id));
        }

        public OrderTransaction Find(Guid id)
        {
            OrderTransaction result = FindForAllStores(id);
            if (result != null)
            {
                if (result.StoreId == context.CurrentStore.Id)
                {
                    return result;
                }
            }
            return null;
        }
        public OrderTransaction FindForAllStores(Guid id)
        {
            Data.EF.ecommrc_OrderTransactions data = repository.FindByPrimaryKey(new PrimaryKey(id));
            if (data == null) return null;
            OrderTransaction result = new OrderTransaction();
            CopyDataToModel(data, result);
            return result;
        }

        public bool RecordPaymentTransaction(BVSoftware.Payment.Transaction t, Orders.Order o)
        {
            OrderTransaction ot = new OrderTransaction(t);
            ot.OrderId = o.bvin;
            ot.StoreId = o.StoreId;
            return Create(ot);
        }

        public List<OrderTransaction> FindForOrder(string orderId)
        {
            List<OrderTransaction> result = new List<OrderTransaction>();

            IQueryable<Data.EF.ecommrc_OrderTransactions> items = repository.Find().Where(y => y.OrderId == orderId)
                                                                          .OrderBy(y => y.Timestamp);
            if (items != null)
            {
                result = ListPoco(items);
            }

            return result;
        }

        public OrderTransaction FindAuthForOrder(string orderId)
        {
            IQueryable<Data.EF.ecommrc_OrderTransactions> items = repository.Find().Where(y => y.OrderId == orderId)
                                        .Where(y => y.Success == true)
                                        .Where(y => y.Voided == false)
                                        .OrderBy(y => y.Timestamp);
            return SinglePoco(items);                                        
        }

        public decimal TransactionsPotentialStoreCredits(List<OrderTransaction> transactions)
        {
            decimal amount = 0;
            foreach (OrderTransaction t in transactions)
            {
                if (t.Action == BVSoftware.Payment.ActionType.GiftCardInfo ||
                    t.Action == BVSoftware.Payment.ActionType.RewardPointsInfo)
                {
                    amount += t.Amount;
                }
            }
            return amount;
        }


        // Page Number is 1 based, NOT zero based
        public List<OrderTransaction> FindForReportByDateRange(DateTime startDateUtc, DateTime endDateUtc, 
                                                                long storeId, int pageSize, 
                                                                int pageNumber, ref int totalCount)
        {
            try
            {
                if (pageNumber < 1) pageNumber = 1;

                int take = pageSize;
                int skip = (pageNumber - 1) * pageSize;

                List<int> reportActions = new List<int>();
                reportActions.Add((int)BVSoftware.Payment.ActionType.CashReceived);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CashReturned);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CheckReceived);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CheckReturned);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CompanyAccountAccepted);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CreditCardCapture);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CreditCardCharge);
                reportActions.Add((int)BVSoftware.Payment.ActionType.CreditCardRefund);
                reportActions.Add((int)BVSoftware.Payment.ActionType.GiftCardCapture);
                reportActions.Add((int)BVSoftware.Payment.ActionType.GiftCardDecrease);
                reportActions.Add((int)BVSoftware.Payment.ActionType.GiftCardIncrease);
                reportActions.Add((int)BVSoftware.Payment.ActionType.PayPalCapture);
                reportActions.Add((int)BVSoftware.Payment.ActionType.PayPalCharge);
                reportActions.Add((int)BVSoftware.Payment.ActionType.PayPalRefund);
                reportActions.Add((int)BVSoftware.Payment.ActionType.RewardPointsCapture);
                reportActions.Add((int)BVSoftware.Payment.ActionType.RewardPointsIncrease);
                reportActions.Add((int)BVSoftware.Payment.ActionType.RewardPointsDecrease);

                IQueryable<Data.EF.ecommrc_OrderTransactions> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                        .Where(y => y.Timestamp >= startDateUtc && y.Timestamp <= endDateUtc)
                                        .Where(y => y.Success == true)
                                        .Where(y => y.Voided == false)
                                        .Where(y => reportActions.Contains(y.Action));

                totalCount = items.Count();

                var xlist = items.OrderBy(y => y.Timestamp).Skip(skip).Take(take);
                return ListPoco(xlist);                
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            return new List<OrderTransaction>();
        }

        public decimal FindBillableTransactionTotal(DateTime startDateUtc, DateTime endDateUtc, long storeId)
        {
            decimal result = 0;

            try
            {
                IQueryable<Data.EF.ecommrc_OrderTransactions> items = repository.Find().Where(y => y.StoreId == context.CurrentStore.Id)
                                        .Where(y => y.Timestamp >= startDateUtc && y.Timestamp <= endDateUtc);
                List<OrderTransaction> transactions = ListPoco(items);
                result = transactions.Sum(y => y.AmountAppliedToOrder);
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }
            return result;
        }

        public decimal FindTotalTransactionsForever()
        {
            decimal result = 0;

            try
            {
                List<long> actioncodes = new List<long>();
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CreditCardCapture);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CreditCardCharge);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CreditCardRefund);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CashReceived);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CashReturned);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CheckReceived);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CheckReturned);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.GiftCardCapture);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.GiftCardDecrease);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.GiftCardIncrease);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.PayPalCapture);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.PayPalCharge);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.PayPalRefund);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.PurchaseOrderAccepted);
                actioncodes.Add((int)BVSoftware.Payment.ActionType.CompanyAccountAccepted);

                var x = repository.Find()
                                    .Where(y => y.StoreId == context.CurrentStore.Id)
                                    .Where(y => y.Success == true)
                                    .Where(y => y.Voided == false)
                                    .Where(y => actioncodes.Contains(y.Action))
                                    .Select(y => y.Amount).Sum();

                if (x > 0) result = (decimal)x;
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
            }

            return result;
        }

        

    }
}
