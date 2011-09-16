using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class Products
    {

        public static ProductData GetProductByUniqueId(string uniqueId, ServiceContext context)
        {
            ProductData result = new ProductData();

            try
            {
                IN202500Content stock = context.IN202500_Schema;

                context.Gate.IN202500Clear();
                IN202500Content[] stockResult = context.Gate.IN202500Submit(new Command[]
                {
                    new Key() { Value = "='" + uniqueId + "'", 
                        FieldName = stock.InventoryItem.InventoryID.FieldName,
                        ObjectName = stock.InventoryItem.InventoryID.ObjectName},
                    stock.InventoryItem.InventoryID,
                    stock.InventoryItem.Description,
                    stock.GeneralSettingsItemDefaults.ItemClass,
                    stock.GeneralSettingsItemDefaults.Type,
                    stock.PriceCostInfoBasePrice.CurrentPrice,
                    stock.AttributesDimensions.Weight,
                });
                if (stockResult.Length > 0)
                {
                    result.LoadFromRowData(stockResult[0]);
                    context.LogMessage("Item was found with ID:" + uniqueId.Trim().ToLowerInvariant());
                    return result;
                }


                IN202000Content nonstock = context.IN202000_Schema;

                context.Gate.IN202500Clear();
                IN202000Content[] nonstockResult = context.Gate.IN202000Submit(new Command[]
                {
                    new Key() { Value = "='" + uniqueId + "'", 
                        FieldName = nonstock.NonStockItem.InventoryID.FieldName,
                        ObjectName = nonstock.NonStockItem.InventoryID.ObjectName},
                    nonstock.NonStockItem.InventoryID,
                    nonstock.NonStockItem.Description,
                    nonstock.GeneralSettingsItemDefaults.ItemClass,
                    nonstock.GeneralSettingsItemDefaults.Type,
                    nonstock.PriceCostInformationBasePrice.CurrentPrice,
                    //nonstock.AttributesDimensions.Weight,
                });
                if (nonstockResult.Length > 0)
                {
                    result.LoadFromRowData(nonstockResult[0]);
                    context.LogMessage("Item was found with ID:" + uniqueId.Trim().ToLowerInvariant());
                    return result;
                }

                context.LogMessage("Item was not found!");
            }
            catch (Exception ex)
            {
                context.Errors.Add(new ServiceError() { Description = ex.Message + " " + ex.StackTrace });
            }

            return result;
        }

        public static ProductData GetOrCreateProduct(ProductData data, ServiceContext context)
        {
            ProductData result = new ProductData();

            // Check for existing Customer
            result = GetProductByUniqueId(data.UniqueId, context);
            
            if (!String.IsNullOrEmpty(result.UniqueId))
            {
                return result;
            }

            // Create Customer
            result = CreateNewProduct(data, context);
            return result;
        }

        public static ProductData CreateNewProduct(ProductData data, ServiceContext context)
        {
            context.Errors.Clear();
            
            string sku = string.Empty;
            data.Warehouse = context.NewItemWarehouseId;

			IN202500Content schema = context.IN202500_Schema;
            try
            {
				context.Gate.IN202500Clear();
				IN202500Content[] content = context.Gate.IN202500Submit(new Command[]
				{
					new Value {Value = data.UniqueId, LinkedCommand = schema.InventoryItem.InventoryID },
					new Value {Value = data.Description, LinkedCommand = schema.InventoryItem.Description },
					new Value {Value = data.Price.ToString(), LinkedCommand = schema.PriceCostInfoBasePrice.CurrentPrice},
					new Value {Value = data.BaseWeight.ToString(), LinkedCommand = schema.AttributesDimensions.Weight},
					new Value {Value = context.NewItemTaxAccountId, LinkedCommand = schema.GeneralSettingsItemDefaults.TaxCategory },
                    new Value {Value = data.Warehouse.Trim(), LinkedCommand = schema.GeneralSettingsStorageDefaults.DefaultWarehouse},
					schema.InventoryItem.InventoryID,
					schema.Actions.Save,
				});
            }
            catch (Exception ex)
            {
                context.Errors.Add(new ServiceError() { Description = ex.Message + " " + ex.StackTrace });
            }

            ProductData result = GetProductByUniqueId(data.UniqueId, context);
            return result;
        }

        public static List<AccountDescriptor> ListAllWarehouses(ServiceContext context)
        {
			List<AccountDescriptor> warehouses = new List<AccountDescriptor>();

			IN204000Content schema = context.IN204000_Schema;

			context.Gate.CS207500Clear();
			IN204000Content[] result = context.Gate.IN204000Submit(new Command[]
			{
				schema.WarehouseSettings.ServiceCommands.EveryWarehouseID,
				schema.WarehouseSettings.WarehouseID,
				schema.WarehouseSettings.Description,
			});

			foreach (IN204000Content carrier in result)
			{
				warehouses.Add(new AccountDescriptor() { Id = carrier.WarehouseSettings.WarehouseID.Value, Description = carrier.WarehouseSettings.Description.Value });
			}

			return warehouses;
        }
    }
}
