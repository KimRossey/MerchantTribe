//using System;
//using System.Collections.ObjectModel;

//namespace BVSoftware.Commerce.Marketing
//{

//    public class FreeShippingOfferTaskProcessor : OfferTaskProcessorBase
//    {

//    //    private int _orderQuantity;
//    //    private decimal _orderTotalMin;
//    //    private decimal _orderTotalMax;
//    //    //private Collection<Content.ComponentSettingListItem> _freeShippingProducts;

//    //    public override DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        DiscountQueue result = new DiscountQueue();
//    //        foreach (Orders.LineItem item in context.Order.Items) {
//    //            if (item.CustomProperties.Count > 0) {
//    //                Collection<CustomProperty> propsToRemove = new Collection<CustomProperty>();
//    //                foreach (CustomProperty prop in item.CustomProperties) {
//    //                    if (prop.DeveloperId == "bvsoftware" && prop.Key == "freeshipping" && prop.Value == context.Offer.Bvin) {
//    //                        propsToRemove.Add(prop);
//    //                    }
//    //                }

//    //                foreach (CustomProperty prop in propsToRemove) {
//    //                    item.CustomProperties.Remove(prop);
//    //                }
//    //            }
//    //        }

//    //        //if ((context.Order.TotalQuantity >= _orderQuantity) && (context.Order.TotalOrderBeforeDiscounts >= _orderTotalMin) && (context.Order.TotalOrderBeforeDiscounts <= _orderTotalMax)) {
//    //        //    foreach (Content.ComponentSettingListItem listitem in _freeShippingProducts) {
//    //        //        foreach (Orders.LineItem item in context.Order.Items) {
//    //        //            if ((listitem.Setting1 == item.ProductId)) {
//    //        //                item.CustomProperties.Add("bvsoftware", "freeshipping", context.Offer.Bvin);
//    //        //            }						
//    //        //        }
//    //        //    }
//    //        //}
//    //        return result;
//    //    }

//    //    public override void GetSettings(BusinessRules.OrderTaskContext context)
//    //    {
//    //        //_freeShippingProducts = SettingsManager.GetSettingList("FreeShippingProducts");
//    //        //_orderQuantity = SettingsManager.GetIntegerSetting("OrderQuantity");
//    //        //_orderTotalMin = SettingsManager.GetDecimalSetting("OrderTotal");
//    //        //_orderTotalMax = SettingsManager.GetDecimalSetting("OrderTotalMax");
//    //        if (_orderTotalMax <= 0) {
//    //            _orderTotalMax = decimal.MaxValue;
//    //        }
//    //    }

//    //    public override byte GetPriority()
//    //    {
//    //        return (byte)PriorityLevels.Shipping;
//    //    }
//    }
//}

