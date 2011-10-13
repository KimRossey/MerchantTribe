using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MerchantTribe.Web;

namespace MerchantTribe.Commerce.Content.Parts
{
    public class Column: IContentPart, IColumn
    {
        private List<IContentPart> _Parts = new List<IContentPart>();

        private IColumn _Container = null;

        public ColumnSize Size { get; set; }
        public bool NoGutter { get; set; }
        public List<IContentPart> Parts
        {
            get { return _Parts; }
        }

        public Column()
        {
            Id = System.Guid.NewGuid().ToString();
            Size = ColumnSize.Size12;
            NoGutter = false;        
        }

        public void SetContainer(IColumn container)
        {
            this._Container = container;
        }

        public bool AddPart(IContentPart part)
        {
            if (this.Size < part.MinimumSize())
            {
                return false;
            }
            part.SetContainer(this);

            _Parts.Add(part);
            return true;
        }

        public bool RemovePart(string partId)
        {
            int index = -1;
            for (int i = 0; i < _Parts.Count; i++)
            {
                if (_Parts[i].Id == partId)
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                _Parts.RemoveAt(index);
                return true;
            }
            return false;
        }
        public bool SortParts(List<string> sortedIds)
        {
            List<IContentPart> output = new List<IContentPart>();

            foreach (string id in sortedIds)
            {
                var part = this.Parts.Where(y => y.Id == id).FirstOrDefault();
                if (part != null)
                {
                    this.Parts.Remove(part);
                    output.Add(part);
                }                
            }
            foreach (var remainingPart in this.Parts)
            {
                output.Add(remainingPart);
            }
            this._Parts = output;

            return true;
        }

        public string Id { get; set; }

        public string RenderForDisplay(RequestContext context, Catalog.Category containerCategory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IContentPart p in _Parts)
            {
                sb.Append(p.RenderForDisplay(context, containerCategory));
            }
            return sb.ToString();            
        }

        public string RenderForEdit(RequestContext context, Catalog.Category containerCategory)
        {
            StringBuilder sb = new StringBuilder();
            foreach (IContentPart p in _Parts)
            {
                sb.Append(p.RenderForEdit(context, containerCategory));                
            }
            //sb.Append("<div class=\"editplaceholder\">Drag Parts Here</div>");
            return sb.ToString();
        }

        public ColumnSize MinimumSize()
        {
            ColumnSize result = ColumnSize.Size1;

            foreach (IContentPart p in _Parts)
            {
                if (p.MinimumSize() > result)
                {
                    result = p.MinimumSize();
                }
            }

            return result;
        }


        public PartJsonResult ProcessJsonRequest(System.Collections.Specialized.NameValueCollection form, 
                                                MerchantTribeApplication app, Catalog.Category containerCategory)
        {

            PartJsonResult result = new PartJsonResult();
            
            string action = form["partaction"];
            switch(action.ToLowerInvariant())
            {
                case "addpart":
                    string parttype = form["parttype"];
                    IContentPart part = PartFactory.Instantiate(System.Guid.NewGuid().ToString(), parttype, this);
                    if (part != null)
                    {
                        this.AddPart(part);
                        app.CatalogServices.Categories.Update(containerCategory);
                        result.ResultHtml = part.RenderForEdit(app.CurrentRequestContext, containerCategory);
                    }
                    break;
                case "resort":
                    string sortedIds = form["sortedIds[]"];
                    string[] ids = sortedIds.Split(',');
                    List<string> idList = new List<string>();
                    foreach (string s in ids)
                    {
                        idList.Add(s.Trim().Replace("part",""));
                    }
                    result.Success = this.SortParts(idList);
                    app.CatalogServices.Categories.Update(containerCategory);                    
                    break;
            }
            return result;
        }



        public void SerializeToXml(ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement("part");

            xw.WriteElementString("id",this.Id);
            xw.WriteElementString("typecode", "column");
            xw.WriteElementString("size", this.Size.ToString());
            xw.WriteStartElement("nogutter"); xw.WriteValue(this.NoGutter); xw.WriteEndElement();

            xw.WriteStartElement("parts");
            foreach (IContentPart p in this.Parts)
            {
                p.SerializeToXml(ref xw);                
            }
            xw.WriteEndElement();

            xw.WriteEndElement();            
        }

        public void DeserializeFromXml(string xml)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            XmlNode sizeNode = xdoc.SelectSingleNode("/part/size");
            {
                string sizetest = sizeNode.InnerText;
                ColumnSize sizeout = ColumnSize.Size1;
                if (Enum.TryParse<ColumnSize>(sizetest,out sizeout))
                {
                    this.Size = sizeout;
                }
            }
            XmlNode gutterNode = xdoc.SelectSingleNode("/part/nogutter");
            if (gutterNode != null)
            {
                bool gutterout = false;
                if (bool.TryParse(gutterNode.InnerText, out gutterout))
                {
                    this.NoGutter = gutterout;
                }
            }

            this.Parts.Clear();

            XmlNodeList partNodes = xdoc.SelectNodes("/part/parts/part");
            if (partNodes != null)
            {
                foreach (XmlNode node in partNodes)
                {
                    ImportPart(node.OuterXml);                    
                }
            }            
        }

        private void ImportPart(string xml)
        {
            XElement x = XElement.Parse(xml, LoadOptions.None);
            string id = Xml.Parse(x, "id");
            string typecode = Xml.Parse(x, "typecode");
            IContentPart p = PartFactory.Instantiate(id, typecode, this);
            p.DeserializeFromXml(xml);
            this.AddPart(p);
        }
    }
}
