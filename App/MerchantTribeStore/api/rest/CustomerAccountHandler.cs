using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Membership;
using MerchantTribe.CommerceDTO.v1.Contacts;
using MerchantTribe.Commerce.Membership;
using MerchantTribe.Commerce.Contacts;

namespace BVCommerce.api.rest
{
    public class CustomerAccountHandler: BaseRestHandler
    {
        public CustomerAccountHandler(MerchantTribe.Commerce.BVApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;
            string email = querystring["email"];

            if ((string.Empty == (parameters ?? string.Empty)) && (email.Trim().Length < 1))
            {
                // List
                ApiResponse<List<CustomerAccountDTO>> response = new ApiResponse<List<CustomerAccountDTO>>();

                List<CustomerAccount> results = BVApp.MembershipServices.Customers.FindAll();
                List<CustomerAccountDTO> dto = new List<CustomerAccountDTO>();

                foreach (CustomerAccount item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<CustomerAccountDTO> response = new ApiResponse<CustomerAccountDTO>();                                
                string bvin = FirstParameter(parameters);

                CustomerAccount item;
                if (email.Trim().Length > 0)
                {
                    item = BVApp.MembershipServices.Customers.FindByEmail(email);
                }
                else
                {
                    item = BVApp.MembershipServices.Customers.Find(bvin);
                }
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check bvin and try again."));
                }
                else
                {
                    response.Content = item.ToDto();
                }
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }                                   

            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<CustomerAccountDTO> response = new ApiResponse<CustomerAccountDTO>();

            CustomerAccountDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<CustomerAccountDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            CustomerAccount item = new CustomerAccount();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                CustomerAccount existing = BVApp.MembershipServices.Customers.FindByEmail(item.Email);
                if (existing == null || existing.Bvin == string.Empty)
                {
                    string clearPassword = querystring["pwd"];
                    if (clearPassword.Trim().Length < 1)
                    {
                        clearPassword = MerchantTribe.Web.PasswordGenerator.GeneratePassword(10);
                    }
                    // Create
                    bool result = BVApp.MembershipServices.CreateCustomer(item, clearPassword);
                    bvin = item.Bvin;
                }
                else
                {
                    bvin = existing.Bvin;
                }
            }
            else
            {
                BVApp.MembershipServices.UpdateCustomer(item);
            }
            CustomerAccount resultItem = BVApp.MembershipServices.Customers.Find(bvin);
            if (resultItem != null)
            {
                response.Content = resultItem.ToDto();
                // Address Import
                foreach (AddressDTO a in postedItem.Addresses)
                {
                    Address addr = new Address();
                    addr.FromDto(a);
                    BVApp.MembershipServices.CheckIfNewAddressAndAddWithUpdate(resultItem,addr);
                }
            }
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            if (bvin == string.Empty)
            {
                // Clear All Requested
                response.Content = BVApp.MembershipServices.DestroyAllCustomers(BVApp);
            }
            else
            {
                // Delete Single Customer
                response.Content = BVApp.DestroyCustomerAccount(bvin);
            }

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}