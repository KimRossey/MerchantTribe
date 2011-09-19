using System;
using com.paypal.sdk.services;
using com.paypal.soap.api;

namespace MerchantTribe.PaypalWebServices
{
    /// <summary>
    /// Summary description for PayPalAPI.
    /// </summary>
    public class PayPalAPI
    {
        private readonly CallerServices caller;

        public static CurrencyCodeType GetCurrencyCodeType(String currencyName)
        {
            switch (currencyName)
            {
                case "USD":
                    return CurrencyCodeType.USD;
                case "GBP":
                    return CurrencyCodeType.GBP;
                case "TWD":
                    return CurrencyCodeType.TWD;
                case "SEK":
                    return CurrencyCodeType.SEK;
                case "SGD":
                    return CurrencyCodeType.SGD;
                case "EUR":
                    return CurrencyCodeType.EUR;
                case "CHR":
                    return CurrencyCodeType.CHF;
                case "AUD":
                    return CurrencyCodeType.AUD;
                case "HKD":
                    return CurrencyCodeType.HKD;
                case "CAD":
                    return CurrencyCodeType.CAD;
                case "INR":
                    return CurrencyCodeType.INR;
                default:
                    return CurrencyCodeType.USD;
            }

        }

        public static CountryCodeType GetCountryCode(String isoCode)
        {
            if (Enum.IsDefined(typeof(CountryCodeType), isoCode))
            {
                return (CountryCodeType)Enum.Parse(typeof(CountryCodeType), isoCode, true);
            }
            else
            {
                return CountryCodeType.US;
            }                      
        }

        public PayPalAPI(com.paypal.sdk.profiles.IAPIProfile PayPalProfile)
        {
            caller = new CallerServices();
            caller.APIProfile = PayPalProfile;
        }

        public TransactionSearchResponseType TransactionSearch(DateTime startDate, DateTime endDate)
        {
            // Create the request object
            TransactionSearchRequestType concreteRequest = new TransactionSearchRequestType();

            concreteRequest.StartDate = startDate.ToUniversalTime();
            concreteRequest.EndDate = endDate.AddDays(1).ToUniversalTime(); //end date inclusive
            concreteRequest.EndDateSpecified = true;
            return (TransactionSearchResponseType)caller.Call("TransactionSearch", concreteRequest);
        }

        public GetTransactionDetailsResponseType GetTransactionDetails(string trxID)
        {
            // Create the request object
            GetTransactionDetailsRequestType concreteRequest = new GetTransactionDetailsRequestType();

            concreteRequest.TransactionID = trxID;
            return (GetTransactionDetailsResponseType)caller.Call("GetTransactionDetails", concreteRequest);
        }

        public RefundTransactionResponseType RefundTransaction(string trxID, string refundType, string amount)
        {
            // Create the request object
            RefundTransactionRequestType concreteRequest = new RefundTransactionRequestType();
            concreteRequest.TransactionID = trxID;
                        
            concreteRequest.RefundType = RefundType.Partial;
            concreteRequest.RefundTypeSpecified = true;
            concreteRequest.Amount = new BasicAmountType();

            concreteRequest.Amount.currencyID = CurrencyCodeType.USD;
            concreteRequest.Amount.Value = amount;            

            return (RefundTransactionResponseType)caller.Call("RefundTransaction", concreteRequest);
        }

        public DoDirectPaymentResponseType DoDirectPayment(string paymentAmount, string buyerBillingLastName, string buyerBillingFirstName, string buyerShippingLastName, string buyerShippingFirstName, string buyerBillingAddress1, string buyerBillingAddress2, string buyerBillingCity, string buyerBillingState, string buyerBillingPostalCode, CountryCodeType buyerBillingCountryCode, string creditCardType, string creditCardNumber, string CVV2, int expMonth, int expYear, PaymentActionCodeType paymentAction, string ipAddress, string buyerShippingAddress1, string buyerShippingAddress2, string buyerShippingCity, string buyerShippingState, string buyerShippingPostalCode, CountryCodeType buyerShippingCountryCode, string invoiceId)
        {
            // Create the request object
            DoDirectPaymentRequestType pp_Request = new DoDirectPaymentRequestType();

            // Create the request details object
            pp_Request.DoDirectPaymentRequestDetails = new DoDirectPaymentRequestDetailsType();

            pp_Request.DoDirectPaymentRequestDetails.IPAddress = ipAddress;
            pp_Request.DoDirectPaymentRequestDetails.MerchantSessionId = "";
            pp_Request.DoDirectPaymentRequestDetails.PaymentAction = paymentAction;            
            pp_Request.DoDirectPaymentRequestDetails.CreditCard = new CreditCardDetailsType();
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardNumber = creditCardNumber;
            switch (creditCardType)
            {
                case "Visa":
                    pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.Visa;
                    break;
                case "MasterCard":
                    pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.MasterCard;
                    break;
                case "Discover":
                    pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.Discover;
                    break;
                case "Amex":
                    pp_Request.DoDirectPaymentRequestDetails.CreditCard.CreditCardType = CreditCardTypeType.Amex;
                    break;
            }
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CVV2 = CVV2;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpMonth = expMonth;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpYear = expYear;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpMonthSpecified = true;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.ExpYearSpecified = true;

            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner = new PayerInfoType();
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Payer = "";
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerID = "";
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerStatus = PayPalUserStatusCodeType.unverified;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerCountry = CountryCodeType.US;

            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address = new AddressType();
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.Street1 = buyerBillingAddress1;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.Street2 = buyerBillingAddress2;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.CityName = buyerBillingCity;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.StateOrProvince = buyerBillingState;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.PostalCode = buyerBillingPostalCode;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.Country = buyerBillingCountryCode;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.Address.CountrySpecified = true;

            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails = new PaymentDetailsType();
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress = new AddressType();
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.Name = buyerShippingFirstName + " " + buyerShippingLastName;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.Street1 = buyerShippingAddress1;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.Street2 = buyerShippingAddress2;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.CityName = buyerShippingCity;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.StateOrProvince = buyerShippingState;            
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.PostalCode = buyerShippingPostalCode;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.Country = buyerShippingCountryCode;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ShipToAddress.CountrySpecified = true;

            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerName = new PersonNameType();
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerName.FirstName = buyerBillingFirstName;
            pp_Request.DoDirectPaymentRequestDetails.CreditCard.CardOwner.PayerName.LastName = buyerBillingLastName;
            
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.OrderTotal = new BasicAmountType();
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.InvoiceID = invoiceId;
            
            // NOTE: The only currency supported by the Direct Payment API at this time is US dollars (USD).
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.OrderTotal.currencyID = CurrencyCodeType.USD;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.OrderTotal.Value = paymentAmount;
            pp_Request.DoDirectPaymentRequestDetails.PaymentDetails.ButtonSource = "BVCommerce_Cart_DP_US";
            return (DoDirectPaymentResponseType)caller.Call("DoDirectPayment", pp_Request);
        }


        public SetExpressCheckoutResponseType SetExpressCheckout(string paymentAmount, string returnURL, string cancelURL, PaymentActionCodeType paymentAction, CurrencyCodeType currencyCodeType, string invoiceId)
        {
            // Create the request object
            SetExpressCheckoutRequestType pp_request = new SetExpressCheckoutRequestType();

            // Create the request details object
            pp_request.SetExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType();
            pp_request.SetExpressCheckoutRequestDetails.PaymentAction = paymentAction;
            pp_request.SetExpressCheckoutRequestDetails.PaymentActionSpecified = true;
            pp_request.SetExpressCheckoutRequestDetails.InvoiceID = invoiceId;
            pp_request.SetExpressCheckoutRequestDetails.OrderTotal = new BasicAmountType();

            pp_request.SetExpressCheckoutRequestDetails.OrderTotal.currencyID = currencyCodeType;
            pp_request.SetExpressCheckoutRequestDetails.OrderTotal.Value = paymentAmount;            

            pp_request.SetExpressCheckoutRequestDetails.CancelURL = cancelURL;
            pp_request.SetExpressCheckoutRequestDetails.ReturnURL = returnURL;

            return (SetExpressCheckoutResponseType)caller.Call("SetExpressCheckout", pp_request);
        }

        public SetExpressCheckoutResponseType SetExpressCheckout(string paymentAmount, string returnURL, string cancelURL, PaymentActionCodeType paymentAction, CurrencyCodeType currencyCodeType, String name, String countryISOCode, String street1, String street2, String city, String region, String postalCode, String phone, string invoiceId)
        {
            // Create the request object
            SetExpressCheckoutRequestType pp_request = new SetExpressCheckoutRequestType();

            // Create the request details object
            pp_request.SetExpressCheckoutRequestDetails = new SetExpressCheckoutRequestDetailsType();
            pp_request.SetExpressCheckoutRequestDetails.PaymentAction = paymentAction;
            pp_request.SetExpressCheckoutRequestDetails.PaymentActionSpecified = true;

            pp_request.SetExpressCheckoutRequestDetails.AddressOverride = "1";
            pp_request.SetExpressCheckoutRequestDetails.InvoiceID = invoiceId;

            pp_request.SetExpressCheckoutRequestDetails.OrderTotal = new BasicAmountType();

            pp_request.SetExpressCheckoutRequestDetails.OrderTotal.currencyID = currencyCodeType;
            pp_request.SetExpressCheckoutRequestDetails.OrderTotal.Value = paymentAmount;

            pp_request.SetExpressCheckoutRequestDetails.CancelURL = cancelURL;
            pp_request.SetExpressCheckoutRequestDetails.ReturnURL = returnURL;
            
            pp_request.SetExpressCheckoutRequestDetails.Address = new AddressType();            
            pp_request.SetExpressCheckoutRequestDetails.Address.AddressStatusSpecified = false;
            pp_request.SetExpressCheckoutRequestDetails.Address.AddressOwnerSpecified = false;

            pp_request.SetExpressCheckoutRequestDetails.Address.Street1 = street1;
            pp_request.SetExpressCheckoutRequestDetails.Address.Street2 = street2;
            pp_request.SetExpressCheckoutRequestDetails.Address.CityName = city;
            pp_request.SetExpressCheckoutRequestDetails.Address.StateOrProvince = region;
            pp_request.SetExpressCheckoutRequestDetails.Address.PostalCode = postalCode;
            pp_request.SetExpressCheckoutRequestDetails.Address.CountrySpecified = true;
            pp_request.SetExpressCheckoutRequestDetails.Address.Country = GetCountryCode(countryISOCode);                        
            pp_request.SetExpressCheckoutRequestDetails.Address.Phone = phone;
            pp_request.SetExpressCheckoutRequestDetails.Address.Name = name;                        
            return (SetExpressCheckoutResponseType)caller.Call("SetExpressCheckout", pp_request);
        }


        public GetExpressCheckoutDetailsResponseType GetExpressCheckoutDetails(string token)
        {
            // Create the request object
            GetExpressCheckoutDetailsRequestType pp_request = new GetExpressCheckoutDetailsRequestType();

            pp_request.Token = token;

            return (GetExpressCheckoutDetailsResponseType)caller.Call("GetExpressCheckoutDetails", pp_request);
        }

        public DoExpressCheckoutPaymentResponseType DoExpressCheckoutPayment(string token, string payerID, string paymentAmount, PaymentActionCodeType paymentAction, CurrencyCodeType currencyCodeType, string invoiceId)
        {
            // Create the request object
            DoExpressCheckoutPaymentRequestType pp_request = new DoExpressCheckoutPaymentRequestType();

            // Create the request details object
            pp_request.DoExpressCheckoutPaymentRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
            pp_request.DoExpressCheckoutPaymentRequestDetails.Token = token;
            pp_request.DoExpressCheckoutPaymentRequestDetails.PayerID = payerID;
            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentAction = paymentAction;
            

            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails = new PaymentDetailsType();
            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.InvoiceID = invoiceId;
            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.OrderTotal = new BasicAmountType();

            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.OrderTotal.currencyID = currencyCodeType;
            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.OrderTotal.Value = paymentAmount;
            pp_request.DoExpressCheckoutPaymentRequestDetails.PaymentDetails.ButtonSource = "BVCommerce_Cart_EC_US";
            return (DoExpressCheckoutPaymentResponseType)caller.Call("DoExpressCheckoutPayment", pp_request);
        }

        public DoVoidResponseType DoVoid(string authorizationId, string note)
        {
            DoVoidRequestType pp_request = new DoVoidRequestType();
            pp_request.AuthorizationID = authorizationId;            
            pp_request.Note = note;
            return (DoVoidResponseType)caller.Call("DoVoid", pp_request);
        }

        public DoCaptureResponseType DoCapture(string authorizationId, string note, string value, CurrencyCodeType currencyId, string invoiceId)
        {
            DoCaptureRequestType pp_request = new DoCaptureRequestType();
            pp_request.AuthorizationID = authorizationId;
            pp_request.Note = note;
            pp_request.Amount = new BasicAmountType();
            pp_request.Amount.Value = value;
            pp_request.Amount.currencyID = currencyId;
            pp_request.InvoiceID = invoiceId;
            return (DoCaptureResponseType)caller.Call("DoCapture", pp_request);
        }
    }
}