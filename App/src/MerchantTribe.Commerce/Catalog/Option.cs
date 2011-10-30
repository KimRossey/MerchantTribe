using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.ObjectModel;
using MerchantTribe.Web;
using System.Xml;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    
    public class Option
    {
        public IOptionProcessor Processor { get; set; }
        public string Bvin {get;set;}
        public long StoreId {get;set;}
        public virtual OptionTypes OptionType { get { return Processor.GetOptionType(); } }
        public string Name {get;set;}
        public bool NameIsHidden {get;set;}
        public bool IsVariant {get;set;}
        public bool IsShared {get;set;}
        public OptionSettings Settings {get;set;}
        public List<OptionItem> Items {get;set;}

        public Option()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.Name = string.Empty;
            this.NameIsHidden = false;
            this.IsVariant = false;
            this.IsShared = false;
            this.Settings = new OptionSettings();
            this.Items = new List<OptionItem>();
            this.Processor = new Options.DropDownList();
        }

        public void SetProcessor(OptionTypes type)
        {
            switch (type)
            {
                case OptionTypes.CheckBoxes:
                    this.Processor = new Options.CheckBoxes();
                    break;
                case OptionTypes.DropDownList:
                    this.Processor = new Options.DropDownList();
                    break;
                case OptionTypes.FileUpload:
                    this.Processor = new Options.Html();
                    break;
                case OptionTypes.Html:
                    this.Processor = new Options.Html();
                    break;
                case OptionTypes.RadioButtonList:
                    this.Processor = new Options.RadioButtonList();
                    break;
                case OptionTypes.TextInput:
                    this.Processor = new Options.TextInput();
                    break;
            }
        }

        public static Option Factory(OptionTypes type)
        {
            Option result = new Option();
            result.SetProcessor(type);
            return result;
        }
       
        public void LoadItemsFromList(List<OptionItem> items)
        {
            if (items != null)
            {
                var parts = (from i in items
                             where i.OptionBvin == this.Bvin
                             orderby i.SortOrder
                             select i).ToList();
                if (parts != null)
                {
                    Items.Clear();
                    Items.AddRange(parts);                    
                }
            }
        }

        public string Render() 
        {
            return this.Processor.Render(this);
        }
        public string RenderWithSelection(OptionSelectionList selections)
        {
            return this.Processor.RenderWithSelection(this, selections);
        }
        public void RenderAsControl(System.Web.UI.WebControls.PlaceHolder ph)
        {
            this.Processor.RenderAsControl(this, ph);
        }
        public Catalog.OptionSelection ParseFromPlaceholder(System.Web.UI.WebControls.PlaceHolder ph)
        {
            return this.Processor.ParseFromPlaceholder(this, ph);
        }
        public Catalog.OptionSelection ParseFromForm(System.Collections.Specialized.NameValueCollection form)
        {
            return this.Processor.ParseFromForm(this, form);
        }
        public void SetSelectionsInPlaceholder(System.Web.UI.WebControls.PlaceHolder ph, Catalog.OptionSelectionList selections)
        {
            this.Processor.SetSelectionsInPlaceholder(this, ph, selections);
        }
        public string CartDescription(Catalog.OptionSelectionList selections)
        {
            return this.Processor.CartDescription(this, selections);
        }

        public bool AddItem(string itemName)
        {
            OptionItem oi = new OptionItem();
            oi.Name = itemName;
            return AddItem(oi);
        }
        public bool AddItem(OptionItem item)
        {
            if (item == null) return false;
            item.OptionBvin = this.Bvin;
            this.Items.Add(item);
            return true;
        }      
        public bool ItemsContains(string itemBvin)
        {
            // check to see if this option contains a specific item
            foreach (OptionItem oi in this.Items)
            {
                if (oi.Bvin == itemBvin) return true;
            }

            return false;
        }


        public Option Clone()
        {
            Option result = Catalog.Option.Factory(this.OptionType);

            result.Bvin = string.Empty;
            result.IsShared = this.IsShared;
            result.IsVariant = this.IsVariant;            
            foreach (OptionItem oi in this.Items)
            {
                result.Items.Add(oi.Clone());
            }
            result.Name = this.Name;
            result.NameIsHidden = this.NameIsHidden;
            foreach (var set in this.Settings)
            {
                result.Settings.AddOrUpdate(set.Key, set.Value);
            }
            result.StoreId = this.StoreId;

            return result;
        }

        //DTO
        public OptionDTO ToDto()
        {
            OptionDTO dto = new OptionDTO();

            dto.Bvin = this.Bvin;
            dto.IsShared = this.IsShared;
            dto.IsVariant = this.IsVariant;
            dto.Items = new List<OptionItemDTO>();
            foreach (OptionItem oi in this.Items)
            {
                dto.Items.Add(oi.ToDto());
            }
            dto.Name = this.Name;
            dto.NameIsHidden = this.NameIsHidden;
            dto.OptionType = (OptionTypesDTO)((int)this.OptionType);
            dto.Settings = new List<OptionSettingDTO>();
            foreach (var set in this.Settings)
            {
                OptionSettingDTO setdto = new OptionSettingDTO();
                setdto.Key = set.Key;
                setdto.Value = set.Value;
                dto.Settings.Add(setdto);
            }           
            dto.StoreId = this.StoreId;
            
            return dto;
        }
        public void FromDto(OptionDTO dto)
        {
            if (dto == null) return;

            this.Bvin = dto.Bvin ?? string.Empty;
            this.IsShared = dto.IsShared;
            this.IsVariant = dto.IsVariant;
            this.Items.Clear();
            this.Name = dto.Name ?? string.Empty;
            this.NameIsHidden = dto.NameIsHidden;                        
            this.StoreId = dto.StoreId;
            
            OptionTypes typeCode = OptionTypes.DropDownList;
            typeCode = (OptionTypes)dto.OptionType;
            this.SetProcessor(typeCode);

            foreach (OptionItemDTO oi in dto.Items)
            {
                OptionItem opt = new OptionItem();
                opt.FromDto(oi);
                this.AddItem(opt);                
            }
            this.Settings = new OptionSettings();
            foreach (OptionSettingDTO set in dto.Settings)
            {
                this.Settings.AddOrUpdate(set.Key, set.Value);
            }           
        }



       
    }
}
