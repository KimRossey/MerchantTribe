
namespace MerchantTribe.Commerce.Controls
{	
	public class LoginCompleteEventArgs
	{
		private string _userId = string.Empty;
        private string _UserEmail = string.Empty;

		public string UserId {
			get { return _userId; }
			set { _userId = value; }
		}

        public string UserEmail
        {
            get { return _UserEmail; }
            set { _UserEmail = value; }
        }
	}
}
