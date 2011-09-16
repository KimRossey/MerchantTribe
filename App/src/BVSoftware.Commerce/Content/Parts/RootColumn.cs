﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MerchantTribe.Web;
using System.Xml.Linq;

namespace BVSoftware.Commerce.Content.Parts
{
    public class RootColumn: IColumn
    {
        public List<IContentPart> Parts { get; set; }

        public RootColumn()
        {
            Id = System.Guid.NewGuid().ToString();
            Parts = new List<IContentPart>();
        }

        public ColumnSize Size
        {
            get { return ColumnSize.Size12; }
            set { }
        }

        public void SetContainer(IColumn container)
        {
            //ignore
        }

        private string Render(RequestContext context, bool isEditMode, Catalog.Category containerCategory)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class=\"cols\">");
            sb.Append("<div class=\"grid_12");
            sb.Append("\" >");

            if (isEditMode)
            {
                sb.Append("<div class=\"droppable\" id=\"part" + Id.ToString() + "\">");
                sb.Append("<div class=\"edittools\" id=\"edittools" + this.Id + "\"><strong>Page Root</strong> </div>");
            }
            foreach (IContentPart p in Parts)
            {
                if (isEditMode)
                {
                    
                    sb.Append(p.RenderForEdit(context, containerCategory));
                    
                }
                else
                {
                    sb.Append(p.RenderForDisplay(context, containerCategory));
                }
            }

            if (isEditMode)
            {
               //sb.Append("<div class=\"editplaceholder\">Drag Parts Here</div>");
               sb.Append("</div>"); // Close out droppable div
            }

            sb.Append("</div>");
            sb.Append("<div class=\"clearcol\"></div>");
            sb.Append("</div>");
            return sb.ToString();
        }
        public string RenderForDisplay(RequestContext context, Catalog.Category containerCategory)
        {
            return Render(context, false, containerCategory);
        }
        public string RenderForEdit(RequestContext context, Catalog.Category containerCategory)
        {
            return Render(context, true, containerCategory);
        }

        public bool NoGutter
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }

        public IContentPart FindPart(string partId)
        {
            if (partId == this.Id) return this;

            foreach (IContentPart p in this.Parts)
            {
                if (p.Id == partId)
                {
                    return p;
                }
                if (p is IColumnContainer)
                {
                    IColumnContainer container = p as IColumnContainer;
                    IContentPart result = container.FindPart(partId);
                    if (result != null) return result;                    
                }
            }

            return null;
        }

        public bool AddPart(IContentPart part)
        {
            Parts.Add(part);
            return true;
        }
        public bool RemovePart(string id)
        {
            if (id == this.Id) return false;

            IContentPart toRemove = null;

            foreach (IContentPart p in this.Parts)
            {
                if (p.Id == id)
                {
                    toRemove = p;
                    break;
                }
                if (p is IColumnContainer)
                {
                    IColumnContainer container = p as IColumnContainer;
                    bool result = container.RemovePart(id);
                    if (result) return result;
                }
            }

            if (toRemove != null) this.Parts.Remove(toRemove);            
            return true;
        }
        
        public string Id {get;set;}

        public ColumnSize MinimumSize()
        {
            return ColumnSize.Size12;
        }

        public PartJsonResult ProcessJsonRequest(System.Collections.Specialized.NameValueCollection form,
                                          BVSoftware.Commerce.RequestContext context, Catalog.Category containerCategory)
        {
            PartJsonResult result = new PartJsonResult();

            Catalog.CatalogService CatalogServices = Catalog.CatalogService.InstantiateForDatabase(context);

            string action = form["partaction"];
            switch (action.ToLowerInvariant())
            {
                case "addpart":
                    string parttype = form["parttype"];
                    IContentPart part = PartFactory.Instantiate(System.Guid.NewGuid().ToString(), parttype, this);
                    if (part != null)
                    {
                        this.AddPart(part);
                        CatalogServices.Categories.Update(containerCategory);
                        result.ResultHtml = part.RenderForEdit(context, containerCategory);
                    }
                    break;
                case "deletepart":
                    string deleteid = form["partid"];
                    this.RemovePart(deleteid);
                    CatalogServices.Categories.Update(containerCategory);
                    result.Success = true;
                    break;
                    
            }
            return result;
        }

        public string SerializeToString()
        {
            XmlWriterSettings writersettings = new XmlWriterSettings();
            string data = string.Empty;
            StringBuilder sb = new StringBuilder();
            writersettings.ConformanceLevel = ConformanceLevel.Fragment;
            writersettings.NewLineOnAttributes = false;            
            writersettings.Indent = false;
            writersettings.OmitXmlDeclaration = true;
            XmlWriter xw = XmlWriter.Create(sb, writersettings);

            SerializeToXml(ref xw);

            xw.Flush();
            xw.Close();
            data = sb.ToString();
            return data;
        }
        
        public void SerializeToXml(ref System.Xml.XmlWriter xw)
        {
            xw.WriteStartElement("rootcolumn");
            xw.WriteElementString("id", this.Id);            
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

            XmlNode idNode = xdoc.SelectSingleNode("/rootcolumn/id");
            if (idNode != null)
            {
                this.Id = idNode.InnerText;
            }

            this.Parts.Clear();

            XmlNodeList partNodes;
            partNodes = xdoc.SelectNodes("/rootcolumn/parts/part");
            if (partNodes != null)
            {
                for (int i = 0; i <= partNodes.Count - 1; i++)
                {
                    ImportPart(partNodes[i].OuterXml);
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
