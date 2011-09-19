using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing
{
    public class MarketingService
    {
        private RequestContext context = null;

        public PromotionRepository Promotions { get; private set; }

        public static MarketingService InstantiateForMemory(RequestContext c)
        {
            return new MarketingService(c,
                                        PromotionRepository.InstantiateForMemory(c));
        }
        public static MarketingService InstantiateForDatabase(RequestContext c)
        {
            return new MarketingService(c, PromotionRepository.InstantiateForDatabase(c));
        }
        public MarketingService(RequestContext c,
                              PromotionRepository promotions)
        {
            context = c;
            Promotions = promotions;
        }
     
        public Promotion GetPredefinedPromotion(PreDefinedPromotion type)
        {
            Promotion p = new Promotion();
            
            switch(type)
            {
                case PreDefinedPromotion.SaleCategories:
                    p.Name = "New Category Sale";
                    p.Mode = PromotionType.Sale;
                    p.AddQualification(new PromotionQualifications.ProductCategory());
                    p.AddAction(new PromotionActions.ProductPriceAdjustment());
                    break;
                case PreDefinedPromotion.SaleProducts:
                    p.Name = "New Product Sale";
                    p.Mode = PromotionType.Sale;
                    p.AddQualification(new PromotionQualifications.ProductBvin());
                    p.AddAction(new PromotionActions.ProductPriceAdjustment());
                    break;
                case PreDefinedPromotion.SaleProductType:
                    p.Name = "New Product Type Sale";
                    p.Mode = PromotionType.Sale;
                    p.AddQualification(new PromotionQualifications.ProductType());
                    p.AddAction(new PromotionActions.ProductPriceAdjustment());
                    break;
                case PreDefinedPromotion.SaleStorewide:
                    p.Name = "New Storewide Sale";
                    p.Mode = PromotionType.Sale;
                    p.AddQualification(new PromotionQualifications.AnyProduct());
                    p.AddAction(new PromotionActions.ProductPriceAdjustment());
                    break;
                case PreDefinedPromotion.SaleUser:
                    p.Name = "New Sale By User";
                    p.Mode = PromotionType.Sale;
                    p.AddQualification(new PromotionQualifications.UserIs());
                    p.AddAction(new PromotionActions.ProductPriceAdjustment());
                    break;
                case PreDefinedPromotion.SaleUserGroup:
                    p.Name = "New Sale By Price Group";
                    p.Mode = PromotionType.Sale;
                    p.AddQualification(new PromotionQualifications.UserIsInGroup());
                    p.AddAction(new PromotionActions.ProductPriceAdjustment());
                    break;
                case PreDefinedPromotion.OrderDiscountCoupon:
                    p.Name = "New Order Discount With Coupon";
                    p.Mode = PromotionType.Offer;
                    p.AddQualification(new PromotionQualifications.OrderHasCoupon());
                    p.AddAction(new PromotionActions.OrderTotalAdjustment());
                    break;
                case PreDefinedPromotion.OrderDiscountUser:
                    p.Name = "New Order Discount by User";
                    p.Mode = PromotionType.Offer;
                    p.AddQualification(new PromotionQualifications.UserIs());
                    p.AddAction(new PromotionActions.OrderTotalAdjustment());
                    break;
                case PreDefinedPromotion.OrderDiscountUserGroup:
                    p.Name = "New Order Discount by Price Group";
                    p.Mode = PromotionType.Offer;
                    p.AddQualification(new PromotionQualifications.UserIsInGroup());
                    p.AddAction(new PromotionActions.OrderTotalAdjustment());
                    break;
                case PreDefinedPromotion.OrderFreeShipping:
                    p.Name = "New Free Shipping Discount";
                    p.Mode = PromotionType.Offer;
                    p.AddQualification(new PromotionQualifications.AnyShippingMethod());
                    p.AddAction(new PromotionActions.OrderShippingAdjustment(AmountTypes.Percent, -100m));
                    break;
                case PreDefinedPromotion.OrderShippingDiscount:
                    p.Name = "New Free Shipping Discount";
                    p.Mode = PromotionType.Offer;
                    p.AddQualification(new PromotionQualifications.AnyShippingMethod());
                    p.AddAction(new PromotionActions.OrderShippingAdjustment(AmountTypes.Percent, 0m));
                    break;
            }

            return p;
        }
      
    }
}
