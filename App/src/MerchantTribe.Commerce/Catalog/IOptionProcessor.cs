using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public interface IOptionProcessor
    {                
        OptionTypes GetOptionType();
        string Render(Option baseOption);
        string RenderWithSelection(Option baseOption, OptionSelectionList selections);
        void RenderAsControl(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph);
        Catalog.OptionSelection ParseFromPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph);
        Catalog.OptionSelection ParseFromForm(Option baseOption, System.Collections.Specialized.NameValueCollection form);
        void SetSelectionsInPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph, Catalog.OptionSelectionList selections);
        string CartDescription(Option baseOption, Catalog.OptionSelectionList selections);
    }
}
