using System;

namespace MerchantTribe.Commerce.BusinessRules
{

	public class ShippingTaskContext : TaskContext
	{

		private Utilities.SortableCollection<Shipping.ShippingRateDisplay> _Rates;
		private Orders.Order _Order;

		public Utilities.SortableCollection<Shipping.ShippingRateDisplay> Rates {
			get { return _Rates; }
			set { _Rates = value; }
		}

		public Orders.Order Order {
			get { return _Order; }
			set { _Order = value; }
		}

	}
}

