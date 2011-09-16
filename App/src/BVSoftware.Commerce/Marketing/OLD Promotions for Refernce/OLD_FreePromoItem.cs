//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;

//namespace BVSoftware.Commerce.Marketing.Offers
//{

//    //public class FreePromoItem : Marketing.OfferTaskProcessorBase
//    //{


//    //    private int _QualificationQty = 1;
//    //    //private Collection<Content.ComponentSettingListItem> _QualificationCategories;
//    //    private string _PromoProductId = string.Empty;


//    //    public override Marketing.DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        Marketing.DiscountQueue result = new Marketing.DiscountQueue();

//    //        bool promoItemExistsInOrder = false;
//    //        int availableCount = 0;
//    //        foreach (Orders.LineItem li in context.Order.Items) {

//    //            if ((li.ProductId == _PromoProductId)) {
//    //                promoItemExistsInOrder = true;
//    //            }

//    //            //if ((MatchesCategoryList(li, _QualificationCategories))) {
//    //            //    if ((li.ProductId == _PromoProductId)) {
//    //            //        // Promo item can't self qualify for offer
//    //            //        if (((int)li.Quantity > 1)) {
//    //            //            availableCount += ((int)li.Quantity - 1);
//    //            //        }
//    //            //    }
//    //            //    else {
//    //            //        availableCount += (int)li.Quantity;
//    //            //    }
//    //            //}
//    //        }

//    //        if ((availableCount < _QualificationQty)) {
//    //            return result;
//    //        }

//    //        // Add Promo Item automatically
//    //        if ((!promoItemExistsInOrder)) {
//    //            context.Order.AddItem(_PromoProductId, 1);
//    //        }

//    //        foreach (Orders.LineItem li in context.Order.Items) {
//    //            if ((li.ProductId == _PromoProductId)) {
//    //                result.AddDiscount(li.Bvin, li.BasePrice, Marketing.DiscountQueueItemType.LineItemAdditional);
//    //                return result;
//    //            }
//    //        }

//    //        return result;
//    //    }

//    //    //private bool MatchesCategoryList(Orders.LineItem li, Collection<Content.ComponentSettingListItem> categoriesToMatch)
//    //    //{
//    //    //    Collection<Catalog.Category> cats;			
//    //    //    cats = Catalog.Product.GetCategories(li.ProductId);			

//    //    //    if ((cats != null)) {
//    //    //        if (categoriesToMatch != null) {
//    //    //            foreach (Content.ComponentSettingListItem cat in categoriesToMatch) {
//    //    //                foreach (Catalog.Category productCat in cats) {
//    //    //                    if ((productCat.Bvin == cat.Setting1)) {
//    //    //                        return true;
//    //    //                    }
//    //    //                }
//    //    //            }
//    //    //        }
//    //    //    }

//    //    //    return false;
//    //    //}

//    //    public override byte GetPriority()
//    //    {
//    //        return (byte)Marketing.PriorityLevels.BuyOneGetOne;
//    //    }

//    //    public override void GetSettings(BusinessRules.OrderTaskContext context)
//    //    {

//    //        //_QualificationQty = SettingsManager.GetIntegerSetting("QualificationQty");
//    //        //_QualificationCategories = SettingsManager.GetSettingList("QualificationCategories");
//    //        //_PromoProductId = SettingsManager.GetSetting("PromoProductId");

//    //    }

//    //}

//}

