using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace MerchantTribeStore
{
    public static class HtmlExtensions
    {
        public static string SafeRegisterScript(this HtmlHelper helper, string scriptName)
        {
            string keyName = "SafeRegisteredScripts";
            object list = helper.ViewData[keyName];
            if (list == null) list = new List<string>();

            if (list != null)
            {
                List<string> registered = (List<string>)list;
                if (registered != null)
                {
                    if (!registered.Contains(scriptName))
                    {
                        registered.Add(scriptName);
                    }
                    helper.ViewData[keyName] = registered;
                }
            }
            return string.Empty;
        }
    }
}