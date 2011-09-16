using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BVSoftware.Commerce;
using BVSoftware.Commerce.Contacts;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Content;

namespace BVCommerce
{

    partial class BVModules_Controls_AddressBook : BVSoftware.Commerce.Content.BVUserControl
    {
        public delegate void AddressSelectedDelegate(string addresstype, Address a);
        public event AddressSelectedDelegate AddressSelected;

        private int _tabIndex = -1;

        public int TabIndex
        {
            get { return _tabIndex; }
            set { _tabIndex = value; }
        }

        private ThemeManager themes = null;

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            themes = MyPage.BVApp.ThemeManager();

            if (!SessionManager.IsUserAuthenticated(MyPage.BVApp))
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                if (!Page.IsPostBack)
                {
                    this.AddressBookImageButton.ImageUrl = themes.ButtonUrl("AddressBook", Request.IsSecureConnection);
                    if (SessionManager.GetCurrentUserId() != string.Empty)
                    {
                        CustomerAccount user = MyPage.BVApp.CurrentCustomer;
                        if (user.Bvin != string.Empty)
                        {
                            if (user.Addresses.Count > 0)
                            {
                                AddressGridView.DataSource = user.Addresses;
                                AddressGridView.DataBind();
                            }
                            else
                            {
                                this.Visible = false;
                            }
                        }
                    }

                    if (this.TabIndex != -1)
                    {
                        AddressBookImageButton.TabIndex = (short)this.TabIndex;
                        int startIndex = this.TabIndex + 1;
                        foreach (GridViewRow row in AddressGridView.Rows)
                        {
                            ((ImageButton)row.FindControl("BillToAddressImageButton")).TabIndex = (short)startIndex;
                            ((ImageButton)row.FindControl("ShipToAddressImageButton")).TabIndex = (short)(startIndex + 1);
                            startIndex = startIndex + 2;
                        }
                    }
                }
            }
        }

        protected void AddressBookImageButton_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            AddressGridView.Visible = !AddressGridView.Visible;
        }


        protected void AddressGridView_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    Address address = (Address)e.Row.DataItem;
                    ImageButton button = (ImageButton)e.Row.FindControl("BillToAddressImageButton");
                    button.CommandArgument = address.Bvin;
                    button = (ImageButton)e.Row.FindControl("BillToAddressImageButton");
                    button.CommandArgument = address.Bvin;
                    button.ImageUrl = themes.ButtonUrl("billto", Request.IsSecureConnection);

                    ImageButton button2 = (ImageButton)e.Row.FindControl("ShipToAddressImageButton");
                    button2.CommandArgument = address.Bvin;
                    button2 = (ImageButton)e.Row.FindControl("ShipToAddressImageButton");
                    button2.CommandArgument = address.Bvin;
                    button2.ImageUrl = themes.ButtonUrl("shipto", Request.IsSecureConnection);

                    HtmlGenericControl line = (HtmlGenericControl)e.Row.FindControl("lineone");
                    line.InnerText = address.Line1;

                    line = (HtmlGenericControl)e.Row.FindControl("linename");
                    line.InnerText = address.FirstName + " " + address.MiddleInitial + " " + address.LastName;
                    line.Visible = true;

                    line = (HtmlGenericControl)e.Row.FindControl("linetwo");
                    if (address.Line2.Trim() != string.Empty)
                    {
                        line.InnerText = address.Line2;
                        line.Visible = true;
                    }
                    else
                    {
                        line.Visible = false;
                    }

                    line = (HtmlGenericControl)e.Row.FindControl("linethree");
                    if (address.Line3.Trim() != string.Empty)
                    {
                        line.InnerText = address.Line3;
                        line.Visible = true;
                    }
                    else
                    {
                        line.Visible = false;
                    }

                    line = (HtmlGenericControl)e.Row.FindControl("linefour");
                    line.InnerText = address.City + ", " + address.RegionName + " " + address.PostalCode;
                }
            }
        }

        protected void AddressGridView_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            CustomerAccount user = MyPage.BVApp.MembershipServices.Customers.Find(SessionManager.GetCurrentUserId());

            if (user != null && user.Bvin != string.Empty)
            {
                if (e.CommandName == "BillTo")
                {
                    foreach (Address address in user.Addresses)
                    {
                        if (address.Bvin == (string)e.CommandArgument)
                        {
                            if (AddressSelected != null)
                            {
                                AddressSelected("Billing", address);
                            }
                            break;
                        }
                    }
                    AddressGridView.Visible = !AddressGridView.Visible;
                }
                else if (e.CommandName == "ShipTo")
                {
                    foreach (Address address in user.Addresses)
                    {
                        if (address.Bvin == (string)e.CommandArgument)
                        {
                            if (AddressSelected != null)
                            {
                                AddressSelected("Shipping", address);
                            }
                            break;
                        }
                    }
                    AddressGridView.Visible = !AddressGridView.Visible;
                }
            }
        }

    }
}