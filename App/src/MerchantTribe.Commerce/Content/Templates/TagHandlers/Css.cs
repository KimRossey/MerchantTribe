using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class Css : ITagHandler
    {
        public string TagName
        {
            get { return "sys:css"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            string fileUrl = string.Empty;
            bool secure = app.CurrentRequestContext.RoutingContext.HttpContext.Request.IsSecureConnection;

            ThemeManager tm = app.ThemeManager();

            string mode = tag.GetSafeAttribute("mode");
            if (mode == "legacy")
            {
                fileUrl = tm.CurrentStyleSheet(app, secure);
            }
            else if (mode == "system")
            {
                string cssFile = tag.GetSafeAttribute("file");
                fileUrl = app.StoreUrl(secure, false) + cssFile.TrimStart('/');
            }
            else
            {
                string fileName = tag.GetSafeAttribute("file");
                fileUrl = tm.ThemeFileUrl(fileName, app);
            }

            string result = string.Empty;
            result = "<link href=\"" + fileUrl + "\" rel=\"stylesheet\" type=\"text/css\" />";
            return result;
        }
    }
}
