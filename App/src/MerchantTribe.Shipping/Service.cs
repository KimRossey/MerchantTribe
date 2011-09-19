using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Shipping
{
    public class Service
    {
        public static List<IShippingService> FindAll()
        {
            List<IShippingService> result = new List<IShippingService>();

            result.Add(new Services.FlatRatePerItem());
            result.Add(new Services.FlatRatePerOrder());            
            result.Add(new Services.PerItem());
            result.Add(new Services.RateTableByItemCount());
            result.Add(new Services.RateTableByTotalPrice());
            result.Add(new Services.RateTableByTotalWeight());
            result.Add(new Services.RatePerWeightFormula());
            return result;
        }

        public static IShippingService FindById(string id)
        {
            switch (id)
            {
                case "41B590A7-003C-48d1-8446-EAE93C156AA1":
                    return new Services.PerItem();
                case "301AA2B8-F43C-42fe-B77E-A7E1CB1DD40E":
                    return new Services.FlatRatePerOrder();  
                case "3D6623E7-1E2C-444d-B860-A8F542133093":
                    return new Services.FlatRatePerItem();
                case "06C22589-14A7-470f-88EC-AF559D040A7A":
                    return new Services.RateTableByTotalWeight();
                case "9F896073-EE1F-400c-8B54-D9858B06AA01":
                    return new Services.RateTableByTotalPrice();
                case "7068B66A-0336-4228-B1A8-03E034FECCDA":
                    return new Services.RateTableByItemCount();
                case "5AAF9016-B03F-4e7c-8596-193F5EFFFDC3":
                    return new Services.RatePerWeightFormula();
            }

            return null;
        }

    }
}
