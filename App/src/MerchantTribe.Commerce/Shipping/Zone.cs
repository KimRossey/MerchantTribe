using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web.Geography;
using MerchantTribe.Web.Validation;

namespace MerchantTribe.Commerce.Shipping
{
    public class Zone : IValidatable
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Name { get; set; }
        public List<ZoneArea> Areas { get; set; }
        public Zone()
        {
            Id = 0;
            StoreId = 0;
            Name = string.Empty;
            Areas = new List<ZoneArea>();
        }

        public bool IsBuiltInZone
        {
            get { return (this.Id < 0); }
        }

        public bool AddressIsInZone(IAddress address)
        {
            bool result = false;

            Country c = Country.FindByBvin(address.CountryData.Bvin);
            if (c != null)
            {
                foreach (ZoneArea a in Areas)
                {
                    if (a.CountryIsoAlpha3.Trim().ToLowerInvariant() ==
                        c.IsoAlpha3.Trim().ToLowerInvariant())
                    {
                        // Country matches
                        if (a.RegionAbbreviation.Trim() == string.Empty)
                        {
                            // empty region abbreviation means match all
                            return true;
                        }
                        else
                        {
                            if (address.RegionData.Abbreviation.Trim().ToLowerInvariant() ==
                                a.RegionAbbreviation.Trim().ToLowerInvariant())
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static Zone UnitedStatesAll()
        {
            Zone usAll = new Zone();
            usAll.Id = -100;
            usAll.Name = "United States - All";
            usAll.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "" });
            return usAll;
        }

        public static Zone UnitedStates48Contiguous()
        {
            Zone us48 = new Zone();
            us48.Id = -101;
            us48.Name = "United States - 48 contiguous states";

            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AL" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AZ" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AR" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AE" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AE" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AP" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "CA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "CO" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "CT" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "DE" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "DC" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "FL" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "GA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "ID" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "IL" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "IN" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "IA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "KS" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "KY" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "LA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "ME" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MD" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MI" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MN" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MS" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MO" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "MT" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NE" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NV" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NH" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NJ" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NM" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NY" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "NC" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "ND" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "OH" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "OK" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "OR" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "PA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "RI" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "SC" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "SD" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "TN" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "TX" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "UT" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "VT" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "VA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "WA" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "WV" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "WI" });
            us48.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "WY" });

            return us48;
        }

        public static Zone UnitedStatesAlaskaAndHawaii()
        {
            Zone z = new Zone();
            z.Id = -102;
            z.Name = "United States - Alaska and Hawaii";

            z.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "AK" });
            z.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = "USA", RegionAbbreviation = "HI" });

            return z;
        }

        public static Zone International(string homeCountryIsoAlpha3)
        {
            Zone international = new Zone();
            international.Id = -103;
            international.Name = "International";
            foreach (Country c in Country.FindAll())
            {
                if (c.IsoAlpha3.Trim().ToLowerInvariant() != homeCountryIsoAlpha3.Trim().ToLowerInvariant())
                {
                    international.Areas.Add(new ZoneArea() { CountryIsoAlpha3 = c.IsoAlpha3, RegionAbbreviation = "" });
                }
            }

            return international;
        }

        #region IValidatable Members

        public bool IsValid()
        {
            if (GetRuleViolations().Count > 0)
            {
                return false;
            }
            return true;
        }

        public List<RuleViolation> GetRuleViolations()
        {
            List<MerchantTribe.Web.Validation.RuleViolation> violations = new List<MerchantTribe.Web.Validation.RuleViolation>();

            MerchantTribe.Web.Validation.ValidationHelper.Required("Name", Name, violations, "name");

            // Check name already exists
            //MerchantTribe.Web.Validation.ValidationHelper.ValidateFalse(ZoneManager.NameExists(this.Name, this.Id, StoreId),
            //                                                                "A zone with that name already exists",
            //                                                                "Name", Name, violations, "name");

            return violations;
        }

        #endregion
    }
}
