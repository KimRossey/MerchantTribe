using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Services
{
    public class RateTableSettings: ServiceSettings
    {
        public List<RateTableLevel> GetLevels()
        {
            List<RateTableLevel> result = LoadLevelsFromSettings();
            result.Sort();
            return result;
        }

        public bool AddLevel(RateTableLevel r)
        {
            List<RateTableLevel> levels = GetLevels();

            bool found = false;
            foreach (RateTableLevel sl in levels)
            {
                if (sl.Level == r.Level)
                {
                    sl.Rate = r.Rate;
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                levels.Add(r);                
            }
            SaveLevelsToSettings(levels);
            return true;
            
        }

        public bool RemoveLevel(RateTableLevel r)
        {
            bool result = false;
            List<RateTableLevel> levels = GetLevels();
            result = levels.Remove(r);
            SaveLevelsToSettings(levels);
            return result;
        }

        private List<RateTableLevel> LoadLevelsFromSettings()
        {
            List<RateTableLevel> result = new List<RateTableLevel>();

            string json = GetSettingOrEmpty("Levels");
            if (String.IsNullOrEmpty(json)) return result;

            result = MerchantTribe.Web.Json.ObjectFromJson<List<RateTableLevel>>(json);
            
            return result;
        }

        private void SaveLevelsToSettings(List<RateTableLevel> levels)
        {
            string json = MerchantTribe.Web.Json.ObjectToJson(levels);
            AddOrUpdate("Levels", json);
        }
       
    }
}
