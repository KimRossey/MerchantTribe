using System;
using System.Web;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
	public class MakePlacedOrder : OrderTask
	{

		public override bool Execute(OrderTaskContext context)
		{
            if (context.Order.IsPlaced) return true;
            if (context.Order.Items.Count == 0)
            {
                context.Errors.Add(new WorkflowMessage("Order already placed.", Content.SiteTerms.GetTerm(Content.SiteTermIds.OrderAlreadyPlaced), true));
                return false;
            }

       

			context.Order.IsPlaced = true;
			context.Order.TimeOfOrderUtc = DateTime.UtcNow;            
			//if (!WebAppSettings.DisableInventory) {				
					List<string> errors = new List<string>();
					if (!context.MTApp.OrdersReserveInventoryForAllItems(context.Order,errors)) {
						foreach (string item in errors) {
							context.Errors.Add(new WorkflowMessage("Stock Too Low", item, true));
						}
						return false;
					}				
			//}

			if (System.Web.HttpContext.Current != null) {
				Orders.OrderNote note = new Orders.OrderNote();
                note.IsPublic = false;
				note.Note = "Customer IP: " + HttpContext.Current.Request.UserHostAddress;
				note.Note += "<br> Customer Host: " + HttpContext.Current.Request.UserHostName;
				note.Note += "<br> Browser: " + HttpContext.Current.Request.UserAgent;
				context.Order.Notes.Add(note);
			}

			Orders.OrderStatusCode c = Orders.OrderStatusCode.FindByBvin(Orders.OrderStatusCode.Received);
			if (c != null) {
				context.Order.StatusName = c.StatusName;
				context.Order.StatusCode = c.Bvin;
                context.Order.AffiliateID = context.MTApp.ContactServices.GetValidAffiliateId(context.MTApp).ToString();
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
            // No Rollback for this. Never unplace an order

            //context.MTApp.OrdersUnreserveInventoryForAllItems(context.Order);
            //context.Order.IsPlaced = false;
            //context.Order.StatusCode = string.Empty;
            //context.Order.StatusName = "Shopping Cart";
            //context.MTApp.OrderServices.Orders.Update(context.Order);
			return true;
		}

		public override string TaskId()
		{
			return "8F2BB6B4-2FEF-406d-A62D-075CD74D2551";
		}

		public override string TaskName()
		{
			return "Make Placed Order";
		}

		public override string StepName()
		{
			string result = string.Empty;
			if (result == string.Empty) {
				result = this.TaskName();
			}
			return result;
		}

		public override Task Clone()
		{
			return new MakePlacedOrder();
		}
	}

}
