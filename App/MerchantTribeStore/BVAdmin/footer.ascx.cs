using System;

namespace BVCommerce
{

    partial class BVAdmin_footer : System.Web.UI.UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                LoadCopyright();
            }
        }

        protected void LoadCopyright()
        {
            this.output.Text = Helpers.Html.AdminFooter();
        }

    }
}