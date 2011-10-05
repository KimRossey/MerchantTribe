
namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	public class MakeOrderAddressUsersCurrentAddress : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
			if (context.UserId != string.Empty) {
				Membership.CustomerAccount user = context.MTApp.MembershipServices.Customers.Find(context.UserId);
                if (user == null) return true;

                context.Order.ShippingAddress.CopyTo(user.ShippingAddress);
                if (!context.Order.BillingAddress.IsEqualTo(context.Order.ShippingAddress))
                {
                    context.Order.BillingAddress.CopyTo(user.BillingAddress);
                }
				context.MTApp.MembershipServices.UpdateCustomer(user);
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "7cf8a5b6-3999-41b0-b283-e8280d19f3bb";
		}

		public override string TaskName()
		{
			return "Make Order Address User's Current Address";
		}

		public override string StepName()
		{
			string result = string.Empty;
            result = "Make Order Address User's Current Address";
			return result;
		}

		public override Task Clone()
		{
			return new MakeOrderAddressUsersCurrentAddress();
		}
	}

}

