using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.BusinessRules
{
	public class TaskContext
	{

        private RequestContext _CurrentRequest = null;
		private Collection<WorkflowMessage> _Errors = new Collection<WorkflowMessage>();
		private Collection<WorkflowMessage> _Outputs = new Collection<WorkflowMessage>();
		private CustomPropertyCollection _Inputs = new CustomPropertyCollection();

		private string _UserId = string.Empty;

		public string UserId {
			get { return _UserId; }
			set { _UserId = value; }
		}
		public Collection<WorkflowMessage> Errors {
			get { return _Errors; }
			set { _Errors = value; }
		}
		public CustomPropertyCollection Inputs {
			get { return _Inputs; }
			set { _Inputs = value; }
		}
		public Collection<WorkflowMessage> Outputs {
			get { return _Outputs; }
			set { _Outputs = value; }
		}

		public Collection<WorkflowMessage> GetCustomerVisibleErrors()
		{
			Collection<WorkflowMessage> result = new Collection<WorkflowMessage>();
			foreach (WorkflowMessage item in this.Errors) {
				if (item.CustomerVisible) {
					result.Add(item);
				}
			}
			return result;
		}

        public TaskContext()
        {
            
        }
	}
}

