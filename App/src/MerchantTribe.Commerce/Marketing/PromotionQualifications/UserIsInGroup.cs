using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Marketing.PromotionQualifications
{
    public class UserIsInGroup: PromotionQualificationBase
    {
        public override Guid TypeId
        {
            get { return new Guid("{" + PromotionQualificationBase.TypeIdUserIsInGroup + "}"); }
        }
        public List<String> CurrentGroupIds()
        {
            List<String> result = new List<string>();
            string all = GetSetting("groupids");
            string[] parts = all.Split(',');
            foreach (string s in parts)
            {
                if (s != string.Empty)
                {
                    result.Add(s.Trim().ToLowerInvariant());
                }
            }
            return result;
        }
        private void SaveGroupIdsToSettings(List<String> groupids)
        {
            string all = string.Empty;
            foreach (string s in groupids)
            {
                if (s != string.Empty)
                {
                    all += s.Trim().ToLowerInvariant() + ",";
                }
            }
            all = all.TrimEnd(',');
            SetSetting("groupids", all);
        }

        public override string FriendlyDescription(MerchantTribeApplication app)
        {
            string result = "When Current User Is In Group:<ul>";
            
            foreach (string gid in this.CurrentGroupIds())
            {                
                Contacts.PriceGroup g = app.ContactServices.PriceGroups.Find(gid);
                if (g != null)
                {
                    result += "<li>" + g.Name + "</li>";
                }
            }
            result += "</ul>";
            return result;
        }
        public UserIsInGroup()
            : base()
        {
            this.ProcessingCost = RelativeProcessingCost.Lower;            
        }

        public void AddGroup(string groupid)
        {
            List<String> _Groups = CurrentGroupIds();

            string possible = groupid.Trim().ToLowerInvariant();
            if (possible == string.Empty) return;
            if (_Groups.Contains(possible)) return;
            _Groups.Add(possible);
            SaveGroupIdsToSettings(_Groups);
        }
        public void RemoveGroup(string groupid)
        {
            List<String> _Groups = this.CurrentGroupIds();
            if (_Groups.Contains(groupid.Trim().ToLowerInvariant()))
            {
                _Groups.Remove(groupid.Trim().ToLowerInvariant());
                SaveGroupIdsToSettings(_Groups);
            }
        }

        public override bool MeetsQualification(PromotionContext context)
        {
            if (context == null) return false;
            if (context.CurrentCustomer == null) return false;
            if (context.CurrentCustomer.PricingGroupId == string.Empty) return false;
            
            if (CurrentGroupIds().Contains(context.CurrentCustomer.PricingGroupId.Trim().ToLowerInvariant()))
            {
                return true;
            }

            return false;
        }
    }
}