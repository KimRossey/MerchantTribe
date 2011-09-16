using System.Web.Mvc;

namespace BVCommerce
{
    public interface IStorePage
    {
        ViewDataDictionary ViewData { get; set; }
        void AddBodyClass(string css);
    }
}