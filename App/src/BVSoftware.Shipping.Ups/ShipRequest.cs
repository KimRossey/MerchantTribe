
using System.IO;
using System.Xml;
using System.Collections;

namespace BVSoftware.Shipping.Ups
{

    public class ShipRequest
    {

        public ShipRequest()
        {

        }

        private UpsSettings _settings = new UpsSettings();
        private string _XmlConfirmRequest = string.Empty;
        private string _XmlConfirmResponse = string.Empty;
        private bool _AddressVerification = false;
        private ShipLabelFormat _LabelFormat = ShipLabelFormat.Gif;
        private string _BrowserHttpUserAgentString = string.Empty;
        private int _StockSizeHeight = 0;
        private int _StockSizeWidth = 0;
        private string _ShipmentDescription = string.Empty;
        private bool _NonValuedDocumentsOnly = false;
        private string _ReferenceNumber = string.Empty;
        private ReferenceNumberCode _ReferenceNumberType = ReferenceNumberCode.TransactionReferenceNumber;
        private ServiceType _Service = ServiceType.Standard;
        private ArrayList _Packages = new ArrayList(10);
        private bool _COD = false;
        private CODFundsCode _CODPaymentType = CODFundsCode.AllTypesAccepted;
        private CurrencyCode _CODCurrencyCode = CurrencyCode.UsDollar;
        private decimal _CODAmount = 0;
        private bool _InvoiceLineTotal = false;
        private decimal _InvoiceLineTotalAmount = 0;
        private CurrencyCode _InvoiceLineTotalCurrency = CurrencyCode.UsDollar;
        private bool _SaturdayDelivery = false;
        private bool _Notification = false;
        private NotificationCode _NotificationType = NotificationCode.ShipNotification;
        private string _NotificationEmailAddress = string.Empty;
        private string _NotificationUndeliverableEmailAddress = string.Empty;
        private string _NotificationFromName = string.Empty;
        private string _NotificationMemo = string.Empty;
        private NotificationSubjectCode _NotificationSubjectType = NotificationSubjectCode.DefaultCode;
        // Note OnCallAir options are not supported yet
        private PaymentType _BillTo = PaymentType.ShipperUpsAccount;
        private string _BillToAccountNumber = string.Empty;
        private string _BillToCreditCardNumber = string.Empty;
        private int _BillToCreditCardExpirationMonth = 1;
        private int _BillToCreditCardExpirationYear = 1900;
        private CreditCardType _BillToCreditCardType = CreditCardType.Amex;
        private string _BillToPostalCode = string.Empty;
        private string _BillToCountryCode = string.Empty;
        private Entity _Shipper = new Entity();
        private Entity _ShipTo = new Entity();
        private Entity _ShipFrom = new Entity();



        public UpsSettings Settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public string XmlConfirmRequest
        {
            get { return _XmlConfirmRequest; }
            set { _XmlConfirmRequest = value; }
        }
        public string XmlConfirmResponse
        {
            get { return _XmlConfirmResponse; }
            set { _XmlConfirmResponse = value; }
        }
        public bool AddressVerification
        {
            get { return _AddressVerification; }
            set { _AddressVerification = false; }
        }
        public ShipLabelFormat LabelFormat
        {
            get { return _LabelFormat; }
            set { _LabelFormat = value; }
        }
        public string BrowserHttpUserAgentString
        {
            get { return _BrowserHttpUserAgentString; }
            set { _BrowserHttpUserAgentString = value; }
        }
        public int LabelStockSizeHeight
        {
            get { return _StockSizeHeight; }
            set { _StockSizeHeight = value; }
        }
        public int LabelStockSizeWidth
        {
            get { return _StockSizeWidth; }
            set { _StockSizeWidth = value; }
        }
        public string ShipmentDescription
        {
            get { return _ShipmentDescription; }
            set { _ShipmentDescription = value; }
        }
        public bool NonValuedDocumentsOnly
        {
            get { return _NonValuedDocumentsOnly; }
            set { _NonValuedDocumentsOnly = value; }
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
        public ServiceType Service
        {
            get { return _Service; }
            set { _Service = value; }
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
        public bool InvoiceLineTotal
        {
            get { return _InvoiceLineTotal; }
            set { _InvoiceLineTotal = value; }
        }
        public decimal InvoiceLineTotalAmount
        {
            get { return _InvoiceLineTotalAmount; }
            set { _InvoiceLineTotalAmount = value; }
        }
        public CurrencyCode InvoiceLineTotalCurrency
        {
            get { return _InvoiceLineTotalCurrency; }
            set { _InvoiceLineTotalCurrency = value; }
        }
        public bool SaturdayDelivery
        {
            get { return _SaturdayDelivery; }
            set { _SaturdayDelivery = value; }
        }
        public bool Notification
        {
            get { return _Notification; }
            set { _Notification = value; }
        }
        public NotificationCode NotificationType
        {
            get { return _NotificationType; }
            set { _NotificationType = value; }
        }
        public string NotificationEmailAddress
        {
            get { return _NotificationEmailAddress; }
            set { _NotificationEmailAddress = value; }
        }
        public string NotificationUndeliverableEmailAddress
        {
            get { return _NotificationUndeliverableEmailAddress; }
            set { _NotificationUndeliverableEmailAddress = value; }
        }
        public string NotificationFromName
        {
            get { return _NotificationFromName; }
            set { _NotificationFromName = value; }
        }
        public string NotificationMemo
        {
            get { return _NotificationMemo; }
            set { _NotificationMemo = value; }
        }
        public NotificationSubjectCode NotificationSubjectType
        {
            get { return _NotificationSubjectType; }
            set { _NotificationSubjectType = value; }
        }
        public PaymentType BillTo
        {
            get { return _BillTo; }
            set { _BillTo = value; }
        }
        public string BillToAccountNumber
        {
            get { return _BillToAccountNumber; }
            set { _BillToAccountNumber = value; }
        }
        public string BillToCreditCardNumber
        {
            get { return _BillToCreditCardNumber; }
            set { _BillToCreditCardNumber = value; }
        }
        public int BillToCreditCardExpirationMonth
        {
            get { return _BillToCreditCardExpirationMonth; }
            set
            {
                _BillToCreditCardExpirationMonth = value;
                if (_BillToCreditCardExpirationMonth < 1)
                {
                    _BillToCreditCardExpirationMonth = 1;
                }
                if (_BillToCreditCardExpirationMonth > 13)
                {
                    _BillToCreditCardExpirationMonth = 1;
                }
            }
        }
        public int BillToCreditCardExpirationYear
        {
            get { return _BillToCreditCardExpirationYear; }
            set
            {
                _BillToCreditCardExpirationYear = value;
                // Make sure we've got a four digit date
                if (_BillToCreditCardExpirationYear < 2000)
                {
                    _BillToCreditCardExpirationYear += 2000;
                }
            }
        }
        public CreditCardType BillToCreditCardType
        {
            get { return _BillToCreditCardType; }
            set { _BillToCreditCardType = value; }
        }
        public string BillToPostalCode
        {
            get { return _BillToPostalCode; }
            set { _BillToPostalCode = value; }
        }
        public string BillToCountryCode
        {
            get { return _BillToCountryCode; }
            set { _BillToCountryCode = value; }
        }
        public ArrayList Packages
        {
            get { return this._Packages; }
        }

        public Entity Shipper
        {
            get { return _Shipper; }
            set { _Shipper = value; }
        }
        public Entity ShipTo
        {
            get { return _ShipTo; }
            set { _ShipTo = value; }
        }
        public Entity ShipFrom
        {
            get { return _ShipFrom; }
            set { _ShipFrom = value; }
        }

        public bool AddPackage(ref Package p)
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
