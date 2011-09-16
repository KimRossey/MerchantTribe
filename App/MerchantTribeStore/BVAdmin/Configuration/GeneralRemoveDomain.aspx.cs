using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Accounts;

namespace BVCommerce.BVAdmin.Configuration
{
    public partial class GeneralRemoveDomain : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string urlId = Request.Form["id"];
                Delete(urlId);
            }
        }

        private void Delete(string id)
        {
            bool result = false;

            StoreDomainRepository repo = new StoreDomainRepository(BVApp.CurrentRequestContext);

            long lid = 0;
            long.TryParse(id, out lid);
            result = repo.Delete(lid);

            if ((result))
            {
                this.litOutput.Text = "{\"result\":true}";
            }
            else
            {
                this.litOutput.Text = "{\"result\":false}";
            }
        }
    }
}