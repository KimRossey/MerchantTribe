using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.BusinessRules
{
	public class WorkflowMessage
	{
		private bool _CustomerVisible = false;
		private string _Name = string.Empty;
		private string _Description = string.Empty;

		public WorkflowMessage()
		{

		}

		public WorkflowMessage(string messageName, string messageDescription, bool showToCustomer)
		{
			_CustomerVisible = showToCustomer;
			_Name = messageName;
			_Description = messageDescription;
		}

		public bool CustomerVisible {
			get { return _CustomerVisible; }
			set { _CustomerVisible = value; }
		}
		public string Name {
			get { return _Name; }
			set { _Name = value; }
		}
		public string Description {
			get { return _Description; }
			set { _Description = value; }
		}

	}

}

