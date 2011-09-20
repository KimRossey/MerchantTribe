using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using MerchantTribe.Commerce;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Payment;
using System.Text;

namespace MerchantTribeStore
{

    partial class BVModules_Controls_ViewOrder : MerchantTribe.Commerce.Content.BVUserControl
    {

        //private bool _InputsAndModifiersLoaded = false;

        private bool _DisableReturns = false;
        private bool _DisableNotesAndPayment = false;
        private bool _DisableStatus = false;
        private bool _DisableOrderNumberDisplay = false;

        public bool DisableReturns
        {
            get { return _DisableReturns; }
            set { _DisableReturns = value; }
        }
        public bool DisableNotesAndPayment
        {
            get { return _DisableNotesAndPayment; }
            set { _DisableNotesAndPayment = value; }
        }
        public bool DisableStatus
        {
            get { return _DisableStatus; }
            set { _DisableStatus = value; }
        }
        public bool DisableOrderNumberDisplay
        {
            get { return _DisableOrderNumberDisplay; }
            set { _DisableOrderNumberDisplay = value; }
        }

        public string OrderBvin
        {
            get
            {
                object obj = ViewState["OrderBvin"];
                if (obj != null)
                {
                    return (string)obj;
                }
                else
                {
                    return string.Empty;
                }
            }
            set { ViewState["OrderBvin"] = value; }
        }

        private Order _order = null;
        public Order Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public void LoadOrder()
        {
            string bvin = this.OrderBvin;

            Order o = MyPage.MTApp.OrderServices.Orders.FindForCurrentStore(bvin);

            if (o != null)
            {
                if (o.bvin != string.Empty)
                {
                    this.Order = o;
                    PopulateFromOrder(o);
                }
            }
        }

        //public delegate void ThrowErrorDelegate();
        //public event ThrowErrorDelegate ThrowError;

        private void PopulateFromOrder(Order o)
        {

            if (DisableOrderNumberDisplay)
            {
                this.OrderNumberHeader.Visible = false;
            }
            else
            {
                // Header
                this.OrderNumberField.Text = o.OrderNumber;
                this.OrderNumberHeader.Visible = true;
            }


            // Status
            if (_DisableStatus == true)
            {
                this.StatusField.Text = string.Empty;
            }
            else
            {
                this.StatusField.Text = o.FullOrderStatusDescription();
                //Me.lblCurrentStatus.Text = o.StatusName
            }

            // Billing
            this.BillingAddressField.Text = o.BillingAddress.ToHtmlString();

            //// Shipping (hide if the same as billing address)
            //if (o.ShippingAddress.IsEqualTo(o.BillingAddress) == true) {
            //    this.pnlShipTo.Visible = false;
            //}
            //else {
            this.pnlShipTo.Visible = true;
            this.ShippingAddressField.Text = o.ShippingAddress.ToHtmlString();
            //}

            // Payment
            OrderPaymentSummary paySummary = MyPage.MTApp.OrderServices.PaymentSummary(o);
            this.lblPaymentSummary.Text = paySummary.PaymentsSummary;
            this.PaymentTotalField.Text = string.Format("{0:C}", paySummary.AmountCharged);
            this.PaymentChargedField.Text = string.Format("{0:C}", paySummary.AmountCharged - paySummary.GiftCardAmount);
            this.GiftCardAmountLabel.Text = string.Format("{0:C}", paySummary.GiftCardAmount);
            this.PaymentDueField.Text = string.Format("{0:C}", paySummary.AmountDue);
            this.PaymentRefundedField.Text = string.Format("{0:C}", paySummary.AmountRefunded);

            //Items        
            this.ViewOrderItems1.DisableReturns = this.DisableReturns;
            this.ViewOrderItems1.LoadItems(o.Items);


            // Instructions
            if (o.Instructions.Trim().Length > 0)
            {
                this.pnlInstructions.Visible = true;
                this.InstructionsField.Text = o.Instructions;
            }

            // Totals
            this.litTotals.Text = o.TotalsAsTable();
                        

            // Coupons
            this.CouponField.Text = string.Empty;
            for (int i = 0; i <= o.Coupons.Count - 1; i++)
            {
                this.CouponField.Text += o.Coupons[i].CouponCode.Trim().ToUpper() + "<br />";
            }

            // Notes
            Collection<OrderNote> publicNotes = new Collection<OrderNote>();
            for (int i = 0; i <= o.Notes.Count - 1; i++)
            {
                if (o.Notes[i].IsPublic)
                {
                    publicNotes.Add(o.Notes[i]);
                }
            }
            this.PublicNotesField.DataSource = publicNotes;
            this.PublicNotesField.DataBind();

            if (_DisableNotesAndPayment == true)
            {
                this.trNotes.Visible = false;
            }

            //Packages
            if (o.Packages.Count > 0)
            {
                packagesdiv.Visible = true;
                PackagesGridView.DataSource = o.Packages;
                PackagesGridView.DataBind();
            }
            else
            {
                packagesdiv.Visible = false;
            }
           
        }

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            decimal val = 0m;
            PaymentTotalField.Text = val.ToString("c");
            PaymentChargedField.Text = val.ToString("c");
            PaymentRefundedField.Text = val.ToString("c");
            PaymentDueField.Text = val.ToString("c");
            GiftCardAmountLabel.Text = val.ToString("c");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void PackagesGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    OrderPackage val = (OrderPackage)e.Row.DataItem;
                    ((Label)e.Row.FindControl("ShippedDateLabel")).Text = val.ShipDateUtc.ToLocalTime().ToString("d");
                    if (val.TrackingNumber.Trim() != string.Empty)
                    {
                        foreach (MerchantTribe.Shipping.IShippingService item in MerchantTribe.Commerce.Shipping.AvailableServices.FindAll(MyPage.MTApp.CurrentStore))
                        {
                            if (item.Id == val.ShippingProviderId)
                            {
                                HyperLink trackingNumberHyperLink = (HyperLink)e.Row.FindControl("TrackingNumberHyperLink");
                                trackingNumberHyperLink.Text = val.TrackingNumber;
                                //trackingNumberHyperLink.NavigateUrl = item.GetTrackingUrl(val.TrackingNumber);
                            }
                        }
                    }
                }
            }
        }

    }
}