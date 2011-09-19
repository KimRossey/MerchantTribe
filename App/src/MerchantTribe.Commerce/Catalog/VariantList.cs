using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class VariantList: List<Variant>
    {
        public bool ContainsKey(string uniqueKey)
        {
            if (FindByKey(uniqueKey) != null) return true;

            return false;
        }

        public Variant FindByKey(string uniqueKey)
        {
            foreach (Variant v in this)
            {
                if (v.UniqueKey() == uniqueKey)
                {
                    return v;
                }
            }
            return null;
        }
        public Variant FindByBvin(string bvin)
        {
            foreach (Variant v in this)
            {
                if (v.Bvin == bvin)
                {
                    return v;
                }
            }
            return null;
        }               
        public Variant FindBySelectionData(OptionSelectionList selections, OptionList options)
        {
            OptionSelectionList variantSelections = new OptionSelectionList();

            foreach (Option opt in options)
            {
                if (opt.IsVariant)
                {
                    OptionSelection sel = selections.FindByOptionId(opt.Bvin);
                    if (sel != null)
                    {
                        variantSelections.Add(sel);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            string selectionKey = OptionSelection.GenerateUniqueKeyForSelections(variantSelections);
            return this.FindByKey(selectionKey);            
        }
    }
}
