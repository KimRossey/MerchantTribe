using System;

namespace MerchantTribe.Commerce.BusinessRules
{
	public class OrderTaskContext : TaskContext
	{

        public OrderTaskContext(MerchantTribeApplication app)
        {
            this.MTApp = app;
        }

		private Orders.Order _Order = new Orders.Order();

		public Orders.Order Order {
			get { return _Order; }
			set { _Order = value; }
		}

        public MerchantTribeApplication MTApp { get; set; }
        
	}
}

