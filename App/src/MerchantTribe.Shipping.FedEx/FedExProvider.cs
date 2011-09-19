using System.Collections.ObjectModel;
using MerchantTribe.Shipping;
using MerchantTribe.Shipping.Services;
using System.Web;
using System;
using System.Collections.Generic;
using MerchantTribe.Web.Geography;
using System.Linq;

namespace MerchantTribe.Shipping.FedEx
{

    public class FedExProvider : IShippingService
    {

        private List<ShippingServiceMessage> _Messages = new List<ShippingServiceMessage>();
        private MerchantTribe.Web.Logging.ILogger _Logger = new MerchantTribe.Web.Logging.SupressLogger();

        public string Name
        {
            get { return "FedEx"; }
        }
        public string Id
        {
            get { return "43CF0D39-4E2D-4f9d-AF65-87EDF5FF84EA"; }
        }

        public FedExGlobalServiceSettings GlobalSettings { get; set; }
        public FedExServiceSettings Settings { get; set; }
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

        public FedExProvider(FedExGlobalServiceSettings globalSettings, MerchantTribe.Web.Logging.ILogger logger)
        {
            _Logger = logger;
            GlobalSettings = globalSettings;
            Settings = new FedExServiceSettings();
        }

        public List<IShippingRate> RateShipment(IShipment shipment)
        {
            _Messages.Clear();

            List<IShippingRate> result = new List<IShippingRate>();

            bool hasErrors = false;


            int serviceCode = Settings.ServiceCode;
            int packaging = Settings.Packaging;

            try
            {
                List<Shipment> optimizedPackages = this.OptimizeSingleGroup(shipment);

                if (optimizedPackages.Count > 0)
                {
                    result = RatePackages(optimizedPackages);
                }
                else
                {
                    if (this.GlobalSettings.DiagnosticsMode)
                    {
                        _Logger.LogMessage("No packages found to rate for FedEx");
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
                result = new List<IShippingRate>();
            }            
            return result;
        }

        private List<IShippingRate> RatePackages(List<Shipment> packages)
        {
            List<IShippingRate> result = new List<IShippingRate>();

            // Loop through packages, getting rates for each package 
            bool allPackagesRated = true;

            List<ShippingRate> individualRates = new List<ShippingRate>();

            foreach (IShipment s in packages)
            {                
                
                ShippingRate singlePackageRate = new ShippingRate();
                singlePackageRate = RateService.RatePackage(this.GlobalSettings, 
                                                            this._Logger, 
                                                            Settings, 
                                                            s);

                if (singlePackageRate == null)
                {
                    allPackagesRated = false;
                    break;
                }
                else
                {
                    if (singlePackageRate.EstimatedCost < 0)
                    {
                        allPackagesRated = false;
                        break;
                    }
                    else
                    {
                        individualRates.Add(singlePackageRate);
                    }
                }
            }

            //we are done with all packages for this shipping type
            if (allPackagesRated)
            {
                decimal total = 0m;
                foreach (ShippingRate rate in individualRates)
                {
                        total += rate.EstimatedCost;
                }
                if (total > 0m)
                {
                    if (individualRates.Count > 0)
                    {
                        result.Add(
                            new ShippingRate() { EstimatedCost = total,
                                                 DisplayName = this.FindNameByServiceCode(this.Settings.ServiceCode),
                                                ServiceCodes = this.Settings.ServiceCode.ToString(),
                                                ServiceId = this.Id});
                    }
                }
            }

            return result;
        }

        protected bool IsOversized(IShippable prod)
        {
            bool IsOversize = false;
            double girth = (double)(prod.BoxLength + (2 * prod.BoxHeight) + (2 * prod.BoxWidth));
            if ((girth - 130 > 0))
            {
                //this is an oversize product
                IsOversize = true;
            }
            else
            {
                if (prod.BoxHeight > 108)
                {
                    IsOversize = true;
                }
                else if (prod.BoxLength > 108)
                {
                    IsOversize = true;
                }
                else if (prod.BoxWidth > 108)
                {
                    IsOversize = true;
                }
            }

            return IsOversize;
        }

        public string GetTrackingUrl(string trackingCode)
        {
            return "http://www.fedex.com/Tracking?language=english&cntry_code=us&tracknumbers=" + trackingCode;
        }

        public string FindNameByServiceCode(int serviceCode)
        {
            string result = "FedEx";
            List<IServiceCode> codes = ListAllServiceCodes();
            var temp = codes.Where(y => y.Code == serviceCode.ToString()).FirstOrDefault();
            if (temp != null)
            {
                result = temp.DisplayName;
            }
            return result;
        }

        public List<IServiceCode> ListAllServiceCodes()
        {            
            List<IServiceCode> result = new List<IServiceCode>();

            result.Add(new ServiceCode("Priority Overnight", "1"));
            result.Add(new ServiceCode("Standard Overnight", "2"));
            result.Add(new ServiceCode("First Overnight", "3"));
            result.Add(new ServiceCode("FedEx 2 Day", "4"));
            result.Add(new ServiceCode("FedEx Express Saver", "5"));
            result.Add(new ServiceCode("International Priority", "6"));
            result.Add(new ServiceCode("International Economy", "7"));
            result.Add(new ServiceCode("International First", "8"));
            result.Add(new ServiceCode("FedEx 1 Day Freight", "9"));
            result.Add(new ServiceCode("FedEx 2 Day Freight", "10"));
            result.Add(new ServiceCode("FedEx 3 Day Freight", "11"));
            result.Add(new ServiceCode("FedEx Ground", "12"));
            result.Add(new ServiceCode("Ground Home Delivery", "13"));
            result.Add(new ServiceCode("International Priority Freight", "14"));
            result.Add(new ServiceCode("International Economy Freight", "15"));
            result.Add(new ServiceCode("Europe First International Priority", "16"));

            return result;
        }
     
        public List<Shipment> OptimizeSingleGroup(IShipment shipment)
        {
            decimal MAXWEIGHT = 70;
            
            // Set Max Weight for Ground Services
            if (this.Settings.ServiceCode == (int)ServiceType.FEDEXGROUND ||
                this.Settings.ServiceCode == (int)ServiceType.GROUNDHOMEDELIVERY)
            {
                MAXWEIGHT = 150;
            }


            List<Shipment> result = new List<Shipment>();

            List<IShippable> itemsToSplit = new List<IShippable>();

            foreach (IShippable item in shipment.Items)
            {
                if (IsOversized(item))
                {
                    Shipment s1 = Shipment.CloneAddressesFromInterface(shipment);
                    s1.Items.Add(item.CloneShippable());
                    result.Add(s1);
                }
                else
                {
                    itemsToSplit.Add(item);
                }
            }


            IShippable tempPackage = new Shippable();

            foreach (IShippable pak in itemsToSplit)
            {
                if (MAXWEIGHT - tempPackage.BoxWeight >= pak.BoxWeight)
                {
                    // add to current box
                    tempPackage.BoxWeight += pak.BoxWeight;
                    tempPackage.QuantityOfItemsInBox += pak.QuantityOfItemsInBox;
                    tempPackage.BoxValue += pak.BoxValue;
                }
                else
                {
                    // Save the temp package if it has items
                    if (tempPackage.BoxWeight > 0 || tempPackage.QuantityOfItemsInBox > 0)
                    {
                        Shipment s2 = Shipment.CloneAddressesFromInterface(shipment);
                        s2.Items.Add(tempPackage.CloneShippable());
                        result.Add(s2);

                        tempPackage = new Shippable();
                    }

                    // create new box
                    if (pak.BoxWeight > MAXWEIGHT)
                    {
                        //Loop to break up > maxWeight Packages
                        int currentItemsInBox = pak.QuantityOfItemsInBox;
                        decimal currentWeight = pak.BoxWeight;

                        while (currentWeight > 0)
                        {
                            if (currentWeight > MAXWEIGHT)
                            {
                                IShippable newP = pak.CloneShippable();
                                newP.BoxWeight = MAXWEIGHT;
                                if (currentItemsInBox > 0)
                                {
                                    currentItemsInBox -= 1;
                                    newP.QuantityOfItemsInBox = 1;
                                }

                                Shipment s3 = Shipment.CloneAddressesFromInterface(shipment);
                                s3.Items.Add(newP.CloneShippable());
                                result.Add(s3);                                
                                currentWeight = currentWeight - MAXWEIGHT;
                                if (currentWeight < 0)
                                {
                                    currentWeight = 0;
                                }
                            }
                            else
                            {
                                // Create a new shippable box 
                                IShippable newP = pak.CloneShippable();
                                newP.BoxWeight = currentWeight;
                                if (currentItemsInBox > 0)
                                {
                                    newP.QuantityOfItemsInBox = currentItemsInBox;
                                }
                                Shipment s4 = Shipment.CloneAddressesFromInterface(shipment);
                                s4.Items.Add(newP.CloneShippable());
                                result.Add(s4);
                                currentWeight = 0;
                            }
                        }
                    }
                    else
                    {
                        tempPackage = pak.CloneShippable();
                    }
                }
            }

            // Save the temp package if it has items
            if (tempPackage.BoxWeight > 0 || tempPackage.QuantityOfItemsInBox > 0)
            {

                Shipment s5 = Shipment.CloneAddressesFromInterface(shipment);
                s5.Items.Add(tempPackage.CloneShippable());
                result.Add(s5);

                tempPackage = new Shippable();
            }

            return result;
        }


     
    }

}