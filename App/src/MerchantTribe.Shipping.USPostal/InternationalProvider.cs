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
    public class InternationalProvider : MerchantTribe.Shipping.IShippingService
    {
     
        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();
        private List<IServiceCode> _Codes = new List<IServiceCode>();
        private MerchantTribe.Web.Logging.ILogger _Logger = new MerchantTribe.Web.Logging.SupressLogger();

        public string Id
        {
            get { return "BD2CB7D9-CEF3-41D7-84A1-44FD420A1CF3"; }
        }
        public string Name
        {
            get { return "US Postal Service - International"; }
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

        public InternationalProvider(USPostalServiceGlobalSettings globalSettings, MerchantTribe.Web.Logging.ILogger logger)
        {
            _Logger = logger;
            this.GlobalSettings = globalSettings;            
            Settings = new USPostalServiceSettings();
            InitializeCodes();
        }        
        private void InitializeCodes()
        {
            List<IServiceCode> result = new List<IServiceCode>();

            result.Add(new ServiceCode() { Code = "-1", DisplayName = "All Available Services" });

            result.Add(new ServiceCode() { Code="13", DisplayName="First-Class International Letter"});
            result.Add(new ServiceCode() { Code="14", DisplayName="First-Class International Flats"});
            result.Add(new ServiceCode() { Code="15", DisplayName="First-Class International Parcel"});
            
            result.Add(new ServiceCode() { Code="1", DisplayName="Express Mail International"});
            result.Add(new ServiceCode() { Code="10", DisplayName="Express Mail International Flat Rate Envelope"});
            result.Add(new ServiceCode() { Code="17", DisplayName="Express Mail International Legal Flat Rate Envelope"});
            
            result.Add(new ServiceCode() { Code="2", DisplayName="Priority Mail International"});
            result.Add(new ServiceCode() { Code="8", DisplayName="Priority Mail International Flat Rate Envelope"});
            result.Add(new ServiceCode() { Code="9", DisplayName="Priority Mail International Medium Flat Rate Box"});
            result.Add(new ServiceCode() { Code="11", DisplayName="Priority Mail International Large Flat Rate Box"});
            result.Add(new ServiceCode() { Code="18", DisplayName="Priority Mail International Gift Card Flat Rate"});
            result.Add(new ServiceCode() { Code="19", DisplayName="Priority Mail International Window Flat Rate Envelope"});
            result.Add(new ServiceCode() { Code="20", DisplayName="Priority Mail International Small Flat Rate Envelope"});
            result.Add(new ServiceCode() { Code="22", DisplayName="Priority Mail International Legal Flat Rate Envelope"});
            result.Add(new ServiceCode() { Code="23", DisplayName="Priority Mail International Padded Flat Rate Envelope"});
            
            result.Add(new ServiceCode() { Code="4", DisplayName="Global Express Guaranteed"});
            result.Add(new ServiceCode() { Code="6", DisplayName="Global Express Guaranteed Rectangular"});
            result.Add(new ServiceCode() { Code="7", DisplayName="Global Express Guaranteed Non-Rectangular"});
            result.Add(new ServiceCode() { Code="12", DisplayName="Global Express Guaranteed Envelopes"});
            
            result.Add(new ServiceCode() { Code="8888", DisplayName="Airmail Parcel Post"});
            result.Add(new ServiceCode() { Code="9999", DisplayName="Airmail Letter"});
                           
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
                List<InternationalPackage> packagesToRate = OptimizePackages(shipment);

                if (packagesToRate.Count > 0)
                {
                    rates = RatePackages(packagesToRate);
                }
                else
                {
                    if (this.GlobalSettings.DiagnosticsMode)
                    {
                        _Logger.LogMessage("No Packaged to Rate for US Postal Service: Code 797");
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


        private List<InternationalPackage> OptimizePackages(IShipment shipment)
        {
            List<InternationalPackage> result = new List<InternationalPackage>();

            // Determine what service to use when processing            
            string destinationCountry = shipment.DestinationAddress.CountryData.Name;
            
            // Get max weight for this service
            decimal _maxWeight = InternationalService.GetInternationalWeightLimit(destinationCountry);

            // Create many boxes if we exceed max weight
            PackageOptimizer optimizer = new PackageOptimizer(_maxWeight);                                    
            List<IShippable> weightOptimizedPackages = optimizer.OptimizePackagesToMaxWeight(shipment);

            int counter = 0;
            foreach (IShippable s in weightOptimizedPackages)
            {
                InternationalPackage pak = new InternationalPackage();
                pak.Id = counter.ToString();
                pak.DestinationCountry = shipment.DestinationAddress.CountryData.Name;
                pak.ZipOrigination = MerchantTribe.Web.Text.TrimToLength(shipment.SourceAddress.PostalCode, 5);

                pak.Container = InternationalContainerType.Rectangular;
                pak.CommercialRates = false;

                pak.Ounces = MerchantTribe.Web.Conversions.DecimalPoundsToOunces(s.BoxWeight);
                pak.Pounds = (int)Math.Floor(s.BoxWeight);
                pak.Length = 6;
                pak.Height = 3;
                pak.Width = 1;
                                
                counter++;
                result.Add(pak);
            }
            
            return result;
        }

     
        private List<IShippingRate> RatePackages(List<InternationalPackage> packages)
        {

            List<IShippingRate> rates = new List<IShippingRate>();

            InternationalRequest req = new InternationalRequest();
            req.Packages = packages;

            InternationalService svc = new InternationalService();
            InternationalResponse res = svc.ProcessRequest(req);
                        
            
            if (this.GlobalSettings.DiagnosticsMode)
            {
                _Logger.LogMessage("US Postal Intl. Request: " + svc.LastRequest);
                _Logger.LogMessage("US Postal Intl. Response: " + svc.LastResponse);                                
            }

            bool hasErrors = (res.Errors.Count > 0);
                
            
                        
            foreach (InternationalServiceType possibleResponse in Enum.GetValues(typeof(InternationalServiceType)))            
            {
                bool AllPackagesRated = true;
                decimal totalRate = 0m;
                string serviceDesciption = string.Empty;

                foreach (InternationalPackage p in res.Packages)
                {
                    InternationalPostage found = p.Postages.Where(y => y.ServiceId == ((int)possibleResponse).ToString()).FirstOrDefault();
                    if (found == null)
                    {                        
                        AllPackagesRated = false;
                        break;
                    }

                    totalRate += found.Rate;
                    serviceDesciption = System.Web.HttpUtility.HtmlDecode(found.ServiceDescription);
                }

                if (AllPackagesRated && totalRate > 0)
                {
                    // Rate is good to go for all packages
                    rates.Add(new ShippingRate() { EstimatedCost = totalRate, ServiceId = this.Id, ServiceCodes = ((int)possibleResponse).ToString(), 
                                                    DisplayName = "USPS:" + serviceDesciption});
                }            
            }
            
            
            return rates;
        }

      

    }
}
