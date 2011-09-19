using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribe.CommerceDTO.v1;
using MerchantTribe.CommerceDTO.v1.Taxes;
using MerchantTribe.Commerce.Taxes;

namespace BVCommerce.api.rest
{
    public class TaxSchedulesHandler: BaseRestHandler
    {
        public TaxSchedulesHandler(MerchantTribe.Commerce.BVApplication app)
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
                ApiResponse<List<TaxScheduleDTO>> response = new ApiResponse<List<TaxScheduleDTO>>();

                List<TaxSchedule> results = BVApp.OrderServices.TaxSchedules.FindAll(BVApp.CurrentStore.Id);
                List<TaxScheduleDTO> dto = new List<TaxScheduleDTO>();

                foreach (TaxSchedule item in results)
                {
                    dto.Add(item.ToDto());
                }
                response.Content = dto;
                data = MerchantTribe.Web.Json.ObjectToJson(response);
            }
            else
            {
                // Find One Specific Category
                ApiResponse<TaxScheduleDTO> response = new ApiResponse<TaxScheduleDTO>();
                string ids = FirstParameter(parameters);
                long id = 0;
                long.TryParse(ids, out id);
                TaxSchedule item = BVApp.OrderServices.TaxSchedules.Find(id);
                if (item == null)
                {
                    response.Errors.Add(new ApiError("NULL", "Could not locate that item. Check id and try again."));
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
            ApiResponse<TaxScheduleDTO> response = new ApiResponse<TaxScheduleDTO>();

            TaxScheduleDTO postedCategory = null;
            try
            {
                postedCategory = MerchantTribe.Web.Json.ObjectFromJson<TaxScheduleDTO>(postdata);
            }
            catch(Exception ex)
            {
                response.Errors.Add(new ApiError("EXCEPTION", ex.Message));
                return MerchantTribe.Web.Json.ObjectToJson(response);                
            }

            TaxSchedule item = new TaxSchedule();
            item.FromDto(postedCategory);

            if (id < 1)
            {
                TaxSchedule existing = BVApp.OrderServices.TaxSchedules.FindByName(item.Name);
                if (existing == null)
                {
                    // Create
                    BVApp.OrderServices.TaxSchedules.Create(item);
                }
                else
                {
                    item.Id = existing.Id;
                }
            }
            else
            {
                BVApp.OrderServices.TaxSchedules.Update(item);
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
                response.Content = BVApp.OrderServices.TaxSchedules.Delete(id);
            

            data = MerchantTribe.Web.Json.ObjectToJson(response);
            return data;
        }
    }
}