
namespace MerchantTribe.Commerce
{
    public class GoogleCheckoutArgs
    {
        private bool _failed = true;

        public bool Failed
        {
            get { return _failed; }
            set { _failed = value; }
        }
    }
}
