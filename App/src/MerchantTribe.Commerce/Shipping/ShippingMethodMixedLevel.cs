using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Shipping
{

	public class ShippingMethodMixedLevel
	{

		private string _Bvin = System.Guid.NewGuid().ToString();
		private decimal _Level = 0m;
		private decimal _Percentage = 0m;
		private decimal _FixedAmount = 0m;

		public string Bvin {
			get { return _Bvin; }
			set { _Bvin = value; }
		}
		public decimal Level {
			get { return _Level; }
			set { _Level = value; }
		}
		public decimal FixedAmount {
			get { return _FixedAmount; }
			set { _FixedAmount = value; }
		}
		public decimal Percentage {
			get { return _Percentage; }
			set { _Percentage = value; }
		}

		public ShippingMethodMixedLevel()
		{

		}

		public ShippingMethodMixedLevel(decimal levelValue, decimal fixedAmountValue, decimal percentage)
		{
			_Level = levelValue;
			_FixedAmount = fixedAmountValue;
			_Percentage = percentage;
		}

		public decimal TotalAmount(decimal orderSubTotal)
		{
			decimal result = (orderSubTotal * (Percentage / 100)) + FixedAmount;
			return result;
		}

	}

}

