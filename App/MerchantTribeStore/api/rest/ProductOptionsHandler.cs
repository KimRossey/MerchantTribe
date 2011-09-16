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
    public class ProductOptionsHandler: BaseRestHandler
    {
        public ProductOptionsHandler(BVSoftware.Commerce.BVApplication app)
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
                ApiResponse<List<OptionDTO>> response = new ApiResponse<List<OptionDTO>>();

                string productBvin = querystring["productbvin"] ?? string.Empty;
                List<Option> results;
                if (productBvin.Trim().Length > 0)
                {
                    results = BVApp.CatalogServices.ProductOptions.FindAllShared(1, 1000);
                }
                else
                {
                    results = BVApp.CatalogServices.ProductOptions.FindByProductId(productBvin);
                }                 
                List<OptionDTO> dto = new List<OptionDTO>();

                foreach (Option item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<OptionDTO> response = new ApiResponse<OptionDTO>();                                
                string bvin = FirstParameter(parameters);
                Option item = BVApp.CatalogServices.ProductOptions.Find(bvin);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check bvin and try again."));
                }
                else
                {
                    response.Content = ((Option)item).ToDto();
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

            string isProducts = GetParameterByIndex(1, parameters);
            string productBvin = GetParameterByIndex(2, parameters);

            if (isProducts.Trim().ToLower() == "generateonly")
            {
                // Generate Variants Only
                ApiResponse<bool> response = new ApiResponse<bool>();
                Product p = BVApp.CatalogServices.Products.Find(productBvin);
                if (p == null | p.Bvin == string.Empty)
                {                    
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                    return data;
                }
                BVApp.CatalogServices.VariantsGenerateAllPossible(p);
                response.Content = true;
                data = MerchantTribe.Web.Json.ObjectToJson(response);   
            }
            else if (isProducts.Trim().ToLower() == "products")
            {

                string generatevariants = querystring["generatevariants"];

                // Assign to Products
                ApiResponse<bool> response = new ApiResponse<bool>();
                Product p = BVApp.CatalogServices.Products.Find(productBvin);
                if (p == null || p.Bvin == string.Empty)
                {
                    data = MerchantTribe.Web.Json.ObjectToJson(response);
                    return data;
                }
                BVApp.CatalogServices.ProductsAddOption(p, bvin);                
                if (generatevariants.Trim() == "1")
                {
                    BVApp.CatalogServices.VariantsGenerateAllPossible(p);                    
                }
                response.Content = true;
                data = MerchantTribe.Web.Json.ObjectToJson(response);            
            }
            else
            {
                ApiResponse<OptionDTO> response = new ApiResponse<OptionDTO>();

                OptionDTO postedItem = null;
                try
                {
                    postedItem = MerchantTribe.Web.Json.ObjectFromJson<OptionDTO>(postdata);
                }
                catch (Exception ex)
                {
                    response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                    return MerchantTribe.Web.Json.ObjectToJson(response);
                }

                Option existing = BVApp.CatalogServices.ProductOptions.Find(postedItem.Bvin);

                if (existing == null || existing.Bvin == string.Empty)
                {
                    postedItem.StoreId = BVApp.CurrentStore.Id;

                    // Create
                    Option op = new Option();
                    if (op == null)
                    {
                        response.Errors.Add(new ApiError("NULL FACTORY", "Option Factory returned a null object"));
                        return MerchantTribe.Web.Json.ObjectToJson(response);
                    }

                    op.FromDto(postedItem);

                    bool createResult = BVApp.CatalogServices.ProductOptions.Create(op);

                    Option created = BVApp.CatalogServices.ProductOptions.Find(postedItem.Bvin);
                    if (postedItem.Items != null)
                    {
                        foreach (OptionItemDTO oi in postedItem.Items)
                        {
                            OptionItem i = new OptionItem();
                            oi.OptionBvin = created.Bvin;
                            i.FromDto(oi);                            
                            created.Items.Add(i);                            
                        }
                        BVApp.CatalogServices.ProductOptions.Update(created);
                    }
                    response.Content = ((Option)created).ToDto();
                }
                else
                {
                    response.Content = ((Option)existing).ToDto();
                }

                data = MerchantTribe.Web.Json.ObjectToJson(response);            
            }

            
            return data;
        }

        public override string DeleteAction(string parameters, System.Collections.Specialized.NameValueCollection querystring, string postdata)
        {
            string data = string.Empty;
            string bvin = FirstParameter(parameters);
            ApiResponse<bool> response = new ApiResponse<bool>();

                // Single Item Delete
                response.Content = BVApp.CatalogServices.ProductOptions.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}