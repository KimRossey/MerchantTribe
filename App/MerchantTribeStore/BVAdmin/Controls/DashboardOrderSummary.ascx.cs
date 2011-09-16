using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Reporting;

namespace BVCommerce
{

    public partial class BVAdmin_Controls_DashboardOrderSummary : BVSoftware.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            OrderSummary summary = new OrderSummary(MyPage.BVApp.CurrentStore.Id);
            this.litNewCount.Text = summary.NewOrderCount.ToString();
            this.litHoldCount.Text = summary.OnHoldOrderCount.ToString();
            this.litPaymentCount.Text = summary.ReadyForPaymentOrderCount.ToString();
            this.litShippingCount.Text = summary.ReadyForShippingOrderCount.ToString();
        }
    }
}