using System;
using System.Linq;
using System.Web;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce.Controls;
using BVSoftware.Commerce.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MerchantTribe.Web;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce
{
    /// <summary>
    /// Summary description for BaseSuperPage
    /// </summary>
    public class BaseSuperPage : System.Web.UI.Page, IBaseAdminPage, IMultiStorePage
    {
        private Guid? _AuthTokenGuid = null;
        private RequestContext _CurrentRequestContext = new RequestContext();

        public Guid? AuthTokenGuid
        {
            get { return _AuthTokenGuid; }
            set { _AuthTokenGuid = value; }
        }
        public UserAccount CurrentUser
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

        public BVApplication BVApp { get; set; }
        public bool RequiresSSL
        {
            get { return true; }
        }
        public string PageTitle
        {
            get { return this.Title; }
            set { this.Title = value; }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());

            // Determine store id        
            BVApp.CurrentStore = UrlHelper.ParseStoreFromUrl(System.Web.HttpContext.Current.Request.Url, BVApp.AccountServices);
            if (BVApp.CurrentStore == null)
            {
                Response.Redirect("~/storenotfound");
            }

            ValidateSuperLogin();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Request.IsSecureConnection)
            {
                SSL.SSLRedirect(this, this.BVApp.CurrentStore, SSL.SSLRedirectTo.SSL);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WebForms.MakePageNonCacheable(this);
        }

        public void ValidateSuperLogin()
        {
            bool validLogin = false;

            Guid? tokenId = Cookies.GetCookieGuid(WebAppSettings.CookieNameAuthenticationTokenAdmin(),
                    this.Page.Request.RequestContext.HttpContext,
                    new EventLog());

            if (tokenId.HasValue)
            {
                if (BVApp.AccountServices.IsTokenValidForSuperUser(tokenId.Value))
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

    }

}