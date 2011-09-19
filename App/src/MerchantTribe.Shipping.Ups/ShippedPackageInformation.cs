
namespace MerchantTribe.Shipping.Ups
{

    public class ShippedPackageInformation
    {

        private string _TrackingNumber = string.Empty;
        private decimal _ServiceOptionsCharge = 0;
        private CurrencyCode _ServiceOptionsChargeCurrency = CurrencyCode.UsDollar;
        private ShipLabelFormat _LabelFormat = ShipLabelFormat.Gif;
        private string _Base64Image = string.Empty;
        private string _Base64Html = string.Empty;
        private string _Base64Signature = string.Empty;

        public string TrackingNumber
        {
            get { return _TrackingNumber; }
            set { _TrackingNumber = value; }
        }
        public decimal ServiceOptionsCharge
        {
            get { return _ServiceOptionsCharge; }
            set { _ServiceOptionsCharge = value; }
        }
        public CurrencyCode ServiceOptionsChargeCurrency
        {
            get { return _ServiceOptionsChargeCurrency; }
            set { _ServiceOptionsChargeCurrency = value; }
        }
        public ShipLabelFormat LabelFormat
        {
            get { return _LabelFormat; }
            set { _LabelFormat = value; }
        }
        public string Base64Image
        {
            get { return _Base64Image; }
            set { _Base64Image = value; }
        }
        public string Base64Html
        {
            get { return _Base64Html; }
            set { _Base64Html = value; }
        }
        public string Base64Signature
        {
            get { return _Base64Signature; }
            set { _Base64Signature = value; }
        }

        public ShippedPackageInformation()
        {

        }

    }
 
}