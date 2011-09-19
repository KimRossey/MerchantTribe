using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping.USPostal
{
    public class USPSServiceCode
    {
        public int Id { get; set; }
        public string FriendlyName { get; set; }
        public string XmlName { get; set; }
        public int ClassId { get; set; }
        public int InternationalClassId { get; set; }

        public USPSServiceCode()
        {
            Id = -1;
            FriendlyName = "All Available Services";
            XmlName = "ALL";
            ClassId = -1;
            InternationalClassId = -1;
        }

        public static List<USPSServiceCode> ListAll()
        {
            List<USPSServiceCode> result = new List<USPSServiceCode>();
                       
            result.Add(new USPSServiceCode(){FriendlyName = "Global Express Guaranteed", ClassId = 10});
            result.Add(new USPSServiceCode(){FriendlyName = "Global Express Guaranteed Non-Document Rectangular", ClassId = 101});
            result.Add(new USPSServiceCode(){FriendlyName = "Global Express Guaranteed Non-Document Non-Rectangular", ClassId = 102});
            result.Add(new USPSServiceCode() { FriendlyName = "First-Class Mail International", ClassId = 100 }); // 64 max pounds
            result.Add(new USPSServiceCode() { FriendlyName = "Express Mail International (EMS)", ClassId = 9 });
            result.Add(new USPSServiceCode() { FriendlyName = "Express Mail International (EMS) Flat Rate Envelope", ClassId = 109 });
            result.Add(new USPSServiceCode(){FriendlyName = "Priority Mail International", ClassId = 11});
            result.Add(new USPSServiceCode(){FriendlyName = "Priority Mail International Flat Rate Envelope", ClassId = 110});
            result.Add(new USPSServiceCode(){FriendlyName = "Priority Mail International Flat Rate Box", ClassId = 111});
            result.Add(new USPSServiceCode(){FriendlyName = "Intl. Airmail Letter Post", ClassId = 50});
            result.Add(new USPSServiceCode(){FriendlyName = "Intl. Airmail Parcel Post", ClassId = 51});
            result.Add(new USPSServiceCode(){FriendlyName = "Intl. Economy Letter Post", ClassId = 52});
            result.Add(new USPSServiceCode(){ FriendlyName = "Intl. Economy Parcel Post", ClassId = 53 });

            return result;
        }
        public static USPSServiceCode FindById(int id)
        {
            var x = ListAll().Where(y => y.Id == id).SingleOrDefault();
            return x;
        }
        public static USPSServiceCode FindByClassId(int classId)
        {
            var x = ListAll().Where(y => y.ClassId == classId).SingleOrDefault();
            return x;
        }
        public static USPSServiceCode FindByInternationalClassId(int classId)
        {
            var x = ListAll().Where(y => y.InternationalClassId == classId).SingleOrDefault();
            return x;
        }
        public static USPSServiceCode FindByXmlName(string xmlName)
        {
            var x = ListAll().Where(y => y.XmlName == xmlName).SingleOrDefault();
            return x;
        }

    }
}
