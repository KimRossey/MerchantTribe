using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using MerchantTribe.Shipping.USPostal.v4;
using MerchantTribe.Shipping;

namespace MerchantTribe.Shipping.USPostal
{
    public class DomesticProvider : MerchantTribe.Shipping.IShippingService
    {
     
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();
        private List<IServiceCode> _Codes = new List<IServiceCode>();
        private MerchantTribe.Web.Logging.ILogger _Logger = new MerchantTribe.Web.Logging.SupressLogger();

        public string Id
        {
            get { return "B28F245B-8FE5-404E-A857-A6D01904A29A"; }
        }
        public string Name
        {
            get { return "US Postal Service - Domestic"; }
        }
        public USPostalServiceGlobalSettings GlobalSettings { get; set; }
        public USPostalServiceSettings Settings { get; set; }
        public ServiceSettings BaseSettings
        {
            get { return Settings; }
        }
        public List<ShippingServiceMessage> LatestMessages
        {
            get
            {
                return _Messages;
            }
            set
            {
                _Messages = value;
            }
        }
        public List<IServiceCode> ListAllServiceCodes()
        {
            return _Codes;            
        }

        public DomesticProvider(USPostalServiceGlobalSettings globalSettings, MerchantTribe.Web.Logging.ILogger logger)
        {
            _Logger = logger;
            this.GlobalSettings = globalSettings;            
            Settings = new USPostalServiceSettings();
            InitializeCodes();
        }        
        private void InitializeCodes()
        {
            List<IServiceCode> result = new List<IServiceCode>();

            result.Add(new ServiceCode() { Code="-1", DisplayName="All Available Services"});
            result.Add(new ServiceCode() { Code="0", DisplayName="First-Class"});
            result.Add(new ServiceCode() { Code="1", DisplayName="Priority Mail"});
            result.Add(new ServiceCode() { Code="2", DisplayName="Express Mail"});
            result.Add(new ServiceCode() { Code="3", DisplayName="Express Mail Sunday/Holiday"});
            result.Add(new ServiceCode() { Code="4", DisplayName="Express Mail Hold for Pickup"});
            result.Add(new ServiceCode() { Code="6", DisplayName="Parcel Post"});
            result.Add(new ServiceCode() { Code="7", DisplayName="Media Mail"});
            result.Add(new ServiceCode() { Code="8", DisplayName="Library Material"});       
     
            _Codes = result;
        }

        public string GetTrackingUrl(string trackingCode)
        {
            if (trackingCode != string.Empty)
            {
                return "http://trkcnfrm1.smi.usps.com/PTSInternetWeb/InterLabelInquiry.do?origTrackNum=" + trackingCode;
            }
            else
            {
                return "http://www.usps.com";
            }
        }
        
        public bool ShipmentHasAddresses(IShipment shipment)
        {
            if (shipment.SourceAddress == null)
            {
                return false;
            }
            if (shipment.DestinationAddress == null)
            {
                return false;
            }
            return true;
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {
            _Messages.Clear();
            return GetUsPostalRatesForShipment(shipment);
        }

        // Gets all rates filtered by service settings
        public List<IShippingRate> GetUsPostalRatesForShipment(IShipment shipment)
        {
            List<IShippingRate> rates = new List<IShippingRate>();

            List<IShippingRate> allrates = GetAllShippingRatesForShipment(shipment);

            // Filter all rates by just the ones we want
            List<IServiceCode> codefilters = this.Settings.ServiceCodeFilter;

            if (codefilters == null) return allrates;
            if (codefilters.Count < 1) return allrates;
            if (this.Settings.ReturnAllServices()) return allrates;

            foreach (IShippingRate rate in allrates)
            {
                if (codefilters.Where(y => y.Code == rate.ServiceCodes).Count() > 0)
                {
                    rates.Add(rate);
                    continue;
                }
            }

            return rates;
        }

        private List<IShippingRate> GetAllShippingRatesForShipment(IShipment shipment)
        {
            List<IShippingRate> rates = new List<IShippingRate>();

            bool hasErrors = false;
            
            try
            {
                List<DomesticPackage> packagesToRate = OptimizePackages(shipment);

                if (packagesToRate.Count > 0)
                {
                    rates = RatePackages(packagesToRate);
                }
                else
                {
                    if (this.GlobalSettings.DiagnosticsMode)
                    {
                        _Logger.LogMessage("No Packaged to Rate for US Postal Service: Code 795");
                    }
                }
            }

            catch (Exception ex)
            {
                _Logger.LogException(ex);
                ShippingServiceMessage m = new ShippingServiceMessage();
                m.SetError("Exception", ex.Message + " | " + ex.StackTrace);
                _Messages.Add(m);
            }

            if (hasErrors)
            {
                rates = new List<IShippingRate>();
            }
            return rates;
        }


        private List<DomesticPackage> OptimizePackages(IShipment shipment)
        {
            List<DomesticPackage> result = new List<DomesticPackage>();

            // Determine what service to use when processing
            DomesticServiceType service = DomesticServiceType.All;
            service = this.Settings.GetServiceForProcessing();
           
            // Get max weight for this service
            decimal _maxWeight = CalculateMaxWeightPerPackage(service, this.Settings.PackageType);

            // Create many boxes if we exceed max weight
            PackageOptimizer optimizer = new PackageOptimizer(_maxWeight);                                    
            List<IShippable> weightOptimizedPackages = optimizer.OptimizePackagesToMaxWeight(shipment);

            int counter = 0;
            foreach (IShippable s in weightOptimizedPackages)
            {                
                DomesticPackage pak = new DomesticPackage();
                pak.Id = counter.ToString();
                pak.ZipDestination = MerchantTribe.Web.Text.TrimToLength(shipment.DestinationAddress.PostalCode, 5);
                pak.ZipOrigination = MerchantTribe.Web.Text.TrimToLength(shipment.SourceAddress.PostalCode, 5);
                
                pak.Container = this.Settings.PackageType;
                // If we're using first class service, make sure we have a valid package type
                if (service == DomesticServiceType.FirstClass)
                {
                    if ((int)pak.Container < 100)
                    {
                        if (pak.Ounces < 3.5m)
                        {
                            pak.Container = DomesticPackageType.FirstClassLetter;
                        }
                        else
                        {
                            pak.Container = DomesticPackageType.FirstClassParcel;
                        }
                    }
                }

                pak.Ounces = MerchantTribe.Web.Conversions.DecimalPoundsToOunces(s.BoxWeight);
                pak.Pounds = (int)Math.Floor(s.BoxWeight);
                pak.Service = service;

                counter++;
                result.Add(pak);
            }
            
            return result;
        }

      

        private decimal CalculateMaxWeightPerPackage(DomesticServiceType s, DomesticPackageType packageType)
        {
            if (s == DomesticServiceType.FirstClass)
            {
                if (packageType == DomesticPackageType.FirstClassLetter) return USPostalConstants.MaxFirstClassLetterWeightInPounds;
                return USPostalConstants.MaxFirstClassWeightInPounds;
            }

            return USPostalConstants.MaxWeightInPounds;            
        }

        private List<IShippingRate> RatePackages(List<DomesticPackage> packages)
        {

            List<IShippingRate> rates = new List<IShippingRate>();

            DomesticRequest req = new DomesticRequest();
            req.Packages = packages;            

            DomesticService svc = new DomesticService();
            DomesticResponse res = svc.ProcessRequest(req);
                        
            
            if (this.GlobalSettings.DiagnosticsMode)
            {
                _Logger.LogMessage("US Postal Request: " + svc.LastRequest);
                _Logger.LogMessage("US Postal Response: " + svc.LastResponse);                                
            }

            bool hasErrors = (res.Errors.Count > 0);
                                                    
            foreach (DomesticPackageServiceResponse possibleResponse in DomesticPackageServiceResponse.FindAll())            
            {
                bool AllPackagesRated = true;
                decimal totalRate = 0m;

                foreach (DomesticPackage p in res.Packages)
                {
                    DomesticPostage found = p.Postages.Where(y => y.MailServiceClassId == possibleResponse.XmlClassId).FirstOrDefault();
                    if (found == null)
                    {
                        AllPackagesRated = false;
                        break;
                    }

                    totalRate += found.Rate;
                }

                if (AllPackagesRated && totalRate > 0)
                {
                    // Rate is good to go for all packages
                    rates.Add(new ShippingRate() { EstimatedCost = totalRate, ServiceId = this.Id, ServiceCodes = ((int)possibleResponse.ServiceType).ToString(), 
                                                    DisplayName = "USPS:" + possibleResponse.XmlName});
                }            
            }
            
            
            return rates;
        }

      

    }
}
