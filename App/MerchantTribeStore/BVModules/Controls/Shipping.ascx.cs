using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Orders;
using MerchantTribe.Commerce.Shipping;
using MerchantTribe.Commerce.Utilities;

namespace BVCommerce
{

    partial class BVModules_Controls_Shipping : MerchantTribe.Commerce.Content.BVUserControl
    {

        private int _tabIndex = -1;
        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        public void LoadShippingMethodsForOrder(Order o)
        {

            SortableCollection<ShippingRateDisplay> Rates = new SortableCollection<ShippingRateDisplay>();

            if (o.HasShippingItems == false)
            {
                ShippingRateDisplay r = new ShippingRateDisplay();
                r.DisplayName = SiteTerms.GetTerm(SiteTermIds.NoShippingRequired);
                r.ProviderId = "";
                r.ProviderServiceCode = "";
                r.Rate = 0;
                r.ShippingMethodId = "NOSHIPPING";
                Rates.Add(r);
            }
            else
            {
                // Shipping Methods

                Rates = MyPage.MTApp.OrderServices.FindAvailableShippingRates(o);

                if ((Rates.Count < 1))
                {
                    ShippingRateDisplay result = new ShippingRateDisplay();
                    result.DisplayName = "Shipping can not be calculated at this time. We will contact you after receiving your order with the exact shipping charges.";
                    result.ShippingMethodId = "TOBEDETERMINED";
                    result.Rate = 0;
                    Rates.Add(result);
                }

            }

            this.litMain.Text = MerchantTribe.Commerce.Utilities.HtmlRendering.ShippingRatesToRadioButtons(Rates, this.TabIndex, o.ShippingMethodUniqueKey);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

        }

    }
}