using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;

namespace MerchantTribeStore.Models
{
    public class CheckoutPaymentViewModel
    {
        public bool NoPaymentNeeded { get; set; }        
        public bool IsCreditCardActive { get; set; }        
        public bool IsPayPalActive { get; set; }        
        public bool IsTelephoneActive { get; set; }
        public bool IsPurchaseOrderActive { get; set; }
        public bool IsCheckActive { get; set; }
        public bool IsCodActive { get; set; }
        public bool IsCompanyAccountActive { get; set; }
        public Order CurrentOrder { get; set; }
        public string NoPaymentNeededDescription { get; set; }
        public string CreditCardDescription { get; set; }
        public string PayPalDescritpion { get; set; }
        public string TelephoneDescription { get; set; }
        public string PurchaseOrderDescription { get; set; }
        public string CheckDescription { get; set; }
        public string CodDescription { get; set; }
        public string CompanyAccountDescription { get; set; }
        public List<MerchantTribe.Web.Validation.RuleViolation> Violations { get; set; }

        public CheckoutPaymentViewModel()
        {
            this.NoPaymentNeeded = false;
            this.IsCreditCardActive = false;
            this.IsPayPalActive = false;
            this.IsTelephoneActive = false;
            this.IsPurchaseOrderActive = false;
            this.IsCheckActive = false;
            this.IsCodActive = false;
            this.IsCompanyAccountActive = false;
            this.CurrentOrder = new Order();
            this.NoPaymentNeededDescription = string.Empty;
            this.CreditCardDescription = string.Empty;
            this.PayPalDescritpion = string.Empty;
            this.TelephoneDescription = string.Empty;
            this.PurchaseOrderDescription = string.Empty;
            this.CheckDescription = string.Empty;
            this.CodDescription = string.Empty;
            this.CompanyAccountDescription = string.Empty;
            this.Violations = new List<MerchantTribe.Web.Validation.RuleViolation>();
        }
        
    }
}