using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog.Options
{
    public class DropDownList : IOptionProcessor
    {
        public OptionTypes GetOptionType()
        {
            return OptionTypes.DropDownList;
        }

        public string Render(Option baseOption)
        {
            return RenderWithSelection(baseOption, null);
        }

        public string RenderWithSelection(Option baseOption, OptionSelectionList selections)
        {
            string selected = string.Empty;
            if (selections != null)
            {
                OptionSelection sel = selections.FindByOptionId(baseOption.Bvin);
                if (sel != null)
                {
                    selected = sel.SelectionData;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("<select id=\"opt" + baseOption.Bvin.Replace("-", "") + "\" ");
            sb.Append(" name=\"opt" + baseOption.Bvin.Replace("-", "") + "\" ");
            sb.Append(" class=\"isoption\" >");

            foreach (OptionItem o in baseOption.Items)
            {
                if (o.IsLabel)
                {
                    sb.Append("<option value=\"systemlabel\">" + o.Name + "</option>");
                }
                else
                {
                    sb.Append("<option value=\"" + o.Bvin.Replace("-", "") + "\"");
                    if (o.Bvin.Replace("-","") == selected)
                    {
                        sb.Append(" selected ");
                    }
                    sb.Append(">" + o.Name + "</option>");
                }
            }
            sb.Append("</select>");

            return sb.ToString();
        }

        public void RenderAsControl(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            System.Web.UI.WebControls.DropDownList result = new System.Web.UI.WebControls.DropDownList();
            result.ID = "opt" + baseOption.Bvin.Replace("-", "");
            result.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            result.CssClass = "isoption";

            foreach (OptionItem o in baseOption.Items)
            {
                if (o.IsLabel)
                {
                    result.Items.Add(new System.Web.UI.WebControls.ListItem(o.Name, "systemlabel"));                    
                }
                else
                {
                    result.Items.Add(new System.Web.UI.WebControls.ListItem(o.Name, o.Bvin.Replace("-","")));                    
                }
            }

            ph.Controls.Add(result);
        }

        public Catalog.OptionSelection ParseFromPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            OptionSelection result = new OptionSelection();
            result.OptionBvin = baseOption.Bvin;

            System.Web.UI.WebControls.DropDownList ddl = (System.Web.UI.WebControls.DropDownList)ph.FindControl("opt" + baseOption.Bvin.Replace("-", ""));
            if (ddl != null)
            {
                if (ddl.SelectedItem != null)
                {
                    // Why was I parsing Guid only to return as string?
                    // Safety check maybe?
                    //result.SelectionData = new System.Guid(ddl.SelectedItem.Value).ToString();

                    // Removed GUID requirement for BVC2004 migration compatibility
                    string temp = ddl.SelectedItem.Value;
                    result.SelectionData = temp;
                    
                }
            }

            return result;
        }
        public Catalog.OptionSelection ParseFromForm(Option baseOption, System.Collections.Specialized.NameValueCollection form)
        {
            OptionSelection result = new OptionSelection();
            result.OptionBvin = baseOption.Bvin;
            string formid = "opt" + baseOption.Bvin.Replace("-", "");
            string value = form[formid];
            if (value != null)
            {
                result.SelectionData = value;
            }
            return result;
        }

        public void SetSelectionsInPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph, Catalog.OptionSelectionList selections)
        {
            if (ph == null) return;
            if (selections == null) return;
            OptionSelection val = selections.FindByOptionId(baseOption.Bvin);
            if (val == null) return;

            System.Web.UI.WebControls.DropDownList ddl = (System.Web.UI.WebControls.DropDownList)ph.FindControl("opt" + baseOption.Bvin.Replace("-", ""));
            if (ddl != null)
            {
                if (ddl.Items.FindByValue(val.SelectionData) != null)
                {
                    ddl.ClearSelection();
                    ddl.Items.FindByValue(val.SelectionData).Selected = true;
                }                
            }            
        }

        public string CartDescription(Option baseOption, Catalog.OptionSelectionList selections)
        {
            if (selections == null) return string.Empty;
            OptionSelection val = selections.FindByOptionId(baseOption.Bvin);
            if (val == null) return string.Empty;

            foreach (OptionItem oi in baseOption.Items)
            {
                string cleaned = OptionSelection.CleanBvin(oi.Bvin);
                if (cleaned == val.SelectionData)
                {
                    return baseOption.Name + ": " + oi.Name;
                }
            }

            return string.Empty;
        }

    }
}
