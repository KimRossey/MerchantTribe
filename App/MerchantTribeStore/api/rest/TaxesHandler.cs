﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVSoftware.Commerce;
using BVSoftware.CommerceDTO.v1;
using BVSoftware.CommerceDTO.v1.Taxes;
using BVSoftware.Commerce.Taxes;

namespace BVCommerce.api.rest
{
    public class TaxesHandler: BaseRestHandler
    {
        public TaxesHandler(BVSoftware.Commerce.BVApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;

            if (string.Empty == (parameters ?? string.Empty))
            {
                // List
                ApiResponse<List<TaxDTO>> response = new ApiResponse<List<TaxDTO>>();

                List<Tax> results = BVApp.OrderServices.Taxes.FindByStoreId(BVApp.CurrentStore.Id);
                List<TaxDTO> dto = new List<TaxDTO>();

                foreach (Tax item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<TaxDTO> response = new ApiResponse<TaxDTO>();
                string ids = FirstParameter(parameters);
                long id = 0;
                long.TryParse(ids, out id);
                Tax item = BVApp.OrderServices.Taxes.Find(id);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that Tax. Check id and try again."));
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
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);
            ApiResponse<TaxDTO> response = new ApiResponse<TaxDTO>();

            TaxDTO postedCategory = null;
            try
            {
                postedCategory = MerchantTribe.Web.Json.ObjectFromJson<TaxDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            Tax item = new Tax();
            item.FromDto(postedCategory);

            if (id < 1)
            {
                if (!BVApp.OrderServices.Taxes.ExactMatchExists(item))
                {
                    if (BVApp.OrderServices.Taxes.Create(item))
                    {
                        id = item.Id;
                    }
                }
            }
            else
            {
                BVApp.OrderServices.Taxes.Update(item);
            }                               
            response.Content = item.ToDto();            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = BVApp.OrderServices.Taxes.Delete(id);
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}