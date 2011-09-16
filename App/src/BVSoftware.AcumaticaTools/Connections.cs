using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class Connections
    {
        public static bool TestConnection(string name, string password, string siteAddress)
        {
            Screen gate;

            if (string.IsNullOrEmpty(siteAddress))
            {
				gate = new Screen();
            }
            else
            {
				gate = new Screen();
            }

            gate.CookieContainer = new System.Net.CookieContainer();
            gate.AllowAutoRedirect = true;
            gate.EnableDecompression = true;
            gate.Timeout = 1000000;
			
            string url = siteAddress.ToString() + "soap/bvcommerce.asmx";
            gate.Url = url;


            LoginResult result = gate.Login(name, password);
            if (result.Code == ErrorCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ServiceContext Login(string username, string password, string siteAddress)
        {
            ServiceContext result = new ServiceContext();
                    
            result.Gate.CookieContainer = new System.Net.CookieContainer();
            result.Gate.AllowAutoRedirect = true;
            result.Gate.EnableDecompression = true;
            result.Gate.Timeout = 1000000;

            string url = siteAddress.ToString() + "soap/bvcommerce.asmx";
            result.Gate.Url = url;

            LoginResult lr = result.Gate.Login(username, password);
            if (lr.Code != ErrorCode.OK)
            {
                Console.WriteLine("Failed to Login!");
                result.Errors.Add(new ServiceError() { Description = "Login Failed", ErrorCode = "" });
                result.HasLoggedIn = false;
            }
            else
            {
                result.HasLoggedIn = true;
            }

            return result;
        }

        public static void RunSimpleTest(ServiceContext context)
        {
            Console.WriteLine("Starting Simple Test");

            ServiceContext _ServiceContext = new ServiceContext();
            _ServiceContext.Password = "password";
            _ServiceContext.Username = "admin";
            _ServiceContext.SiteAddress = "http://localhost/acumaticaERP2/";
            _ServiceContext.NewItemTaxAccountId = "TAXABLE";
            _ServiceContext.NewItemWarehouseId = "RETAIL    ";
            _ServiceContext.DefaultCustomerPaymentMethod = "AMEX";
            _ServiceContext.UseFullCustomerNameInsteadOfAutoId = false;
            _ServiceContext.NewLineItemWarehouseId = "RETAIL    ";


            //Console.WriteLine("Updating Customer Email");
            //Customers.UpdateCustomerEmail("ABCSTUDIOS", "emailNew@nowhere.com", context);


            // Get Order Test
            Console.WriteLine("Getting Customer Order Records");
            string customerId = Customers.GetCustomerIdByEmail("update2@bvsoftware.com", _ServiceContext);
            MerchantTribe.Web.Text.TrimToLength(customerId, 10);
            if (customerId.Trim().Length > 0)
            {
                List<OrderSummaryData> result = Orders.ListAllOrdersForCustomer(customerId, context);
                foreach (OrderSummaryData d in result)
                {
                    Console.WriteLine(d.Number + " " + d.TimeOfOrder.ToString() + " " + d.Amount.ToString("C") + " " + d.Status);
                }
            }                                    
            Console.WriteLine("Finished pulling orders");



            //AR303000Content schema = context.Gate.AR303000GetSchema();
            //string[][] result = context.Gate.AR303000Export(new Command[] 
            //{ 
            //    schema.CustomerSummary.ServiceCommands.EveryCustomer,
            //    schema.CustomerSummary.Customer,
            //    schema.CustomerSummary.CustomerName,				
            //}, null, 0, false, true);

            //foreach (string[] row in result)
            //{
            //    String customerName = row[1];
            //    Console.WriteLine("Customer: " + customerName);
            //}
        }
    }
}
