using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Templates.TagHandlers
{
    public class AnalyticsTop : ITagHandler
    {

        public string TagName
        {
            get { return "sys:analyticstop"; }
        }

        public string Process(MerchantTribeApplication app, Dictionary<string, ITagHandler> handlers, ParsedTag tag, string contents)
        {
            if (app.CurrentStore.Settings.Analytics.UseGoogleTracker)
            {
                if (app.CurrentStore.Settings.Analytics.UseGoogleEcommerce && app.CurrentRequestContext.CurrentReceiptOrder != null)
                {
                    // Ecommerce + Page Tracker
                    return MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTrackerAndTransaction(
                        app.CurrentStore.Settings.Analytics.GoogleTrackerId,
                        app.CurrentRequestContext.CurrentReceiptOrder,
                        app.CurrentStore.Settings.Analytics.GoogleEcommerceStoreName,
                        app.CurrentStore.Settings.Analytics.GoogleEcommerceCategory);
                }
                else
                {
                    // Page Tracker Only
                    return MerchantTribe.Commerce.Metrics.GoogleAnalytics.RenderLatestTracker(app.CurrentStore.Settings.Analytics.GoogleTrackerId);
                }
            }
            return string.Empty;
        }
    }
}
