//using System;
//using System.Collections.ObjectModel;
//using System.Data;
//using System.Collections.Generic;

//namespace MerchantTribe.Commerce.Marketing.Offers
//{

//    //public class FreeShippingByCategory : Marketing.OfferTaskProcessorBase
//    //{

//    //    private string _ShippingMethodId = string.Empty;
//    //    //private Collection<Content.ComponentSettingListItem> _ExcludedCategories;
//    //    private decimal _orderTotalMin = 0;

//    //    public override Marketing.DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        Marketing.DiscountQueue result = new Marketing.DiscountQueue();

//    //        // Clear old free shipping settings
//    //        foreach (Orders.LineItem item in context.Order.Items) {
//    //            if (item.CustomProperties.Count > 0) {
//    //                Collection<CustomProperty> propsToRemove = new Collection<CustomProperty>();
//    //                foreach (CustomProperty prop in item.CustomProperties) {
//    //                    if (prop.DeveloperId == "bvsoftware" && prop.Key == "freeshipping" && prop.Value == context.Offer.Bvin) {
//    //                        propsToRemove.Add(prop);
//    //                    }
//    //                    if (prop.DeveloperId == "bvsoftware" && prop.Key == "freeshipping" + context.Offer.Bvin) {
//    //                        propsToRemove.Add(prop);
//    //                    }
//    //                }

//    //                foreach (CustomProperty prop in propsToRemove) {
//    //                    item.CustomProperties.Remove(prop);
//    //                }
//    //            }
//    //        }

//    //        if ((context.Order.TotalOrderBeforeDiscounts >= _orderTotalMin)) {
//    //            foreach (Orders.LineItem item in context.Order.Items) {
//    //                if ((DoesItemQualify(item))) {
//    //                    item.CustomProperties.Add("bvsoftware", "freeshipping", context.Offer.Bvin);
//    //                    item.CustomProperties.Add("bvsoftware", "freeshipping" + context.Offer.Bvin, _ShippingMethodId);
//    //                }
//    //            }
//    //        }
//    //        return result;

//    //    }

//    //    private bool DoesItemQualify(Orders.LineItem li)
//    //    {
//    //        //Collection<Catalog.Category> cats;			
//    //        //cats = Catalog.Product.GetCategories(li.ProductId);			

//    //        //if ((cats != null)) {
//    //        //    if (_ExcludedCategories != null) {
//    //        //        foreach (Content.ComponentSettingListItem cat in _ExcludedCategories) {
//    //        //            foreach (Catalog.Category productCat in cats) {
//    //        //                if ((productCat.Bvin == cat.Setting1)) {
//    //        //                    return false;
//    //        //                }
//    //        //            }
//    //        //        }
//    //        //    }
//    //        //}

//    //        return true;
//    //    }

//    //    public override byte GetPriority()
//    //    {
//    //        return (byte)Marketing.PriorityLevels.Shipping;
//    //    }

//    //    public override void GetSettings(BusinessRules.OrderTaskContext context)
//    //    {
//    //        //_ShippingMethodId = SettingsManager.GetSetting("ShippingMethodId");
//    //        //_orderTotalMin = SettingsManager.GetDecimalSetting("OrderTotal");
//    //        //_ExcludedCategories = SettingsManager.GetSettingList("ExcludedCategories");
//    //    }

//    //}
//}

