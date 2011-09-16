using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.Shipping;
using BVSoftware.Shipping.FedEx;

namespace BVSoftware.Commerce.Shipping
{    
    public class AvailableServices
    {

        public static List<IShippingService> FindAll(Accounts.Store currentStore)
        {
            
            List<IShippingService> result = new List<IShippingService>();

            result = Service.FindAll();

            //result.Add(new BVSoftware.Shipping.Fedex.FedExProvider());
            //FedEx            
            BVSoftware.Shipping.FedEx.FedExGlobalServiceSettings fedexGlobal = new FedExGlobalServiceSettings();
            fedexGlobal.AccountNumber = currentStore.Settings.ShippingFedExAccountNumber;
            fedexGlobal.MeterNumber = currentStore.Settings.ShippingFedExMeterNumber;
            fedexGlobal.DefaultDropOffType = (BVSoftware.Shipping.FedEx.DropOffType)currentStore.Settings.ShippingFedExDropOffType;
            fedexGlobal.DefaultPackaging = (BVSoftware.Shipping.FedEx.PackageType)currentStore.Settings.ShippingFedExDefaultPackaging;
            fedexGlobal.DiagnosticsMode = currentStore.Settings.ShippingFedExDiagnostics;
            fedexGlobal.ForceResidentialRates = currentStore.Settings.ShippingFedExForceResidentialRates;
            fedexGlobal.UseListRates = currentStore.Settings.ShippingFedExUseListRates;
            result.Add(new BVSoftware.Shipping.FedEx.FedExProvider(fedexGlobal, new EventLog()));

            // Load US Postal
            BVSoftware.Shipping.USPostal.USPostalServiceGlobalSettings uspostalGlobal = new BVSoftware.Shipping.USPostal.USPostalServiceGlobalSettings();
            uspostalGlobal.DiagnosticsMode = currentStore.Settings.ShippingUSPostalDiagnostics;            
            result.Add(new BVSoftware.Shipping.USPostal.DomesticProvider(uspostalGlobal, new EventLog()));           
            result.Add(new BVSoftware.Shipping.USPostal.InternationalProvider(uspostalGlobal, new EventLog()));
             
            // Load UPS
            BVSoftware.Shipping.Ups.UPSServiceGlobalSettings upsglobal = new BVSoftware.Shipping.Ups.UPSServiceGlobalSettings();
            upsglobal.AccountNumber = currentStore.Settings.ShippingUpsAccountNumber;
            upsglobal.LicenseNumber = currentStore.Settings.ShippingUpsLicense;
            upsglobal.Username = currentStore.Settings.ShippingUpsUsername;
            upsglobal.Password = currentStore.Settings.ShippingUpsPassword;
            upsglobal.DefaultPackaging = (BVSoftware.Shipping.Ups.PackagingType)currentStore.Settings.ShippingUpsDefaultPackaging;
            upsglobal.DiagnosticsMode = currentStore.Settings.ShippingUPSDiagnostics;
            upsglobal.ForceResidential = currentStore.Settings.ShippingUpsForceResidential;
            upsglobal.IgnoreDimensions = currentStore.Settings.ShippingUpsSkipDimensions;
            upsglobal.PickUpType = (BVSoftware.Shipping.Ups.PickupType)currentStore.Settings.ShippingUpsPickupType;
            result.Add(new BVSoftware.Shipping.Ups.UPSService(upsglobal, new EventLog()));            
            
            return result;
        }
        
        public static IShippingService FindById(string id, Accounts.Store currentStore)
        {
            foreach (IShippingService svc in FindAll(currentStore))
            {
                if (svc.Id == id)
                {
                    return svc;
                }
            }            
            return null;
        }
    }
}
