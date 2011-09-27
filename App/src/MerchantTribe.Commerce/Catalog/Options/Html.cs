using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog.Options
{
    public class Html: IOptionProcessor
    {
        public OptionTypes GetOptionType()
        {
            return OptionTypes.Html;
        }

        public string Render(Option baseOption)
        {
            return baseOption.Settings.GetSettingOrEmpty("html");            
        }

        public string RenderWithSelection(Option baseOption, OptionSelectionList selections)
        {
            return Render(baseOption);
        }

        public void RenderAsControl(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            System.Web.UI.LiteralControl result = new System.Web.UI.LiteralControl(baseOption.Settings.GetSettingOrEmpty("html"));
            ph.Controls.Add(result);
        }

        public Catalog.OptionSelection ParseFromPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            return null;
        }
        public Catalog.OptionSelection ParseFromForm(Option baseOption, System.Collections.Specialized.NameValueCollection form)
        {
            return null;
        }

        public void SetSelectionsInPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph, Catalog.OptionSelectionList selections)
        {
            // do nothing;
        }

        public string CartDescription(Option baseOption, Catalog.OptionSelectionList selections)
        {
            return string.Empty;
        }


    }
}
