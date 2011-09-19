using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    public class OptionSelection
    {
        private string _OptionBvin = string.Empty;
        private string _SelectionData = string.Empty;

        public static string CleanBvin(string input)
        {
            return input.Replace("-", "");
        }

        public string OptionBvin
        {
            get { return _OptionBvin; }
            set { _OptionBvin = CleanBvin(value); }
        }
        public string SelectionData
        {
            get { return _SelectionData; }
            set { _SelectionData = CleanBvin(value); }
        }
        public OptionSelection()
        {

        }
        public OptionSelection(string optionBvin, string selectionData)
        {
            _OptionBvin = CleanBvin(optionBvin);
            _SelectionData = CleanBvin(selectionData);
        }

        public static string GenerateUniqueKeyForSelections(List<OptionSelection> selections)
        {
            string result = string.Empty;

            if (selections == null) return result;
            if (selections.Count < 1) return result;

            var sorted = selections.OrderBy(y => y.OptionBvin);
            foreach (OptionSelection s in sorted)
            {
                result += s.OptionBvin + "-" + s.SelectionData + "|";            
            }

            return result;
        }

        public static bool ContainsInvalidSelectionForOptions(OptionList options, List<OptionSelection> selections)
        {
            // Checks to see if a list of selection data contains a selection 
            // that isn't a valid variant in a list of options

            bool result = false;

            foreach( OptionSelection sel in selections)
            {
                if (!options.ContainsVariantSelection(sel))
                {
                    return true;
                }
            }
                        
            return result;
        }

        //DTO
        public OptionSelectionDTO ToDto()
        {
            OptionSelectionDTO dto = new OptionSelectionDTO();

            dto.OptionBvin = this.OptionBvin;
            dto.SelectionData = this.SelectionData;

            return dto;
        }
        public void FromDto(OptionSelectionDTO dto)
        {
            if (dto == null) return;

            this.OptionBvin = dto.OptionBvin ?? string.Empty;
            this.SelectionData = dto.SelectionData ?? string.Empty;
        }
    }
}
