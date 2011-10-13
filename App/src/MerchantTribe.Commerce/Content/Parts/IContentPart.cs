using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Parts
{
    public interface IContentPart
    {

        string Id { get; set; }

        string RenderForDisplay(RequestContext context, Catalog.Category containerCategory);

        string RenderForEdit(RequestContext context, Catalog.Category containerCategory);

        ColumnSize MinimumSize();

        PartJsonResult ProcessJsonRequest(System.Collections.Specialized.NameValueCollection form,
                                          MerchantTribe.Commerce.MerchantTribeApplication app,
                                          Catalog.Category currentCategory);

        void SerializeToXml(ref System.Xml.XmlWriter xw);

        void DeserializeFromXml(string xml);

        void SetContainer(IColumn container);

    }
}
