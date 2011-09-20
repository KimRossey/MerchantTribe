
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Web.UI;
using System.Xml;
using MerchantTribe.Commerce.Content;

namespace MerchantTribeStore
{

    partial class BVModules_ContentBlocks_RSS_Feed_Viewer_view : BVModule
    {

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {

                ContentBlock b = MyPage.MTApp.ContentServices.Columns.FindBlock(this.BlockId);
                if (b != null)
                {
                    string feedUrl = b.BaseSettings.GetSettingOrEmpty("FeedUrl");
                    RSSChannel c = new RSSChannel();
                    c.LoadFromFeed(feedUrl);
                    this.lblTitle.Text = c.Title;
                    this.pnlTitle.Visible = b.BaseSettings.GetBoolSetting("ShowTitle");
                    this.lblDescription.Text = c.Description;
                    this.pnlDescription.Visible = b.BaseSettings.GetBoolSetting("ShowDescription");

                    Collection<RSSItem> items = c.GetChannelItems();
                    Collection<RSSItem> finalItems = new Collection<RSSItem>();
                    int max = b.BaseSettings.GetIntegerSetting("MaxItems");
                    if (max <= 0)
                    {
                        max = 5;
                    }
                    for (int i = 0; i <= max - 1; i++)
                    {
                        if (items.Count > i)
                        {
                            finalItems.Add(items[i]);
                        }
                    }
                    this.DataList1.DataSource = finalItems;
                    this.DataList1.DataBind();
                }
            }
        }
        #region " RSS Classes "

        private class RSSChannel
        {

            private string _Description = string.Empty;
            private string _FeedUrl = string.Empty;
            private string _Link = string.Empty;
            private string _Title = string.Empty;

            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
            }
            public string FeedUrl
            {
                get { return _FeedUrl; }
                set { _FeedUrl = value; }
            }
            public string Link
            {
                get { return _Link; }
                set { _Link = value; }
            }
            public string Title
            {
                get { return _Title; }
                set { _Title = value; }
            }

            public void LoadFromFeed(string url)
            {
                _FeedUrl = url;
                LoadChannel();
            }

            private XmlNodeList GetXMLDoc(string node)
            {
                System.Xml.XmlNodeList tempNodeList = null;
                XmlDocument rssDoc = new XmlDocument();

                string cached = MerchantTribe.Commerce.Datalayer.CacheManager.GetStringFromCache("com.bvsoftware.rssfeedviewer.channel." + this.FeedUrl);

                if ((cached == null) | (cached == string.Empty))
                {
                    // Load From Web
                    WebRequest request = WebRequest.Create(this.FeedUrl);
                    WebResponse response = request.GetResponse();
                    Stream rssStream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(rssStream);
                    string Data = sr.ReadToEnd();
                    rssDoc.Load(new StringReader(Data));
                    MerchantTribe.Commerce.Datalayer.CacheManager.AddStringToCache("com.bvsoftware.rssfeedviewer.channel." + this.FeedUrl, Data, 30);
                }
                else
                {
                    // Load From Cache
                    rssDoc.Load(new StringReader(cached));
                }

                tempNodeList = rssDoc.SelectNodes(node);
                return tempNodeList;
            }

            private void LoadChannel()
            {
                try
                {
                    XmlNodeList rss = GetXMLDoc("rss/channel");
                    Title = rss[0].SelectSingleNode("title").InnerText;
                    Link = rss[0].SelectSingleNode("link").InnerText;
                    Description = rss[0].SelectSingleNode("description").InnerText;
                }
                catch (Exception ex)
                {
                    MerchantTribe.Commerce.EventLog.LogEvent(ex);
                }
            }

            public Collection<RSSItem> GetChannelItems()
            {
                Collection<RSSItem> result = new Collection<RSSItem>();

                try
                {
                    XmlNodeList rssItems = GetXMLDoc("rss/channel/item");
                    foreach (XmlNode item in rssItems)
                    {
                        RSSItem newItem = new RSSItem();

                        newItem.Title = item.SelectSingleNode("title").InnerText;
                        newItem.Link = item.SelectSingleNode("link").InnerText;
                        newItem.Description = item.SelectSingleNode("description").InnerText;

                        result.Add(newItem);
                    }
                }
                catch (Exception ex)
                {
                    MerchantTribe.Commerce.EventLog.LogEvent(ex);
                }

                return result;
            }

        }

        public class RSSItem
        {

            private string _Description = string.Empty;
            private string _Link = string.Empty;
            private string _Title = string.Empty;

            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
            }
            public string Link
            {
                get { return _Link; }
                set { _Link = value; }
            }
            public string Title
            {
                get { return _Title; }
                set { _Title = value; }
            }

        }
        #endregion

    }
}