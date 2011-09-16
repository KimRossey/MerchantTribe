//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;

//namespace BVSoftware.Commerce.Marketing.Offers
//{

//    //public class FreePromoItemByAmount : Marketing.OfferTaskProcessorBase
//    //{

//    //    private decimal _QualificationAmount = 0;
//    //    private string _PromoProductId = string.Empty;


//    //    public override Marketing.DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        Marketing.DiscountQueue result = new Marketing.DiscountQueue();

//    //        bool promoItemExistsInOrder = false;
//    //        decimal promoValue = 0;

//    //        foreach (Orders.LineItem li in context.Order.Items) {
//    //            if ((li.ProductId == _PromoProductId)) {
//    //                promoItemExistsInOrder = true;
//    //                promoValue = li.BasePrice;
//    //            }
//    //        }

//    //        // How much does the order have?
//    //        decimal _orderSubtotal = context.Order.TotalOrderBeforeDiscounts;
//    //        if ((promoItemExistsInOrder)) {
//    //            _orderSubtotal -= promoValue;
//    //        }

//    //        // Is the amount enough to qualify for this promo?
//    //        if ((_orderSubtotal < _QualificationAmount)) {
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

//    //    public override byte GetPriority()
//    //    {
//    //        return (byte)Marketing.PriorityLevels.BuyOneGetOne;
//    //    }

//    //    public override void GetSettings(BusinessRules.OrderTaskContext context)
//    //    {
//    //        //_QualificationAmount = SettingsManager.GetDecimalSetting("QualificationAmount");
//    //        //_PromoProductId = SettingsManager.GetSetting("PromoProductId");
//    //    }

//    //}

//}
