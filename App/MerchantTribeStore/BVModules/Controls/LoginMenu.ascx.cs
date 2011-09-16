using System.Text;
using System.Web.UI;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Membership;

namespace BVCommerce
{

    partial class BVModules_Controls_LoginMenu : BVSoftware.Commerce.Content.BVUserControl
    {

        protected bool _ShowUserName = false;

        public bool ShowUserName
        {
            get { return _ShowUserName; }
            set { _ShowUserName = value; }
        }

        private int _tabIndex = -1;
        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            StringBuilder s = new StringBuilder();

            if (SessionManager.IsUserAuthenticated(MyPage.BVApp) == true)
            {
                if (this.TabIndex != -1)
                {
                    s.Append("<a href=\"" + Page.ResolveUrl("~/logout.aspx") + "\" TabIndex=\"" + this.TabIndex.ToString() + "\">");
                }
                else
                {
                    s.Append("<a href=\"" + Page.ResolveUrl("~/logout.aspx") + "\">");
                }

                if (_ShowUserName == true)
                {
                    CustomerAccount u = MyPage.BVApp.CurrentCustomer;
                    s.Append("Sign Out (" + u.FirstName + " " + u.LastName + ")");
                }
                else
                {
                    s.Append("Sign Out");
                }
                s.Append("</a>");
                this.litLogin.Text = s.ToString();
            }
            else
            {
                string destination = Page.ResolveUrl("~/Login.aspx");
                if (this.TabIndex != -1)
                {
                    s.Append("<a href=\"" + destination + "\" TabIndex=\"" + this.TabIndex.ToString() + "\">");
                }
                else
                {
                    s.Append("<a href=\"" + destination + "\">");
                }
                s.Append("Sign In");
                s.Append("</a>");
                this.litLogin.Text = s.ToString();
            }
        }
    }
}