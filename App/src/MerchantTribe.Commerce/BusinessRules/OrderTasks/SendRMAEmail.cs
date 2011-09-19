
//using System.Net.Mail;
//using System.Text.RegularExpressions;
//using System;

//namespace MerchantTribe.Commerce.BusinessRules.OrderTasks
//{	
//    public class SendRMAEmail : BusinessRules.OrderTask
//    {

//        public override bool Execute(OrderTaskContext context)
//        {
//            if (context.Inputs["rmaid"] != null) {
//                string rmaId = (string)context.Inputs["rmaid"].Value;
//                if (!string.IsNullOrEmpty(rmaId)) {
//                    Orders.RMA rma = Orders.RMA.FindByBvin(rmaId);
//                    if (rma != null) {
//                        if (rma.Bvin != string.Empty) {
//                            string templateBvin = WebAppSettings.RMANewEmailTemplate;
//                            if (templateBvin == string.Empty) {
//                                templateBvin = "c0cb9492-f4be-4bdb-9ae9-0b14c7bd2cd0";
//                            }
//                            string toEmail = context.CurrentRequest.CurrentStore.Settings.MailServer.EmailForNewOrder;
//                            try {
//                                if (toEmail.Trim().Length > 0) {
//                                    Content.EmailTemplate t = Content.EmailTemplate.FindByBvin(templateBvin);
//                                    System.Net.Mail.MailMessage m = new System.Net.Mail.MailMessage();
//                                    m = t.ConvertToMailMessage(t.From, toEmail, rma);
//                                    if (m != null) {
//                                        if (!Regex.IsMatch(m.Body, "\\[\\[.+\\]\\]")) {
//                                            if (!Utilities.MailServices.SendMail(m)) {
//                                                EventLog.LogEvent("New RMA Email", "New RMA Email failed to send.", Metrics.EventLogSeverity.Error);
//                                            }

//                                        }
//                                    }
//                                    else {
//                                        EventLog.LogEvent("New RMA Email", "Message was not created successfully.", Metrics.EventLogSeverity.Error);
//                                    }
//                                }
//                            }
//                            catch (Exception ex) {
//                                EventLog.LogEvent(ex);
//                            }
//                        }
//                    }
//                }
//            }

//            return true;
//        }

//        public override bool Rollback(OrderTaskContext context)
//        {
//            return true;
//        }

//        public override string TaskId()
//        {
//            return "e5a9b457-554e-4d31-9c0d-d01d5e0799a3";
//        }

//        public override string TaskName()
//        {
//            return "Send RMA Email";
//        }

//        public override string StepName()
//        {
//            string result = string.Empty;
//            if (result == string.Empty) {
//                result = this.TaskName();
//            }
//            return result;
//        }

//        public override Task Clone()
//        {
//            return new SendRMAEmail();
//        }

//    }
//}

