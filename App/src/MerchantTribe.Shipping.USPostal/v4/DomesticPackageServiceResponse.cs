using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal.v4
{
    class DomesticPackageServiceResponse
    {
        public string XmlName { get; set; }
        public string XmlClassId { get; set; }
        public DomesticServiceType ServiceType { get; set; }

        public DomesticPackageServiceResponse()
        {
            XmlName = string.Empty;
            XmlClassId = string.Empty;
            ServiceType = DomesticServiceType.All;
        }

        public static List<DomesticPackageServiceResponse> FindAll()
        {
            List<DomesticPackageServiceResponse> result = new List<DomesticPackageServiceResponse>();

            // First Class
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "0",XmlName = "First-Class",ServiceType = DomesticServiceType.FirstClass});
            //result.Add(new DomesticPackageServiceResponse() { XmlClassId = "19", XmlName = "First-Class", ServiceType = DomesticServiceType.FirstClass }); 
            //result.Add(new DomesticPackageServiceResponse() { XmlClassId = "12", XmlName = "First-Class Postcard Stamped", ServiceType = DomesticServiceType.FirstClass });
            //result.Add(new DomesticPackageServiceResponse() { XmlClassId = "15", XmlName = "First-Class Large Postcards", ServiceType = DomesticServiceType.FirstClass });

            // Express Mail            
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "3", XmlName = "Express Mail", ServiceType = DomesticServiceType.ExpressMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "13", XmlName = "Express Mail Flat-Rate Envelope", ServiceType = DomesticServiceType.ExpressMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "30", XmlName = "Express Mail Legal Flat-Rate Envelope", ServiceType = DomesticServiceType.ExpressMail });

            // Express Mail Holiday
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "23", XmlName = "Express Mail Sunday/Holiday", ServiceType = DomesticServiceType.ExpressMailSundayHoliday });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "25", XmlName = "Express Mail Flat-Rate Envelope Sunday/Holiday", ServiceType = DomesticServiceType.ExpressMailSundayHoliday });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "32", XmlName = "Express Mail Legal Flat-Rate Envelope Sunday/Holiday", ServiceType = DomesticServiceType.ExpressMailSundayHoliday });

            // Express Mail Hold
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "2", XmlName = "Express Mail Hold for Pickup", ServiceType = DomesticServiceType.ExpressMailHoldForPickup });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "27", XmlName = "Express Mail Flat-Rate Envelope Hold For Pickup", ServiceType = DomesticServiceType.ExpressMailHoldForPickup });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "31", XmlName = "Express Mail Legal Flat-Rate Envelope Hold For Pickup", ServiceType = DomesticServiceType.ExpressMailHoldForPickup });

            // Priority Mail
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "1", XmlName = "Priority Mail", ServiceType = DomesticServiceType.PriorityMail });            
            //result.Add(new DomesticPackageServiceResponse() { XmlClassId = "18", XmlName = "Priority Mail Keys and IDs", ServiceType = DomesticServiceType.PriorityMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "16", XmlName = "Priority Mail Flat-Rate Envelope", ServiceType = DomesticServiceType.PriorityMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "28", XmlName = "Priority Mail Small Flat-Rate Box", ServiceType = DomesticServiceType.PriorityMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "17", XmlName = "Priority Mail Regular Flat-Rate Box", ServiceType = DomesticServiceType.PriorityMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "22", XmlName = "Priority Mail Flat-Rate Large Box", ServiceType = DomesticServiceType.PriorityMail });
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "40", XmlName = "Priority Mail Window Flat-Rate Envelope", ServiceType = DomesticServiceType.PriorityMail });            
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "44", XmlName = "Priority Mail Legal Flat-Rate Envelope", ServiceType = DomesticServiceType.PriorityMail });            

            // Parcel Post                        
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "4", XmlName = "Parcel Post", ServiceType = DomesticServiceType.ParcelPost });

            // Media Mail
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "6", XmlName = "Media Mail", ServiceType = DomesticServiceType.MediaMail });

            // Library Material
            result.Add(new DomesticPackageServiceResponse() { XmlClassId = "7", XmlName = "Library", ServiceType = DomesticServiceType.LibraryMaterial });                                                                        
                                
            return result;
        }
    }
}
