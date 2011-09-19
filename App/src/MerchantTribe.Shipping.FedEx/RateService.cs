using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using MerchantTribe.Shipping;
using System.Collections.Generic;
using MerchantTribe.Web.Geography;

namespace MerchantTribe.Shipping.FedEx
{

    public class RateService
    {

        private const int DefaultTimeout = 100000;

        public static string SendRequest(string serviceUrl, string postData)
        {
            return SendRequest(serviceUrl, postData, null);
        }

        public static string SendRequest(string serviceUrl, string postData, System.Net.WebProxy proxy)
        {
            WebResponse objResp;
            WebRequest objReq;
            string strResp = string.Empty;
            byte[] byteReq;

            try
            {
                byteReq = Encoding.UTF8.GetBytes(postData);
                objReq = WebRequest.Create(serviceUrl);
                objReq.Method = "POST";
                objReq.ContentLength = byteReq.Length;
                objReq.ContentType = "application/x-www-form-urlencoded";
                objReq.Timeout = DefaultTimeout;
                if (proxy != null)
                {
                    objReq.Proxy = proxy;
                }
                Stream OutStream = objReq.GetRequestStream();
                OutStream.Write(byteReq, 0, byteReq.Length);
                OutStream.Close();
                objResp = objReq.GetResponse();
                StreamReader sr = new StreamReader(objResp.GetResponseStream(), Encoding.UTF8, true);
                strResp += sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error SendRequest: " + ex.Message + " " + ex.Source);
            }

            return strResp;
        }

        private RateService()
        {

        }

        public static ShippingRate RatePackage(FedExGlobalServiceSettings globals,
                                       MerchantTribe.Web.Logging.ILogger logger,
                                       FedExServiceSettings settings,
                                       IShipment package)
        {
            ShippingRate result = new ShippingRate();

                // Get ServiceType
                ServiceType currentServiceType = ServiceType.FEDEXGROUND;
                currentServiceType = (ServiceType)settings.ServiceCode;

                // Get PackageType
                PackageType currentPackagingType = PackageType.YOURPACKAGING;
                currentPackagingType = (PackageType)settings.Packaging;

                // Set max weight by service
                CarrierCode carCode = GetCarrierCode(currentServiceType);

                result.EstimatedCost = RateSinglePackage(globals, 
                                                        logger,
                                                        package, 
                                                        currentServiceType, 
                                                        currentPackagingType, 
                                                        carCode);

            return result;
        }

      
        private static CarrierCode GetCarrierCode(ServiceType service)
        {
            CarrierCode result = CarrierCode.FDXG;

            switch (service)
            {
                case ServiceType.EUROPEFIRSTINTERNATIONALPRIORITY:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.FEDEX1DAYFREIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.FEDEX2DAY:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.FEDEX2DAYFREIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.FEDEX3DAYFREIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.FEDEXEXPRESSSAVER:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.FEDEXGROUND:
                    result = CarrierCode.FDXG;
                    break;
                case ServiceType.FIRSTOVERNIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.GROUNDHOMEDELIVERY:
                    result = CarrierCode.FDXG;
                    break;
                case ServiceType.INTERNATIONALECONOMY:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.INTERNATIONALECONOMYFREIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.INTERNATIONALFIRST:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.INTERNATIONALPRIORITY:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.INTERNATIONALPRIORITYFREIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.PRIORITYOVERNIGHT:
                    result = CarrierCode.FDXE;
                    break;
                case ServiceType.STANDARDOVERNIGHT:
                    result = CarrierCode.FDXE;
                    break;
                default:
                    result = CarrierCode.FDXE;
                    break;
            }

            return result;
        }

        private static decimal RateSinglePackage(FedExGlobalServiceSettings globalSettings,
                                                 MerchantTribe.Web.Logging.ILogger logger,
                                                 IShipment pak, 
                                                 ServiceType service, 
                                                 PackageType packaging, 
                                                 CarrierCode carCode)
        {
            decimal result = 0m;
            
            //Try
            RateRequest req = new RateRequest(globalSettings, logger);
            req.RequestHeader.AccountNumber = globalSettings.AccountNumber;
            req.RequestHeader.MeterNumber = globalSettings.MeterNumber;
            req.RequestHeader.CarrierCode = carCode;

            req.DeclaredValue = 0.1m;

            // Destination Address            
            Country destinationCountry = Country.FindByBvin(pak.DestinationAddress.CountryData.Bvin);
            if (destinationCountry != null)
            {
                req.DestinationAddress.CountryCode = destinationCountry.IsoCode;
                if (destinationCountry.IsoCode == "US" | destinationCountry.IsoCode == "CA")
                {
                    Region destinationRegion 
                        = destinationCountry
                            .Regions
                            .Where(y => y.Abbreviation == pak.DestinationAddress.RegionData.Abbreviation)
                            .SingleOrDefault();

                    req.DestinationAddress.StateOrProvinceCode = destinationRegion.Abbreviation;
                }
            }
            req.DestinationAddress.PostalCode = pak.DestinationAddress.PostalCode;

            // Origin Address
            Country originCountry = Country.FindByBvin(pak.SourceAddress.CountryData.Bvin);
            if (originCountry != null)
            {
                req.OriginAddress.CountryCode = originCountry.IsoCode;
                if (originCountry.IsoCode == "US" | originCountry.IsoCode == "CA")
                {
                    Region originRegion = 
                        originCountry.Regions.Where(y => y.Abbreviation == pak.SourceAddress.RegionData.Abbreviation)
                        .SingleOrDefault();
                    req.OriginAddress.StateOrProvinceCode = originRegion.Abbreviation;
                }
            }
            req.OriginAddress.PostalCode = pak.SourceAddress.PostalCode;

            // Dimensions
            req.Dimensions.Length = pak.Items[0].BoxLength;
            req.Dimensions.Width = pak.Items[0].BoxWidth;
            req.Dimensions.Height = pak.Items[0].BoxHeight;
            //switch ()
            //{
            //    case MerchantTribe.Commerce.Shipping.LengthType.Centimeters:
            //        req.Dimensions.Units = DimensionType.CM;
            //        break;
                //case MerchantTribe.Commerce.Shipping.LengthType.Inches:
                    req.Dimensions.Units = DimensionType.IN;
            //        break;
            //}

            req.PackageCount = 1;
            req.Packaging = packaging;
            req.ReturnType = ReturnShipmentIndicatorType.NONRETURN;
            req.Service = service;
            req.SpecialServices.ResidentialDelivery = globalSettings.ForceResidentialRates;
            req.Weight = pak.Items[0].BoxWeight;
            //switch (WebAppSettings.ApplicationWeightUnits)
            //{
            //    case MerchantTribe.Commerce.Shipping.WeightType.Kilograms:
            //        req.WeightUnits = WeightType.KGS;
            //        break;
            //    case MerchantTribe.Commerce.Shipping.WeightType.Pounds:
                    req.WeightUnits = WeightType.LBS;
            //        break;
            //}


            RateResponse res = req.Send();

            if (res.Errors.Count > 0)
            {
                result = 0m;
            }
            else
            {
                if (globalSettings.UseListRates)
                {
                    if (res.EstimatedCharges.ListCharges.NetCharge > 0)
                    {
                        result = res.EstimatedCharges.ListCharges.NetCharge;
                    }
                    else
                    {
                        result = res.EstimatedCharges.DiscountedCharges.NetCharge;
                    }
                }
                else
                {
                    result = res.EstimatedCharges.DiscountedCharges.NetCharge;
                }
            }

            return result;
        }

    }

}