using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Catalog
{
    public class OptionList: List<Option>
    {
        public List<Option> VariantsOnly()
        {
            return this.AsQueryable().Where(y => y.IsVariant == true).ToList();
        }

        public bool ContainsVariantSelection(OptionSelection selection)
        {
            // look through a list of options to see if it contains a valid option
            // for the given selection data

            bool result = false;

            foreach (Option o in this.VariantsOnly())
            {                
                    if (o.Bvin == selection.OptionBvin)
                    {
                        if (o.ItemsContains(selection.SelectionData))
                        {
                            return true;
                        }
                    }                
            }

            return result;
        }
       
    }
}
