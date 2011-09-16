using System;

namespace BVSoftware.Commerce.BusinessRules
{
	public class OrderTaskContext : TaskContext
	{

        public OrderTaskContext(BVApplication bvapp)
        {
            this.BVApp = bvapp;
        }

		private Orders.Order _Order = new Orders.Order();

		public Orders.Order Order {
			get { return _Order; }
			set { _Order = value; }
		}

        public BVApplication BVApp { get; set; }
        
	}
}

