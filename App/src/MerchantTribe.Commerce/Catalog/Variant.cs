using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MerchantTribe.Web;
using System.Xml;
namespace MerchantTribe.Commerce.Catalog
{
    public class Variant
    {
        public long StoreId { get; set; }
        public string Bvin {get;set;}
        public string ProductId {get;set;}
        public string Sku {get;set;}
        public decimal Price {get;set;}
        public OptionSelectionList Selections {get; private set;}

        public Variant()
        {
            this.StoreId = 0;
            this.Bvin = string.Empty;
            this.ProductId = string.Empty;
            this.Sku = string.Empty;
            this.Price = -1;
            this.Selections = new OptionSelectionList();
        }

        public string UniqueKey()
        {
            return OptionSelection.GenerateUniqueKeyForSelections(Selections);
        }

        public List<string> SelectionNames(List<Option> options)
        {
            List<string> result = new List<string>();

            foreach (Option opt in options)
            {
                OptionSelection sel = Selections.FindByOptionId(opt.Bvin);
                if (sel != null)
                {                
                    string itemBvin = sel.SelectionData;
                    foreach (OptionItem oi in opt.Items)
                    {
                        string cleaned = OptionSelection.CleanBvin(oi.Bvin);
                        if (cleaned == itemBvin)
                        {
                            result.Add(oi.Name);
                            break;
                        }
                    }
                }
            }

            return result;
        }        

    }
}
