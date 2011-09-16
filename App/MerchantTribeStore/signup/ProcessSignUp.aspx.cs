using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using BVSoftware.Commerce;

namespace BVCommerce
{

    public partial class signup_ProcessSignUp : BaseSignupPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        private void LoadData()
        {
            string email = Request.QueryString["e"];
            string storename = Request.QueryString["s"];

            // Bail out if we don't have an email
            if (String.IsNullOrEmpty(email)) Response.Redirect("/signup");
            if (String.IsNullOrEmpty(storename)) Response.Redirect("/signup");

            email = BVSoftware.Cryptography.Base64.ConvertStringFromBase64(email);
            storename = BVSoftware.Cryptography.Base64.ConvertStringFromBase64(storename);

            // Encode store name for safety from injections
            storename = System.Web.HttpUtility.HtmlEncode(storename);

            string baseUrl = WebAppSettings.ApplicationBaseUrl;
            string rootUrl = baseUrl.Replace("www", storename);
            string rootUrlSecure = rootUrl.Replace("http://", "https://");

            this.completeemail.Text = email;
            this.completestorelink.Text = "<a href=\"" + rootUrl + "\">" + System.Web.HttpUtility.HtmlEncode(rootUrl) + "</a>";
            this.completestorelinkadmin.Text = "<a href=\"" + rootUrlSecure + "bvadmin\">" + System.Web.HttpUtility.HtmlEncode(rootUrlSecure) + "bvadmin</a>";
            this.completebiglogin.Text = "<a href=\""
                                            + rootUrlSecure + "account/login?wizard=1&username="
                                            + System.Web.HttpUtility.UrlEncode(email) + "\">Next Step &raquo; Choose a Theme</a>";
        }
    }
}