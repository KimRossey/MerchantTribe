using System.Collections.ObjectModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Orders;
using BVSoftware.Commerce.Payment;
using System.Text;
using BVSoftware.AcumaticaTools;
using MerchantTribe.Web.Geography;
using MerchantTribe.Web;

namespace BVCommerce.BVModules.Controls
{
    public partial class ViewOrderFromAcumatica : BVSoftware.Commerce.Content.BVUserControl
    {

        //private bool _InputsAndModifiersLoaded = false;

        private bool _DisableReturns = true;
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

        private OrderData _order = null;
        public OrderData Order
        {
            get { return _order; }
            set { _order = value; }
        }

        public void LoadOrder()
        {
            string bvin = this.OrderBvin;

            OrderData o = GetOrderData(bvin);
            if (o == null)
            {
                this.OrderNumberField.Text = BVSoftware.Commerce.Content.SiteTerms.GetTerm(BVSoftware.Commerce.Content.SiteTermIds.AcumaticaUnavailable);
            }
            else
            {                
                this.Order = o;
                PopulateFromOrder(o);                
            }
        }
        private OrderData GetOrderData(string orderNumber)
        {
            OrderData result = new OrderData();
            try
            {

                AcumaticaIntegration acumatica = AcumaticaIntegration.Factory(MyPage.BVApp);
                result = acumatica.GetSingleOrder(orderNumber);
                if (result == null) return new OrderData();
            }
            catch (RemoteIntegrationException)
            {
                result = null;
            }
            return result;
        }

        private void PopulateFromOrder(OrderData o)
        {

            if (DisableOrderNumberDisplay)
            {
                this.OrderNumberHeader.Visible = false;
            }
            else
            {
                // Header
                this.OrderNumberField.Text = (o.BVOrderNumber != string.Empty) ? o.BVOrderNumber : o.AcumaticaOrderNumber;
                this.OrderNumberHeader.Visible = true;
            }

            // Status
            //if (_DisableStatus == true)
            //{
                this.StatusField.Text = string.Empty;
            //}
            //else
            //{
            //    this.StatusField.Text = o. o.FullOrderStatusDescription();
            //    //Me.lblCurrentStatus.Text = o.StatusName
            //}

                this.BillingAddressField.Text = string.Empty;
                if (o.Customer != null)
                {
                    if (o.Customer.BillingAddress != null)
                    {
                        SimpleAddress a = (SimpleAddress)o.Customer.BillingAddress;
                        this.BillingAddressField.Text = a.ToHtmlString();
                    }
                }                

            this.pnlShipTo.Visible = true;
            this.ShippingAddressField.Text = string.Empty;
            if (o.ShippingAddress != null)
            {
                SimpleAddress s = (SimpleAddress)o.ShippingAddress;
                this.ShippingAddressField.Text = s.ToHtmlString();
            }

            this.litTotalGrand.Text = o.GrandTotal.ToString("C");
            decimal sub = o.GrandTotal - o.TaxTotal - o.ShippingTotal;
            this.litTotalSub.Text = sub.ToString("C");
            this.litTotalShipping.Text = o.ShippingTotal.ToString("C");
            this.litTotalTax.Text = o.TaxTotal.ToString("C");

            // Payment
//*************            OrderPaymentSummary paySummary = MyPage.BVApp.OrderServices.PaymentSummary(o);
            //this.lblPaymentSummary.Text = paySummary.PaymentsSummary;            
            //this.PaymentTotalField.Text = string.Format("{0:C}", paySummary.AmountCharged);
            //this.PaymentChargedField.Text = string.Format("{0:C}", paySummary.AmountCharged - paySummary.GiftCardAmount);
            //this.GiftCardAmountLabel.Text = string.Format("{0:C}", paySummary.GiftCardAmount);
            //this.PaymentDueField.Text = string.Format("{0:C}", paySummary.AmountDue);
            //this.PaymentRefundedField.Text = string.Format("{0:C}", paySummary.AmountRefunded);

            //Items        
            this.ViewOrderItems1.DisableReturns = this.DisableReturns;

            System.Collections.Generic.List<LineItem> items = new System.Collections.Generic.List<LineItem>();
            foreach (OrderItemData acuItem in o.Items)
            {
                LineItem li = new LineItem();
                li.ProductName = acuItem.Product.Description;
                li.Quantity = (int)acuItem.Quantity;
                li.BasePricePerItem = acuItem.Product.Price.Value;
                li.ProductSku = acuItem.Product.UniqueId;
                items.Add(li);
            }
            this.ViewOrderItems1.HideShippingColumn = true;
            this.ViewOrderItems1.LoadItems(items);
            
            // Instructions
            this.pnlInstructions.Visible = false;
            //if (o.Instructions.Trim().Length > 0)
            //{
            //    this.pnlInstructions.Visible = true;
            //    this.InstructionsField.Text = o.Instructions;
            //}

            // Totals
  //****************          this.litTotals.Text = o.TotalsAsTable();


            // Coupons
            this.CouponField.Text = string.Empty;
            //for (int i = 0; i <= o.Coupons.Count - 1; i++)
            //{
            //    this.CouponField.Text += o.Coupons[i].CouponCode.Trim().ToUpper() + "<br />";
            //}

            // Notes
            //this.PublicNotesField.Visible = false;
            //Collection<OrderNote> publicNotes = new Collection<OrderNote>();
            //for (int i = 0; i <= o.Notes.Count - 1; i++)
            //{
            //    if (o.Notes[i].IsPublic)
            //    {
            //        publicNotes.Add(o.Notes[i]);
            //    }
            //}
            //this.PublicNotesField.DataSource = publicNotes;
            //this.PublicNotesField.DataBind();

            //if (_DisableNotesAndPayment == true)
            //{
                this.trNotes.Visible = false;
            //}

            //Packages
                if (o.Shipments.Count > 0)
                {
                    this.packagesdiv.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" width=\"100%\"><tr class=\"rowheader\">");
                    sb.Append("<th>Shipment #</th>");
                    sb.Append("<th>Ship Date</th>");
                    sb.Append("<th>Status</th>");
                    sb.Append("<th>Items</th>");
                    sb.Append("<th>Tracking #</th>");                    
                    sb.Append("</tr>");

                    foreach (OrderShipmentData shipment in o.Shipments)
                    {
                        sb.Append("<tr>");
                        sb.Append("<td>" + shipment.ShipmentNumber + "</td>");
                        if (shipment.ShipDate.HasValue)
                        {
                            sb.Append("<td>" + shipment.ShipDate.Value.ToShortDateString() + "</td>");
                        }
                        else
                        {
                            sb.Append("<td>-</td>");
                        }
                        sb.Append("<td>" + shipment.StatusCode + "</td>");
                        sb.Append("<td>");
                        foreach (OrderShipmentItem item in shipment.Items)
                        {
                            sb.Append(System.Math.Round(item.Quantity, 0) + " x " + item.ItemId + ", " + item.Description + "<br />");
                        }
                        sb.Append("</td>");

                        sb.Append("<td>");
                        foreach (string s in shipment.TrackingNumber)
                        {
                            sb.Append(s + "<br />");
                        }
                        sb.Append("</td>");
                                                
                        sb.Append("</tr>");
                    }

                    sb.Append("</table>");
                    this.litPackages.Text = sb.ToString();
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
                        foreach (BVSoftware.Shipping.IShippingService item in BVSoftware.Commerce.Shipping.AvailableServices.FindAll(MyPage.BVApp.CurrentStore))
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