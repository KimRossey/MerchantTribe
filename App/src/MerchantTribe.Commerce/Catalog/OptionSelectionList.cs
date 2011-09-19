using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MerchantTribe.Commerce.Catalog
{
    public class OptionSelectionList: List<OptionSelection>
    {
        public bool ContainsSelectionForOption(string optionBvin)
        {
            string cleaned = OptionSelection.CleanBvin(optionBvin);

            foreach (OptionSelection os in this)
            {
                if (os.OptionBvin == cleaned)
                {
                    return true;
                }
            }
            return false;
        }

        public OptionSelection FindByOptionId(string optionId)
        {
            string cleaned = OptionSelection.CleanBvin(optionId);

            foreach (OptionSelection os in this)
            {
                if (os.OptionBvin == cleaned)
                {
                    return os;
                }
            }
            return null;
        }

        public void DeserializeFromXml(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return;
            }

            try
            {
                this.Clear();

                XDocument doc = XDocument.Load(new StringReader(xml));

                var selections = from sel in doc.Descendants("OptionSelection")
                                 select new
                                 {
                                     OptionBvin = sel.Element("OptionBvin").Value,
                                     SelectionData = sel.Element("SelectionData").Value
                                 };

                foreach (var selection in selections)
                {
                    Catalog.OptionSelection sel = new OptionSelection();
                    sel.OptionBvin = selection.OptionBvin;
                    sel.SelectionData = selection.SelectionData;
                    this.Add(sel);
                }
            
            }
            catch (Exception ex)
            {
                this.Clear();
                EventLog.LogEvent(ex);
            }            
        }

        public string SerializeToXml()
        {
            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter xw = new XmlTextWriter(sw))
                {
                    if (xw != null)
                    {
                        xw.WriteStartElement("OptionSelections");

                        foreach (OptionSelection sel in this)
                        {
                            xw.WriteStartElement("OptionSelection");
                            xw.WriteElementString("OptionBvin", sel.OptionBvin);
                            xw.WriteElementString("SelectionData", sel.SelectionData);
                            xw.WriteEndElement();
                        }

                        xw.WriteEndElement();
                    }
                }
                return sw.ToString();
            }
        }

        public decimal GetPriceAdjustmentForSelections(OptionList allOptions)
        {
            decimal result = 0;

            foreach (OptionSelection selection in this)
            {
                foreach (Option opt in allOptions)
                {
                    if (opt.Items != null)
                    {
                        foreach (OptionItem oi in opt.Items)
                        {
                            string cleaned = OptionSelection.CleanBvin(oi.Bvin);
                            if (cleaned == selection.SelectionData)
                            {
                                if (oi.PriceAdjustment != 0)
                                {
                                    result += oi.PriceAdjustment;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
        public decimal GetWeightAdjustmentForSelections(OptionList allOptions)
        {
            decimal result = 0;

            foreach (OptionSelection selection in this)
            {
                foreach (Option opt in allOptions)
                {
                    if (opt.Items != null)
                    {
                        foreach (OptionItem oi in opt.Items)
                        {
                            string cleaned = OptionSelection.CleanBvin(oi.Bvin);
                            if (cleaned == selection.SelectionData)
                            {
                                if (oi.WeightAdjustment != 0)
                                {
                                    result += oi.WeightAdjustment;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool HasLabelsSelected()
        {
            foreach (OptionSelection os in this)
            {
                if (os.SelectionData == "systemlabel")
                {
                    return true;
                }
            }
            return false;
        }
    }
}
