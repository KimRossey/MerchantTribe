using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Content
{

	public class BVValidationResult
	{

		private bool _Success = false;
		private Collection<string> _Messages = new Collection<string>();

		public bool Success {
			get { return _Success; }
			set { _Success = value; }
		}

		public BVValidationResult()
		{

		}

		public void AddMessage(string Message)
		{
			_Messages.Add(Message);
		}

		public Collection<string> GetMessages()
		{
			return _Messages;
		}

	}
}
