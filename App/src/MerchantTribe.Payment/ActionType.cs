using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public enum ActionType
    {
        
        Uknown = 0,
        CreditCardInfo = 1, // A transaction that does nothing but store CC information on the server
        CreditCardHold = 100, // Holds funds as an authorization against future capture
        CreditCardCapture = 101, // Captured funds previously held/authorized
        CreditCardCharge = 102, // A hold + capture or charge in a single step
        CreditCardRefund = 103, // Refunds money to the CC
        CreditCardVoid = 104, // Voids a previous transaction (usually only works before batch is settled)

        CheckReceived = 201, // Receive Payment as a Check
        CheckReturned = 202, // Send Payment as a Check

        CashReceived = 301, // Receive a Payment as Cash
        CashReturned = 302, // Return Cash

        PurchaseOrderInfo = 401, // Purchase Order Number Info Stored
        PurchaseOrderAccepted = 402, // Consider PO as Valid Payment        
        
        CompanyAccountInfo = 450, // Company Account Number Saved
        CompanyAccountAccepted = 451, // Company Account Number Accecpted as Payment

        GiftCardInfo = 501, // Record Gift Card Information
        GiftCardHold = 502, // Hold a Specific Amount on a Gift Card
        GiftCardCapture = 503, // Capture a Held Amount on a Gift Card
        GiftCardDecrease = 504, // Reduce Value of Gift Card
        GiftCardIncrease = 505, // Increase Value of Gift Card
        GiftCardActivate = 506, // Activate a Gift Card Number
        GiftCardBalanceInquiry = 507, // Find the current balance of a gift card

        RewardPointsInfo = 551, // Record Reward Points Information
        RewardPointsHold = 552, // Hold a Specific Amount of Reward Points
        RewardPointsCapture = 553, // Capture a Held Amount of Reward Points
        RewardPointsDecrease = 554, // Reduce Points Available
        RewardPointsIncrease = 555, // Increase Points Available
        RewardPointsBalanceInquiry = 557, // Find the current balance of points available
        RewardPointsUnHold = 558, // Hold a Specific Amount of Reward Points

        PayPalHold = 601, // Funds Held at PayPal
        PayPalCapture = 602, // Capture previously held Funds
        PayPalCharge = 603, // Charge a PayPal Account
        PayPalRefund = 604, // Send Money to a PayPal Account
        PayPalVoid = 605, // Void a Pending Transaction
        PayPalExpressCheckoutInfo = 606, // Customer Requests PayPal Express Checkout
        
        OfflinePaymentRequest = 9999 // Records customer request to pay offline
    }

}
