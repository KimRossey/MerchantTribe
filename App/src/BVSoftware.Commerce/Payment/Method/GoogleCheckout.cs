
using BVSoftware.PaypalWebServices;
using com.paypal.sdk.services;
using com.paypal.soap.api;
using System.Web;
using GCheckout;
using System;
using System.Collections.ObjectModel;

namespace BVSoftware.Commerce.Payment.Method
{
	public class GoogleCheckout : DisplayPaymentMethod
	{
        public static string Id() { return WebAppSettings.PaymentIdGoogleCheckout; }
        public override string MethodId
        {
            get { return Id(); }
        }

        public override string MethodName
        {
            get { return "Google Checkout"; }
        }

        //private Accounts.Store currentStore = RequestContext.GetCurrentRequestContext().CurrentStore;


        //protected override bool ChildCharge(PaymentData data)
        //{
        //    Orders.Order order = data.StoreOrder;

        //    GCheckout.OrderProcessing.ChargeOrderRequest chargeRequest
        //        = new GCheckout.OrderProcessing.ChargeOrderRequest(currentStore.GoogleMerchantId,
        //                                                            currentStore.GoogleMerchantKey,
        //                                                            currentStore.GoogleMode, 
        //                                                            data.Payment().ThirdPartyOrderId);

        //    GCheckout.Util.GCheckoutResponse response = chargeRequest.Send();

        //    return ProcessResponse(response, data, "Charge");
        //}

    //    public override string FriendlyStatus(Orders.OrderPayment op)
    //    {
    //        if (op.AmountRefunded > 0 && op.AmountRefunded < op.AmountCharged) {
    //            return "Google Checkout payment has been partially refunded for " + op.AmountRefunded.ToString("c") + " of " + op.AmountCharged.ToString("c") + ".";
    //        }
    //        else if (op.AmountRefunded > 0 && op.AmountRefunded == op.AmountCharged) {
    //            return "Google Checkout payment has been fully refunded for " + op.AmountRefunded + ".";
    //        }
    //        else if (op.AmountCharged > 0) {
    //            return "Google Checkout payment has been charged for " + op.AmountCharged.ToString("c") + ".";
    //        }
    //        else if (op.AmountAuthorized > 0) {
    //            return "Google Checkout payment has been authorized for " + op.AmountAuthorized.ToString("c") + ".";
    //        }
    //        else {
    //            return "Google Checkout payment waiting for authorization or has been voided.";
    //        }
    //    }
		
    //    protected override bool ChildRefund(PaymentData data)
    //    {
    //        Orders.Order order = data.StoreOrder;

    //        GCheckout.OrderProcessing.RefundOrderRequest refundRequest
    //            = new GCheckout.OrderProcessing.RefundOrderRequest(currentStore.GoogleMerchantId,
    //                                                            currentStore.GoogleMerchantKey,
    //                                                            currentStore.GoogleMode, 
    //                                                            data.Payment().ThirdPartyOrderId, 
    //                                                            "",
    //                                                            currentStore.GoogleCurrency, 
    //                                                            data.Amount, "");

    //        GCheckout.Util.GCheckoutResponse response = refundRequest.Send();

    //        return ProcessResponse(response, data, "Refund");
    //    }

    //    public override bool RefundIsValid(Orders.OrderPayment op)
    //    {
    //        if ((op.AmountCharged > 0) && (op.AmountRefunded < op.AmountCharged)) {
    //            return true;
    //        }
    //        else {
    //            return false;
    //        }
    //    }

    //    protected override bool ChildVoid(PaymentData data)
    //    {
    //        Orders.Order order = data.StoreOrder;

    //        GCheckout.OrderProcessing.CancelOrderRequest voidRequest
    //            = new GCheckout.OrderProcessing.CancelOrderRequest(currentStore.GoogleMerchantId,
    //                                                        currentStore.GoogleMerchantKey,
    //                                                        currentStore.GoogleMode, 
    //                                                        data.Payment().ThirdPartyOrderId, "Test");

    //        GCheckout.Util.GCheckoutResponse response = voidRequest.Send();

    //        return ProcessResponse(response, data, "Void");
    //    }

    //    private bool ProcessResponse(GCheckout.Util.GCheckoutResponse response, PaymentData data, string task)
    //    {
    //        if (response != null) {
    //            if (currentStore.GoogleDebugMode)
    //            {
    //                if (response.ErrorMessage.Length > 0) {
    //                    EventLog.LogEvent("Google Checkout", response.ErrorMessage + " " + task + " Response Xml: " + response.ResponseXml, Metrics.EventLogSeverity.Information);
    //                }
    //                else {
    //                    EventLog.LogEvent("Google Checkout", task + " Response Xml: " + response.ResponseXml, Metrics.EventLogSeverity.Information);
    //                }
    //            }

    //            if (response.IsGood) {
    //                data.ResponseMessage = "Google Checkout " + task + " Attempt Sent.";
    //                data.ResponseMessageType = Content.DisplayMessageType.Success;
    //                return true;
    //            }
    //            else {
    //                data.ResponseMessage = "Google Checkout " + task + " Attempt Failed.";
    //                if (response.ErrorMessage.Length > 0) {
    //                    data.ResponseMessage = data.ResponseMessage + " " + response.ErrorMessage;
    //                }
    //                data.ResponseMessageType = Content.DisplayMessageType.Error;
    //                return false;
    //            }
    //        }
    //        else {
    //            data.ResponseMessage = "Google Checkout " + task + " Attempt Failed.";
    //            data.ResponseMessageType = Content.DisplayMessageType.Error;
    //            return false;
    //        }
    //    }

    //    public override bool VoidIsValid(Orders.OrderPayment op)
    //    {
    //        if ((op.AmountAuthorized > 0) && (op.AmountCharged <= 0)) {
    //            return true;
    //        }
    //        else {
    //            return false;
    //        }
    //    }

    
    
    }
}

