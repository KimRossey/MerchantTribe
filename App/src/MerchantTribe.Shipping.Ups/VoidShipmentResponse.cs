
namespace MerchantTribe.Shipping.Ups
{

    public class VoidShipmentResponse
    {
        private string _ErrorCode = string.Empty;
        private string _ErrorMessage = string.Empty;
        private bool _Success = false;

        public string ErrorCode
        {
            get { return _ErrorCode; }
            set { _ErrorCode = value; }
        }
        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }

    }

}