
namespace MerchantTribe.Shipping.Ups
{

    public class Package
    {

        private PackagingType _Packaging = PackagingType.CustomerSupplied;
        private decimal _Length = 0;
        private decimal _Width = 0;
        private decimal _Height = 0;
        private decimal _Weight = 0;
        private UnitsType _DimensionalUnits = UnitsType.Imperial;
        private UnitsType _WeightUnits = UnitsType.Imperial;
        private string _Description = string.Empty;
        private string _ReferenceNumber = string.Empty;
        private ReferenceNumberCode _ReferenceNumberType = ReferenceNumberCode.TransactionReferenceNumber;
        private string _ReferenceNumber2 = string.Empty;
        private ReferenceNumberCode _ReferenceNumber2Type = ReferenceNumberCode.TransactionReferenceNumber;
        private bool _AdditionalHandlingIsRequired = false;
        private bool _DeliveryConfirmation = false;
        private ConfirmationType _DeliveryConfirmationType = ConfirmationType.NoSignatureRequired;
        private string _DeliveryConfirmationControlNumber = string.Empty;
        private decimal _InsuredValue = 0;
        private CurrencyCode _InsuredValueCurrency = CurrencyCode.UsDollar;
        private bool _COD = false;
        private CODFundsCode _CODPaymentType = CODFundsCode.AllTypesAccepted;
        private CurrencyCode _CODCurrencyCode = CurrencyCode.UsDollar;
        private decimal _CODAmount = 0;
        private bool _VerbalConfirmation = false;
        private string _VerbalConfirmationName = string.Empty;
        private string _VerbalConfirmationPhoneNumber = string.Empty;


        public PackagingType Packaging
        {
            get { return _Packaging; }
            set { _Packaging = value; }
        }
        public decimal Length
        {
            get { return _Length; }
            set { _Length = value; }
        }
        public decimal Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        public decimal Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public decimal Weight
        {
            get { return _Weight; }
            set { _Weight = value; }
        }
        public UnitsType DimensionalUnits
        {
            get { return _DimensionalUnits; }
            set { _DimensionalUnits = value; }
        }
        public UnitsType WeightUnits
        {
            get { return _WeightUnits; }
            set { _WeightUnits = value; }
        }
        public string ReferenceNumber
        {
            get { return _ReferenceNumber; }
            set { _ReferenceNumber = value; }
        }
        public ReferenceNumberCode ReferenceNumberType
        {
            get { return _ReferenceNumberType; }
            set { _ReferenceNumberType = value; }
        }
        public string ReferenceNumber2
        {
            get { return _ReferenceNumber2; }
            set { _ReferenceNumber2 = value; }
        }
        public ReferenceNumberCode ReferenceNumber2Type
        {
            get { return _ReferenceNumber2Type; }
            set { _ReferenceNumber2Type = value; }
        }
        public bool AdditionalHandlingIsRequired
        {
            get { return _AdditionalHandlingIsRequired; }
            set { _AdditionalHandlingIsRequired = value; }
        }
        public bool DeliveryConfirmation
        {
            get { return _DeliveryConfirmation; }
            set { _DeliveryConfirmation = value; }
        }
        public ConfirmationType DeliveryConfirmationType
        {
            get { return _DeliveryConfirmationType; }
            set { _DeliveryConfirmationType = value; }
        }
        public string DeliveryConfirmationControlNumber
        {
            get { return _DeliveryConfirmationControlNumber; }
            set { _DeliveryConfirmationControlNumber = value; }
        }
        public decimal InsuredValue
        {
            get { return _InsuredValue; }
            set { _InsuredValue = value; }
        }
        public CurrencyCode InsuredValueCurrency
        {
            get { return _InsuredValueCurrency; }
            set { _InsuredValueCurrency = value; }
        }
        public bool COD
        {
            get { return _COD; }
            set { _COD = value; }
        }
        public CODFundsCode CODPaymentType
        {
            get { return _CODPaymentType; }
            set { _CODPaymentType = value; }
        }
        public CurrencyCode CODCurrencyCode
        {
            get { return _CODCurrencyCode; }
            set { _CODCurrencyCode = value; }
        }
        public decimal CODAmount
        {
            get { return _CODAmount; }
            set { _CODAmount = value; }
        }
        public bool VerbalConfirmation
        {
            get { return _VerbalConfirmation; }
            set { _VerbalConfirmation = value; }
        }
        public string VerbalConfirmationName
        {
            get { return _VerbalConfirmationName; }
            set { _VerbalConfirmationName = value; }
        }
        public string VerbalConfirmationPhoneNumber
        {
            get { return _VerbalConfirmationPhoneNumber; }
            set { _VerbalConfirmationPhoneNumber = XmlTools.CleanPhoneNumber(value); }
        }

        public Package()
        {

        }

    }
 
}