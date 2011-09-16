using BVSoftware.Commerce;

namespace BVCommerce
{

    partial class BVModules_Controls_EmailAddressEntry : BVSoftware.Commerce.Content.BVUserControl
    {

        private int _tabIndex = -1;
        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (SessionManager.IsUserAuthenticated(MyPage.BVApp))
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                EmailAddressRequiredFieldValidator.Enabled = true;
                if (this.TabIndex != -1)
                {
                    EmailTextBox.TabIndex = (short)this.TabIndex;
                }
            }
        }

        public string GetUserEmail()
        {
            return this.EmailTextBox.Text;
        }
    }
}