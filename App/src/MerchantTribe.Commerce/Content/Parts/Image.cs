using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using MerchantTribe.Web;

namespace MerchantTribe.Commerce.Content.Parts
{
    public class Image: IContentPart
    {

        private IColumn _Container = null;

        public List<ImageDisplayFile> Images { get; set; }

        public string Id {get;set;}        

        public Image()
        {
            Id = System.Guid.NewGuid().ToString();
            Images = new List<ImageDisplayFile>();
        }

        public void SetContainer(IColumn container)
        {
            this._Container = container;
        }

        public string RenderForDisplay(RequestContext context, Catalog.Category cat)
        {
            string sizeClass = "flexsize12";
            if (this._Container != null)
            {
                sizeClass = "flexsize";
                sizeClass += ((int)_Container.Size).ToString();
                if (_Container.NoGutter && _Container.Size != ColumnSize.Size12)
                {
                    sizeClass += "w";
                }                
            }            
            string url = System.Web.VirtualPathUtility.ToAbsolute("~/images/system/flexedit/imagePlaceholder.png");
            string alt = "Placeholder Image";
            if (Images.Count > 0)
            {
                ImageDisplayFile img = this.Images[0];
                long versionId = cat.GetCurrentVersion().Id;
                url = Storage.DiskStorage.FlexPageImageUrl(context.CurrentStore.Id, cat.Bvin, versionId.ToString(), img.FileName, true);
                alt = img.AltText;                
            }

            return "<img src=\"" + url + "\" alt=\"" + alt + "\" class=\"" + sizeClass + "\" />";
        }

        public string RenderForEdit(RequestContext context, Catalog.Category cat)
        {
            StringBuilder sb = new StringBuilder();            
            sb.Append("<div id=\"part" + Id + "\" class=\"editable issortable\">");
            sb.Append(PartHelper.RenderEditTools(this.Id));
            sb.Append(RenderForDisplay(context, cat));
            sb.Append("</div>");

            return sb.ToString();
        }

        public ColumnSize MinimumSize()
        {
            return ColumnSize.Size1;
        }


       public PartJsonResult ProcessJsonRequest(System.Collections.Specialized.NameValueCollection form,
                                          MerchantTribeApplication app, Catalog.Category containerCategory)
        {
            PartJsonResult result = new PartJsonResult();

            string action = form["partaction"];
          
            StringBuilder sb = new StringBuilder();          
            switch (action.Trim().ToLowerInvariant())
            {
                case "showeditor":             
                    result.IsFinishedEditing = false;
                    result.Success = true;
                    result.ResultHtml = BuildEditor(containerCategory, string.Empty, app.CurrentRequestContext);
                    //result.ScriptFunction = initScript;
                    break;
                case "saveedit":  

                    // Update from Form Here                    
                    string editMessage = string.Empty;

                    if (form["uploadedfilename"] != null)
                    {
                        string uploadedname = form["uploadedfilename"];

                        if (uploadedname.Length > 0)
                        {
                            if (this.Images.Count < 1)
                            {
                                this.Images.Add(new ImageDisplayFile() { AltText = uploadedname, FileName = uploadedname, SortOrder = 0 });
                            }
                            else
                            {
                                this.Images[0].FileName = uploadedname;
                            }
                        }
                    }

                    if (this.Images.Count > 0)
                    {
                        string altText = form["altfield"];
                        this.Images[0].AltText = altText;
                        editMessage = "Changes Saved!";
                    }
                    else
                    {
                        editMessage = "Upload an Image before Saving Changes!";
                    }
                    
                    app.CatalogServices.Categories.Update(containerCategory);
                    result.Success = true;
                    result.IsFinishedEditing = false;
                    result.ResultHtml = BuildEditor(containerCategory, editMessage, app.CurrentRequestContext);
                    //result.ScriptFunction = initScript;
                    break;
                case "canceledit":
                    result.Success = true;
                    result.IsFinishedEditing = true;
                    result.ResultHtml = this.RenderForEdit(app.CurrentRequestContext, containerCategory);
                    break;
                case "deletepart":
                    containerCategory.GetCurrentVersion().Root.RemovePart(this.Id);
                    app.CatalogServices.Categories.Update(containerCategory);
                    break;
            }

            return result;
        }

   



       public void SerializeToXml(ref System.Xml.XmlWriter xw)
       {
           xw.WriteStartElement("part");
           xw.WriteElementString("id", this.Id);
           xw.WriteElementString("typecode", "image");
           xw.WriteStartElement("images");
           foreach (ImageDisplayFile image in this.Images)
           {
               xw.WriteStartElement("image");
               xw.WriteElementString("sortorder", image.SortOrder.ToString());
               xw.WriteElementString("filename", image.FileName);
               xw.WriteElementString("alttext", image.AltText);
               xw.WriteEndElement();
           }
           xw.WriteEndElement();          
           xw.WriteEndElement();           
       }
       public void DeserializeFromXml(string xml)
       {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            this.Images.Clear();

            XmlNode idNode = xdoc.SelectSingleNode("/part/id");
            if (idNode != null)
            {
                this.Id = idNode.InnerText;
            }           

           XmlNodeList imageNodes = xdoc.SelectNodes("/part/images/image");
           if (imageNodes != null)
           {
               foreach (XmlNode node in imageNodes)
               {
                   ImageDisplayFile img = new ImageDisplayFile();

                   XmlNode fileNameNode = node.SelectSingleNode("filename");
                   if (fileNameNode != null)
                   {
                       img.FileName = fileNameNode.InnerText;
                   }
                   XmlNode altNode = node.SelectSingleNode("alttext");
                   if (altNode != null)
                   {
                       img.AltText = altNode.InnerText;
                   }
                   XmlNode sortNode = node.SelectSingleNode("sortorder");
                   if (sortNode != null)
                   {
                       string tempsort = sortNode.InnerText;
                       int temp = 0;
                       if (int.TryParse(tempsort, out temp))
                       {
                           img.SortOrder = temp;
                       }
                   }

                   this.Images.Add(img);
               }
           }                  
       }

       private string BuildEditor(Catalog.Category containerCategory, string message, MerchantTribe.Commerce.RequestContext context)
       {
           long versionId = containerCategory.GetCurrentVersion().Id;
           ImageDisplayFile img = new ImageDisplayFile();
           if (this.Images.Count > 0) img = this.Images[0];

           string previewUrl = "";
           string alt = string.Empty;

           previewUrl = System.Web.VirtualPathUtility.ToAbsolute("~/images/system/flexedit/imagePlaceholder.png");
           if (this.Images.Count > 0)
           {
               alt = this.Images[0].AltText;
               if (this.Images[0].FileName.Trim().Length > 0)
               {
                   previewUrl = Storage.DiskStorage.FlexPageImageUrl(containerCategory.StoreId, containerCategory.Bvin, versionId.ToString(), img.FileName, false);
               }               
           }
                     
           StringBuilder sb = new StringBuilder();
           sb.Append("<div class=\"flexeditarea\">");
           sb.Append("<div id=\"uploadimagemessage\">" + message + "</div>");
           sb.Append("<table width=\"100%\">");
           
           sb.Append("<tr><td class=\"formlabel\">Image:</td><td class=\"formfield\">");
           sb.Append("<img width=\"200px\" height=\"200px\" src=\"" + previewUrl + "\" id=\"uploadimagepreview\" name=\"uploadimagepreview\" /><br />");

           sb.Append("<div id=\"silverlightControlHost\">");
           sb.Append("<object data=\"data:application/x-silverlight-2,\" type=\"application/x-silverlight-2\" width=\"100\" height=\"55\">");

           string SilverLightUrl = System.Web.VirtualPathUtility.ToAbsolute("~/ClientBin/BVSoftware.SilverlightFileUpload.xap");
	 	   sb.Append("<param name=\"source\" value=\""+ SilverLightUrl + "\"/>");

	 	   sb.Append("<param name=\"background\" value=\"black\" />");
           sb.Append("<param name=\"onError\" value=\"onSilverlightError\" />");
		   sb.Append("<param name=\"minRuntimeVersion\" value=\"4.0.50826.0\" />");
		   sb.Append("<param name=\"autoUpgrade\" value=\"true\" />");

           sb.Append("<param name=\"initParams\" value=\"scriptname=ImageWasUploaded,uploadurl=");

           // We have to pull the host out because the ToAbsolute of the virutal path utility
           // will append sub folder name if the web site is not the root app in IIS
           string currentFullRoot = context.CurrentStore.RootUrl();
           Uri fullUri = new Uri(currentFullRoot);
           string host = fullUri.DnsSafeHost;
           sb.Append("http://" + host);
           sb.Append(System.Web.VirtualPathUtility.ToAbsolute("~/fileuploadhandler/1/" + containerCategory.Bvin + "/" + versionId.ToString()));
           sb.Append("\"/>");

           sb.Append("<a href=\"http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50826.0\" style=\"text-decoration:none\">");
           sb.Append("<img src=\"http://go.microsoft.com/fwlink/?LinkId=161376\" alt=\"Get Microsoft Silverlight\" style=\"border-style:none\"/>");
		   sb.Append("</a>");
	       sb.Append("</object><iframe id=\"_sl_historyFrame\" style=\"visibility:hidden;height:0px;width:0px;border:0px\"></iframe>");
           sb.Append("</div>");

           sb.Append("</td></tr>");

           sb.Append("<tr><td class=\"formlabel\">Alt. Text:</td><td class=\"formfield\">");
           sb.Append("<input type=\"text\" id=\"altfield\" name=\"altfield\" value=\"" + System.Web.HttpUtility.HtmlEncode(alt) + "\" />");
           sb.Append("</td></tr>");
         
           sb.Append("</table>");           


           sb.Append("</div>");
           sb.Append("<div class=\"flexeditbuttonarea\">");
           sb.Append("<input type=\"hidden\" name=\"uploadedfilename\" id=\"uploadedfilename\" value=\"\" />");
           sb.Append("<input type=\"hidden\" name=\"partaction\" class=\"editactionhidden\" value=\"saveedit\" />");
           sb.Append("<input type=\"submit\" name=\"canceleditbutton\" value=\"Close\">");
           sb.Append("<input type=\"submit\" name=\"savechanges\" value=\"Save Changes\">");
           sb.Append("</div>");
                 
           return sb.ToString();
       }

    }
}
