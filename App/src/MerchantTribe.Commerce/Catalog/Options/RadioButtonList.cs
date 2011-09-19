﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog.Options
{
    public class RadioButtonList : IOptionProcessor
    {
        public OptionTypes GetOptionType()
        {
            return OptionTypes.RadioButtonList;
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
            foreach (OptionItem o in baseOption.Items)
            {
                sb.Append("<input type=\"radio\" name=\"opt" + baseOption.Bvin.Replace("-", "") + "\" value=\"" + o.Bvin.Replace("-", "") + "\"");                
                sb.Append(" class=\"isoption\" ");

                string cleaned = OptionSelection.CleanBvin(o.Bvin);
                if (cleaned == selected)
                {
                    sb.Append(" checked=\"checked\" ");
                }
                sb.Append("/>" + o.Name + "<br />");
            }
            return sb.ToString();
        }

        public void RenderAsControl(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {

            foreach (OptionItem o in baseOption.Items)
            {
                if (!o.IsLabel)
                {
                    System.Web.UI.HtmlControls.HtmlInputRadioButton rb = new System.Web.UI.HtmlControls.HtmlInputRadioButton();                                        
                    rb.ClientIDMode = System.Web.UI.ClientIDMode.Static;                         
                    rb.ID = "opt" + o.Bvin.Replace("-", "");
                    rb.Name = "opt" + baseOption.Bvin.Replace("-", "");
                    rb.Attributes["class"] = "isoption radio" + baseOption.Bvin.Replace("-", "");
                    rb.Value = o.Bvin.Replace("-", "");
                    ph.Controls.Add(rb);
                }
                ph.Controls.Add(new System.Web.UI.LiteralControl(" " + o.Name + "<br />"));                           
            }

        }

        public Catalog.OptionSelection ParseFromPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph)
        {
            OptionSelection result = new OptionSelection();
            result.OptionBvin = baseOption.Bvin;

            foreach (OptionItem o in baseOption.Items)
            {
                if (!o.IsLabel)
                {
                    string radioId = "opt" + o.Bvin.Replace("-", "");
                    System.Web.UI.HtmlControls.HtmlInputRadioButton rb = (System.Web.UI.HtmlControls.HtmlInputRadioButton)ph.FindControl(radioId);
                    if (rb != null)
                    {
                        if (rb.Checked)
                        {
                            result.SelectionData = o.Bvin;
                            return result;
                        }
                    }
                }
            }

            return result;
        }

        public void SetSelectionsInPlaceholder(Option baseOption, System.Web.UI.WebControls.PlaceHolder ph, Catalog.OptionSelectionList selections)
        {
            if (ph == null) return;
            if (selections == null) return;
            OptionSelection val = selections.FindByOptionId(baseOption.Bvin);
            if (val == null) return;

            string radioId = "opt" + val.SelectionData.Replace("-","");
            System.Web.UI.HtmlControls.HtmlInputRadioButton rb = (System.Web.UI.HtmlControls.HtmlInputRadioButton)ph.FindControl(radioId);
            if (rb != null)
            {
                rb.Checked = true;
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
