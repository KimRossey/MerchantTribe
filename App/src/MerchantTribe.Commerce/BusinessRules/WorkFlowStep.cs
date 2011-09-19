using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.BusinessRules
{
	public class WorkFlowStep
    {

        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }						
		public string WorkFlowBvin {get;set;}
		public int SortOrder {get;set;}
		public string ControlName {get;set;}
		public string DisplayName {get;set;}

        private string _StepName = string.Empty;
		public string StepName {
			get {
				if (_StepName == string.Empty) {
					return DisplayName;
				}
				else {
					return _StepName;
				}
			}
			set { _StepName = value; }
		}
        public WorkFlowStep()
        {
            this.WorkFlowBvin = string.Empty;
            this.SortOrder = 1;
            this.ControlName = string.Empty;
            this.DisplayName = string.Empty;
            this._StepName = string.Empty;
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
        }

	}
}

