using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{
    public abstract class BaseProductAdminPage : BaseAdminPage
    {

        protected NotifyClickControl _ProductNavigator = new NotifyClickControl();

        protected abstract bool Save();

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);
            // Added after conversion to wireup
            _ProductNavigator.Clicked += ProductNavigator_Clicked;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            PopulateMenuControl();
        }

        private void PopulateMenuControl()
        {
            System.Web.UI.Control c = Page.Master.FindControl("ProductNavigator");
            if (c != null)
            {
                this._ProductNavigator = (NotifyClickControl)c;
            }
        }

        protected void ProductNavigator_Clicked(object sender, MerchantTribe.Commerce.Content.NotifyClickControl.ClickedEventArgs e)
        {
            if (!this.Save())
            {
                e.ErrorOccurred = true;
            }
        }
    }
}