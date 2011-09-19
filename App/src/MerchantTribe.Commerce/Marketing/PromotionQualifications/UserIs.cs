using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class UserIs: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdUserIs + "}"); }
        }
        public List<String> UserIds()
        {
            List<String> result = new List<string>();
            string all = GetSetting("userids");
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
        private void SaveUserIdsToSettings(List<String> coupons)
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
            SetSetting("userids", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "When User Is:<ul>";
            
             foreach (string userid in this.UserIds())
            {
                Membership.CustomerAccount c = app.MembershipServices.Customers.Find(userid);
                if (c != null)
                {
                    result += "<li>" + c.Email + "</li>";
                }
            }
         
            result += "</ul>";
            return result;
        }
        public UserIs()
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lower;            
        }

        public void AddUserId(string uid)
        {
            List<String> _UserIds = UserIds();

            string possible = uid.Trim().ToUpperInvariant();
            if (possible == string.Empty) return;
            if (_UserIds.Contains(possible)) return;
            _UserIds.Add(possible);
            SaveUserIdsToSettings(_UserIds);
        }
        public void RemoveUserId(string uid)
        {
            List<String> _UserIds = this.UserIds();
            if (_UserIds.Contains(uid.Trim().ToUpperInvariant()))
            {
                _UserIds.Remove(uid.Trim().ToUpperInvariant());
                SaveUserIdsToSettings(_UserIds);
            }
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.CurrentCustomer == null) return false;
            if (context.CurrentCustomer.Bvin == string.Empty) return false;

            string currentId = context.CurrentCustomer.Bvin.Trim().ToUpperInvariant();

            foreach (string uid in UserIds())
            {
                if (currentId == uid) return true;
            }

            return false;            
        }
    }
}