
using BVSoftware.PaypalWebServices;
using com.paypal.sdk.services;
using com.paypal.soap.api;
using System.Web;
using System;
using System.Collections.ObjectModel;

namespace BVSoftware.Commerce.Payment.Method
{
    public class PaypalExpress : DisplayPaymentMethod
    {
        public static string Id() { return WebAppSettings.PaymentIdPaypalExpress; }
        public override string MethodId
        {
            get { return Id(); }
        }

        public override string MethodName
        {
            get { return "PayPal Express"; }
        }

        private Accounts.Store currentStore = RequestContext.GetCurrentRequestContext().CurrentStore;

        public bool Authorize(BVSoftware.Payment.Transaction t)
        {
            PayPalAPI ppAPI = Utilities.PaypalExpressUtilities.GetPaypalAPI();
            try
            {
                DoExpressCheckoutPaymentResponseType paymentResponse;
                if (t.PreviousTransactionNumber != null)
                {
                    paymentResponse = ppAPI.DoExpressCheckoutPayment(t.PreviousTransactionNumber,
                                   t.PreviousTransactionAuthCode,
                                    string.Format("{0:N}", t.Amount),
                                    PaymentActionCodeType.Order,
                                    PayPalAPI.GetCurrencyCodeType(currentStore.Settings.PayPal.Currency),
                                    t.MerchantInvoiceNumber + System.Guid.NewGuid().ToString());

                    if ((paymentResponse.Ack == AckCodeType.Success) || (paymentResponse.Ack == AckCodeType.SuccessWithWarning))
                    {
                        t.Result.Succeeded = true;
                        t.Result.ReferenceNumber = paymentResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.TransactionID;
                        t.Result.ResponseCode = "OK";
                        t.Result.ResponseCodeDescription = "PayPal Express Payment Authorized Successfully.";
                        return true;
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("PayPal Express Payment Authorization Failed.", "", BVSoftware.Payment.MessageType.Error));
                        foreach (ErrorType ppError in paymentResponse.Errors)
                        {
                            t.Result.Messages.Add(new BVSoftware.Payment.Message(ppError.LongMessage, ppError.ErrorCode, BVSoftware.Payment.MessageType.Error));
                        }
                        return false;
                    }
                }
            }
            finally
            {
                ppAPI = null;
            }

            return false;
        }

        public bool Capture(BVSoftware.Payment.Transaction t)
        {
            PayPalAPI ppAPI = Utilities.PaypalExpressUtilities.GetPaypalAPI();
            try
            {
                string OrderNumber = t.MerchantInvoiceNumber + System.Guid.NewGuid().ToString();

                DoCaptureResponseType captureResponse = ppAPI.DoCapture(t.PreviousTransactionNumber,
                                                                        "Thank you for your payment.",
                                                                        string.Format("{0:N}", t.Amount),
                                                                        PayPalAPI.GetCurrencyCodeType(currentStore.Settings.PayPal.Currency), 
                                                                        OrderNumber);
                
                if ((captureResponse.Ack == AckCodeType.Success) || (captureResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                    t.Result.ReferenceNumber = captureResponse.DoCaptureResponseDetails.PaymentInfo.TransactionID;

                    if (captureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.Pending)
                    {              
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "PENDING";
                        t.Result.ResponseCodeDescription = "PayPal Express Payment PENDING";
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment PENDING.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else if (captureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.InProgress)
                    {                  
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "PENDING";
                        t.Result.ResponseCodeDescription = "PayPal Express Payment IN PROGRESS";
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment PENDING. In Progress.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else if (captureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.None)
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "PENDING";
                        t.Result.ResponseCodeDescription = "PayPal Express Payment: No Status Yet";
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment PENDING. No Status Yet.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else if (captureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.Processed)
                    {                
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "PENDING";
                        t.Result.ResponseCodeDescription = "PayPal Express Payment PENDING";
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment PENDING.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else if (captureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.Completed)
                    {
                        t.Result.Succeeded = true;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("PayPal Express Payment Captured Successfully.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("An error occurred while trying to capture your PayPal payment.", "", BVSoftware.Payment.MessageType.Error));
                        return false;
                    }
                }
                else
                {
                    t.Result.Succeeded = false;
                    t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment Charge Failed.", "", BVSoftware.Payment.MessageType.Error));
                    foreach (ErrorType ppError in captureResponse.Errors)
                    {
                        t.Result.Messages.Add(new BVSoftware.Payment.Message(ppError.LongMessage, ppError.ErrorCode, BVSoftware.Payment.MessageType.Error));
                    }
                    return false;
                }


            }
            finally
            {
                ppAPI = null;
            }

        }
        public bool Charge(BVSoftware.Payment.Transaction t)
        {       
            PayPalAPI ppAPI = Utilities.PaypalExpressUtilities.GetPaypalAPI();
            try {
                string OrderNumber = t.MerchantInvoiceNumber + System.Guid.NewGuid().ToString();

                DoExpressCheckoutPaymentResponseType paymentResponse;
                //there was no authorization so we just need to do a direct sale
                paymentResponse = ppAPI.DoExpressCheckoutPayment(t.PreviousTransactionNumber,
                                                                t.PreviousTransactionAuthCode,
                                                                string.Format("{0:N}", t.Amount),
                                                                PaymentActionCodeType.Sale, 
                                                                PayPalAPI.GetCurrencyCodeType(currentStore.Settings.PayPal.Currency), 
                                                                OrderNumber);

                if ((paymentResponse.Ack == AckCodeType.Success) || (paymentResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                    t.Result.ReferenceNumber = paymentResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.TransactionID;

                    if (paymentResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.Completed)
                    {
                        t.Result.Succeeded = true;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("PayPal Express Payment Charged Successfully.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else if (paymentResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.PaymentStatus == PaymentStatusCodeType.Pending)
                    {
                        t.Result.Succeeded = true;
                        t.Result.ResponseCode = "PENDING";
                        t.Result.ResponseCodeDescription = "PayPal Express Payment PENDING";
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment PENDING.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("An error occurred while trying to charge your PayPal payment.", "", BVSoftware.Payment.MessageType.Error));
                        return false;
                    }
                }
                else
                {
                    t.Result.Succeeded = false;
                    t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment Charge Failed.", "", BVSoftware.Payment.MessageType.Error));
                    foreach (ErrorType ppError in paymentResponse.Errors)
                    {
                        t.Result.Messages.Add(new BVSoftware.Payment.Message(ppError.LongMessage, ppError.ErrorCode, BVSoftware.Payment.MessageType.Error));
                    }
                    return false;
                }

            }
            finally 
            {
                ppAPI = null;
            }

        }


        public bool Refund(BVSoftware.Payment.Transaction t)
        {
                PayPalAPI ppAPI = Utilities.PaypalExpressUtilities.GetPaypalAPI();
                try {
                    if (t.PreviousTransactionNumber != null)
                    {
                        string refundType = string.Empty;
                        //per paypal's request, the refund type should always be set to partial
                        refundType = "Partial";
                        RefundTransactionResponseType refundResponse
                            = ppAPI.RefundTransaction(t.PreviousTransactionNumber,
                                                        refundType, string.Format("{0:N}", t.Amount));
                        if ((refundResponse.Ack == AckCodeType.Success) || (refundResponse.Ack == AckCodeType.SuccessWithWarning))
                        {
                            t.Result.Succeeded = true;
                            t.Result.Messages.Add(new BVSoftware.Payment.Message("PayPal Express Payment Refunded Successfully.", "OK", BVSoftware.Payment.MessageType.Information));
                            return true;
                        }
                        else
                        {
                            t.Result.Succeeded = false;
                            t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment Refund Failed.", "", BVSoftware.Payment.MessageType.Error));
                            foreach (ErrorType ppError in refundResponse.Errors)
                            {
                                t.Result.Messages.Add(new BVSoftware.Payment.Message(ppError.LongMessage, ppError.ErrorCode, BVSoftware.Payment.MessageType.Error));
                            }
                            return false;
                       }
                    }
                }
                finally
                {
                    ppAPI = null;
                }

                return false;
        }

        public bool Void(BVSoftware.Payment.Transaction t)
        {
            PayPalAPI ppAPI = Utilities.PaypalExpressUtilities.GetPaypalAPI();
            try
            {
                if (t.PreviousTransactionNumber != null)
                {
                    DoVoidResponseType voidResponse = ppAPI.DoVoid(t.PreviousTransactionNumber,
                                                                    "Transaction Voided");
                    if ((voidResponse.Ack == AckCodeType.Success) || (voidResponse.Ack == AckCodeType.SuccessWithWarning))
                    {
                        t.Result.Succeeded = true;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("PayPal Express Payment Voided Successfully.", "OK", BVSoftware.Payment.MessageType.Information));
                        return true;
                    }
                    else
                    {
                        t.Result.Succeeded = false;
                        t.Result.Messages.Add(new BVSoftware.Payment.Message("Paypal Express Payment Void Failed.", "", BVSoftware.Payment.MessageType.Error));
                        foreach (ErrorType ppError in voidResponse.Errors)
                        {
                            t.Result.Messages.Add(new BVSoftware.Payment.Message(ppError.LongMessage, ppError.ErrorCode, BVSoftware.Payment.MessageType.Error));
                        }
                        return false;
                    }
                }
            }
            finally
            {
                ppAPI = null;
            }
            return false;
        }

    }
}
