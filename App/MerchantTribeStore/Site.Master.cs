

namespace MerchantTribeStore
{
    partial class Site : System.Web.UI.MasterPage, IStorePage
    {
        private System.Web.Mvc.ViewDataDictionary _ViewData = new System.Web.Mvc.ViewDataDictionary();

        public virtual System.Web.Mvc.ViewDataDictionary ViewData
        {
            get { return _ViewData; }
            set { _ViewData = value; }
        }        

        protected override void OnLoad(System.EventArgs e)        
        {
            base.OnLoad(e);

            this.litAdditionalMetaTags.Text = (string)ViewData["AdditionalMetaTags"];
            this.litCss.Text = "<link href=\"" + (string)ViewData["css"] + "\" rel=\"stylesheet\" type=\"text/css\" />";
            this.litCss.Text += "<link href=\"" + Page.ResolveUrl("~/css/system/styles.css") + "\" rel=\"stylesheet\" type=\"text/css\" />";
            this.litHeader.Text = (string)ViewData["header"];
            this.litFooter.Text = (string)ViewData["footer"];
            this.litMessage.Text = (string)ViewData["message"];

            this.litAnaltyicsTop.Text = (string)ViewData["analyticstop"];
            this.litAnalyticsBottom.Text = (string)ViewData["analyticsbottom"];
        }

        public void AddBodyClass(string css)
        {
            if (!this.IsPostBack)
            {
                this.pagebody.Attributes.Add("class", css);
            }
        }
    }
}