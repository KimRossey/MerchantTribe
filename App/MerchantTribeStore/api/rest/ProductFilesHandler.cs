using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using BVSoftware.Commerce;
using BVSoftware.CommerceDTO.v1;
using BVSoftware.CommerceDTO.v1.Catalog;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce.api.rest
{
    public class ProductFilesHandler: BaseRestHandler
    {
        public ProductFilesHandler(BVSoftware.Commerce.BVApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            string data = string.Empty;

            
                // Find One Specific Category
                ApiResponse<ProductFileDTO> response = new ApiResponse<ProductFileDTO>();                                
                string bvin = FirstParameter(parameters);
                ProductFile item = BVApp.CatalogServices.ProductFiles.Find(bvin);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check bvin and try again."));
                }
                else
                {
                    response.Content = item.ToDto();
                }
                data = MerchantTribe.Web.Json.ObjectToJson(response);
                                               

            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<ProductFileDTO> response = new ApiResponse<ProductFileDTO>();

            ProductFileDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductFileDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            ProductFile item = new ProductFile();
            item.FromDto(postedItem);

            if (bvin == string.Empty)
            {
                if (BVApp.CatalogServices.ProductFiles.Create(item))
                {
                    bvin = item.Bvin;
                }
            }
            else
            {
                BVApp.CatalogServices.ProductFiles.Update(item);
            }
            ProductFile resultItem = BVApp.CatalogServices.ProductFiles.Find(bvin);                    
            if (resultItem != null) response.Content = resultItem.ToDto();
            
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = BVApp.CatalogServices.ProductFiles.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}