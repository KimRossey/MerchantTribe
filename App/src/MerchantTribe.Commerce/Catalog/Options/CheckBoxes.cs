using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog.Options
{
    public class CheckBoxes : IOptionProcessor
    {
        public OptionTypes GetOptionType()
        {
            return OptionTypes.CheckBoxes;
        }

        public string Render(Option baseOption)
        {
            return RenderWithSelection(baseOption, null);
        }

        public string RenderWithSelection(Option baseOption, OptionSelectionList selections)
        {                             
            StringBuilder sb = new StringBuilder();
            foreach (OptionItem o in baseOption.Items)
            {
                sb.Append("<input type=\"checkbox\"");
                sb.Append(" class=\"isoption check" + baseOption.Bvin.Replace("-","") + "\" name=\"opt" + baseOption.Bvin.Replace("-", "") + "\" value=\"" + o.Bvin.Replace("-", "") + "\"");                
                if (IsSelected(o, selections))
                {
                    sb.Append(" checked=\"checked\" ");
                }
                sb.Append("/>" + o.Name + "<br />");
            }
            return sb.ToString();
        }
        private bool IsSelected(OptionItem item, OptionSelectionList selections)
        {
            bool result = false;
            if (selections == null) return false;

            OptionSelection val = selections.FindByOptionId(item.OptionBvin);
            if (val == null) return result;

            string[] vals = val.SelectionData.Split(',');
            foreach (string s in vals)
            {
                if (s == item.Bvin.Replace("-", ""))
                {
                    return true;
                }                
            }                       
            return result;
        }

        public void RenderAsControl(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {

            foreach (OptionItem o in baseOption.Items)
            {
                if (!o.IsLabel)
                {
                    System.Web.UI.HtmlControls.HtmlInputCheckBox cb = new System.Web.UI.HtmlControls.HtmlInputCheckBox();
                    cb.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                    cb.ID = "opt" + o.Bvin.Replace("-", "");
                    cb.Name = "opt" + baseOption.Bvin.Replace("-", "");
                    cb.Attributes["class"] = "isoption check" + baseOption.Bvin.Replace("-", "");
                    cb.Value = o.Bvin.Replace("-", "");
                    ph.Controls.Add(cb);
                }
                ph.Controls.Add(new System.Web.UI.LiteralControl(" " + o.Name + "<br />"));
            }
            
        }

        public Catalog.OptionSelection ParseFromPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {

            OptionSelection result = new OptionSelection();
            result.OptionBvin = baseOption.Bvin;

            string val = string.Empty;

            foreach (OptionItem o in baseOption.Items)
            {
                
                if (!o.IsLabel)
                {
                    string checkId = "opt" + o.Bvin.Replace("-", "");
                    System.Web.UI.HtmlControls.HtmlInputCheckBox cb = (System.Web.UI.HtmlControls.HtmlInputCheckBox)ph.FindControl(checkId);
                    if (cb != null)
                    {
                        if (cb.Checked)
                        {                                                     
                                string temp = "";
                                if (val.Length > 0)
                                {
                                    temp += ",";
                                }
                                temp += o.Bvin;
                                val += temp;                         
                        }
                    }
                }
            }

            result.SelectionData = val;
     
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

            string[] vals = val.SelectionData.Split(',');
            foreach (string s in vals)
            {
                string checkId = "opt" + s.Replace("-", "");
                System.Web.UI.HtmlControls.HtmlInputCheckBox cb = (System.Web.UI.HtmlControls.HtmlInputCheckBox)ph.FindControl(checkId);
                if (cb != null)
                {
                    cb.Checked = true;
                }            
            }                                           
        }

        public string CartDescription(Option baseOption, Catalog.OptionSelectionList selections)
        {
            if (selections == null) return string.Empty;
            OptionSelection val = selections.FindByOptionId(baseOption.Bvin);
            if (val == null) return string.Empty;
            string[] vals = val.SelectionData.Split(',');

            string result = baseOption.Name + ": ";
            bool first = true;

            foreach (OptionItem oi in baseOption.Items)
            {
                string cleaned = OptionSelection.CleanBvin(oi.Bvin);
                if (vals.Contains(cleaned))
                {
                    if (!first) { result += ", "; }
                    else { first = false; }                   
                    result += oi.Name;                    
                }
            }

            return result;
        }
    }
}
