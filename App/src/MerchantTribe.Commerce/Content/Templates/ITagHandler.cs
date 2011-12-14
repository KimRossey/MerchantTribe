using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates
{
    public interface ITagHandler
    {
        string TagName { get; }
        string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents);
    }
}
