using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Logging;

namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{
    public class AssignOrderNumber : BusinessRules.OrderTask
    {

        public override bool Execute(OrderTaskContext context)
        {
            // Assign Order Number
            if (context.Order.OrderNumber == string.Empty)
            {
                context.Order.OrderNumber = context.MTApp.OrderServices.GenerateNewOrderNumber(context.MTApp.CurrentRequestContext.CurrentStore.Id).ToString();

                Orders.OrderNote note = new Orders.OrderNote();
                note.IsPublic = false;
                note.Note = "This order was assigned number " + context.Order.OrderNumber;
                context.Order.Notes.Add(note);                
            }
            return true;
        }

        public override bool Rollback(OrderTaskContext context)
        {
            EventLog.LogEvent("Order Workflow",
                              "Order number " + context.Order.OrderNumber + " was assigned but the order was not completed. The cart ID is " + context.Order.bvin,
                               EventLogSeverity.Information);            
            return true;
        }

        public override string TaskId()
        {
            return "7DA816D7-CC81-4727-8788-0CE911F4A93E";
        }

        public override string TaskName()
        {
            return "Assign Order Number";
        }

        public override string StepName()
        {
            string result = string.Empty;
            result = "Assign Order Number";
            if (result == string.Empty)
            {
                result = this.TaskName();
            }
            return result;
        }

        public override Task Clone()
        {
            return new AssignOrderNumber();
        }

    }
}

