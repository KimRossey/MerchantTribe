using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.Ups
{
    public class UPSServiceGlobalSettings
    {
        public string AccountNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool DiagnosticsMode {get;set;}
        public bool ForceResidential {get;set;}
        public bool IgnoreDimensions {get;set;}
        public PickupType PickUpType {get;set;}
        public PackagingType DefaultPackaging {get;set;}

        public UPSServiceGlobalSettings()
        {
            AccountNumber = string.Empty;
            LicenseNumber = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            DiagnosticsMode = false;
            ForceResidential = true;
            IgnoreDimensions = true;
            PickUpType = PickupType.CustomerCounter;
            DefaultPackaging = PackagingType.CustomerSupplied;
        }
    }
}
