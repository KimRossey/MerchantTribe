using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.Payment
{
    [Serializable()]
    public class AvailablePayments
    {
        public AvailablePayments()
        {
            Populate();
        }

        private void Populate()
        {
            _Methods.Add(new Payment.Method.CreditCard());
            _Methods.Add(new Payment.Method.PurchaseOrder());
            _Methods.Add(new Payment.Method.CompanyAccount());
            _Methods.Add(new Payment.Method.Check());
            //_Methods.Add(new Payment.Method.Cash());
            _Methods.Add(new Payment.Method.Telephone());
            _Methods.Add(new Payment.Method.CashOnDelivery());
            //_Methods.Add(new Payment.Method.GiftCertificate());
            _Methods.Add(new Payment.Method.PaypalExpress());
        }
        private Collection<DisplayPaymentMethod> _Methods = new Collection<DisplayPaymentMethod>();

        public Collection<DisplayPaymentMethod> Methods
        {
            get { return _Methods; }
        }

        public Collection<DisplayPaymentMethod> AvailableMethodsForPlan(int planId)
        {
            Collection<DisplayPaymentMethod> result = new Collection<DisplayPaymentMethod>();

            result.Add(new Payment.Method.CreditCard());
                        
            switch (planId)
            {
                case 2: // plus
                case 3: // premium
                case 99: // max
                    result.Add(new Payment.Method.PurchaseOrder());
                    result.Add(new Payment.Method.CompanyAccount());
                    result.Add(new Payment.Method.Check());
                    result.Add(new Payment.Method.Telephone());
                    result.Add(new Payment.Method.CashOnDelivery());                    
                    break;
            }

            result.Add(new Payment.Method.PaypalExpress());

            return result;
        }
        public Collection<DisplayPaymentMethod> EnabledMethods(Accounts.Store currentStore)
        {
            Collection<DisplayPaymentMethod> result = new Collection<DisplayPaymentMethod>();

            Dictionary<string, string> enabledList = currentStore.Settings.PaymentMethodsEnabled;
            if (enabledList != null)
            {
                for (int i = 0; i <= Methods.Count - 1; i++)
                {
                    if (enabledList.ContainsKey(Methods[i].MethodId))
                    {
                        result.Add(Methods[i]);
                    }
                }
            }

            return result;
        }      

    }

}
