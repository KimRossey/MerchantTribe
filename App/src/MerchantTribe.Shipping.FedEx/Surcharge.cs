
namespace MerchantTribe.Shipping.FedEx
{

    public class Surcharge
    {

        private string _Description = string.Empty;
        private decimal _Amount = 0m;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        public Surcharge()
        {

        }
        public Surcharge(string surchargeDescription, decimal surchargeAmount)
        {
            _Description = surchargeDescription;
            _Amount = surchargeAmount;
        }

    }

}