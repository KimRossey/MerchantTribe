using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MerchantTribe.PaypalWebServices;
using com.paypal.sdk.services;
using com.paypal.soap.api;
namespace MerchantTribe.Payment.Methods
{
    public class PayPalPaymentsPro: Method
    {
        private const string versionNumber = "4.3.1";

        public override string Name
        {
            get { return "PayPal Payments Pro"; }
        }

        public override string Id
        {
            get { return "0B81046B-7A24-4512-8A6B-6C4C59D4C503"; }
        }

        public PayPalPaymentsProSettings Settings { get; set; }

        public override MethodSettings BaseSettings
        {
            get { return Settings; }
        }

        public PayPalPaymentsPro()
        {
            Settings = new PayPalPaymentsProSettings();
        }

        public override void ProcessTransaction(Transaction t)
        {
            switch (t.Action)
            {
                case ActionType.CreditCardHold:
                    this.Authorize(t);
                    break;
                case ActionType.CreditCardCharge:
                    this.Charge(t);
                    break;
                case ActionType.CreditCardCapture:
                    this.Capture(t);
                    break;
                case ActionType.CreditCardRefund:
                    this.Refund(t);
                    break;
                case ActionType.CreditCardVoid:
                    this.Void(t);
                    break;
            }
        }

        private void Authorize(Transaction t)
        {
            PayPalAPI ppAPI = this.GetPaypalAPI();
            
            try
            {
                string OrderNumber = string.Empty;
                                
                OrderNumber = t.MerchantInvoiceNumber;
                // Solves Duplicate Order Number Problem
                OrderNumber = OrderNumber + System.Guid.NewGuid().ToString();

                //        Dim cardType As Core.Payment.CreditCardType = Core.Payment.CreditCardType.FindByCode(data.Payment.CreditCardType)
                
                
                DoDirectPaymentResponseType authResponse = ppAPI.DoDirectPayment(String.Format("{0:N}", t.Amount),
                    t.Customer.LastName,
                    t.Customer.FirstName,
                    t.Customer.ShipLastName,
                    t.Customer.ShipFirstName,
                    t.Customer.Street,
                    "",
                    t.Customer.City,
                    t.Customer.Region,
                    t.Customer.PostalCode,
                    this.ConvertCountryName(t.Customer.Country),
                    t.Card.CardTypeName,
                    t.Card.CardNumber,
                    t.Card.SecurityCode,
                    t.Card.ExpirationMonth,
                    t.Card.ExpirationYear,
                    PaymentActionCodeType.Authorization,
                    t.Customer.IpAddress,
                    t.Customer.ShipStreet,
                    "",
                    t.Customer.ShipCity,
                    t.Customer.ShipRegion,
                    t.Customer.ShipPostalCode,
                    this.ConvertCountryName(t.Customer.ShipCountry),
                    OrderNumber);

                if (Settings.DebugMode)
                {
                    StringBuilder posted = new StringBuilder();
                    posted.Append("Amount=" + String.Format("{0:N}", t.Amount));
                    posted.Append(", LastName=" + t.Customer.LastName);
                    posted.Append(", FirstName=" + t.Customer.FirstName);
                    posted.Append(", ShipLastName=" + t.Customer.ShipLastName);
                    posted.Append(", ShipFirstName=" + t.Customer.ShipFirstName);
                    posted.Append(", Street=" + t.Customer.Street);
                    posted.Append(", City=" + t.Customer.Street);
                    posted.Append(", Region=" + t.Customer.Region);
                    posted.Append(", PostalCode=" + t.Customer.PostalCode);
                    posted.Append(", CountryName=" + this.ConvertCountryName(t.Customer.Country));
                    posted.Append(", CardTypeName=" + t.Card.CardTypeName);
                    posted.Append(", CardNumber=**********" + t.Card.CardNumberLast4Digits);
                    posted.Append(", CardExpMonth=" + t.Card.ExpirationMonth);
                    posted.Append(", CardExpYear=" + t.Card.ExpirationYear);
                    posted.Append(", OrderNumber=" + OrderNumber);

                    t.Result.Messages.Add(new Message(posted.ToString(), "DEBUG", MessageType.Information));                    
                }

                if ((authResponse.Ack == AckCodeType.Success) || (authResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                                t.Result.ReferenceNumber= authResponse.TransactionID;                                
                                t.Result.Succeeded = true;
                }
                else
                {
                                t.Result.Messages.Add(new Message("Paypal Payment Authorization Failed.", "", MessageType.Warning));
                                foreach (ErrorType ppError in authResponse.Errors)
                                {
                                    t.Result.Messages.Add(new Message(ppError.LongMessage, ppError.ErrorCode, MessageType.Error));
                                }
                                t.Result.Succeeded = false;
                }
            }
            finally
            {
                ppAPI = null;
            }    
        }

        private void Charge(Transaction t)
        {
            PayPalAPI ppAPI = this.GetPaypalAPI();

            try
            {
                string OrderNumber = string.Empty;

                OrderNumber = t.MerchantInvoiceNumber;
                // Solves Duplicate Order Number Problem
                OrderNumber = OrderNumber + System.Guid.NewGuid().ToString();

                DoDirectPaymentResponseType chargeResponse = ppAPI.DoDirectPayment(String.Format("{0:N}", t.Amount),
                    t.Customer.LastName,
                    t.Customer.FirstName,
                    t.Customer.ShipLastName,
                    t.Customer.ShipFirstName,
                    t.Customer.Street,
                    "",
                    t.Customer.City,
                    t.Customer.Region,
                    t.Customer.PostalCode,
                    this.ConvertCountryName(t.Customer.Country),
                    t.Card.CardTypeName,
                    t.Card.CardNumber,
                    t.Card.SecurityCode,
                    t.Card.ExpirationMonth,
                    t.Card.ExpirationYear,
                    PaymentActionCodeType.Sale,
                    t.Customer.IpAddress,
                    t.Customer.ShipStreet,
                    "",
                    t.Customer.ShipCity,
                    t.Customer.ShipRegion,
                    t.Customer.ShipPostalCode,
                    this.ConvertCountryName(t.Customer.ShipCountry),
                    OrderNumber);



                if ((chargeResponse.Ack == AckCodeType.Success) || (chargeResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                    t.Result.ReferenceNumber = chargeResponse.TransactionID;
                    t.Result.Succeeded = true;
                }
                else
                {
                    t.Result.Messages.Add(new Message("Paypal Charge Failed.", "", MessageType.Warning));
                    foreach (ErrorType ppError in chargeResponse.Errors)
                    {
                        t.Result.Messages.Add(new Message(ppError.LongMessage, ppError.ErrorCode, MessageType.Error));
                    }
                    t.Result.Succeeded = false;
                }
            }
            finally
            {
                ppAPI = null;
            }    
        }

        private void Capture(Transaction t)
        {
            PayPalAPI ppAPI = this.GetPaypalAPI();

            try
            {
                string OrderNumber = string.Empty;

                OrderNumber = t.MerchantInvoiceNumber;

                DoCaptureResponseType captureResponse = ppAPI.DoCapture(t.PreviousTransactionNumber, "Thank you for your payment.", String.Format("{0:N}", t.Amount), CurrencyCodeType.USD, OrderNumber);

                if ((captureResponse.Ack == AckCodeType.Success) || (captureResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                    t.Result.ReferenceNumber = captureResponse.DoCaptureResponseDetails.PaymentInfo.TransactionID;
                    t.Result.Succeeded = true;

                    switch(captureResponse.DoCaptureResponseDetails.PaymentInfo.PaymentStatus)
                    {
                        case PaymentStatusCodeType.Pending:
                            t.Result.Messages.Add(new Message("PayPal Pro Payment Pending", "", MessageType.Information));
                            t.Result.Succeeded = true;
                            break;
                        case PaymentStatusCodeType.InProgress:
                            t.Result.Messages.Add(new Message("PayPal Pro Payment In Progress", "", MessageType.Information));
                            t.Result.Succeeded = true;
                            break;
                        case PaymentStatusCodeType.None:
                            t.Result.Messages.Add(new Message("PayPal Pro Payment No Status Yet", "", MessageType.Information));
                            t.Result.Succeeded = true;
                            break;
                        case PaymentStatusCodeType.Processed:
                            t.Result.Messages.Add(new Message("PayPal Pro Charged Successfully", "", MessageType.Information));
                            t.Result.Succeeded = true;
                            break;
                        case PaymentStatusCodeType.Completed:
                            t.Result.Messages.Add(new Message("PayPal Pro Charged Successfully", "", MessageType.Information));
                            t.Result.Succeeded = true;
                            break;
                        default:
                            t.Result.Messages.Add(new Message("An error occured while attempting to capture this PayPal Payments Pro payment", "", MessageType.Information));
                            t.Result.Succeeded = false;
                            break;
                    }
                }
                else
                {
                    t.Result.Messages.Add(new Message("Paypal Payment Capture Failed.", "", MessageType.Warning));
                    foreach (ErrorType ppError in captureResponse.Errors)
                    {
                        t.Result.Messages.Add(new Message(ppError.LongMessage, ppError.ErrorCode, MessageType.Error));
                    }
                    t.Result.Succeeded = false;
                }
            }
            finally
            {
                ppAPI = null;
            }    
        }

        private void Refund(Transaction t)
        {
            PayPalAPI ppAPI = this.GetPaypalAPI();

            try
            {
                //per paypal's request, the refund type should always be set to partial
                string refundType = "Partial";
                RefundTransactionResponseType refundResponse = ppAPI.RefundTransaction(
                    t.PreviousTransactionNumber, refundType, String.Format("{0:N}", t.Amount));

                if ((refundResponse.Ack == AckCodeType.Success) || (refundResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                    t.Result.ReferenceNumber = refundResponse.RefundTransactionID;
                    t.Result.Succeeded = true;
                }
                else
                {
                    t.Result.Messages.Add(new Message("Paypal Payment Refund Failed.", "", MessageType.Warning));
                    foreach (ErrorType ppError in refundResponse.Errors)
                    {
                        t.Result.Messages.Add(new Message(ppError.LongMessage, ppError.ErrorCode, MessageType.Error));
                    }
                    t.Result.Succeeded = false;
                }
            }
            finally
            {
                ppAPI = null;
            }    
        }

        private void Void(Transaction t)
        {
            PayPalAPI ppAPI = this.GetPaypalAPI();

            try
            {
                DoVoidResponseType voidResponse = ppAPI.DoVoid(t.PreviousTransactionNumber, "Transaction Voided");

                if ((voidResponse.Ack == AckCodeType.Success) || (voidResponse.Ack == AckCodeType.SuccessWithWarning))
                {
                    t.Result.Succeeded = true;
                }
                else
                {
                    t.Result.Messages.Add(new Message("Paypal Payment Void Failed.", "", MessageType.Warning));
                    foreach (ErrorType ppError in voidResponse.Errors)
                    {
                        t.Result.Messages.Add(new Message(ppError.LongMessage, ppError.ErrorCode, MessageType.Error));
                    }
                    t.Result.Succeeded = false;
                }
            }
            finally
            {
                ppAPI = null;
            }    
        }


        private PayPalAPI GetPaypalAPI()
        {                        
            com.paypal.sdk.profiles.IAPIProfile APIProfile = CreateAPIProfile(Settings.PayPalUserName,
                                                                              Settings.PayPalPassword,
                                                                              Settings.PayPalSignature);
            return new PayPalAPI(APIProfile);
        }

        private com.paypal.sdk.profiles.IAPIProfile CreateAPIProfile(string PayPalUserName, string PayPalPassword, string PayPalSignature)
        {
            com.paypal.sdk.profiles.IAPIProfile profile = com.paypal.sdk.profiles.ProfileFactory.createSignatureAPIProfile();
            if (profile != null)
            {
                profile.APIUsername = PayPalUserName;
                profile.APIPassword = PayPalPassword;
                profile.Subject = string.Empty;
                profile.Environment = Settings.PayPalMode;
                profile.APISignature = PayPalSignature;
            }
            else
            {
                throw new ArgumentException("Paypal com.paypal.sdk.profiles.ProfileFactory.CreateAPIProfile has failed.");
            }
            return profile;
        }        

        com.paypal.soap.api.CountryCodeType ConvertCountryName(string name)
        {
            com.paypal.soap.api.CountryCodeType result = CountryCodeType.US;

            MerchantTribe.Web.Geography.Country country = MerchantTribe.Web.Geography.Country.FindByName(name);
            if (country == null) return result;

            
            if (Enum.IsDefined(typeof(com.paypal.soap.api.CountryCodeType), country.IsoCode))
            {
                result = (com.paypal.soap.api.CountryCodeType)Enum.Parse(typeof(com.paypal.soap.api.CountryCodeType), country.IsoCode, true);
            }

            return result;
        }                                

    }
}
