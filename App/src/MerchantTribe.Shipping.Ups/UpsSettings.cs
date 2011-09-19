namespace MerchantTribe.Shipping.Ups
{

    public class UpsSettings
    {

        private string _userID = string.Empty;
        private string _password = string.Empty;
        private string _license = string.Empty;
        private string _ServerUrl = string.Empty;

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public string License
        {
            get { return _license; }
            set { _license = value; }
        }
        public string ServerUrl
        {
            get { return _ServerUrl; }
            set { _ServerUrl = value; }
        }

        public UpsSettings()
        {

        }

    }

}