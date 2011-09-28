
using System.Web;
using MerchantTribe.Commerce.Security;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class LocalFraudCheck : OrderTask
	{

		public override Task Clone()
		{
			return new LocalFraudCheck();
		}

		public override bool Execute(OrderTaskContext context)
		{
			bool result = true;

			if (context.Order != null) {
				Security.FraudCheckData d = new Security.FraudCheckData();
				PopulateFraudData(d, context);

                FraudScorer scorer = new FraudScorer(context.MTApp.CurrentRequestContext);

                context.Order.FraudScore = scorer.ScoreData(d);

				if (context.Order.FraudScore >= 5) {					
					Orders.OrderStatusCode s = Orders.OrderStatusCode.FindByBvin(Orders.OrderStatusCode.OnHold);
					context.Order.StatusCode = s.Bvin;
					context.Order.StatusName = s.StatusName;
                    context.MTApp.OrderServices.Orders.Update(context.Order);
				}

                if (d.Messages.Count > 0)
                {
                    Orders.OrderNote n = new Orders.OrderNote();
                    n.IsPublic = false;
                    n.Note = "Fraud Check Failed";
                    foreach (string m in d.Messages)
                    {
                        n.Note += " | " + m;
                    }                    
                    context.Order.Notes.Add(n);                    
                }

                context.MTApp.OrderServices.Orders.Update(context.Order);

			}

			return result;
		}

		private void PopulateFraudData(Security.FraudCheckData d, OrderTaskContext context)
		{
			if (HttpContext.Current != null) {
				if (HttpContext.Current.Request.UserHostAddress != null) {
					d.IpAddress = HttpContext.Current.Request.UserHostAddress;
				}
			}
			
            if (context.Order.UserEmail != string.Empty) {
				d.EmailAddress = context.Order.UserEmail;
				string[] parts = d.EmailAddress.Split('@');
				if (parts.Length > 1) {
					d.DomainName = parts[1];
				}
			}
			
            d.PhoneNumber = context.Order.BillingAddress.Phone;

            foreach (Orders.OrderTransaction p in context.MTApp.OrderServices.Transactions.FindForOrder(context.Order.bvin))
            {
                if (p.Action == MerchantTribe.Payment.ActionType.CreditCardInfo)
                {
					d.CreditCard = p.CreditCard.CardNumber;
                    break; 
				}				
			}
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "14D38D1A-8B26-4a8b-B143-8485B4E7A584";
		}

		public override string TaskName()
		{
			return "Local Fraud Check";
		}

	}
}

