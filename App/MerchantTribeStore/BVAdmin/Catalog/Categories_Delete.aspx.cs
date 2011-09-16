using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Catalog;

namespace BVCommerce
{

    public partial class BVAdmin_Catalog_Categories_Delete : BaseAdminJsonPage
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                string catId = Request.Form["id"];
                Delete(catId);
            }
        }

        private void Delete(string bvin)
        {
            bool result = false;

            result = BVApp.DestroyCategory(bvin);
            
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