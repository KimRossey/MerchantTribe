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
    public class ProductRelationshipsHandler : BaseRestHandler
    {
        public ProductRelationshipsHandler(BVSoftware.Commerce.BVApplication app)
            : base(app)
        {

        }

        // List or Find Single
        public override string GetAction(string parameters, System.Collections.Specialized.NameValueCollection querystring)
        {
            string data = string.Empty;


            // Find One Specific Category
            ApiResponse<ProductRelationshipDTO> response = new ApiResponse<ProductRelationshipDTO>();
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);

            var item = BVApp.CatalogServices.ProductRelationships.Find(id);
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
            string ids = FirstParameter(parameters);
            long id = 0;
            long.TryParse(ids, out id);

            ApiResponse<ProductRelationshipDTO> response = new ApiResponse<ProductRelationshipDTO>();

            ProductRelationshipDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<ProductRelationshipDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            ProductRelationship item = new ProductRelationship();
            item.FromDto(postedItem);

            if (id < 1)
            {
                if (BVApp.CatalogServices.ProductRelationships.Create(item))
                {
                    id = item.Id;
                }
            }
            else
            {
                BVApp.CatalogServices.ProductRelationships.Update(item);
            }
            ProductRelationship resultItem = BVApp.CatalogServices.ProductRelationships.Find(id);
            if (resultItem != null) response.Content = resultItem.ToDto();

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            string otherBvin = GetParameterByIndex(1, parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

            // Single Item Delete
            response.Content = BVApp.CatalogServices.ProductRelationships.UnrelateProducts(bvin, otherBvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}