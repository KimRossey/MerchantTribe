using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Utilities
{
	public abstract class UrlRewritingRule
	{

		private static Collection<UrlRewritingRule> _availableRules = null;

		public static Collection<UrlRewritingRule> AvailableRules {
			get { return _availableRules; }
			set { _availableRules = value; }
		}

		public abstract bool Execute(ref System.Web.HttpApplication app, System.Uri sourceUrl, Utilities.UrlRewriterParts parts);

	}
}
