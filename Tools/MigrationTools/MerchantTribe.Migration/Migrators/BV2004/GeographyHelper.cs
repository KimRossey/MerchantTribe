using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Migration.Migrators.BV2004
{
    public class GeographyHelper
    {
        public static Country TranslateCountry(string connectionString, string countryCode)
        {
            Country result = new Country();
            data.bvc2004Entities db = new data.bvc2004Entities(connectionString);
            var old = db.bvc_Country.Where(y => y.Code == countryCode).FirstOrDefault();
            if (old == null) return Country.FindByISOCode("US");
            result = Country.FindByISOCode(old.UPSCode);
            return result;
        }

        public static string TranslateRegionBvinToAbbreviation(string connString, string stateCode)
        {
            string result = string.Empty;
            int stateId = 0;
            int.TryParse(stateCode, out stateId);
            data.bvc2004Entities db = new data.bvc2004Entities(connString);
            var old = db.bvc_Region.Where(y => y.ID == stateId).FirstOrDefault();
            if (old == null) return result;
            result = old.Abbreviation;
            return result;
        }
    }
}
