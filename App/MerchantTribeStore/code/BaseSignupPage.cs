using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BVSoftware.Commerce.Accounts;
using BVSoftware.Commerce;

namespace BVCommerce
{
    /// <summary>
    /// Summary description for BaseSignupPage
    /// </summary>
    /// 
    public class BaseSignupPage : System.Web.UI.Page
    {
        protected ViewDataDictionary ViewData { get; set; }
        public BVApplication BVApp { get; set; }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            BVApp = BVApplication.InstantiateForDataBase(new RequestContext());
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewData = new ViewDataDictionary();
        }

    }
}