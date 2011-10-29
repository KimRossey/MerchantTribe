using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using MerchantTribe.Commerce.Marketing;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Marketing;
using MerchantTribe.CommerceDTO.v1.Catalog;
using MerchantTribe.CommerceDTO.v1.Shipping;
using MerchantTribe.CommerceDTO.v1.Orders;
using MerchantTribe.CommerceDTO.v1;

namespace MerchantTribe.Commerce.Orders
{

    public class LineItem : IEquatable<LineItem>, Taxes.ITaxable, Content.IReplaceable
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public System.DateTime LastUpdatedUtc { get; set; }
        public decimal BasePricePerItem { get; set; }
        public List<Marketing.DiscountDetail> DiscountDetails { get; set; }
        public string OrderBvin { get; set; }
        public string ProductId { get; set; }
        public string VariantId { get; set; }
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string ProductShortDescription { get; set; }
        public int Quantity { get; set; }
        public int QuantityReturned { get; set; }
        public int QuantityShipped { get; set; }
        public decimal ShippingPortion { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public decimal TaxPortion { get; set; }
        public Catalog.OptionSelectionList SelectionData { get; set; }
        public long ShippingSchedule { get; set; }
        public long TaxSchedule { get; set; }
        public decimal ProductShippingWeight { get; set; }
        public decimal ProductShippingLength { get; set; }
        public decimal ProductShippingWidth { get; set; }
        public decimal ProductShippingHeight { get; set; }
        public CustomPropertyCollection CustomProperties { get; set; }
        public Shipping.ShippingMode ShipFromMode { get; set; }
        public string ShipFromNotificationId { get; set; }
        public Contacts.Address ShipFromAddress { get; set; }
        public bool ShipSeparately { get; set; }
        public decimal ExtraShipCharge { get; set; }

        // CONSTRUCTORS
        private void Init()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.BasePricePerItem = 0m;
            this.DiscountDetails = new List<Marketing.DiscountDetail>();
            this.OrderBvin = string.Empty;
            this.ProductId = string.Empty;
            this.VariantId = string.Empty;
            this.ProductName = string.Empty;
            this.ProductSku = string.Empty;
            this.ProductShortDescription = string.Empty;
            this.Quantity = 1;
            this.QuantityReturned = 0;
            this.QuantityShipped = 0;
            this.ShippingPortion = 0m;
            this.StatusCode = string.Empty;
            this.StatusName = string.Empty;
            this.TaxPortion = 0m;
            this.SelectionData = new Catalog.OptionSelectionList();
            this.ShippingSchedule = 0;
            this.TaxSchedule = 0;
            this.ProductShippingHeight = 0m;
            this.ProductShippingLength = 0m;
            this.ProductShippingWeight = 0m;
            this.ProductShippingWidth = 0m;
            this.CustomProperties = new CustomPropertyCollection();
            this.ShipFromAddress = new Contacts.Address();
            this.ShipFromMode = Shipping.ShippingMode.ShipFromSite;
            this.ShipFromNotificationId = string.Empty;
            this.ShipSeparately = false;
            this.ExtraShipCharge = 0;
        }
        public LineItem()
        {
            Init();
        }

        public Catalog.Product GetAssociatedProduct(MerchantTribeApplication app)
        {
            return app.CatalogServices.Products.Find(this.ProductId);
        }

        // Custom Property Helpers
        public bool CustomPropertyExists(string devId, string propertyKey)
        {
            bool result = false;
            for (int i = 0; i <= CustomProperties.Count - 1; i++)
            {
                if (CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (CustomProperties[i].Key.Trim().ToLower() == propertyKey.Trim().ToLower())
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public void CustomPropertySet(string devId, string key, string value)
        {
            bool found = false;

            for (int i = 0; i <= CustomProperties.Count - 1; i++)
            {
                if (CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (CustomProperties[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        CustomProperties[i].Value = value;
                        found = true;
                        break;
                    }
                }
            }

            if (found == false)
            {
                CustomProperties.Add(new CustomProperty(devId, key, value));
            }
        }
        public string CustomPropertyGet(string devId, string key)
        {
            string result = string.Empty;

            for (int i = 0; i <= CustomProperties.Count - 1; i++)
            {
                if (CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (CustomProperties[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        result = CustomProperties[i].Value;
                        break;
                    }
                }
            }

            return result;
        }
        public bool CustomPropertyRemove(string devId, string key)
        {
            bool result = false;

            for (int i = 0; i <= CustomProperties.Count - 1; i++)
            {
                if (CustomProperties[i].DeveloperId.Trim().ToLower() == devId.Trim().ToLower())
                {
                    if (CustomProperties[i].Key.Trim().ToLower() == key.Trim().ToLower())
                    {
                        CustomProperties.Remove(CustomProperties[i]);
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }
        public string CustomPropertiesToXml()
        {
            string result = string.Empty;

            try
            {
                StringWriter sw = new StringWriter();
                XmlSerializer xs = new XmlSerializer(CustomProperties.GetType());
                xs.Serialize(sw, CustomProperties);
                result = sw.ToString();
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                result = string.Empty;
            }

            return result;
        }
        public bool CustomPropertiesFromXml(string data)
        {
            bool result = false;

            try
            {
                StringReader tr = new StringReader(data);
                XmlSerializer xs = new XmlSerializer(CustomProperties.GetType());
                CustomProperties = (CustomPropertyCollection)xs.Deserialize(tr);
                if (CustomProperties != null)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                EventLog.LogEvent(ex);
                CustomProperties = new CustomPropertyCollection();
                result = false;
            }
            return result;
        }


        // Calculated Properties
        public decimal AdjustedPricePerItem
        {
            get { return CalculateAdjustedPrice(); }
        }
        public decimal LineTotalWithoutDiscounts
        {
            get
            {
                return (this.BasePricePerItem * this.Quantity);
            }
        }
        public decimal LineTotal
        {
            get { return CalculateLineTotal(); }
        }
        private decimal SumUpDiscounts()
        {
            if (this.DiscountDetails == null) return 0;
            if (this.DiscountDetails.Count < 1) return 0;
            return this.DiscountDetails.Sum(y => y.Amount);
        }
        private decimal CalculateAdjustedPrice()
        {
            if ((decimal)Quantity == 0) return 0;

            decimal result = CalculateLineTotal();            
            result = result / (decimal)Quantity;
            return result;
        }
        private decimal CalculateLineTotal()
        {
            decimal result = BasePricePerItem * Quantity;
            result += SumUpDiscounts();
            return result;
        }
        public OrderShippingStatus ShippingStatus
        {
            get { return EvaluateShippingStatus(); }
        }
        public string ShippingStatusDescription
        {
            get
            {
                return MerchantTribe.Commerce.Utilities.EnumToString.OrderShippingStatus(this.ShippingStatus);
            }
        }
        private OrderShippingStatus EvaluateShippingStatus()
        {
            if (this.ShippingSchedule == -1)
            {
                return OrderShippingStatus.NonShipping;
            }

            if (QuantityShipped >= Quantity)
            {
                return OrderShippingStatus.FullyShipped;
            }
            else
            {
                if (QuantityShipped > 0)
                {
                    return OrderShippingStatus.PartiallyShipped;
                }
                else
                {
                    return OrderShippingStatus.Unshipped;
                }
            }
        }
        public string DiscountDetailsAsHtml()
        {
            if (this.DiscountDetails == null) return string.Empty;
            if (this.DiscountDetails.Count < 1) return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (DiscountDetail d in this.DiscountDetails)
            {
                sb.Append(d.Description + " " + d.Amount.ToString("c") + "<br />");
            }
            return sb.ToString();
        }

        public List<Content.HtmlTemplateTag> GetReplaceableTags(MerchantTribeApplication app)
        {
            List<Content.HtmlTemplateTag> result = new List<Content.HtmlTemplateTag>();

            result.Add(new Content.HtmlTemplateTag("[[LineItem.AdjustedPrice]]", this.AdjustedPricePerItem.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.BasePrice]]", this.BasePricePerItem.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.Discounts]]", this.DiscountDetailsAsHtml()));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.LineTotal]]", this.LineTotal.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ProductId]]", this.ProductId));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.VariantId]]", this.VariantId));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ProductName]]", this.ProductName));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ProductSku]]", this.ProductSku));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.Sku]]", this.ProductSku));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ProductDescription]]", this.ProductShortDescription));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.Quantity]]", this.Quantity.ToString("#")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.QuantityShipped]]", this.QuantityShipped.ToString("#")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.QuantityReturned]]", this.QuantityReturned.ToString("#")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ShippingStatus]]", Utilities.EnumToString.OrderShippingStatus(this.ShippingStatus)));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ShippingPortion]]", this.ShippingPortion.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.TaxPortion]]", this.TaxPortion.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ExtraShipCharge]]", this.ExtraShipCharge.ToString("c")));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ShipFromAddress]]", this.ShipFromAddress.ToHtmlString()));
            result.Add(new Content.HtmlTemplateTag("[[LineItem.ShipSeparately]]", this.ShipSeparately ? "Yes" : "No"));

            return result;
        }

        public LineItem Clone()
        {
            return Clone(false);
        }

        public LineItem Clone(bool copyId)
        {
            LineItem result = new LineItem();

            result.LastUpdatedUtc = this.LastUpdatedUtc;
            result.BasePricePerItem = this.BasePricePerItem;
            result.DiscountDetails = DiscountDetail.ListFromXml(DiscountDetail.ListToXml(this.DiscountDetails));
            result.OrderBvin = this.OrderBvin;
            result.ProductId = this.ProductId;
            result.VariantId = this.VariantId;
            result.ProductName = this.ProductName;
            result.ProductSku = this.ProductSku;
            result.ProductShortDescription = this.ProductShortDescription;
            result.Quantity = this.Quantity;
            result.QuantityReturned = this.QuantityReturned;
            result.QuantityShipped = this.QuantityShipped;
            result.ShippingPortion = this.ShippingPortion;
            result.StatusCode = this.StatusCode;
            result.StatusName = this.StatusName;
            result.TaxPortion = this.TaxPortion;
            foreach (var x in this.SelectionData)
            {
                result.SelectionData.Add(x);
            }
            result.ShippingSchedule = this.ShippingSchedule;
            result.TaxSchedule = this.TaxSchedule;
            result.ProductShippingHeight = this.ProductShippingHeight;
            result.ProductShippingLength = this.ProductShippingLength;
            result.ProductShippingWeight = this.ProductShippingWeight;
            result.ProductShippingWidth = this.ProductShippingWidth;
            foreach (var y in this.CustomProperties)
            {
                result.CustomProperties.Add(y.Clone());
            }

            if (copyId)
            {
                result.Id = this.Id;
            }

            return result;
        }

        //public void MoveToNextStatus()
        //{
        //    Collection<Orders.LineItemStatusCode> codes = Orders.LineItemStatusCode.FindAll();

        //    for (int i = 0; i <= codes.Count - 1; i++)
        //    {
        //        if (codes[i].Bvin == this.StatusCode)
        //        {
        //            // Found Current                    
        //            if (i < codes.Count - 1)
        //            {
        //                this.StatusCode = codes[i + 1].Bvin;
        //                this.StatusName = codes[i + 1].StatusName;
        //            }
        //            break;
        //        }
        //    }
        //}

        //public void MoveToPreviousStatus()
        //{
        //    Collection<Orders.LineItemStatusCode> codes = Orders.LineItemStatusCode.FindAll();

        //    for (int i = 0; i <= codes.Count - 1; i++)
        //    {
        //        if (codes[i].Bvin == this.StatusCode)
        //        {
        //            // Found Current                    
        //            if (i > 0)
        //            {
        //                this.StatusCode = codes[i - 1].Bvin;
        //                this.StatusName = codes[i - 1].StatusName;
        //            }
        //            break;
        //        }
        //    }
        //}

        public bool ContainsSameProduct(LineItem other)
        {
            if (other.ProductId == this.ProductId)
            {
                if (other.AdjustedPricePerItem != this.AdjustedPricePerItem)
                {
                    return false;
                }

                if (other.VariantId != string.Empty | this.VariantId != string.Empty)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public decimal GetTotalWeight()
        {
            decimal weight = this.ProductShippingWeight;
            weight *= (this.Quantity - this.QuantityShipped);
            return weight;
        }

        public virtual bool Equals(LineItem other)
        {
            return this.Id == other.Id;
        }
        
        //private decimal ReserveKitInventory(decimal qty)
        //{
        //    Collection<Catalog.KitPart> parts = Services.KitService.GetKitPartsForKitSelections(this.KitSelections);
        //    Dictionary<string, Catalog.ProductInventory> inventories = new Dictionary<string, Catalog.ProductInventory>();
        //    Dictionary<string, decimal> quantitiesNeeded = new Dictionary<string, decimal>();
        //    int quantityRequested = (int)qty;
        //    int availableKitQuantity = quantityRequested;

        //    for (int i = 1; i <= quantityRequested; i++)
        //    {
        //        bool enoughInventory = true;

        //        foreach (Catalog.KitPart part in parts)
        //        {
        //            if (part.IsTrackingInventory)
        //            {
        //                decimal neededQuantity = i * part.Quantity;
        //                if (quantitiesNeeded.ContainsKey(part.ProductBvin))
        //                {
        //                    quantitiesNeeded[part.ProductBvin] += neededQuantity;
        //                }
        //                else
        //                {
        //                    quantitiesNeeded.Add(part.ProductBvin, neededQuantity);
        //                }
        //            }
        //        }

        //        foreach (Catalog.KitPart part in parts)
        //        {
        //            //this check is to see if we have enough stock, so if the
        //            //current product is purchaseable without stock, we don't need to 
        //            //check it
        //            if (!part.IsPurchasableWhenOutOfStock)
        //            {
        //                if (quantitiesNeeded.ContainsKey(part.ProductBvin))
        //                {
        //                    Catalog.ProductInventory inventory = null;
        //                    if (!inventories.ContainsKey(part.ProductBvin))
        //                    {
        //                        inventory = Catalog.ProductInventory.FindByProductId(part.ProductBvin);
        //                        inventories.Add(part.ProductBvin, inventory);
        //                    }
        //                    else
        //                    {
        //                        inventory = inventories[part.ProductBvin];
        //                    }

        //                    decimal neededQuantity = quantitiesNeeded[part.ProductBvin];
        //                    if (inventory.QuantityAvailableForSale < neededQuantity)
        //                    {
        //                        enoughInventory = false;
        //                        break;
        //                    }
        //                }
        //            }
        //        }

        //        if (!enoughInventory)
        //        {
        //            availableKitQuantity = i - 1;
        //        }
        //    }

        //    Dictionary<string, decimal> reservedQuantities = new Dictionary<string, decimal>();
        //    bool success = true;

        //    if (availableKitQuantity > 0)
        //    {
        //        foreach (Catalog.KitPart part in parts)
        //        {
        //            if (part.IsTrackingInventory)
        //            {
        //                decimal reservedResult = ReserveSingleItemInventory(part.ProductBvin, part.Quantity * availableKitQuantity);
        //                if (reservedQuantities.ContainsKey(part.ProductBvin))
        //                {
        //                    reservedQuantities[part.ProductBvin] += reservedResult;
        //                }
        //                else
        //                {
        //                    reservedQuantities.Add(part.ProductBvin, reservedResult);
        //                }
        //                if (reservedResult != (part.Quantity * availableKitQuantity))
        //                {
        //                    success = false;
        //                    break;
        //                }
        //            }
        //        }
        //    }

        //    if (!success)
        //    {
        //        foreach (string key in reservedQuantities.Keys)
        //        {
        //            if (reservedQuantities[key] > 0)
        //            {
        //                UnreserveSingleItemInventory(key, reservedQuantities[key]);
        //            }
        //        }
        //        return 0;
        //    }
        //    else
        //    {
        //        return availableKitQuantity;
        //    }
        //}

        //private bool UnreserveKitInventory(decimal qty)
        //{
        //    Collection<Catalog.KitPart> parts = Services.KitService.GetKitPartsForKitSelections(this.KitSelections);
        //    Dictionary<string, Catalog.ProductInventory> inventories = new Dictionary<string, Catalog.ProductInventory>();

        //    Dictionary<string, decimal> quantities = new Dictionary<string, decimal>();
        //    foreach (Catalog.KitPart part in parts)
        //    {
        //        if (part.IsTrackingInventory)
        //        {
        //            if (quantities.ContainsKey(part.ProductBvin))
        //            {
        //                quantities[part.ProductBvin] += part.Quantity * qty;
        //            }
        //            else
        //            {
        //                quantities.Add(part.ProductBvin, part.Quantity * qty);
        //            }
        //        }
        //    }

        //    foreach (string productId in quantities.Keys)
        //    {
        //        if (quantities[productId] > 0)
        //        {
        //            UnreserveSingleItemInventory(productId, quantities[productId]);
        //        }
        //    }
        //    return true;
        //}

        

        #region ITaxable Members

        public decimal TaxableValue()
        {
            return this.LineTotal;
        }

        public long TaxScheduleId()
        {
            return this.TaxSchedule;
        }
        public decimal TaxableShippingValue()
        {
            return this.ShippingPortion;
        }

        public void IncrementTaxValue(decimal calculatedTax)
        {
            this.TaxPortion += calculatedTax;
        }

        public void ClearTaxValue()
        {
            this.TaxPortion = 0m;
        }

        #endregion

        //DTO
        public LineItemDTO ToDto()
        {
            LineItemDTO dto = new LineItemDTO();

            dto.Id = this.Id;
            dto.StoreId = this.StoreId;
            dto.LastUpdatedUtc = this.LastUpdatedUtc;
            dto.BasePricePerItem = this.BasePricePerItem;
            foreach (DiscountDetail detail in this.DiscountDetails)
            {
                dto.DiscountDetails.Add(detail.ToDto());
            }
            dto.OrderBvin = this.OrderBvin ?? string.Empty;
            dto.ProductId = this.ProductId ?? string.Empty;
            dto.VariantId = this.VariantId ?? string.Empty;
            dto.ProductName = this.ProductName ?? string.Empty;
            dto.ProductSku = this.ProductSku ?? string.Empty;
            dto.ProductShortDescription = this.ProductShortDescription ?? string.Empty;
            dto.Quantity = this.Quantity;
            dto.QuantityReturned = this.QuantityReturned;
            dto.QuantityShipped = this.QuantityShipped;
            dto.ShippingPortion = this.ShippingPortion;
            dto.StatusCode = this.StatusCode ?? string.Empty;
            dto.StatusName = this.StatusName ?? string.Empty;
            dto.TaxPortion = this.TaxPortion;
            foreach (Catalog.OptionSelection op in this.SelectionData)
            {
                dto.SelectionData.Add(op.ToDto());
            }
            dto.ShippingSchedule = this.ShippingSchedule;
            dto.TaxSchedule = this.TaxSchedule;
            dto.ProductShippingHeight = this.ProductShippingHeight;
            dto.ProductShippingLength = this.ProductShippingLength;
            dto.ProductShippingWeight = this.ProductShippingWeight;
            dto.ProductShippingWidth = this.ProductShippingWidth;
            foreach (CustomProperty cp in this.CustomProperties)
            {
                dto.CustomProperties.Add(cp.ToDto());
            }
            dto.ShipFromAddress = this.ShipFromAddress.ToDto();
            dto.ShipFromMode = (ShippingModeDTO)((int)this.ShipFromMode);
            dto.ShipFromNotificationId = this.ShipFromNotificationId ?? string.Empty;
            dto.ShipSeparately = this.ShipSeparately;
            dto.ExtraShipCharge = this.ExtraShipCharge;

            return dto;
        }
        public void FromDto(LineItemDTO dto)
        {
            if (dto == null) return;

            this.Id = dto.Id;
            this.StoreId = dto.StoreId;
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
            this.BasePricePerItem = dto.BasePricePerItem;
            this.DiscountDetails.Clear();
            if (dto.DiscountDetails != null)
            {
                foreach (DiscountDetailDTO detail in dto.DiscountDetails)
                {
                    DiscountDetail d = new DiscountDetail();
                    d.FromDto(detail);
                    this.DiscountDetails.Add(d);
                }
            }
            this.OrderBvin = dto.OrderBvin ?? string.Empty;
            this.ProductId = dto.ProductId ?? string.Empty;
            this.VariantId = dto.VariantId ?? string.Empty;
            this.ProductName = dto.ProductName ?? string.Empty;
            this.ProductSku = dto.ProductSku ?? string.Empty;
            this.ProductShortDescription = dto.ProductShortDescription ?? string.Empty;
            this.Quantity = dto.Quantity;
            this.QuantityReturned = dto.QuantityReturned;
            this.QuantityShipped = dto.QuantityShipped;
            this.ShippingPortion = dto.ShippingPortion;
            this.StatusCode = dto.StatusCode ?? string.Empty;
            this.StatusName = dto.StatusName ?? string.Empty;
            this.TaxPortion = dto.TaxPortion;
            this.SelectionData.Clear();
            if (dto.SelectionData != null)
            {
                foreach (OptionSelectionDTO op in dto.SelectionData)
                {
                    Catalog.OptionSelection o = new Catalog.OptionSelection();
                    o.FromDto(op);
                    this.SelectionData.Add(o);
                }
            }
            this.ShippingSchedule = dto.ShippingSchedule;
            this.TaxSchedule = dto.TaxSchedule;
            this.ProductShippingHeight = dto.ProductShippingHeight;
            this.ProductShippingLength = dto.ProductShippingLength;
            this.ProductShippingWeight = dto.ProductShippingWeight;
            this.ProductShippingWidth = dto.ProductShippingWidth;
            this.CustomProperties.Clear();
            if (dto.CustomProperties != null)
            {
                foreach (CustomPropertyDTO cpd in dto.CustomProperties)
                {
                    CustomProperty prop = new CustomProperty();
                    prop.FromDto(cpd);
                    this.CustomProperties.Add(prop);
                }
            }
            this.ShipFromAddress.FromDto(dto.ShipFromAddress);
            this.ShipFromMode = (Shipping.ShippingMode)((int)dto.ShipFromMode);
            this.ShipFromNotificationId = dto.ShipFromNotificationId ?? string.Empty;
            this.ShipSeparately = dto.ShipSeparately;
            this.ExtraShipCharge = dto.ExtraShipCharge;

        }

    }

}
