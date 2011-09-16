using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BVSoftware.Commerce;
using BVSoftware.CommerceDTO.v1;
using BVSoftware.CommerceDTO.v1.Catalog;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce.api.rest
{
    public class ProductTypesHandler: BaseRestHandler
    {
        public ProductTypesHandler(BVSoftware.Commerce.BVApplication app)
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
                ApiResponse<List<ProductTypeDTO>> response = new ApiResponse<List<ProductTypeDTO>>();

                List<ProductType> results = BVApp.CatalogServices.ProductTypes.FindAll();
                List<ProductTypeDTO> dto = new List<ProductTypeDTO>();

                foreach (ProductType item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<ProductTypeDTO> response = new ApiResponse<ProductTypeDTO>();                                
                string bvin = FirstParameter(parameters);
                ProductType item = BVApp.CatalogServices.ProductTypes.Find(bvin);
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
            
            //
            // <site Url>/producttypes/<guid>/properties/<propertyid>/<sortOrder>
            //
            string isProperty = GetParameterByIndex(1, parameters);
            if (isProperty.Trim().ToLowerInvariant() == "properties")
            {
                ApiResponse<bool> response2 = new ApiResponse<bool>();

                string propertyIds = GetParameterByIndex(2, parameters);
                long propertyId = 0;
                long.TryParse(propertyIds, out propertyId);                              

                response2.Content = BVApp.CatalogServices.ProductTypeAddProperty(bvin, propertyId);
                data = MerchantTribe.Web.Json.ObjectToJson(response2);            
            }
            else
            {
                ApiResponse<ProductTypeDTO> response = new ApiResponse<ProductTypeDTO>();
                ProductTypeDTO postedItem = null;
                try
                {
                    postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductTypeDTO>(postdata);
                }
                catch (Exception ex)
                {
                    response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                    return MerchantTribe.Web.Json.ObjectToJson(response);
                }

                ProductType item = new ProductType();
                item.FromDto(postedItem);

                if (bvin == string.Empty)
                {
                    if (BVApp.CatalogServices.ProductTypes.Create(item))
                    {
                        bvin = item.Bvin;
                    }
                }
                else
                {
                    BVApp.CatalogServices.ProductTypes.Update(item);
                }
                ProductType resultItem = BVApp.CatalogServices.ProductTypes.Find(bvin);
                if (resultItem != null) response.Content = resultItem.ToDto();
                data = MerchantTribe.Web.Json.ObjectToJson(response);            
            }


            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // 
            // <site url>/producttypes/<guid>/properties/<propertyid>
            //
            string isProperty = GetParameterByIndex(1, parameters);
            if (isProperty.Trim().ToLowerInvariant() == "properties")
            {
                // Properties Delete
                ApiResponse<bool> response2 = new ApiResponse<bool>();

                string propertyIds = GetParameterByIndex(2, parameters);
                long propertyId = 0;
                long.TryParse(propertyIds, out propertyId);

                response2.Content = BVApp.CatalogServices.ProductTypeRemoveProperty(bvin, propertyId);
                data = MerchantTribe.Web.Json.ObjectToJson(response2);
            }
            else
            {
                // Single Item Delete
                response.Content = BVApp.CatalogServices.ProductTypeDestroy(bvin);
            }

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}