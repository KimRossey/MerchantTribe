
using System.Collections.Generic;
using MerchantTribe.Commerce;

namespace MerchantTribeStore
{

    partial class BVAdmin_People_MailingLists : BaseAdminPage
    {

        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = "Mailing Lists";
            this.CurrentTab = AdminTabType.People;
            ValidateCurrentUserHasPermission(MerchantTribe.Commerce.Membership.SystemPermissions.PeopleView);
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadMailingLists();
            }
        }

        private void LoadMailingLists()
        {
            List<MerchantTribe.Commerce.Contacts.MailingListSnapShot> m;
            m = MTApp.ContactServices.MailingLists.FindAll();
            this.GridView1.DataSource = m;
            this.GridView1.DataBind();
            if (m.Count == 1)
            {
                this.lblResults.Text = "1 list found";
            }
            else
            {
                this.lblResults.Text = m.Count + " lists found";
            }
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {
            long id = (long)GridView1.DataKeys[e.RowIndex].Value;
            MTApp.ContactServices.MailingLists.Delete(id);

            LoadMailingLists();
        }

        protected void btnNew_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Response.Redirect("MailingLists_edit.aspx");
        }

        protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            long id = (long)GridView1.DataKeys[e.NewEditIndex].Value;
            Response.Redirect("MailingLists_edit.aspx?id=" + id);
        }
    }
}