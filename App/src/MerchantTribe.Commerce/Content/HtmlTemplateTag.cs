using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Content
{

	public class HtmlTemplateTag
	{

		private string _Tag = string.Empty;
		private string _Replacement = string.Empty;

		public string Tag {get;set;}
		public string Replacement {get;set;}

		public HtmlTemplateTag()
		{
            Tag = string.Empty;
            Replacement = string.Empty;
		}

		public HtmlTemplateTag(string tagValue, string replacementValue)
		{
			Tag = tagValue;
			Replacement = replacementValue;
		}

		public string ReplaceTags(string input)
		{
            string result = input;
	        result = input.Replace(Tag, Replacement);
            return result;
		}

	}
}

