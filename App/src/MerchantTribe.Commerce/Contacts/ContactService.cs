using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Contacts
{
    public class ContactService
    {
        private RequestContext context = null;

        public AffiliateRepository Affiliates { get; private set; }
        public AffiliateReferralRepository AffiliateReferrals { get; private set; }
        public AddressRepository Addresses { get; private set; }
        public PriceGroupRepository PriceGroups { get; private set; }
        public MailingListRepository MailingLists { get; private set; }
        public VendorRepository Vendors { get; private set; }
        public ManufacturerRepository Manufacturers { get; private set; }

        public static ContactService InstantiateForMemory(RequestContext c)
        {
            return new ContactService(c,
                                      AddressRepository.InstantiateForMemory(c),
                                      PriceGroupRepository.InstantiateForMemory(c),
                                      MailingListRepository.InstantiateForMemory(c),
                                      VendorRepository.InstantiateForMemory(c),
                                      ManufacturerRepository.InstantiateForMemory(c),
                                      AffiliateRepository.InstantiateForMemory(c),
                                      AffiliateReferralRepository.InstantiateForMemory(c));
        }
        public static ContactService InstantiateForDatabase(RequestContext c)
        {
            return new ContactService(c,
                                      AddressRepository.InstantiateForDatabase(c),
                                      PriceGroupRepository.InstantiateForDatabase(c),
                                      MailingListRepository.InstantiateForDatabase(c),
                                      VendorRepository.InstantiateForDatabase(c),
                                      ManufacturerRepository.InstantiateForDatabase(c),
                                      AffiliateRepository.InstantiateForDatabase(c),
                                      AffiliateReferralRepository.InstantiateForDatabase(c));
        }
        public ContactService(RequestContext c,
                              AddressRepository addresses,
                              PriceGroupRepository pricegroups,
                              MailingListRepository mailingLists,
                              VendorRepository vendors,
                              ManufacturerRepository manufacturers,
                              AffiliateRepository affiliates,
                              AffiliateReferralRepository affiliateReferrals)
        {
            context = c;
            Addresses = addresses;
            PriceGroups = pricegroups;
            this.MailingLists = mailingLists;
            this.Vendors = vendors;
            this.Manufacturers = manufacturers;
            this.Affiliates = affiliates;
            this.AffiliateReferrals = affiliateReferrals;
        }


        public void RecordAffiliateReferral(string referrerId, string referralUrl, MerchantTribeApplication app)
        {
            long current = SessionManager.CurrentAffiliateID(app.CurrentStore);

            // Need to Set if Mode is Force New Affiliate
            bool NeedToSet = (this.context.CurrentStore.Settings.AffiliateConflictMode == Contacts.AffiliateConflictMode.FavorNewAffiliate);            
            
            // Need to Set if no current Affiliate
            if (current < 1) NeedToSet = true;

            if (NeedToSet)
            {
                SetNewAffiliate(referrerId, referralUrl, app);
            }                            
            else
            {
                LogReferral(current, referralUrl);
            }

        }
        private bool SetNewAffiliate(string referrerId, string referrerUrl, MerchantTribeApplication app)
        {

            Contacts.Affiliate aff = Affiliates.FindByReferralId(referrerId);
            if (aff == null) return false;
            if (!aff.Enabled) return false;

            if (aff.Id != SessionManager.CurrentAffiliateID(app.CurrentStore))
            {
                System.DateTime expires = System.DateTime.UtcNow;
                if (aff.ReferralDays > 0)
                {                    
                    TimeSpan ts = new TimeSpan(aff.ReferralDays, 0, 0, 0);
                    expires = expires.Add(ts);                    
                }
                else
                {
                    expires = System.DateTime.UtcNow.AddYears(50);
                }
                SessionManager.SetCurrentAffiliateId(aff.Id, expires, app.CurrentStore);

            }
            return LogReferral(aff.Id, referrerUrl);
        }
        private bool LogReferral(long affiliateId, string referrerUrl)
        {

            Contacts.Affiliate aff = Affiliates.Find(affiliateId);
            if (aff == null) return false;
            if (!aff.Enabled) return false;

            AffiliateReferral r = new AffiliateReferral();
            r.AffiliateId = aff.Id;
            r.ReferrerUrl = referrerUrl;
            return AffiliateReferrals.Create(r);                        
        }

        public long GetValidAffiliateId(MerchantTribeApplication app)
        {            
            long current = SessionManager.CurrentAffiliateID(app.CurrentStore);
            if (current < 0) return 0;
            
            Contacts.Affiliate aff = Affiliates.Find(current);
            if (aff == null) return 0;
            if (!aff.Enabled) return 0;
                
            return current;
        }
    }
}
