using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class OrderHasCoupon : PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdOrderHasCoupon + "}"); }
        }
        public List<String> CurrentCoupons()
        {
            List<String> result = new List<string>();
            string all = GetSetting("coupons");
            string[] parts = all.Split(',');
            foreach (string s in parts)
            {
                if (s != string.Empty)
                {
                    result.Add(s.Trim().ToUpperInvariant());
                }
            }
            return result;
        }
        private void SaveCouponsToSettings(List<String> coupons)
        {
            string all = string.Empty;
            foreach (string s in coupons)
            {
                if (s != string.Empty)
                {
                    all += s.Trim().ToUpperInvariant() + ",";
                }
            }
            all = all.TrimEnd(',');
            SetSetting("coupons", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "When Order Has Any of These Coupon Codes:<ul>";
            foreach (string coupon in this.CurrentCoupons())
            {                
                result += "<li>" + coupon + "</li>";
            }
            result += "</ul>";
            return result;
        }
        public OrderHasCoupon():base()
        {
            this.ProcessingCost = RelativeProcessingCost.Normal;            
        }

        public void AddCoupon(string coupon)
        {
            List<String> _Coupons = CurrentCoupons();

            string possible = coupon.Trim().ToUpperInvariant();
            if (possible == string.Empty) return;
            if (_Coupons.Contains(possible)) return;
            _Coupons.Add(possible);
            SaveCouponsToSettings(_Coupons);
        }
        public void RemoveCoupon(string coupon)
        {
            List<String> _Coupons = this.CurrentCoupons();
            if (_Coupons.Contains(coupon.Trim().ToUpperInvariant()))
            {
                _Coupons.Remove(coupon.Trim().ToUpperInvariant());
                SaveCouponsToSettings(_Coupons);
            }
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.Mode != PromotionType.Offer) return false;
            if (context.Order == null) return false;
            if (context.Order.Coupons == null) return false;

            foreach (string coupon in CurrentCoupons())
            {
                if (context.Order.CouponCodeExists(coupon)) return true;
            }

            return false;            
        }
    }
}