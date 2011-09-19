using System;

namespace MerchantTribe.Commerce.Membership
{
	public class UserQuestionOption
	{
        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Value { get; set; }

		public UserQuestionOption()
		{
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.Value = string.Empty;
		}

		public UserQuestionOption(string val): this()
		{
            this.Value = val;
		}
	
	}
}
