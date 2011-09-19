using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Shipping
{

	public class AvailableShippingMethod
	{
		private string _DisplayName = "";
		private string _ServiceCode = "";
		//private decimal _Cost = 0m;

		public string DisplayName {
			get { return _DisplayName; }
			set { _DisplayName = value; }
		}
		public string ServiceCode {
			get { return _ServiceCode; }
			set { _ServiceCode = value; }
		}
        //public decimal Cost {
        //    get { return _Cost; }
        //    set { _Cost = value; }
        //}
	}

}
