using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class PageTitle : ITagHandler
    {
        public string TagName
        {
            get { return "sys:pagetitle"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string result = string.Empty;

            if (app.CurrentRequestContext.CurrentCategory != null)
            {
                result = app.CurrentRequestContext.CurrentCategory.MetaTitle;
                if (result == string.Empty) result = app.CurrentRequestContext.CurrentCategory.Name;
            }
            else if (app.CurrentRequestContext.CurrentProduct != null)
            {
                result = app.CurrentRequestContext.CurrentProduct.MetaTitle;
                if (result == string.Empty) result = app.CurrentRequestContext.CurrentProduct.ProductName;
            }

            if (result == string.Empty)
            {
                result = app.CurrentRequestContext.PageTitle;
            }

            return result;
        }

        public PageTitle()
        {
        }

    }
}
