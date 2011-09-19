
using System.Collections;
namespace MerchantTribe.Shipping.Ups
{

    public class ShipAcceptResponse
    {
        private string _ErrorCode = string.Empty;
        private string _ErrorMessage = string.Empty;
        private bool _Success = false;
        private decimal _TransportationCharge = 0;
        private CurrencyCode _TransportationChargeCurrency = CurrencyCode.UsDollar;
        private decimal _ServiceOptionsCharge = 0;
        private CurrencyCode _ServiceOptionsChargeCurrency = CurrencyCode.UsDollar;
        private decimal _TotalCharge = 0;
        private CurrencyCode _TotalChargeCurrency = CurrencyCode.UsDollar;
        private decimal _BillingWeight = 0;
        private UnitsType _BillingWeightUnits = UnitsType.Imperial;
        private string _TrackingNumber = string.Empty;
        private ArrayList _Packages = new ArrayList(10);

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
        public decimal TransportationCharge
        {
            get { return _TransportationCharge; }
            set { _TransportationCharge = value; }
        }
        public CurrencyCode TransportationChargeCurrency
        {
            get { return _TransportationChargeCurrency; }
            set { _TransportationChargeCurrency = value; }
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
        public decimal TotalCharge
        {
            get { return _TotalCharge; }
            set { _TotalCharge = value; }
        }
        public CurrencyCode TotalChargeCurrency
        {
            get { return _TotalChargeCurrency; }
            set { _TotalChargeCurrency = value; }
        }
        public decimal BillingWeight
        {
            get { return _BillingWeight; }
            set { _BillingWeight = value; }
        }
        public UnitsType BillingWeightUnits
        {
            get { return _BillingWeightUnits; }
            set { _BillingWeightUnits = value; }
        }
        public string TrackingNumber
        {
            get { return _TrackingNumber; }
            set { _TrackingNumber = value; }
        }
        public ArrayList Packages
        {
            get { return _Packages; }
        }

        public ShipAcceptResponse()
        {

        }

        public bool AddPackage(ref ShippedPackageInformation p)
        {
            bool result = false;

            _Packages.Add(p);
            result = true;

            return result;
        }

        public bool ClearPackages()
        {
            bool result = true;

            _Packages = new ArrayList(10);
            result = true;

            return result;
        }


    }
}
