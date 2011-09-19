namespace MerchantTribe.Commerce
{
    public class PaypalExpressCheckoutArgs
    {
        private bool _failed = true;

        public bool Failed
        {
            get { return _failed; }
            set { _failed = value; }
        }
    }

}