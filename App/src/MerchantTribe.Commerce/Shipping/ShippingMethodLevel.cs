using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Shipping
{
	
	public class ShippingMethodLevel
	{

		private string _Bvin = System.Guid.NewGuid().ToString();
		private decimal _Level = 0m;
		private decimal _Amount = 0m;

		public string Bvin {
			get { return _Bvin; }
			set { _Bvin = value; }
		}
		public decimal Level {
			get { return _Level; }
			set { _Level = value; }
		}
		public decimal Amount {
			get { return _Amount; }
			set { _Amount = value; }
		}

		public ShippingMethodLevel()
		{

		}

		public ShippingMethodLevel(decimal levelValue, decimal amountValue)
		{
			_Level = levelValue;
			_Amount = amountValue;
		}

	}

}

