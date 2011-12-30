using System;
using System.Web;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	public class RequireLoginBeforeCheckout : OrderTask
	{

		public override Task Clone()
		{
			return new RequireLoginBeforeCheckout();
		}

		public override bool Execute(OrderTaskContext context)
		{
			try {
				if (context.UserId.Trim() == string.Empty) {
                    if (context.MTApp.CurrentRequestContext.RoutingContext.HttpContext != null)
                    {
                        context.MTApp.CurrentRequestContext.RoutingContext.HttpContext.Response.Redirect("~/login.aspx?ReturnTo=Checkout");
					}
				}
			}
			catch (Exception ex){
                EventLog.LogEvent(ex);
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "141375d5-6dcd-4ab1-8f2f-515b2d5c5778";
		}

		public override string TaskName()
		{
			return "Require Login Before Checkout";
		}
	}
}
