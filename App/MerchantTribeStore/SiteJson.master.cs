
namespace MerchantTribeStore
{

    partial class SiteJson : System.Web.UI.MasterPage, IStorePage
    {

        private System.Web.Mvc.ViewDataDictionary _ViewData = new System.Web.Mvc.ViewDataDictionary();

        public virtual System.Web.Mvc.ViewDataDictionary ViewData
        {
            get { return _ViewData; }
            set { _ViewData = value; }
        }

        public void AddBodyClass(string css)
        {
            // do nothing
        }
    }

}