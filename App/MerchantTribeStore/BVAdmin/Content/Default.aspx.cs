using BVSoftware.Commerce.Membership;

namespace BVCommerce
{

    partial class BVAdmin_Content_Default : BaseAdminPage
    {
        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Content";
            this.CurrentTab = AdminTabType.Content;
            ValidateCurrentUserHasPermission(SystemPermissions.ContentView);
        }

    }
}