
using System.Xml;

namespace MerchantTribe.Shipping.FedEx
{

    public class RateEstimatedCharges
    {

        private bool _DimensionalWeightUsed = false;
        private string _RateScale = string.Empty;
        private string _RateZone = string.Empty;
        private string _CurrencyCode = string.Empty;
        private decimal _BilledWeight = 0m;
        private decimal _DimensionalWeight = 0m;

        private RateCharge _DiscountedCharges = new RateCharge();
        private RateCharge _ListCharges = new RateCharge();

        private decimal _EffectiveNetDiscount = 0m;
        private decimal _MTWNetCharge = 0m;

        public bool DimensionalWeightUsed
        {
            get { return _DimensionalWeightUsed; }
            set { _DimensionalWeightUsed = value; }
        }
        public string RateScale
        {
            get { return _RateScale; }
            set { _RateScale = value; }
        }
        public string RateZone
        {
            get { return _RateZone; }
            set { _RateZone = value; }
        }
        public string CurrencyCode
        {
            get { return _CurrencyCode; }
            set { _CurrencyCode = value; }
        }
        public decimal BilledWeight
        {
            get { return _BilledWeight; }
            set { _BilledWeight = value; }
        }
        public decimal DimensionalWeight
        {
            get { return _DimensionalWeight; }
            set { _DimensionalWeight = value; }
        }
        public RateCharge DiscountedCharges
        {
            get { return _DiscountedCharges; }
            set { _DiscountedCharges = value; }
        }
        public RateCharge ListCharges
        {
            get { return _ListCharges; }
            set { _ListCharges = value; }
        }
        public decimal EffectiveNetDiscount
        {
            get { return _EffectiveNetDiscount; }
            set { _EffectiveNetDiscount = value; }
        }
        public decimal MTWNetCharge
        {
            get { return _MTWNetCharge; }
            set { _MTWNetCharge = value; }
        }

        public RateEstimatedCharges()
        {

        }

        public RateEstimatedCharges(XmlNode n)
        {
            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            _DimensionalWeightUsed = XmlHelper.ParseBoolean(n, "DimWeightUsed");
            _RateScale = XmlHelper.ParseInnerText(n, "RateScale");
            _RateZone = XmlHelper.ParseInnerText(n, "RateZone");
            _CurrencyCode = XmlHelper.ParseInnerText(n, "CurrencyCode");
            _BilledWeight = XmlHelper.ParseDecimal(n, "BilledWeight");
            _DimensionalWeight = XmlHelper.ParseDecimal(n, "DimWeight");

            if (n != null)
            {
                _DiscountedCharges.ParseNode(n.SelectSingleNode("DiscountedCharges"));
                _ListCharges.ParseNode(n.SelectSingleNode("ListCharges"));
            }

            _EffectiveNetDiscount = XmlHelper.ParseDecimal(n, "EffectiveNetDiscount");
            _MTWNetCharge = XmlHelper.ParseDecimal(n, "MTWNetCharge");

        }
    }
}