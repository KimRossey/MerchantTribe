using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using MerchantTribe.Web;
namespace BVSoftware.Commerce.Content.Parts
{
    public class Html: IContentPart
    {

        private IColumn _Container = null;

        public string RawHtml { get; set; }

        public string Id {get;set;}        

        public Html()
        {
            Id = System.Guid.NewGuid().ToString();
            RawHtml = "<p>" + MerchantTribe.Web.Text.PlaceholderText() + "</p>";
        }

        public void SetContainer(IColumn container)
        {
            this._Container = container;
        }

        public string RenderForDisplay(RequestContext context, Catalog.Category containerCategory)
        {
            return RawHtml;            
        }

        public string RenderForEdit(RequestContext context, Catalog.Category containerCategory)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id=\"part" + Id + "\" class=\"editable\">");
            sb.Append("<div class=\"edittools\"><a href=\"#\" class=\"deletepart\" id=\"dp" + this.Id + "\"><span>x</span></a></div>");
            sb.Append(RenderForDisplay(context, containerCategory));
            sb.Append("</div>");

            return sb.ToString();
        }

        public ColumnSize MinimumSize()
        {
            return ColumnSize.Size1;
        }


       public PartJsonResult ProcessJsonRequest(System.Collections.Specialized.NameValueCollection form,
                                          BVSoftware.Commerce.RequestContext context, Catalog.Category containerCategory)
        {
            PartJsonResult result = new PartJsonResult();

            Catalog.CatalogService CatalogServices = Catalog.CatalogService.InstantiateForDatabase(context);

            string action = form["partaction"];            

            StringBuilder sb = new StringBuilder();
          
            switch (action.Trim().ToLowerInvariant())
            {
                case "showeditor":                    
                    sb.Append("<textarea name=\"changedtext\" style=\"height:400px;width:675px;\">");
                    sb.Append(this.RawHtml);
                    sb.Append("</textarea><br />");
                    sb.Append("<input type=\"hidden\" name=\"partaction\" class=\"editactionhidden\" value=\"saveedit\" />");
                    sb.Append("<input type=\"submit\" name=\"canceleditbutton\" value=\"Close\">");
                    sb.Append("<input type=\"submit\" name=\"savechanges\" value=\"Save Changes\">");
                    result.IsFinishedEditing = false;
                    result.Success = true;
                    result.ResultHtml = sb.ToString();
                    break;
                case "saveedit":                    
                    this.RawHtml = form["changedtext"];
                    result.IsFinishedEditing = true;                    
                    result.Success = true;
                    result.ResultHtml = this.RenderForEdit(context, containerCategory);
                    CatalogServices.Categories.Update(containerCategory);
                    break;
                case "canceledit":
                    result.Success = true;
                    result.IsFinishedEditing = true;
                    result.ResultHtml = this.RenderForEdit(context, containerCategory);
                    break;
                case "deletepart":
                    containerCategory.GetCurrentVersion().Root.RemovePart(this.Id);
                    CatalogServices.Categories.Update(containerCategory);
                    break;
            }

            return result;
        }



       public void SerializeToXml(ref System.Xml.XmlWriter xw)
       {
           xw.WriteStartElement("part");
           xw.WriteElementString("id", this.Id);
           xw.WriteElementString("typecode", "htmlpart");
           xw.WriteElementString("rawhtml", this.RawHtml);           
           xw.WriteEndElement();           
       }

       public void DeserializeFromXml(string xml)
       {
           XElement x = XElement.Parse(xml, LoadOptions.None);

           this.Id = Xml.Parse(x, "id");
           this.RawHtml = Xml.Parse(x, "rawhtml");           
       }
    }
}
