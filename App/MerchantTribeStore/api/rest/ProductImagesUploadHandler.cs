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
    public class ProductImagesUploadHandler: BaseRestHandler
    {
        public ProductImagesUploadHandler(BVSoftware.Commerce.BVApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {            
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Errors.Add(new ApiError("NOTSUPPORTED", "GET method is not supported for this object."));
            response.Content = false;            
            string data = string.Empty;
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        // Create or Update
        public override string PostAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string productBvin = FirstParameter(parameters);
            string imageBvin = GetParameterByIndex(1, parameters);
            string fileName = querystring["filename"];
            ApiResponse<bool> response = new ApiResponse<bool>();

            byte[] postedData = null;
            try
            {
                postedData = MerchantTribe.Web.Json.ObjectFromJson<byte[]>(postdata);
                response.Content = BVSoftware.Commerce.Storage.DiskStorage.UploadProductAdditionalImage(BVApp.CurrentStore.Id, productBvin, imageBvin, fileName, postedData);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }
                        
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();
            response.Errors.Add(new ApiError("NOTSUPPORTED", "Delete method is not supported for this object."));
            response.Content = false;            
            string data = string.Empty;
            data = MerchantTribe.Web.Json.ObjectToJson(response);            
            return data;
        }
    }
}