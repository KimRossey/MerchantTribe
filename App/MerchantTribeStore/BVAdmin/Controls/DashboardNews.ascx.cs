using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BVSoftware.Commerce.Reporting;

namespace BVCommerce
{

    public partial class BVAdmin_Controls_DashboardNews : BVSoftware.Commerce.Content.BVUserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadNews();
        }

        private void LoadNews()
        {
            BVNewsMessageManager manager = BVNewsMessageManager.InstantiateForDatabase(MyPage.BVApp.CurrentRequestContext);
            List<BVNewsMessage> messages = manager.GetLatestNews(10);
            foreach (BVNewsMessage m in messages)
            {
                this.litNews.Text += "<div class=\"flash-message-minor\"><b>";
                this.litNews.Text += TimeZoneInfo.ConvertTimeFromUtc(m.TimeStampUtc, MyPage.BVApp.CurrentStore.Settings.TimeZone).ToShortDateString();
                this.litNews.Text += "</b><br />" + m.Message + "</div>";
            }
        }

    }
}