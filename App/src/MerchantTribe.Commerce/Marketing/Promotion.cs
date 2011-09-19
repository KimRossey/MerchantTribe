using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using MerchantTribe.Commerce.Marketing.PromotionQualifications;
using MerchantTribe.Commerce.Marketing.PromotionActions;

namespace MerchantTribe.Commerce.Marketing
{
    public class Promotion
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public PromotionType Mode { get; set; }
        public DateTime LastUpdatedUtc { get; set; }        
        public string Name { get; set; }
        public string CustomerDescription { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public bool IsEnabled { get; set; }

        private List<IPromotionQualification> _Qualifications = new List<IPromotionQualification>();
        public ReadOnlyCollection<IPromotionQualification> Qualifications
        {
            get { return _Qualifications.AsReadOnly(); }
        }
        private List<IPromotionAction> _Actions = new List<IPromotionAction>();
        public ReadOnlyCollection<IPromotionAction> Actions
        {
            get { return _Actions.AsReadOnly(); }
        }

        public Promotion()
        {
            Id = 0;                        
            StoreId = 0;
            Mode = PromotionType.Sale;
            LastUpdatedUtc = DateTime.UtcNow;
            Name = "New Promotion";
            CustomerDescription = string.Empty;
            StartDateUtc = MerchantTribe.Web.Dates.ZeroOutTime(DateTime.Now).ToUniversalTime();
            EndDateUtc = MerchantTribe.Web.Dates.MaxOutTime(DateTime.Now.AddMonths(12)).ToUniversalTime();
            IsEnabled = false;
        }

        public PromotionStatus GetStatus(DateTime currentUtcTime)
        {
            if (!this.IsEnabled) return PromotionStatus.Disabled;

            if (StartDateUtc.Ticks > currentUtcTime.Ticks) return PromotionStatus.Upcoming;

            if (EndDateUtc.Ticks < currentUtcTime.Ticks) return PromotionStatus.Expired;

            return PromotionStatus. Active;
        }
           
        public string ActionsToXml()
        {            
            var x = new XElement("Actions",
                        from a in this.Actions
                        select new XElement("Action",
                                                new XElement("Id", a.Id),
                                                new XElement("TypeId", a.TypeId),
                                                new XElement("Settings", 
                                                                from s in a.Settings 
                                                                select new XElement("Setting",
                                                                                    new XElement("Key", s.Key),
                                                                                    new XElement("Value", s.Value)
                                                                                   )                    
                                                            )
                                            )
                               );
            return x.ToString(SaveOptions.None);
        }
        public void ActionsFromXml(string xml)
        {
            this._Actions.Clear();
            if (xml.Trim().Length < 1) return;
            
            XDocument doc = XDocument.Parse(xml, LoadOptions.None);
            var query = from xElem in doc.Descendants("Action")
                        select ActionFactory(xElem.Descendants());
            this._Actions.AddRange(query);            
        }
        private IPromotionAction ActionFactory(IEnumerable<XElement> nodes)
        {
            if (nodes == null) return null;
            
            XElement nodeTypeId = nodes.Where(y => y.Name == "TypeId").FirstOrDefault();
            if (nodeTypeId == null) return null;
            Guid typeId = new Guid(nodeTypeId.Value);

            IPromotionAction result = PromotionActionBase.Factory(typeId.ToString().ToUpperInvariant());
            
            if (result == null) return null;

            XElement nodeId = nodes.Where(y => y.Name == "Id").FirstOrDefault();
            if (nodeId != null)
            {
                long temp = 0;
                long.TryParse(nodeId.Value, out temp);
                result.Id = temp;
            }
            XElement nodeSettings = nodes.Where(y => y.Name == "Settings").FirstOrDefault();
            if (nodeSettings != null)
            {
                foreach (XElement setting in nodeSettings.Descendants("Setting"))
                {
                    string key = setting.Element("Key").Value;
                    string value = setting.Element("Value").Value;
                    result.Settings[key] = value;
                }
            }

            return result;
        }

        public string QualificationsToXml()
        {
            var x = new XElement("Qualifications",
                        from q in this.Qualifications
                        select new XElement("Qualification",
                                                new XElement("Id", q.Id),
                                                new XElement("TypeId", q.TypeId),
                                                new XElement("ProcessingCost", (int)q.ProcessingCost),
                                                new XElement("Settings", 
                                                    from s in q.Settings
                                                    select new XElement("Setting",
                                                        new XElement("Key", s.Key),
                                                        new XElement("Value", s.Value)
                                                        )
                                                    )
                                            )
                        );
            return x.ToString(SaveOptions.None);
        }
        public void QualificationsFromXml(string xml)
        {
            this._Qualifications.Clear();
            if (xml.Trim().Length < 1) return;

            XDocument doc = XDocument.Parse(xml, LoadOptions.None);
            var query = from xElem in doc.Descendants("Qualification")
                        select QualificationFactory(xElem.Descendants());
            this._Qualifications.AddRange(query);       
        }
        private IPromotionQualification QualificationFactory(IEnumerable<XElement> nodes)
        {
            if (nodes == null) return null;

            XElement nodeTypeId = nodes.Where(y => y.Name == "TypeId").FirstOrDefault();
            if (nodeTypeId == null) return null;
            Guid typeId = new Guid(nodeTypeId.Value);

            IPromotionQualification result = PromotionQualificationBase.Factory(typeId.ToString().ToUpperInvariant());

            if (result == null) return null;

            XElement nodeId = nodes.Where(y => y.Name == "Id").FirstOrDefault();
            if (nodeId != null)
            {
                long temp = 0;
                long.TryParse(nodeId.Value, out temp);
                result.Id = temp;
            }
            XElement nodeSettings = nodes.Where(y => y.Name == "Settings").FirstOrDefault();
            if (nodeSettings != null)
            {
                foreach (XElement setting in nodeSettings.Descendants("Setting"))
                {
                    string key = setting.Element("Key").Value;
                    string value = setting.Element("Value").Value;
                    result.Settings[key] = value;
                }
            }

            return result;
        }

        public bool AddQualification(IPromotionQualification q)
        {

            long maxid = 0;
            if (this._Qualifications.Count > 0)
            {
                maxid = this._Qualifications.Max(y => y.Id);
            }
            if (maxid < 0) maxid = 0;
    
            q.Id = maxid + 1;
            _Qualifications.Add(q);

            return true;
        }
        public bool RemoveQualification(long id)
        {
            var d = _Qualifications.Where(y => y.Id == id).SingleOrDefault();
            if (d != null)
            {
                _Qualifications.Remove(d);
                return true;
            }
            return false;
        }
        public IPromotionQualification GetQualification(long id)
        {
            var d = _Qualifications.Where(y => y.Id == id).SingleOrDefault();
            return d;
        }
        public bool AddAction(IPromotionAction a)
        {

            long maxid = 0;
            if (this._Actions.Count > 0)
            {
                maxid = this._Actions.Max(y => y.Id);
            }
            if (maxid < 0) maxid = 0;
            
            a.Id = maxid + 1;
            _Actions.Add(a);

            return true;
        }
        public bool RemoveAction(long id)
        {
            var d = _Actions.Where(y => y.Id == id).SingleOrDefault();
            if (d != null)
            {
                _Actions.Remove(d);
                return true;
            }
            return false;
        }
        public IPromotionAction GetAction(long id)
        {
            var d = _Actions.Where(y => y.Id == id).SingleOrDefault();
            return d;
        }
        public bool ApplyToProduct(MerchantTribeApplication app, 
                                   Catalog.Product p, 
                                   Catalog.UserSpecificPrice price, 
                                   Membership.CustomerAccount currentCustomer, 
                                   DateTime currentDateTimeUtc)
        {
            if (app == null) return false;
            if (p == null) return false;
            if (price == null) return false;
            if (currentDateTimeUtc == null) return false;

            PromotionContext context = new PromotionContext(app, p, price, currentCustomer, currentDateTimeUtc);
            context.CustomerDescription = this.CustomerDescription;

            // Make sure we have an active promotion before applying
            if (GetStatus(context.CurrentDateAndTimeUtc) != PromotionStatus.Active) return false;

            // Make sure we meet all requirements
            // NOTE: we order by processing cost which should allow us to check
            // the fastest items first. For example, checking userID is faster
            // than checking user group because ID is in the context and group
            // requires a database call.
            foreach (IPromotionQualification q in this._Qualifications.OrderBy(y => y.ProcessingCost))
            {
                if (!q.MeetsQualification(context)) return false;
            }

            // We're qualified, do actions
            foreach (IPromotionAction a in this._Actions)
            {
                a.ApplyAction(context, PromotionActionMode.Unknown);
            }

            return true;
        }
        public bool ApplyToOrder(MerchantTribeApplication app,
                                 Orders.Order o,
                                 Membership.CustomerAccount currentCustomer,
                                 DateTime currentDateTimeUtc,
                                 PromotionActionMode mode)
        {
            if (app == null) return false;
            if (o == null) return false;
            if (currentDateTimeUtc == null) return false;

            PromotionContext context = new PromotionContext(app, o, currentCustomer, currentDateTimeUtc);
            context.CustomerDescription = this.CustomerDescription;

            // Make sure we have an active promotion before applying
            if (GetStatus(context.CurrentDateAndTimeUtc) != PromotionStatus.Active) return false;

            // Make sure we meet all requirements
            // NOTE: we order by processing cost which should allow us to check
            // the fastest items first. For example, checking userID is faster
            // than checking user group because ID is in the context and group
            // requires a database call.
            foreach (IPromotionQualification q in this._Qualifications.OrderBy(y => y.ProcessingCost))
            {
                if (!q.MeetsQualification(context)) return false;
            }

            // We're qualified, do actions
            foreach (IPromotionAction a in this._Actions)
            {
                a.ApplyAction(context, mode);
            }

            return true;
        }
        
    }
}
