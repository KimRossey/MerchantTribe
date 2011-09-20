using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MerchantTribe.Commerce.Catalog;
using MerchantTribe.Commerce.Content;
using MerchantTribe.Commerce.Utilities;
using MerchantTribeStore.Controllers.Shared;
using MerchantTribeStore.Models;
using MerchantTribe.Web.Rss;
using MerchantTribeStore.Areas.ContentBlocks.Models;

namespace MerchantTribeStore.Areas.ContentBlocks.Controllers
{
    public class RssFeedViewerController : BaseAppController
    {
        //
        // GET: /ContentBlocks/RssFeedViewer/
        public ActionResult Index(ContentBlock block)
        {
            RssFeedViewModel model = new RssFeedViewModel();
            model.Channel = new RSSChannel(new MerchantTribe.Commerce.EventLog());

            if (block != null)
            {
                string feedUrl = block.BaseSettings.GetSettingOrEmpty("FeedUrl");                
                model.Channel.LoadFromFeed(feedUrl);
                model.ShowTitle = block.BaseSettings.GetBoolSetting("ShowTitle");
                model.ShowDescription = block.BaseSettings.GetBoolSetting("ShowDescription");                                                
                int max = block.BaseSettings.GetIntegerSetting("MaxItems");
                if (max <= 0)
                {
                    max = 5;
                }
                model.MaxItems = max;
            }

            return View(model);
        }

    }
}
