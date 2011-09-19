using System;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Metrics
{

	public enum EventLogSeverity : int
	{
		None = 0,
		Information = 1,
		Warning = 2,
		Error = 3
	}
}
