//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;

//namespace BVSoftware.Commerce.Marketing
//{
//    public class BuyXGetYOfferTaskProcessor : OfferTaskProcessorBase
//    {

//    //    private int _promoQuantity;
//    //    private string _promoCategory;
//    //    private string _discountCategory;
//    //    private decimal _amount;
//    //    private Marketing.AmountTypes _amountType;
//    //    private int _orderQuantity;
//    //    private decimal _orderTotalMin;
//    //    private decimal _orderTotalMax;
//    //    private bool _addItemsAutomatically;

//    //    public override DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        DiscountQueue result = new DiscountQueue();

//    //        if ((context.Order.TotalOrderBeforeDiscounts >= _orderTotalMin && context.Order.TotalOrderBeforeDiscounts <= _orderTotalMax)) {
//    //            Dictionary<string, decimal> quantityPromo = new Dictionary<string, decimal>();
//    //            Dictionary<string, decimal> quantityDisc = new Dictionary<string, decimal>();
//    //            bool found = false;
//    //            decimal totalQtyPromo = 0;
//    //            decimal totalQtyDisc = 0;

//    //            foreach (Orders.LineItem lineItem in context.Order.Items) {
//    //                if (CheckCategory(lineItem.ProductId, _promoCategory) && (lineItem.Quantity >= _orderQuantity) && (lineItem.Discounts == 0m)) {
//    //                    found = true;
//    //                    totalQtyPromo += lineItem.Quantity;
//    //                    if (quantityPromo.ContainsKey(lineItem.ProductId)) {
//    //                        quantityPromo[lineItem.ProductId] += lineItem.Quantity;
//    //                    }
//    //                    else {
//    //                        quantityPromo.Add(lineItem.ProductId, lineItem.Quantity);
//    //                    }
//    //                }
					
//    //                if (_promoCategory != _discountCategory) {
//    //                    if (CheckCategory(lineItem.ProductId, _discountCategory) && (lineItem.Discounts == 0m)) {
//    //                        totalQtyDisc += lineItem.Quantity;
//    //                        if (quantityDisc.ContainsKey(lineItem.ProductId)) {
//    //                            quantityDisc[lineItem.ProductId] += lineItem.Quantity;
//    //                        }
//    //                        else {
//    //                            quantityDisc.Add(lineItem.ProductId, lineItem.Quantity);
//    //                        }
//    //                    }						
//    //                }
//    //            }

//    //            if (found) {
//    //                decimal quantityToDiscount = decimal.MaxValue;

//    //                if (_promoCategory == _discountCategory) {
//    //                    //Set discount variables
//    //                    quantityDisc = quantityPromo;
//    //                    totalQtyDisc = totalQtyPromo;
//    //                    //Determine quanity to discount
//    //                    quantityToDiscount = (int)(totalQtyPromo / (_promoQuantity + 1));
//    //                }
//    //                else {
//    //                    quantityToDiscount = (int)(totalQtyPromo / _promoQuantity);
//    //                }
//    //                decimal totalQuantityNeededToDiscount = quantityToDiscount;

//    //                foreach (string ProdId in quantityDisc.Keys) {
//    //                    Collection<Orders.LineItem> items = GetItemsFromCart(context, ProdId);
//    //                    decimal currentQuantityNeededToDiscount = 0m;

//    //                    if (totalQuantityNeededToDiscount > 0) {
//    //                        foreach (Orders.LineItem item in items) {
//    //                            if (item.Quantity < totalQuantityNeededToDiscount) {
//    //                                currentQuantityNeededToDiscount = item.Quantity;
//    //                            }
//    //                            else {
//    //                                currentQuantityNeededToDiscount = totalQuantityNeededToDiscount;
//    //                            }
//    //                            totalQuantityNeededToDiscount -= currentQuantityNeededToDiscount;
//    //                            if (currentQuantityNeededToDiscount > 0) {
//    //                                if (_amountType == Marketing.AmountTypes.MonetaryAmount) {
//    //                                    result.AddDiscount(item.Bvin, Utilities.Money.GetDiscountAmount(item.AdjustedPrice, _amount) * currentQuantityNeededToDiscount, DiscountQueueItemType.LineItemAdditional);
//    //                                }
//    //                                else if (_amountType == Marketing.AmountTypes.Percent) {
//    //                                    result.AddDiscount(item.Bvin, Utilities.Money.GetDiscountAmountByPercent(item.AdjustedPrice, _amount) * currentQuantityNeededToDiscount, DiscountQueueItemType.LineItemAdditional);
//    //                                }
//    //                            }
//    //                        }
//    //                    }
//    //                    else {
//    //                        break;
//    //                    }
//    //                }
//    //            }
//    //        }

//    //        return result;
//    //    }

//    //    private Collection<Orders.LineItem> GetItemsFromCart(BusinessRules.OrderTaskContext context, string bvin)
//    //    {
//    //        Collection<Orders.LineItem> result = new Collection<Orders.LineItem>();
//    //        for (int i = (context.Order.Items.Count - 1); i >= 0; i += -1) {
//    //            Orders.LineItem lineItem = context.Order.Items[i];
//    //            if (string.Compare(lineItem.ProductId, bvin) == 0) {
//    //                result.Add(lineItem);
//    //            }				
//    //        }
//    //        return result;
//    //    }

//    //    public override byte GetPriority()
//    //    {
//    //        return (byte)PriorityLevels.BuyOneGetOne;
//    //    }

//    //    private bool CheckCategory(string ProductId, string CategoryID)
//    //    {

//    //        List<Catalog.CategoryProductAssociation> result = new List<Catalog.CategoryProductAssociation>();
//    //        bool found = false;
//    //        Catalog.Product productRecord = new Catalog.Product();
//    //        string parentId = ProductId;

//    //        //Need to determine parent product Id
//    //        productRecord = Catalog.Product.FindByBvin(ProductId);
//    //        //Make sure found record
//    //        if (productRecord.Bvin == ProductId) {

//    //            Catalog.CatalogService catalogServices = new Catalog.CatalogService(RequestContext.GetCurrentRequestContext());

//    //            //Select product categories by categories since this was the only method available
//    //            result = catalogServices.CategoriesXProducts.FindForCategory(CategoryID, 1, int.MaxValue);
//    //            if (result != null) {

//    //                // Confirm found correct ProductID
//    //                for (int i = 0; i <= result.Count - 1; i++) {
//    //                    if (result[i].ProductId == parentId) {
//    //                        found = true;
//    //                    }
//    //                }

//    //            }
//    //        }

//    //        return found;
//    //    }

//    //    public override void GetSettings(BusinessRules.OrderTaskContext context)
//    //    {
//    //        //_promoQuantity = SettingsManager.GetIntegerSetting("PromoQuantity");
//    //        //_promoCategory = SettingsManager.GetSetting("PromoCat");
//    //        //_discountCategory = SettingsManager.GetSetting("DiscountCat");
//    //        //_amount = SettingsManager.GetDecimalSetting("Amount");
//    //        //_amountType = (AmountTypes)SettingsManager.GetIntegerSetting("AmountType");
//    //        //_addItemsAutomatically = SettingsManager.GetBooleanSetting("AddItemsAutomatically");
//    //        //_orderQuantity = SettingsManager.GetIntegerSetting("OrderQuantity");
//    //        //_orderTotalMin = SettingsManager.GetDecimalSetting("OrderTotal");
//    //        //_orderTotalMax = SettingsManager.GetDecimalSetting("OrderTotalMax");
//    //        if (_orderTotalMax <= 0) {
//    //            _orderTotalMax = decimal.MaxValue;
//    //        }
//    //    }
//    //}
//}
