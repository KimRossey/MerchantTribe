using System;
using System.Web;

namespace MerchantTribe.Commerce.Content
{
	public class NotifyClickControl : System.Web.UI.UserControl
	{

		public event EventHandler<ClickedEventArgs> Clicked;

		public class ClickedEventArgs : EventArgs
		{

			private string _info = string.Empty;
			private bool _errorOccurred = false;

			public string Info {
				get { return _info; }
				set { _info = value; }
			}

			public bool ErrorOccurred {
				get { return _errorOccurred; }
				set { _errorOccurred = value; }
			}

			public ClickedEventArgs()
			{

			}

			public ClickedEventArgs(string data)
			{
				if (data != null) {
					_info = data;
				}
			}

		}

		protected bool NotifyClicked(string data)
		{
			ClickedEventArgs args = new ClickedEventArgs(data);
			if (Clicked != null) {
				Clicked(this, args);
			}
			return !args.ErrorOccurred;
		}

		protected bool NotifyClicked()
		{
			ClickedEventArgs args = new ClickedEventArgs();
			if (Clicked != null) {
				Clicked(this, args);
			}
			return !args.ErrorOccurred;
		}

	}
}

