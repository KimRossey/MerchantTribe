using System;
using MerchantTribe.Commerce;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MerchantTribe.Commerce.Accounts;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Contacts;

namespace BVCommerce
{
    public class BaseAdminPage : System.Web.UI.Page, MerchantTribe.Commerce.Controls.IBaseAdminPage, IMultiStorePage
    {
        protected const string DefaultCatalogPage = "~/BVAdmin/Catalog/Default.aspx";

        private Guid? _AuthTokenGuid = null;

        //private MerchantTribe.Commerce.RequestContext _CurrentRequestContext = new MerchantTribe.Commerce.RequestContext();
        private IMessageBox _messageBox = null;

        public BVApplication BVApp { get; set; }
     
        public IMessageBox PageMessageBox
        {
            get { return _messageBox; }
            set { _messageBox = value; }
        }
        public Guid? AuthTokenGuid
        {
            get { return _AuthTokenGuid; }
            set { _AuthTokenGuid = value; }
        }
        public MerchantTribe.Commerce.Accounts.UserAccount CurrentUser
        {
            get
            {
                if (AuthTokenGuid.HasValue)
                {
                    return BVApp.AccountServices.FindAdminUserByAuthTokenId(AuthTokenGuid.Value);
                }
                return null;
            }
        }
        public virtual bool RequiresSSL
        {
            get { return false; }
        }
        public string PageTitle
        {
            get { return this.Title; }
            set { this.Title = value; }
        }
        public AdminTabType CurrentTab
        {
            get
            {
                if (Session["ActiveAdminTab"] == null)
                {
                    return AdminTabType.Dashboard;
                }
                else
                {
                    return (AdminTabType)Session["ActiveAdminTab"];
                }
            }
            set { Session["ActiveAdminTab"] = value; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Store routing context for URL Rewriting
            BVApp.CurrentRequestContext.RoutingContext = this.Request.RequestContext;

            // Determine store id        
            BVApp.CurrentStore = MerchantTribe.Commerce.Utilities.UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            if (BVApp.CurrentStore.Status == MerchantTribe.Commerce.Accounts.StoreStatus.Deactivated)
            {
                Response.Redirect("~/storenotavailable");
            }

            // Culture Settings
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(BVApp.CurrentStore.Settings.CultureCode);

            IntegrationLoader.AddIntegrations(this.BVApp.CurrentRequestContext.IntegrationEvents, this.BVApp.CurrentStore);

			ValidateAdminLogin();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // don't redirect during design time
            if (DesignMode) { return; }
            
            if (RequiresSSL)
            {
                if (WebAppSettings.UseSsl)
                {
                    if (!Request.IsSecureConnection)
                    {
                        MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this, this.BVApp.CurrentStore, MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.SSL);
                    }
                }
            }
            else
            {
                if (WebAppSettings.UseSsl)
                {
                    //if (Request.IsSecureConnection)
                    //{
                    //    MerchantTribe.Commerce.Utilities.SSL.SSLRedirect(this, this.BVApp.CurrentStore, MerchantTribe.Commerce.Utilities.SSL.SSLRedirectTo.NonSSL);
                    //}
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!DesignMode)
            {
                MerchantTribe.Commerce.Utilities.WebForms.MakePageNonCacheable(this);
            }
        }

        public void ValidateAdminLogin()
        {
            bool validLogin = false;

            Guid? tokenId = MerchantTribe.Web.Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                    this.Page.Request.RequestContext.HttpContext,
                    new EventLog());

            if (tokenId.HasValue)
            {
                if (this.BVApp.AccountServices.IsTokenValidForStore(BVApp.CurrentStore.Id, tokenId.Value))
                {
                    validLogin = true;
                }
            }

            if (validLogin == false)
            {
                Response.Redirect("~/account/login");
            }

            _AuthTokenGuid = tokenId;

        }

        public void ValidateCurrentUserHasPermission(string p)
        {
            Collection<string> l = new Collection<string>();
            l.Add(p);
            //ValidateCurrentUserHasPermissions(l)
        }

        public void ShowMessage(string message, ErrorTypes type)
        {
            switch (type)
            {
                case ErrorTypes.Ok:
                    this.PageMessageBox.ShowOk(message);
                    break;
                case ErrorTypes.Info:
                    this.PageMessageBox.ShowInformation(message);
                    break;
                case ErrorTypes.Error:
                    this.PageMessageBox.ShowError(message);
                    break;
                case ErrorTypes.Warning:
                    this.PageMessageBox.ShowWarning(message);
                    break;
            }
        }

    }

}