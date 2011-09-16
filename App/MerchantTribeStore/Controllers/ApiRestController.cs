using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using BVSoftware.CommerceDTO.v1;
using System.Dynamic;
using System.Xml.Serialization;
using System.IO;
using BVSoftware.Commerce.Catalog;

using BVCommerce.api.rest;

namespace BVCommerce.Controllers
{    
    public class ApiRestController : Controller, IMultiStorePage
    {

        // Initialize Store Specific Request Data
        BVSoftware.Commerce.RequestContext _BVRequestContext = new RequestContext();
        public BVApplication BVApp { get; set; }               
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            BVApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            BVApp.CurrentStore = BVSoftware.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == BVSoftware.Commerce.Accounts.StoreStatus.Deactivated)
            {
                Response.Redirect("~/storenotavailable");
            }

            HttpContext.Items.Add("bvapp", BVApp);

            // Jquery
            //ViewData["JQueryInclude"] = Helpers.Html.JQueryIncludes(Url.Content("~/scripts"), this.Request.IsSecureConnection);
        }

        // Authenticate API Key Methods
        public bool AuthenticateKey(string apikey, IApiResponse response)
        {

            if (apikey.Trim().Length < 1)
            {
                response.Errors.Add(new ApiError("KEY", "Api Key is invalid or missing."));
                return false;
            }

            ApiKey key = BVApp.AccountServices.ApiKeys.FindByKey(apikey);
            if (key == null)
            {
                response.Errors.Add(new ApiError("KEY", "Api Key is invalid or missing."));
                return false;
            }
            if (key.Key != apikey)
            {
                response.Errors.Add(new ApiError("KEY", "Api Key is invalid or missing."));
                return false;
            }
            if (key.StoreId < 0)
            {
                response.Errors.Add(new ApiError("KEY", "Api Key is invalid or missing."));
                return false;
            }

            long storeID = key.StoreId;
            BVApp.CurrentStore = BVApp.AccountServices.Stores.FindById(storeID);
            if (BVApp.CurrentStore == null)
            {
                response.Errors.Add(new ApiError("STORENOTFOUND", "No store was found at this URL."));
                return false;
            }
            if (BVApp.CurrentStore.Id < 0)
            {
                response.Errors.Add(new ApiError("STORENOTFOUND2", "No store was found at this URL."));
                return false;
            }
            if (BVApp.CurrentStore.Status == BVSoftware.Commerce.Accounts.StoreStatus.Deactivated)
            {
                response.Errors.Add(new ApiError("STOREDISABLED", "Store is not active. Contact an Administrator for Assistance"));
                return false;
            }

            return true;
        }

        // Actual Responder             
        public ActionResult Index(string version, string modelname, string parameters)
        {
            string data = string.Empty;

            // Key Api Key First
            string key = Request.QueryString["key"];
            ApiResponse<object> FailedKeyResponse = new ApiResponse<object>();
            if (!AuthenticateKey(key, FailedKeyResponse))
            {
                data = MerchantTribe.Web.Json.ObjectToJson(FailedKeyResponse);                
            }
            else
            {
                // Create Handler
                IRestHandler handler = RestHandlerFactory.Instantiate(version, modelname, BVApp);

                // Read Posted JSON
                string postedString = string.Empty;
                Stream inputStream = Request.InputStream;
                if (inputStream != null)
                {
                    StreamReader rdr = new StreamReader(inputStream);
                    postedString = rdr.ReadToEnd();
                }

                switch (Request.HttpMethod.ToUpperInvariant())
                {
                    case "GET":
                        data = handler.GetAction(parameters, Request.QueryString);
                        break;
                    case "POST":
                        data = handler.PostAction(parameters, Request.QueryString, postedString);
                        break;
                    case "PUT":
                        data = handler.PutAction(parameters, Request.QueryString, postedString);
                        break;
                    case "DELETE":
                        data = handler.DeleteAction(parameters, Request.QueryString, postedString);
                        break;
                }                
            }

            // Return Result (or text formatted result)
            if (Request.QueryString["apiformat"] == "text")
            {
                return new RawResult(data, "text/html");
            }
            return new PreJsonResult(data);                        
        }



    }
}
