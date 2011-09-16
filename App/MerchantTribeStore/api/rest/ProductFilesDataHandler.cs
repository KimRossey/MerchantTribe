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
    public class ProductFilesDataHandler: BaseRestHandler
    {
        public ProductFilesDataHandler(BVSoftware.Commerce.BVApplication app)
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
            string bvin = FirstParameter(parameters);
            string fileName = querystring["filename"];
            string description = querystring["description"];
            string firstPart = querystring["first"];

            ApiResponse<bool> response = new ApiResponse<bool>();

            byte[] postedData = null;
            try
            {
                if (firstPart == "1")
                {
                    ProductFile existing = BVApp.CatalogServices.ProductFiles.Find(bvin);
                    if (existing == null || existing.Bvin == string.Empty)
                    {
                        existing.Bvin = bvin;
                        existing.FileName = fileName;
                        existing.ShortDescription = description;
                        existing.StoreId = BVApp.CurrentStore.Id;
                        BVApp.CatalogServices.ProductFiles.Create(existing);
                    }
                }

                postedData = MerchantTribe.Web.Json.ObjectFromJson<byte[]>(postdata);

                string diskFileName = bvin + "_" + fileName + ".config";
                if (data != null)
                {
                    if (data.Length > 0)
                    {
                        if (firstPart == "1")
                        {
                            response.Content = BVSoftware.Commerce.Storage.DiskStorage.FileVaultUploadPartial(BVApp.CurrentStore.Id, diskFileName, postedData, true);
                        }
                        else
                        {
                            response.Content = BVSoftware.Commerce.Storage.DiskStorage.FileVaultUploadPartial(BVApp.CurrentStore.Id, diskFileName, postedData, false);
                        }
                    }
                }
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