//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;

//namespace BVSoftware.Commerce.Marketing.Offers
//{

//    //public class BuyXGetYByCategory : Marketing.OfferTaskProcessorBase
//    //{

//    //    private class LineItemData
//    //    {

//    //        private Orders.LineItem _LineItem = new Orders.LineItem();
//    //        private int _QtyUsed = 0;

//    //        public Orders.LineItem LineItem {
//    //            get { return _LineItem; }
//    //            set { _LineItem = value; }
//    //        }
//    //        public int QtyUsed {
//    //            get { return _QtyUsed; }
//    //            set { _QtyUsed = value; }
//    //        }
//    //        public int QtyLeft {
//    //            get { return (int)_LineItem.Quantity - _QtyUsed; }
//    //        }
//    //        public int QtyOriginal {
//    //            get { return (int)_LineItem.Quantity; }
//    //        }
//    //        public decimal ItemAdjustedPrice {
//    //            get { return _LineItem.AdjustedPrice; }
//    //        }

//    //        public LineItemData()
//    //        {

//    //        }

//    //        public LineItemData(Orders.LineItem li)
//    //        {
//    //            _LineItem = li;
//    //        }

//    //    }

//    //    private int _QualificationQty = 1;
//    //    //private Collection<Content.ComponentSettingListItem> _QualificationCategories;
//    //    //private Collection<Content.ComponentSettingListItem> _PromoCategories;


//    //    public override Marketing.DiscountQueue Execute(BusinessRules.OrderTaskContext context)
//    //    {
//    //        Marketing.DiscountQueue result = new Marketing.DiscountQueue();

//    //        //// Get All Items that Qualify for the discount
//    //        //List<LineItemData> qualifying = new List<LineItemData>();
//    //        //foreach (Orders.LineItem li in context.Order.Items) {
//    //        //    if ((MatchesCategoryList(li, _QualificationCategories))) {
//    //        //        qualifying.Add(new LineItemData(li));
//    //        //    }
//    //        //}
//    //        //int totalQualifyingCount = 0;
//    //        //foreach (LineItemData lid in qualifying) {
//    //        //    totalQualifyingCount += lid.QtyOriginal;
//    //        //}

//    //        //// We don't have enough to qualify so skip the rest
//    //        //if (((int)totalQualifyingCount < _QualificationQty)) {
//    //        //    return result;
//    //        //}


//    //        //// Find all items that qualify as promotional
//    //        //List<LineItemData> promoitems = new List<LineItemData>();
//    //        //foreach (Orders.LineItem li in context.Order.Items) {
//    //        //    if ((MatchesCategoryList(li, _PromoCategories))) {
//    //        //        promoitems.Add(new LineItemData(li));
//    //        //    }
//    //        //}
//    //        //// Nothing to discount so skip the rest
//    //        //if ((promoitems.Count < 1)) {
//    //        //    return result;
//    //        //}


//    //        //// Visit each qualifying product and gather a qty needed for a promo

//    //        //List<LineItemData> sortedQualifying = (from q in qualifying 
//    //        //                                        orderby q.ItemAdjustedPrice descending
//    //        //                                        select q).ToList();

//    //        //List<LineItemData> sortedPromo = (from p in promoitems 
//    //        //                                  orderby p.ItemAdjustedPrice descending
//    //        //                                  select p).ToList();

//    //        //int workingQty = 0;
//    //        //while (sortedQualifying.Count > 0) {
//    //        //    int qtyLeftToQualify = _QualificationQty - workingQty;
//    //        //    if ((qtyLeftToQualify == 0)) {
//    //        //        // When we've hit the required qty, look for an item to discount
//    //        //        workingQty = 0;
//    //        //        DiscountAPromoItem(result, sortedPromo);
//    //        //    }
//    //        //    else {
//    //        //        if ((sortedQualifying[0].QtyLeft > qtyLeftToQualify)) {
//    //        //            sortedQualifying[0].QtyUsed += qtyLeftToQualify;
//    //        //            workingQty += qtyLeftToQualify;
//    //        //            RemoveFromPromo(sortedQualifying[0].LineItem.Bvin, qtyLeftToQualify, sortedPromo);
//    //        //        }
//    //        //        else {
//    //        //            workingQty += sortedQualifying[0].QtyLeft;
//    //        //            RemoveFromPromo(sortedQualifying[0].LineItem.Bvin, sortedQualifying[0].QtyLeft, sortedPromo);
//    //        //            sortedQualifying.RemoveAt(0);
//    //        //        }
//    //        //    }
//    //        //}

//    //        //// If we were working on a discount when we removed the last qualifying item, check to see if we have enough to discount
//    //        //if ((workingQty > 0)) {
//    //        //    if ((workingQty >= _QualificationQty)) {
//    //        //        DiscountAPromoItem(result, sortedPromo);
//    //        //    }
//    //        //}

//    //        return result;
//    //    }

//    //    private void DiscountAPromoItem(Marketing.DiscountQueue result, List<LineItemData> promoitems)
//    //    {
//    //        if ((promoitems != null)) {
//    //            if ((result != null)) {

//    //                foreach (LineItemData lid in promoitems) {
//    //                    if ((lid.QtyLeft > 0)) {
//    //                        bool existsAlready = false;
//    //                        foreach (Marketing.DiscountQueueItem m in result) {
//    //                            if ((m.ItemType == Marketing.DiscountQueueItemType.LineItemAdditional)) {
//    //                                if ((m.Id == lid.LineItem.Bvin)) {
//    //                                    existsAlready = true;
//    //                                    m.Value += lid.ItemAdjustedPrice;
//    //                                }
//    //                            }
//    //                        }

//    //                        if ((!existsAlready)) {
//    //                            result.AddDiscount(lid.LineItem.Bvin, lid.ItemAdjustedPrice, Marketing.DiscountQueueItemType.LineItemAdditional);
//    //                        }

//    //                        lid.QtyUsed += 1;
//    //                        return;
//    //                    }
//    //                }

//    //            }
//    //        }
//    //    }
//    //    private void RemoveFromPromo(string libvin, int qty, List<LineItemData> promoitems)
//    //    {
//    //        if ((promoitems != null)) {
//    //            foreach (LineItemData lid in promoitems) {
//    //                if ((lid.LineItem.Bvin == libvin)) {
//    //                    lid.QtyUsed += qty;
//    //                }
//    //            }
//    //        }
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
//    //        //_PromoCategories = SettingsManager.GetSettingList("PromoCategories");

//    //    }

//    //}

//}
