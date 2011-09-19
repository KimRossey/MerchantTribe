using System;

namespace MerchantTribe.Commerce.Controls
{	
	public class ControlNotFoundException : ApplicationException
	{

		public ControlNotFoundException(): base()
		{			
		}

		public ControlNotFoundException(string message): base(message)
		{

		}

		public ControlNotFoundException(string message, Exception innerException): base(message,innerException)
		{

		}

	}
}

