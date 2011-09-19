
using System.Xml;
using System.Collections.ObjectModel;

namespace MerchantTribe.Shipping.FedEx
{

    public class RateCharge
    {

        private decimal _BaseCharge = 0m;
        private Collection<Surcharge> _Surcharges = new Collection<Surcharge>();
        private decimal _TotalSurcharge = 0m;
        private decimal _NetCharge = 0m;
        private decimal _ShipmentNetCharge = 0m;
        private decimal _TotalRebate = 0m;
        private decimal _TotalDiscount = 0m;

        public decimal BaseCharge
        {
            get { return _BaseCharge; }
            set { _BaseCharge = value; }
        }
        public Collection<Surcharge> Surcharges
        {
            get { return _Surcharges; }
            set { _Surcharges = value; }
        }
        public decimal TotalSurcharge
        {
            get { return _TotalSurcharge; }
            set { _TotalSurcharge = value; }
        }
        public decimal NetCharge
        {
            get { return _NetCharge; }
            set { _NetCharge = value; }
        }
        public decimal ShipmentNetCharge
        {
            get { return _ShipmentNetCharge; }
            set { _ShipmentNetCharge = value; }
        }
        public decimal TotalRebate
        {
            get { return _TotalRebate; }
            set { _TotalRebate = value; }
        }
        public decimal TotalDiscount
        {
            get { return _TotalDiscount; }
            set { _TotalDiscount = value; }
        }

        public RateCharge()
        {

        }

        public RateCharge(XmlNode n)
        {
            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            _BaseCharge = XmlHelper.ParseDecimal(n, "BaseCharge");
            if (n != null)
            {
                XmlNode sn = n.SelectSingleNode("Surcharges");
                if (sn != null)
                {
                    foreach (XmlNode snn in sn.ChildNodes)
                    {
                        string description = snn.Name;
                        string tempAmount = snn.InnerText;
                        decimal amount = 0m;
                        decimal.TryParse(tempAmount, System.Globalization.NumberStyles.Float, 
                            System.Globalization.CultureInfo.InvariantCulture,out amount);
                        Surcharges.Add(new Surcharge(description, amount));
                    }
                }
            }
            _TotalSurcharge = XmlHelper.ParseDecimal(n, "TotalSurcharge");
            _NetCharge = XmlHelper.ParseDecimal(n, "NetCharge");
            _ShipmentNetCharge = XmlHelper.ParseDecimal(n, "ShipmentNetCharge");
            _TotalRebate = XmlHelper.ParseDecimal(n, "TotalRebate");
            _TotalDiscount = XmlHelper.ParseDecimal(n, "TotalDiscount");
        }


    }

}