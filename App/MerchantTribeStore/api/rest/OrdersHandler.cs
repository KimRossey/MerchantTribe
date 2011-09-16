using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using BVSoftware.Commerce;
using BVSoftware.CommerceDTO.v1;
using BVSoftware.CommerceDTO.v1.Orders;
using BVSoftware.Commerce.Orders;


namespace BVCommerce.api.rest
{
    public class OrdersHandler: BaseRestHandler
    {
        public OrdersHandler(BVSoftware.Commerce.BVApplication app)
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
                ApiResponse<List<OrderSnapshotDTO>> response = new ApiResponse<List<OrderSnapshotDTO>>();

                List<OrderSnapshot> results = BVApp.OrderServices.Orders.FindAll();
                List<OrderSnapshotDTO> dto = new List<OrderSnapshotDTO>();

                foreach (OrderSnapshot item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<OrderDTO> response = new ApiResponse<OrderDTO>();
                string bvin = FirstParameter(parameters);
                Order item = BVApp.OrderServices.Orders.FindForCurrentStore(bvin);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that order. Check bvin and try again."));
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
            //string bvin = FirstParameter(parameters);
            ApiResponse<OrderDTO> response = new ApiResponse<OrderDTO>();

            OrderDTO postedItem = null;
            try
            {
                postedItem = MerchantTribe.Web.Json.ObjectFromJson<OrderDTO>(postdata);
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);
            }

            Order item = new Order();            
            item.FromDTO(postedItem);

            Order existing = BVApp.OrderServices.Orders.FindForCurrentStore(item.bvin);
            if (existing == null || existing.bvin == string.Empty)
            {
                item.StoreId = BVApp.CurrentStore.Id;
                BVApp.OrderServices.Orders.Create(item);
            }            
            else
            {
                BVApp.OrderServices.Orders.Update(item);
            }
            Order resultItem = BVApp.OrderServices.Orders.FindForCurrentStore(item.bvin);
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
            response.Content = BVApp.OrderServices.Orders.Delete(bvin);

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}