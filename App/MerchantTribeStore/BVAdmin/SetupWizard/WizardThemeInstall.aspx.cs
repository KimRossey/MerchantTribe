
namespace BVCommerce
{

    public partial class BVAdmin_SetupWizard_WizardThemeInstall : BaseAdminJsonPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            string themeId = Request.QueryString["id"];
            this.BVApp.ThemeManager().InstallTheme(themeId);
            BVApp.UpdateCurrentStore();
            Response.Redirect("WizardPayment.aspx");
        }
    }
}