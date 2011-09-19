using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Accounts
{
    public class HostedPlan
    {
        public int Id {get;set;}
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public decimal PercentageOfSales { get; set; }
        public int MaxProducts { get; set; }
        public string Limits { get; set; }
        public decimal SalesCap { get; set; }
        

        public HostedPlan()
        {
            Id = 0;
            Name = string.Empty;
            Rate = 0;
            PercentageOfSales = 0;
            MaxProducts = 0;
            Limits = string.Empty;
            SalesCap = 0;
        }

        public static List<HostedPlan> FindAll()
        {
            List<HostedPlan> result = new List<HostedPlan>();
        
            result.Add(new HostedPlan() {Id=0, Name = "Trial Store", Rate = 0, PercentageOfSales = 0, SalesCap = 1000, Limits = "PayPal Only", MaxProducts = 10 });
            result.Add(new HostedPlan() {Id=1, Name = "Basic", Rate = 49, PercentageOfSales = 0, SalesCap = 0, Limits = "All Payment Types", MaxProducts = 100 });
            result.Add(new HostedPlan() {Id=2, Name = "Plus", Rate = 99, PercentageOfSales = 0, SalesCap = 0, Limits = "All Payment Types", MaxProducts = 5000 });
            result.Add(new HostedPlan() {Id=3, Name = "Premium", Rate = 199, PercentageOfSales = 0, SalesCap = 0, Limits = "All Payment Types", MaxProducts = 10000 });
            result.Add(new HostedPlan() {Id=99, Name = "Max", Rate = 499, PercentageOfSales = 0, SalesCap = 0, Limits = "All Payment Types", MaxProducts = 50000 });

            return result;
        }

        public static HostedPlan FindById(int id)
        {
            foreach (HostedPlan h in FindAll())
            {
                if (h.Id == id) return h;
            }
            return null;
        }

    }
}
