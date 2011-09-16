using System;

namespace BVSoftware.Commerce.Controls
{
	public class UserSelectedEventArgs : EventArgs
	{

		private Membership.CustomerAccount _userAccount;

		public Membership.CustomerAccount UserAccount {
			get { return _userAccount; }
			set { _userAccount = value; }
		}
	}
}

