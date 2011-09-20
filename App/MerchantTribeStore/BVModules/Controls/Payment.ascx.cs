using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using MerchantTribe.Web.Validation;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_Payment : MerchantTribe.Commerce.Content.BVModule, MerchantTribe.Web.Validation.IValidatable
    {

        private int _tabIndex = -1;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        public void LoadPaymentMethods(decimal orderCost)
        {
            MerchantTribe.Commerce.Payment.AvailablePayments availablePayments = new MerchantTribe.Commerce.Payment.AvailablePayments();
            Collection<DisplayPaymentMethod> enabledMethods;
            enabledMethods = availablePayments.EnabledMethods(MyPage.MTApp.CurrentStore);

            if ((orderCost > 0) || (!MyPage.MTApp.CurrentStore.Settings.AllowZeroDollarOrders))
            {
                this.rowNoPaymentNeeded.Visible = false;
                foreach (DisplayPaymentMethod m in enabledMethods)
                {
                    switch (m.MethodId)
                    {
                        case WebAppSettings.PaymentIdCheck:
                            this.lblCheckDescription.Text = WebAppSettings.PaymentCheckDescription;
                            this.rowCheck.Visible = true;
                            break;
                        case WebAppSettings.PaymentIdCreditCard:
                            this.rowCreditCard.Visible = true;
                            break;
                        case WebAppSettings.PaymentIdTelephone:
                            this.lblTelephoneDescription.Text = WebAppSettings.PaymentTelephoneDescription;
                            this.rowTelephone.Visible = true;
                            break;
                        case WebAppSettings.PaymentIdPurchaseOrder:
                            this.lblPurchaseOrderDescription.Text = WebAppSettings.PaymentPurchaseOrderName;
                            this.trPurchaseOrder.Visible = true;
                            break;
                        case WebAppSettings.PaymentIdCompanyAccount:
                            this.lblCompanyAccountDescription.Text = WebAppSettings.PaymentCompanyAccountName;
                            this.trCompanyAccount.Visible = true;
                            break;
                        case WebAppSettings.PaymentIdCashOnDelivery:
                            this.lblCOD.Text = WebAppSettings.PaymentCODDescription;
                            this.trCOD.Visible = true;
                            break;
                        case WebAppSettings.PaymentIdPaypalExpress:
                            this.trPaypal.Visible = true;
                            break;
                        default:
                            // do nothing
                            break;
                    }
                }

                if (enabledMethods.Count == 1)
                {
                    switch (enabledMethods[0].MethodId)
                    {
                        case WebAppSettings.PaymentIdCheck:
                            rbCheck.Checked = true;
                            break;
                        case WebAppSettings.PaymentIdCreditCard:
                            rbCreditCard.Checked = true;
                            break;
                        case WebAppSettings.PaymentIdTelephone:
                            rbTelephone.Checked = true;
                            break;
                        case WebAppSettings.PaymentIdPurchaseOrder:
                            rbPurchaseOrder.Checked = true;
                            break;
                        case WebAppSettings.PaymentIdCompanyAccount:
                            rbCompanyAccount.Checked = true;
                            break;
                        case WebAppSettings.PaymentIdCashOnDelivery:
                            rbCOD.Checked = true;
                            break;
                        case WebAppSettings.PaymentIdPaypalExpress:
                            rbPaypal.Checked = true;
                            break;
                    }
                }
                else
                {
                    if (rbCreditCard.Visible)
                    {
                        rbCreditCard.Checked = true;
                    }
                }
            }
            else
            {
                rbNoPayment.Checked = true;
                rbNoPayment.Text = WebAppSettings.PaymentNoPaymentNeededDescription;
                foreach (Control item in this.Controls)
                {
                    if (item is HtmlTableRow)
                    {
                        if (object.ReferenceEquals(item, rowNoPaymentNeeded))
                        {
                            item.Visible = true;
                        }
                        else
                        {
                            item.Visible = false;
                        }
                    }
                }
            }
        }

        public void SetPaymentMethod(string methodID)
        {
            switch (methodID)
            {
                case WebAppSettings.PaymentIdCheck:
                    this.rbCheck.Checked = true;
                    break;
                case WebAppSettings.PaymentIdCreditCard:
                    this.rbCreditCard.Checked = true;
                    break;
                case WebAppSettings.PaymentIdTelephone:
                    this.rbTelephone.Checked = true;
                    break;
                case WebAppSettings.PaymentIdPurchaseOrder:
                    this.rbPurchaseOrder.Checked = true;
                    break;
                case WebAppSettings.PaymentIdCashOnDelivery:
                    this.rbCOD.Checked = true;
                    break;
                case WebAppSettings.PaymentIdPaypalExpress:
                    this.rbPaypal.Checked = true;
                    break;
                case WebAppSettings.PaymentIdCompanyAccount:
                    this.rbCompanyAccount.Checked = true;
                    break;
                default:
                    // do nothing
                    break;
            }
        }

        public bool PayPalSelected
        {
            get { return rbPaypal.Checked; }
        }

        public void SavePaymentInfo(Order o)
        {
            OrderPaymentManager payManager = new OrderPaymentManager(o, MyPage.MTApp);

            payManager.ClearAllNonStoreCreditTransactions();

            bool found = false;            

            if (this.rbCreditCard.Checked == true)
            {
                found = true;
                payManager.CreditCardAddInfo(this.CreditCardInput1.GetCardData(), o.TotalGrandAfterStoreCredits(MyPage.MTApp.OrderServices));
            }

            if ((found == false) && (this.rbCheck.Checked == true))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(o.TotalGrandAfterStoreCredits(MyPage.MTApp.OrderServices), "Customer will pay by check.");
            }

            if ((found == false) && (this.rbTelephone.Checked == true))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(o.TotalGrandAfterStoreCredits(MyPage.MTApp.OrderServices), "Customer will call with payment info.");
            }

            if ((found == false) && (this.rbPurchaseOrder.Checked == true))
            {
                found = true;
                payManager.PurchaseOrderAddInfo(this.ponumber.Text.Trim(), o.TotalGrandAfterStoreCredits(MyPage.MTApp.OrderServices));
            }
            if ((found == false) && (this.rbCompanyAccount.Checked == true))
            {
                found = true;
                payManager.CompanyAccountAddInfo(this.accountnumber.Text.Trim(), o.TotalGrandAfterStoreCredits(MyPage.MTApp.OrderServices));
            }

            if ((found == false) && (this.rbCOD.Checked == true))
            {
                found = true;
                payManager.OfflinePaymentAddInfo(o.TotalGrandAfterStoreCredits(MyPage.MTApp.OrderServices), "Customer will pay cash on delivery.");
            }

            if ((found == false) && (this.rbPaypal.Checked == true))
            {
                found = true;
                // Need token and id before we can add this to the order
                // Handled on the checkout page.
                //payManager.PayPalExpressAddInfo(o.TotalGrand);
            }

            //if (o.GiftCertificates.Count > 0) {
            //    foreach (string item in o.GiftCertificates) {
            //        p = new OrderPayment();
            //        p.OrderID = o.Bvin;
            //        p.AuditDate = DateTime.Now;
            //        p.GiftCertificateNumber = item;
            //        p.PaymentMethodName = "Gift Certificate";
            //        p.PaymentMethodId = WebAppSettings.PaymentIdGiftCertificate;
            //        o.AddPayment(p);
            //    }
            //}
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                if (this.TabIndex != -1)
                {
                    rbNoPayment.TabIndex = (short)this.TabIndex;
                    rbCreditCard.TabIndex = (short)(this.TabIndex + 1);
                    this.CreditCardInput1.TabIndex = (short)(this.TabIndex + 2);
                    this.rbPaypal.TabIndex = (short)(this.TabIndex + 9);
                    rbPurchaseOrder.TabIndex = (short)(this.TabIndex + 10);
                    ponumber.TabIndex = (short)(this.TabIndex + 11);
                    accountnumber.TabIndex = (short)(this.TabIndex + 11);
                    rbCheck.TabIndex = (short)(this.TabIndex + 12);
                    rbTelephone.TabIndex = (short)(this.TabIndex + 13);
                    rbCOD.TabIndex = (short)(this.TabIndex + 14);
                }
            }
        }

        public System.Collections.Generic.List<MerchantTribe.Web.Validation.RuleViolation> GetRuleViolations()
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            // Clear any potential CSS Violations
            this.ponumber.CssClass = "";
            this.CreditCardInput1.ClearCssViolations();

            // Nothing to validate if no payment is needed
            if (this.rowNoPaymentNeeded.Visible)
            {
                return violations;
            }

            if (this.rbCreditCard.Checked)
            {
                return this.CreditCardInput1.GetRuleViolations();
            }
            if (this.rbCheck.Checked)
            {
                return violations;
            }
            if (this.rbTelephone.Checked)
            {
                return violations;
            }
            if (this.rbPurchaseOrder.Checked)
            {
                ValidationHelper.Required("Purchase Order Number", this.ponumber.Text.Trim(), violations, "ponumber");
                if (violations.Count > 0)
                {
                    this.ponumber.CssClass = "input-validation-error";
                }
                return violations;
            }
            if (this.rbCompanyAccount.Checked)
            {
                ValidationHelper.Required("Company Account Number", this.accountnumber.Text.Trim(), violations, "accountnumber");
                if (violations.Count > 0)
                {
                    this.accountnumber.CssClass = "input-validation-error";
                }
                return violations;
            }
            if (this.rbCOD.Checked)
            {
                return violations;
            }
            if (this.rbPaypal.Checked)
            {
                return violations;
            }

            // We haven't return anything so nothing is selected.
            // Try CC as default payment method        
            if (this.CreditCardInput1.CardNumber.Length > 12)
            {
                this.rbCreditCard.Checked = true;
                return this.CreditCardInput1.GetRuleViolations();
            }

            // nothing selected, trial of cc failed
            violations.Add(new RuleViolation("Payment Method", "", "Please select a payment method", ""));

            return violations;
        }

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

    }
}