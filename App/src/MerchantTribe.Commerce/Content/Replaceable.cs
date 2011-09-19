using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    /// <summary>
    /// Holds a temporary list of replacement tags
    /// Allows you to generate "one off" tags or lists as needed
    /// Used by the "new password" email messages
    /// </summary>
    public class Replaceable: IReplaceable
    {
        public List<HtmlTemplateTag> Tags { get; set; }

        public Replaceable()
        {
            Tags = new List<HtmlTemplateTag>();
        }
        public Replaceable(string tagName, string replacement)
        {
            Tags = new List<HtmlTemplateTag>();
            HtmlTemplateTag t = new HtmlTemplateTag() { Replacement = replacement, Tag = tagName };
            Tags.Add(t);
        }

        public List<HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
            return Tags;
        }
    }
}
