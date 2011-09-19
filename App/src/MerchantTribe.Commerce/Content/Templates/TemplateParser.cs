using System;
using System.IO;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Content.Templates
{

	public class TemplateParser
	{

		public static Collection<ParserMessage> ValidateProductPageTemplate(string template)
		{
			Collection<ParserMessage> result = new Collection<ParserMessage>();



			return result;
		}

		public static bool GenerateProductPage(string template)
		{
			Collection<ParserMessage> messages = ValidateProductPageTemplate(template);
			if (messages.Count > 0) {
				return false;
			}
			else {
				// Run Parser

				return true;
			}
		}

	}
}
