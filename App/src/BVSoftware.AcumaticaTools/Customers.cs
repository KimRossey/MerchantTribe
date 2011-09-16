using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class Customers
    {
        // TEST 
        public static bool UpdateCustomerEmail(string customerID, string emailNew, ServiceContext context)
        {
            AR303000Content schema = context.AR303000_Schema;
            context.Gate.AR303000Submit(new Command[]
            {
                new Value { Value = customerID, LinkedCommand = schema.CustomerSummary.Customer },
                new Value { Value = emailNew, LinkedCommand = schema.GeneralInfoCompanyMainInfo.Email },
                schema.Actions.Save
            });

            return true;
        }

        public static string GetCustomerIdByEmail(string email, ServiceContext context)
        {
            string customerId = string.Empty;

			AR303000Content schema = context.AR303000_Schema;            

			string[][] data = context.Gate.AR303000Export(new Command[] 
			{ 
				schema.CustomerSummary.ServiceCommands.EveryCustomer,
				schema.CustomerSummary.Customer,
				schema.CustomerSummary.CustomerName,
				schema.GeneralInfoCompanyMainInfo.Email,
			},
			new Filter[]
			{
				new Filter { Field = schema.CustomerSummary.ServiceCommands.FilterEmail, Condition = FilterCondition.Contain, Value = email}
				//new Filter {Field = schema.CustomerSummary.ServiceCommands.FilterEmail, Condition = FilterCondition.Equals, Value = "info@Artrages.con" }
			}, 0, false, true);

			if (data.Length > 0)
			{
				string found_email = data[0][2].Trim();
				if (String.Compare(found_email, email, true) == 0)
				{
					customerId = data[0][0];
				}
			}
                        
            return customerId;
        }

        public static string GetOrCreateCustomer(CustomerData data, ServiceContext context)
        {
            string customerId = string.Empty;

            // Check for existing Customer
            customerId = GetCustomerIdByEmail(data.Email, context);
            if (customerId != string.Empty)
            {
                return customerId;
            }

            // Create Customer
            context.Errors.Add(new ServiceError() { Description = "Creating Customer because existing not found.", ErrorCode = "INFO" });
            
            customerId = CreateNewCustomer(data, context);

            return customerId;
        }

        private static string GenerateCustomerId(string email, bool createFromEmail)
        {
            string result = "<NEW>";

            if (createFromEmail)
            {
                try
                {
                    result = email.Replace("@", "");
                    result = result.Replace(".", "");
                    result = result.Replace("-", "");
                    result = result.Replace("_", "");
                    result = result.ToUpperInvariant();
                    result = MerchantTribe.Web.Text.TrimToLength(result, 10);
                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public static string CreateNewCustomer(CustomerData data, ServiceContext context)
        {
            context.Errors.Clear();

            string customerId = string.Empty;
			
			AR303000Content schema = context.AR303000_Schema;
            try
            {
                // Generate Customer ID
                string newId = GenerateCustomerId(data.Email, context.UseFullCustomerNameInsteadOfAutoId);

				context.Gate.AR303000Clear();

                // Safety Check to make sure we have a default country for Acumatica if no customer address yet.
                string country = MerchantTribe.Web.Geography.Country.FindByBvin(data.BillingAddress.CountryData.Bvin).IsoCode;
                if (country == string.Empty) country = "US";
                string shipcountry = MerchantTribe.Web.Geography.Country.FindByBvin(data.ShippingAddress.CountryData.Bvin).IsoCode;
                if (shipcountry == string.Empty) shipcountry = "US";

                // Submit Command                
				AR303000Content[] content = context.Gate.AR303000Submit(new Command[]
				{
				    new Value {Value = newId, LinkedCommand = schema.CustomerSummary.Customer },	                                    
					new Value {Value = data.LastName + ", " + data.FirstName, LinkedCommand = schema.CustomerSummary.CustomerName },
					new Value {Value = data.Email, LinkedCommand = schema.GeneralInfoCompanyMainInfo.Email},
					new Value {Value = data.PaymentMethod, LinkedCommand = schema.PaymentSettingsInboundPaymentDefaultSettingsPaymentMethod.PaymentMethod},
                    new Value {Value = data.PaymentIdentifier, LinkedCommand = schema.PaymentSettingsInboundPaymentDefaultSettingsPaymentMethod.Identifier},
						
                    new Value {Value = data.CompanyName, LinkedCommand = schema.GeneralInfoCompanyMainInfo.BusinessName },
                    new Value {Value = data.PhoneNumber, LinkedCommand = schema.GeneralInfoCompanyMainInfo.Phone1 },
					new Value {Value = data.BillingAddress.Street, LinkedCommand = schema.GeneralInfoMainAddress.AddressLine1 },
                    new Value {Value = data.BillingAddress.Street2, LinkedCommand = schema.GeneralInfoMainAddress.AddressLine2 },
					new Value {Value = data.BillingAddress.City, LinkedCommand = schema.GeneralInfoMainAddress.City },
                	new Value {Value = country, LinkedCommand = schema.GeneralInfoMainAddress.Country },
					new Value {Value = data.BillingAddress.RegionData.Abbreviation, LinkedCommand = schema.GeneralInfoMainAddress.State },
					new Value {Value = MerchantTribe.Web.Text.TrimToLength(data.BillingAddress.PostalCode,5), LinkedCommand = schema.GeneralInfoMainAddress.PostalCode },
								
					new Value {Value = "False", LinkedCommand = schema.DeliverySettingsShippingAddress.SameAsMain },
                    new Value {Value = data.CompanyName, LinkedCommand = schema.DeliverySettingsShippingInfo.BusinessName },
                    new Value {Value = data.PhoneNumber, LinkedCommand = schema.DeliverySettingsShippingInfo.Phone1},
					new Value {Value = data.ShippingAddress.Street, LinkedCommand = schema.DeliverySettingsShippingAddress.AddressLine1 },
                    new Value {Value = data.ShippingAddress.Street2, LinkedCommand = schema.DeliverySettingsShippingAddress.AddressLine2 },
					new Value {Value = data.ShippingAddress.City, LinkedCommand = schema.DeliverySettingsShippingAddress.City },
					new Value {Value = shipcountry, LinkedCommand = schema.DeliverySettingsShippingAddress.Country },
					new Value {Value = data.ShippingAddress.RegionData.Abbreviation, LinkedCommand = schema.DeliverySettingsShippingAddress.State },
					new Value {Value = MerchantTribe.Web.Text.TrimToLength(data.ShippingAddress.PostalCode,5), LinkedCommand = schema.DeliverySettingsShippingAddress.PostalCode },

					schema.CustomerSummary.Customer,
					schema.Actions.Save,
				});                      
            }
            catch (Exception ex)
            {
                context.Errors.Add(new ServiceError() { Description = ex.Message + " " + ex.StackTrace });
                return string.Empty;
            }

            // Search for customer in Acumatica to verify creation
            customerId = GetCustomerIdByEmail(data.Email, context);

            return customerId;
        }

        public static bool UpdateCustomerData(CustomerData data, ServiceContext context)
        {
            context.Errors.Clear();
            
            AR303000Content schema = context.AR303000_Schema;
            try
            {
                context.Gate.AR303000Clear();

                // Safety Check to make sure we have a default country for Acumatica if no customer address yet.
                string country = MerchantTribe.Web.Geography.Country.FindByBvin(data.BillingAddress.CountryData.Bvin).IsoCode;
                if (country == string.Empty) country = "US";
                string shipcountry = MerchantTribe.Web.Geography.Country.FindByBvin(data.ShippingAddress.CountryData.Bvin).IsoCode;
                if (shipcountry == string.Empty) shipcountry = "US";

                // Submit Command
                AR303000Content[] content = context.Gate.AR303000Submit(new Command[]
				{
					new Value {Value = data.AcumaticaId, LinkedCommand = schema.CustomerSummary.Customer },
					new Value {Value = data.LastName + ", " + data.FirstName, LinkedCommand = schema.CustomerSummary.CustomerName },
					
					new Value {Value = data.Email, LinkedCommand = schema.GeneralInfoCompanyMainInfo.Email},
					new Value {Value = data.PaymentMethod, LinkedCommand = schema.PaymentSettingsInboundPaymentDefaultSettingsPaymentMethod.PaymentMethod},
                    new Value {Value = data.PaymentIdentifier, LinkedCommand = schema.PaymentSettingsInboundPaymentDefaultSettingsPaymentMethod.Identifier},
						
                    new Value {Value = data.CompanyName, LinkedCommand = schema.GeneralInfoCompanyMainInfo.BusinessName},
                    new Value {Value = data.PhoneNumber, LinkedCommand = schema.GeneralInfoCompanyMainInfo.Phone1},
					new Value {Value = data.BillingAddress.Street, LinkedCommand = schema.GeneralInfoMainAddress.AddressLine1 },
                    new Value {Value = data.BillingAddress.Street2, LinkedCommand = schema.GeneralInfoMainAddress.AddressLine2},
					new Value {Value = data.BillingAddress.City, LinkedCommand = schema.GeneralInfoMainAddress.City },
                	new Value {Value = country, LinkedCommand = schema.GeneralInfoMainAddress.Country },
					new Value {Value = data.BillingAddress.RegionData.Abbreviation, LinkedCommand = schema.GeneralInfoMainAddress.State },
					new Value {Value = MerchantTribe.Web.Text.TrimToLength(data.BillingAddress.PostalCode,5), LinkedCommand = schema.GeneralInfoMainAddress.PostalCode },
								
					new Value {Value = "False", LinkedCommand = schema.DeliverySettingsShippingAddress.SameAsMain },
                    new Value {Value = data.CompanyName, LinkedCommand = schema.DeliverySettingsShippingInfo.BusinessName},
                    new Value {Value = data.PhoneNumber, LinkedCommand = schema.DeliverySettingsShippingInfo.Phone1},
					new Value {Value = data.ShippingAddress.Street, LinkedCommand = schema.DeliverySettingsShippingAddress.AddressLine1 },
                    new Value {Value = data.ShippingAddress.Street2, LinkedCommand = schema.DeliverySettingsShippingAddress.AddressLine2 },
					new Value {Value = data.ShippingAddress.City, LinkedCommand = schema.DeliverySettingsShippingAddress.City },
					new Value {Value = shipcountry, LinkedCommand = schema.DeliverySettingsShippingAddress.Country },
					new Value {Value = data.ShippingAddress.RegionData.Abbreviation, LinkedCommand = schema.DeliverySettingsShippingAddress.State },
					new Value {Value = MerchantTribe.Web.Text.TrimToLength(data.ShippingAddress.PostalCode, 5), LinkedCommand = schema.DeliverySettingsShippingAddress.PostalCode },

					schema.CustomerSummary.Customer,
					schema.Actions.Save,
				});
            }
            catch (Exception ex)
            {
                context.Errors.Add(new ServiceError() { Description = ex.Message + " " + ex.StackTrace });
                return false;
            }

            return true;            
        }

        public static List<CustomerData> FindCustomersUpdatedAfter(DateTime lastcheckUtc, ServiceContext context)
        {
            List<CustomerData> result = new List<CustomerData>();

            AR303000Content schema = context.AR303000_Schema;

            string[][] data = context.Gate.AR303000Export(new Command[] 
            {
                schema.CustomerSummary.ServiceCommands.EveryCustomer,
                schema.CustomerSummary.Customer,
                schema.CustomerSummary.CustomerName,                                                
                schema.GeneralInfoCompanyMainInfo.Email,
            },
            new Filter[]
            {
                new Filter {
                      Field = new Field { ObjectName = schema.CustomerSummary.Customer.ObjectName, FieldName = "LastModifiedDateTime" },
                      Condition = FilterCondition.GreaterOrEqual,
                      Value = lastcheckUtc,
                      Operator = FilterOperator.Or
                      },
                new Filter {
                      Field = new Field { ObjectName = schema.CustomerSummary.Customer.ObjectName, FieldName = "Contact__LastModifiedDateTime" },
                      Condition = FilterCondition.GreaterOrEqual,
                      Value = lastcheckUtc
                      }
            },
            0,
            false,
            false
            );

            if (data.Length > 0)
            {
                foreach (string[] contactData in data)
                {
                    CustomerData c = new CustomerData();
                    c.AcumaticaId = contactData[0] ?? string.Empty;
                    c.Email = contactData[2] ?? string.Empty;
                    result.Add(c);
                }                
            }

            return result;
        }

        public static CustomerData GetCustomer(string acumaticaId, ServiceContext context)
        {
            string customerId = string.Empty;
            
            AR303000Content schema = context.AR303000_Schema;

            AR303000Content[] content = context.Gate.AR303000Submit(new Command[]            
			{ 
                new Key() { Value = "='" + acumaticaId + "'", 
					FieldName = schema.CustomerSummary.Customer.FieldName,
					ObjectName = schema.CustomerSummary.Customer.ObjectName,
				},
                schema.CustomerSummary.Customer,
                schema.CustomerSummary.CustomerName,
                schema.GeneralInfoCompanyMainInfo.Email,	
			    
                schema.GeneralInfoCompanyMainInfo.BusinessName,
                schema.GeneralInfoCompanyMainInfo.Phone1,
                schema.GeneralInfoMainAddress.AddressLine1,
                schema.GeneralInfoMainAddress.AddressLine2,
                schema.GeneralInfoMainAddress.City,
                schema.GeneralInfoMainAddress.Country,                
                schema.GeneralInfoMainAddress.PostalCode,
                schema.GeneralInfoMainAddress.State,

                schema.DeliverySettingsShippingInfo.BusinessName,
                schema.DeliverySettingsShippingAddress.AddressLine1,
                schema.DeliverySettingsShippingAddress.AddressLine2,
                schema.DeliverySettingsShippingAddress.City,
                schema.DeliverySettingsShippingAddress.Country,
                schema.DeliverySettingsShippingAddress.PostalCode,
                schema.DeliverySettingsShippingAddress.State,
                schema.DeliverySettingsShippingInfoSameAsMain.SameAsMain,
                                
                schema.PaymentSettingsBillToAddress.AddressLine1,
                schema.PaymentSettingsBillToAddress.AddressLine2,
                schema.PaymentSettingsBillToAddress.City,
                schema.PaymentSettingsBillToAddress.Country,
                schema.PaymentSettingsBillToAddress.PostalCode,
                schema.PaymentSettingsBillToAddress.State,
                schema.PaymentSettingsBillToAddressSameAsMain.SameAsMain,

                schema.Contacts.Name,

			});

            CustomerData result = new CustomerData();

            if (content.Length > 0)
            {
                AR303000Content adata = content[0];

                result.AcumaticaId = adata.CustomerSummary.Customer.Value ?? string.Empty;
                result.Email = adata.GeneralInfoCompanyMainInfo.Email.Value ?? string.Empty;
                result.FirstName = "";
                result.LastName = "";
                result.CompanyName = adata.GeneralInfoCompanyMainInfo.BusinessName.Value ?? string.Empty;
                result.PhoneNumber = adata.GeneralInfoCompanyMainInfo.Phone1.Value ?? string.Empty;

                if (adata.Contacts != null)
                {
                    if (adata.Contacts.Name != null)
                    {
                        string fullName = adata.Contacts.Name.Value ?? string.Empty;
                        result.FirstName = fullName;
                        result.LastName = fullName;

                        string[] parts = fullName.Split(',');
                        if (parts.Length > 1)
                        {
                            result.FirstName = parts[1];
                            result.LastName = parts[0];
                        }
                    }
                }
                else
                {
                    string fullName = adata.CustomerSummary.CustomerName.Value ?? string.Empty;
                    result.FirstName = fullName;
                    result.LastName = fullName;
                    string[] parts = fullName.Split(',');
                    if (parts.Length > 1)
                    {
                        result.FirstName = parts[1];
                        result.LastName = parts[0];
                    }
                }
                try
                {
                    if (adata.GeneralInfoMainAddress != null)
                    {                        
                        result.BillingAddress.Street = adata.GeneralInfoMainAddress.AddressLine1.Value ?? string.Empty;
                        result.BillingAddress.Street2 = adata.GeneralInfoMainAddress.AddressLine2.Value ?? string.Empty;
                        result.BillingAddress.City = adata.GeneralInfoMainAddress.City.Value ?? string.Empty;
                        result.BillingAddress.RegionData.Name = adata.GeneralInfoMainAddress.State.Value ?? string.Empty;
                        result.BillingAddress.PostalCode = adata.GeneralInfoMainAddress.PostalCode.Value ?? string.Empty;
                        result.BillingAddress.CountryData.Name = adata.GeneralInfoMainAddress.Country.Value ?? string.Empty;

                        result.ShippingAddress.Street = adata.GeneralInfoMainAddress.AddressLine1.Value ?? string.Empty;
                        result.ShippingAddress.Street2 = adata.GeneralInfoMainAddress.AddressLine2.Value ?? string.Empty;
                        result.ShippingAddress.City = adata.GeneralInfoMainAddress.City.Value ?? string.Empty;
                        result.ShippingAddress.RegionData.Name = adata.GeneralInfoMainAddress.State.Value ?? string.Empty;
                        result.ShippingAddress.PostalCode = adata.GeneralInfoMainAddress.PostalCode.Value ?? string.Empty;
                        result.ShippingAddress.CountryData.Name = adata.GeneralInfoMainAddress.Country.Value ?? string.Empty;
                    }
                }
                catch
                {

                }

                try
                {
                    if (adata.PaymentSettingsBillToAddressSameAsMain != null)
                    {
                        if (adata.PaymentSettingsBillToAddressSameAsMain.SameAsMain.Value == "False")
                        {
                            result.BillingAddress.Street = adata.PaymentSettingsBillToAddress.AddressLine1.Value ?? string.Empty;
                            result.BillingAddress.Street2 = adata.PaymentSettingsBillToAddress.AddressLine2.Value ?? string.Empty;
                            result.BillingAddress.City = adata.PaymentSettingsBillToAddress.City.Value ?? string.Empty;
                            result.BillingAddress.RegionData.Name = adata.PaymentSettingsBillToAddress.State.Value ?? string.Empty;
                            result.BillingAddress.PostalCode = adata.PaymentSettingsBillToAddress.PostalCode.Value ?? string.Empty;
                            result.BillingAddress.CountryData.Name = adata.PaymentSettingsBillToAddress.Country.Value ?? string.Empty;
                        }
                    }
                }
                catch
                {

                }

                try
                {
                    if (adata.DeliverySettingsShippingInfoSameAsMain != null)
                    {
                        if (adata.DeliverySettingsShippingInfoSameAsMain.SameAsMain.Value == "False")
                        {
                            result.ShippingAddress.Street = adata.DeliverySettingsShippingAddress.AddressLine1.Value ?? string.Empty;
                            result.ShippingAddress.Street2 = adata.DeliverySettingsShippingAddress.AddressLine2.Value ?? string.Empty;
                            result.ShippingAddress.City = adata.DeliverySettingsShippingAddress.City.Value ?? string.Empty;
                            result.ShippingAddress.RegionData.Name = adata.DeliverySettingsShippingAddress.State.Value ?? string.Empty;
                            result.ShippingAddress.PostalCode = adata.DeliverySettingsShippingAddress.PostalCode.Value ?? string.Empty;
                            result.ShippingAddress.CountryData.Name = adata.DeliverySettingsShippingAddress.Country.Value ?? string.Empty;
                        }
                    }
                }
                catch
                {

                }
            }

            return result;
        }
    }

    
}
