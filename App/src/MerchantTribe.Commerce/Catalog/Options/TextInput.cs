using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog.Options
{
    public class TextInput: IOptionProcessor
    {        
        public OptionTypes GetOptionType()
        {
            return OptionTypes.TextInput;
        }

        public void SetRows(Option baseOption, string value)
        {
            baseOption.Settings.AddOrUpdate("rows", value);
        }
        public string GetRows(Option baseOption)
        {
            return baseOption.Settings.GetSettingOrEmpty("rows");
        }
        public void SetColumns(Option baseOption, string value)
        {
            baseOption.Settings.AddOrUpdate("cols", value);
        }
        public string GetColumns(Option baseOption)
        {
            return baseOption.Settings.GetSettingOrEmpty("cols");
        }
        public void SetMaxLength(Option baseOption, string value)
        {
            baseOption.Settings.AddOrUpdate("maxlength", value);
        }
        public string GetMaxLength(Option baseOption)
        {
            return baseOption.Settings.GetSettingOrEmpty("maxlength");
        }

        //set { BaseOption.Settings.AddOrUpdate("rows", value); }
        //set { BaseOption.Settings.AddOrUpdate("cols", value); }

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
            string c = this.GetColumns(baseOption);
            if (c == "") c = "20";
            string r = this.GetRows(baseOption);
            if (r == "") r = "1";
            
            if (r != "1")
            {
                sb.Append("<textarea id=\"opt" + baseOption.Bvin.Replace("-","") + "\" cols=\"" + c + "\" rows=\"" + r + "\" ");
                sb.Append(" name=\"opt" + baseOption.Bvin.Replace("-", "") + "\" ");
                sb.Append(">");
                sb.Append(selected);
                sb.Append("</textarea>");
            }
            else
            {                
                sb.Append("<input type=\"text\" id=\"opt" + baseOption.Bvin.Replace("-","") + "\" cols=\"" + c + "\" maxlength=\"" + this.GetMaxLength(baseOption) + "\"");
                sb.Append(" name=\"opt" + baseOption.Bvin.Replace("-", "") + "\" ");
                sb.Append(" value=\"" + selected + "\"");
                sb.Append("/>");
            }

            return sb.ToString();
        }

        public void RenderAsControl(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            System.Web.UI.WebControls.TextBox result = new System.Web.UI.WebControls.TextBox();
            result.ID = "opt" + baseOption.Bvin.Replace("-", "");
            result.ClientIDMode = System.Web.UI.ClientIDMode.Static;

            string c = this.GetColumns(baseOption);
            if (c == "") c = "20";
            string r = this.GetRows(baseOption);
            if (r == "") r = "1";

            int rint = 1;
            int.TryParse(r,out rint);
            int cint = 20;
            int.TryParse(c,out  cint);
            int mint = 255;
            int.TryParse(this.GetMaxLength(baseOption),out mint);

            result.Rows = rint;
            result.Columns = cint;
            result.MaxLength = mint;

            if (r != "1")
            {
                result.TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine;            
            }

            ph.Controls.Add(result);                    
        }

        public Catalog.OptionSelection ParseFromPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            OptionSelection result = new OptionSelection();
            result.OptionBvin = baseOption.Bvin;

            System.Web.UI.WebControls.TextBox tb = (System.Web.UI.WebControls.TextBox)ph.FindControl("opt" + baseOption.Bvin.Replace("-", ""));
            if (tb != null)
            {
                result.SelectionData = tb.Text.Trim();
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
                result.SelectionData = value.Trim();
            }
            return result;
        }

        public void SetSelectionsInPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph, Catalog.OptionSelectionList selections)
        {
            if (ph == null) return;
            if (selections == null) return;
            OptionSelection val = selections.FindByOptionId(baseOption.Bvin);
            if (val == null) return;

            System.Web.UI.WebControls.TextBox tb = (System.Web.UI.WebControls.TextBox)ph.FindControl("opt" + baseOption.Bvin.Replace("-", ""));
            if (tb != null)
            {
                tb.Text = val.SelectionData;                
            }

        }

        public string CartDescription(Option baseOption, Catalog.OptionSelectionList selections)
        {
            if (selections == null) return string.Empty;
            OptionSelection val = selections.FindByOptionId(baseOption.Bvin);
            if (val == null) return string.Empty;
            
            if (val.SelectionData.Trim().Length > 0)
            {
                return baseOption.Name + ": " + System.Web.HttpUtility.HtmlEncode(val.SelectionData);
            }

            return string.Empty;
        }


    }
}
