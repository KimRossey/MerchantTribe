
using System.Collections.ObjectModel;
namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
{	
	public class IssueGiftCertificates : OrderTask
	{

		public override Task Clone()
		{
			return new IssueGiftCertificates();
		}

		public override bool Execute(OrderTaskContext context)
		{
			if (context.Order != null) {
				foreach (Orders.LineItem item in context.Order.Items) {
                    //if ((item.AssociatedProduct.IsGiftCertificate)) {
                    //    Collection<Catalog.GiftCertificate> gcsAlreadyIssued = Catalog.GiftCertificate.FindByLineItem(item.Bvin);
                    //    int gcsToIssue = 0;
                    //    if (gcsAlreadyIssued.Count < item.Quantity) {
                    //        gcsToIssue = (int)item.Quantity - gcsAlreadyIssued.Count;
                    //    }
                    //    for (int i = 0; i <= ((int)gcsToIssue - 1); i++) {
                    //        Catalog.GiftCertificate gc = new Catalog.GiftCertificate();
                    //        gc.AssociatedProductId = item.ProductId;
                    //        gc.OriginalAmount = item.AdjustedPrice;
                    //        gc.LineItemId = item.Bvin;
                    //        if (!Catalog.GiftCertificate.Insert(gc)) {
                    //            WorkflowMessage message = new WorkflowMessage("Gift Certificate Insert Failed", "Gift Certificate for line item " + item.Bvin + " in order " + context.Order.OrderNumber + " failed.", false);
                    //            context.Errors.Add(message);
                    //        }
                    //        else {
                    //            if (context.Order.UserEmail != string.Empty) {
                    //                Content.EmailTemplate template = Content.EmailTemplate.FindByBvin("34f5dffd-03ab-4bc9-b305-cd15020045ca");
                    //                System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
                    //                if (gc != null) {
                    //                    m = template.ConvertToMailMessage(template.From, gc, context.Order.UserEmail);
                    //                }

                    //                if (m != null) {
                    //                    Utilities.MailServices.SendMail(m);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
				}
			}
			return true;
		}

		public override bool Rollback(OrderTaskContext context)
		{
			return true;
		}

		public override string TaskId()
		{
			return "203bf29e-52e4-468a-8899-0498f1f48886";
		}

		public override string TaskName()
		{
			return "Issue Gift Certificates";
		}
	}
}
