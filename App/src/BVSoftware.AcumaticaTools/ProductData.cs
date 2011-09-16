using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class ProductData
    {
        public string UniqueId { get; set; }
        public string Bvin { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        //public decimal? QuantityOnHand { get; set; }
        public decimal? BaseWeight { get; set; }
        public string Warehouse { get; set; }

        public ProductData()
        {
            UniqueId = string.Empty;
            Bvin = string.Empty;
            Description = string.Empty;
            Price = 0;
            BaseWeight = 0;
            Warehouse = string.Empty;
        }

        public void LoadFromRowData(IN202500Content rowData)
        {
            this.UniqueId = rowData.InventoryItem.InventoryID.Value;
			this.Description = rowData.InventoryItem.Description.Value;
			//this.QuantityOnHand = rowData.QtyOnHand;

			if(!String.IsNullOrEmpty(rowData.PriceCostInfoBasePrice.CurrentPrice.Value))
				this.Price =  Decimal.Parse(rowData.PriceCostInfoBasePrice.CurrentPrice.Value);
			if (!String.IsNullOrEmpty(rowData.AttributesDimensions.Weight.Value))
				this.BaseWeight = Decimal.Parse(rowData.AttributesDimensions.Weight.Value);	
        }

        public void LoadFromRowData(IN202000Content rowData)
        {
            this.UniqueId = rowData.NonStockItem.InventoryID.Value;
            this.Description = rowData.NonStockItem.Description.Value;
            //this.QuantityOnHand = rowData.QtyOnHand;

            if (!String.IsNullOrEmpty(rowData.PriceCostInformationBasePrice.CurrentPrice.Value))
                this.Price = Decimal.Parse(rowData.PriceCostInformationBasePrice.CurrentPrice.Value);
            //if (!String.IsNullOrEmpty(rowData.AttributesDimensions.Weight.Value))
            //    this.BaseWeight = Decimal.Parse(rowData.AttributesDimensions.Weight.Value);
        }

    }
}
