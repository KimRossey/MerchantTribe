using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Membership;
using BVSoftware.Commerce.Catalog;
using BVSoftware.Commerce.Marketing;
using BVSoftware.Commerce.Utilities;
using BVSoftware.Commerce;

namespace BVCommerce.BVAdmin.Marketing
{
    public partial class RewardsPoints : BaseAdminPage
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.PageTitle = BVApp.CurrentStore.Settings.RewardsPointsName;
            this.CurrentTab = AdminTabType.Marketing;
            ValidateCurrentUserHasPermission(SystemPermissions.MarketingView);
        }        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                LoadInfo();
            }
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            this.MessageBox1.ClearMessage();

            if (SaveInfo())
            {
                this.MessageBox1.ShowOk("Changed Saved");
            }

        }

        private void LoadInfo()
        {
            CustomerPointsManager manager = CustomerPointsManager.InstantiateForDatabase(BVApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent,
                                                                      BVApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit,
                                                                      BVApp.CurrentStore.Id);

            int pointsIssued = manager.TotalPointsIssuedForStore(BVApp.CurrentStore.Id);
            this.lblPointsIssued.Text = pointsIssued.ToString();
            this.lblPointsIssuedValue.Text = manager.DollarCreditForPoints(pointsIssued).ToString("c");

            int pointsReserverd = manager.TotalPointsReservedForStore(BVApp.CurrentStore.Id);
            this.lblPointsReserved.Text = pointsReserverd.ToString();
            this.lblPointsReservedValue.Text = manager.DollarCreditForPoints(pointsReserverd).ToString("c");

            this.RewardsNameField.Text = BVApp.CurrentStore.Settings.RewardsPointsName;
            this.chkPointForDollars.Checked = BVApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive;
            this.chkPointsForProducts.Checked = BVApp.CurrentStore.Settings.RewardsPointsOnProductsActive;
            this.PointsCreditField.Text = BVApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit.ToString();
            this.PointsPerDollarField.Text = BVApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent.ToString();
        }

        private bool SaveInfo()
        {
            BVApp.CurrentStore.Settings.RewardsPointsName = this.RewardsNameField.Text.Trim();
            BVApp.CurrentStore.Settings.RewardsPointsOnProductsActive = this.chkPointsForProducts.Checked;
            BVApp.CurrentStore.Settings.RewardsPointsOnPurchasesActive = this.chkPointForDollars.Checked;
            int pointPerDollar = 1;
            if (int.TryParse(this.PointsPerDollarField.Text, out pointPerDollar))
            {
                BVApp.CurrentStore.Settings.RewardsPointsIssuedPerDollarSpent = pointPerDollar;
            }
            else
            {
                this.MessageBox1.ShowWarning("Please enter a valid point about issued");
                return false;
            }
            int pointsPerCredit = 100;
            if (int.TryParse(this.PointsCreditField.Text, out pointsPerCredit))
            {
                BVApp.CurrentStore.Settings.RewardsPointsNeededPerDollarCredit = pointsPerCredit;
            }
            else
            {
                this.MessageBox1.ShowWarning("Please enter a valid point amount for redemption");
                return false;
            }

            return BVApp.UpdateCurrentStore();            
        }
    }
}