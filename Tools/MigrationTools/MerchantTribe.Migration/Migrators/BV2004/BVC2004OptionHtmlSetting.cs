using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Migration.Migrators.BV2004
{
    public class BVC2004OptionHtmlSetting
    {
        public string HtmlData { get; set; }

        public BVC2004OptionHtmlSetting()
        {
            HtmlData = string.Empty;
        }
    }
}
