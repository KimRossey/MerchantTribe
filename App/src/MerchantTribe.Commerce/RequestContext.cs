using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce
{
    public class RequestContext
    {
        
        public Accounts.Store CurrentStore { get; set; }
        public Accounts.UserAccount CurrentAdministrator(MerchantTribeApplication app) 
        {
                if (!IsAdmin(app)) return null;
                Accounts.AccountService accountServices = Accounts.AccountService.InstantiateForDatabase(this);
                Accounts.UserAccount admin = accountServices.FindAdminUserByAuthTokenId(_adminAuthTokenId.Value);
                
                if (admin == null) return null;
                if (admin.Id < 1) return null;

                return admin;        
        }
       
        public string ConnectionString { get; set; }
        public string ConnectionStringForEntityFramework { get; set; }
        public Integration IntegrationEvents { get; set; }
        public string FlexPageId { get; set; }
        public System.Web.Routing.RequestContext RoutingContext { get; set; }
        public System.Web.Mvc.UrlHelper UrlHelper { get; set; }

        private bool? _adminResult;
        private Guid? _adminAuthTokenId;

        public bool IsAdmin(MerchantTribeApplication app)
        {
            // don't check more than once per request
            if (_adminResult.HasValue) return _adminResult.Value;
    
            try
            {
                if (System.Web.HttpContext.Current == null) return false;
                if (System.Web.HttpContext.Current.Request == null) return false;
                if (System.Web.HttpContext.Current.Request.RequestContext == null) return false;
                if (System.Web.HttpContext.Current.Request.RequestContext.HttpContext == null) return false;

                                  
                    Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(app.CurrentStore.Id),
                   System.Web.HttpContext.Current.Request.RequestContext.HttpContext, new EventLog());

                    // no token, return
                    if (!tokenId.HasValue) return false;


                    Accounts.AccountService accountServices = Accounts.AccountService.InstantiateForDatabase(this);

                    if (accountServices.IsTokenValidForStore(CurrentStore.Id, tokenId.Value))
                    {
                        _adminResult = true;
                        _adminAuthTokenId = tokenId.Value;
                        return true;
                    }
                
            }
            catch
            {
                return false;
            }

            return false;                                      
        }


        public RequestContext()
        {
            CurrentStore = new Accounts.Store();            
            ConnectionString = WebAppSettings.ApplicationConnectionString;
            ConnectionStringForEntityFramework = WebAppSettings.ApplicationConnectionStringForEntityFramework;
            IntegrationEvents = new Integration();
            RoutingContext = null;
        }

        
        public static RequestContext GetCurrentRequestContext()
        {
            RequestContext alternateContext = new RequestContext();
            alternateContext.CurrentStore.Id = -1;

            try
            {
                // Try to pull from HttpContext.Items first in case
                // we already created this in an MVC controller
                if (System.Web.HttpContext.Current.Items != null)
                {
                    object maybe = System.Web.HttpContext.Current.Items["mtapp"];
                    if (maybe != null)
                    {
                        return ((MerchantTribeApplication)maybe).CurrentRequestContext;
                    }
                }
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Handler != null)
                    {
                        if (System.Web.HttpContext.Current.Handler is IMultiStorePage)
                        {
                            return ((IMultiStorePage)System.Web.HttpContext.Current.Handler).MTApp.CurrentRequestContext;
                        }
                    }
                }
                else if (System.ServiceModel.Web.WebOperationContext.Current != null)
                {
                    // WCF call instead of web page
                    if (System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest != null)
                    {
                        if (System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.Headers != null)
                        {
                            Accounts.AccountService accountServices = Accounts.AccountService.InstantiateForDatabase(alternateContext);

                            string requestHost = System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.Headers["Host"];
                            long wcfStoreId = Utilities.UrlHelper.ParseStoreId(new Uri("http://" + requestHost), accountServices);
                            alternateContext.CurrentStore.Id = wcfStoreId;
                        }                        
                    }
                }
            }
            catch
            {

            }
            
            return alternateContext;
        }

        public static bool ForceCurrentRequestContext(RequestContext contextToSet)
        {
            try
            {
                if (System.Web.HttpContext.Current != null)
                {
                    if (System.Web.HttpContext.Current.Handler != null)
                    {
                        if (System.Web.HttpContext.Current.Handler is IMultiStorePage)
                        {
                            ((IMultiStorePage)System.Web.HttpContext.Current.Handler).MTApp.CurrentRequestContext = contextToSet;
                        }
                    }
                }
            }
            catch
            {

            }

            return false;
        }
    }
}
