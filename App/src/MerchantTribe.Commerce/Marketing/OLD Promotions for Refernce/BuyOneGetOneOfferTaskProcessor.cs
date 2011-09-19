//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;

//namespace MerchantTribe.Commerce.Marketing
//{
//    public class BuyOneGetOneOfferTaskProcessor : OfferTaskProcessorBase
//    {

//    //    private decimal _amount;
//    //    private Marketing.AmountTypes _amountType;
//    //    private int _orderQuantity;
//    //    private decimal _orderTotalMin;
//    //    private decimal _orderTotalMax;
//    //    private bool _addItemsAutomatically;
//    //    //private Collection<Content.ComponentSettingListItem> _purchasedProducts;
//    //    //private Collection<Content.ComponentSettingListItem> _discountedProducts;
//    //    //private string PurchaseId()
//    //    //{
//    //    //    string result = string.Empty;
//    //    //    if (_purchasedProducts != null) {
//    //    //        if (_purchasedProducts.Count > 0) {
//    //    //            result = _purchasedProducts[0].Setting1;
//    //    //        }
//    //    //    }
//    //    //    return result;
//    //    //}
//    //    //private string DiscountId()
//    //    //{
//    //    //    string result = string.Empty;
//    //    //    if (_discountedProducts != null) {
//    //    //        if (_discountedProducts.Count > 0) {
//    //    //            result = _discountedProducts[0].Setting1;
//    //    //        }
//    //    //    }
//    //    //    return result;
//    //    //}

//    //    public override DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        DiscountQueue result = new DiscountQueue();

//    //        //if ((context.Order.TotalOrderBeforeDiscounts >= _orderTotalMin && context.Order.TotalOrderBeforeDiscounts <= _orderTotalMax)) {

//    //        //    Dictionary<string, decimal> quantityFound = new Dictionary<string, decimal>();
//    //        //    bool found = false;

//    //        //    foreach (Content.ComponentSettingListItem setting in _purchasedProducts) {
//    //        //        found = false;
//    //        //        foreach (Orders.LineItem lineItem in context.Order.Items) {
//    //        //            if ((string.Compare(lineItem.ProductId, setting.Setting1) == 0) && (lineItem.Quantity >= _orderQuantity) && (lineItem.Discounts == 0m)) {
//    //        //                found = true;
//    //        //                if (quantityFound.ContainsKey(lineItem.ProductId)) {
//    //        //                    quantityFound[lineItem.ProductId] += lineItem.Quantity;
//    //        //                }
//    //        //                else {
//    //        //                    quantityFound.Add(lineItem.ProductId, lineItem.Quantity);
//    //        //                }
//    //        //            }
//    //        //        }

//    //        //        if (!found) {
//    //        //            break;
//    //        //        }
//    //        //    }

//    //        //    if (found) {
//    //        //        decimal quantityToDiscount = decimal.MaxValue;
//    //        //        foreach (decimal item in quantityFound.Values) {
//    //        //            if (item < quantityToDiscount) {
//    //        //                quantityToDiscount = item;
//    //        //            }
//    //        //        }

//    //        //        foreach (Content.ComponentSettingListItem setting in _discountedProducts) {
//    //        //            if (quantityFound.ContainsKey(setting.Setting1)) {
//    //        //                if (Math.Floor(quantityFound[setting.Setting1] / 2) < quantityToDiscount) {
//    //        //                    quantityToDiscount = Math.Floor(quantityFound[setting.Setting1] / 2);
//    //        //                }
//    //        //            }
//    //        //        }

//    //        //        foreach (Content.ComponentSettingListItem setting in _discountedProducts) {
//    //        //            Collection<Orders.LineItem> items = GetItemsFromCart(context, setting.Setting1);

//    //        //            decimal totalQuantityNeededToDiscount = quantityToDiscount;
//    //        //            decimal currentQuantityNeededToDiscount = 0m;
//    //        //            foreach (Orders.LineItem item in items) {
//    //        //                if (item.Quantity < totalQuantityNeededToDiscount) {
//    //        //                    currentQuantityNeededToDiscount = item.Quantity;
//    //        //                }
//    //        //                else {
//    //        //                    currentQuantityNeededToDiscount = totalQuantityNeededToDiscount;
//    //        //                }
//    //        //                totalQuantityNeededToDiscount -= currentQuantityNeededToDiscount;
//    //        //                if (currentQuantityNeededToDiscount > 0) {
//    //        //                    if (_amountType == Marketing.AmountTypes.MonetaryAmount) {
//    //        //                        result.AddDiscount(item.Bvin, Utilities.Money.GetDiscountAmount(item.AdjustedPrice, _amount) * currentQuantityNeededToDiscount, DiscountQueueItemType.LineItemAdditional);
//    //        //                    }
//    //        //                    else if (_amountType == Marketing.AmountTypes.Percent) {
//    //        //                        result.AddDiscount(item.Bvin, Utilities.Money.GetDiscountAmountByPercent(item.AdjustedPrice, _amount) * currentQuantityNeededToDiscount, DiscountQueueItemType.LineItemAdditional);
//    //        //                    }
//    //        //                }
//    //        //            }						
//    //        //        }
//    //        //    }
//    //        //}

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

//    //    public override void GetSettings(BusinessRules.OrderTaskContext context)
//    //    {
//    //        //_purchasedProducts = SettingsManager.GetSettingList("PurchasedProducts");
//    //        //_discountedProducts = SettingsManager.GetSettingList("DiscountedProducts");
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
