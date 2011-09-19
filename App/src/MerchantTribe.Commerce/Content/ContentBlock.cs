using System;
using System.IO;
using System.Web;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace MerchantTribe.Commerce.Content
{	
	public class ContentBlock
	{                       
        public string Bvin {get;set;}
        public long StoreId { get; set; }
        public DateTime LastUpdated {get;set;}
        public string ColumnId { get; set; }
        public int SortOrder { get; set; }
        public string ControlName { get; set; }        
        public ContentBlockSettings BaseSettings {get;set;}
        public ContentBlockSettingList Lists {get;set;}

        public ContentBlock()
        {
            this.BaseSettings = new ContentBlockSettings();
            this.Lists = new ContentBlockSettingList();
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.ColumnId = string.Empty;
            this.SortOrder = 1;
            this.ControlName = string.Empty;
        }

        public ContentBlock Clone()
        {
            ContentBlock clone = new ContentBlock();
            clone.StoreId = this.StoreId;
            clone.LastUpdated = DateTime.UtcNow;
            clone.ColumnId = this.ColumnId;
            clone.SortOrder = this.SortOrder;
            clone.ControlName = this.ControlName;
            foreach (var q in BaseSettings)
            {
                clone.BaseSettings.Add(q.Key, q.Value);
            }
            foreach (var y in Lists.Items)
            {
                clone.Lists.AddItem(y.Clone());
            }
            return clone;
        }

        private XmlWriterSettings _BVXmlWriterSettings = new XmlWriterSettings();
        private XmlReaderSettings _BVXmlReaderSettings = new XmlReaderSettings();
        public virtual string ToXml(bool omitDeclaration)
        {
            string response = string.Empty;
            StringBuilder sb = new StringBuilder();
            _BVXmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
            XmlWriter xw = XmlWriter.Create(sb, _BVXmlWriterSettings);
            ToXmlWriter(ref xw);
            xw.Flush();
            xw.Close();
            response = sb.ToString();
            return response;
        }
        public virtual bool FromXmlString(string x)
        {
            System.IO.StringReader sw = new System.IO.StringReader(x);
            XmlReader xr = XmlReader.Create(sw);
            bool result = FromXml(ref xr);
            sw.Dispose();
            xr.Close();
            return result;
        }
        public bool FromXml(ref System.Xml.XmlReader xr)
        {
            bool results = false;

            try
            {
                while (xr.Read())
                {
                    if (xr.IsStartElement())
                    {
                        if (!xr.IsEmptyElement)
                        {
                            switch (xr.Name)
                            {
                                case "Bvin":
                                    xr.Read();
                                    Bvin = xr.ReadString();
                                    break;
                                case "ControlName":
                                    xr.Read();
                                    ControlName = xr.ReadString();
                                    break;
                                case "SortOrder":
                                    xr.Read();
                                    SortOrder = int.Parse(xr.ReadString());
                                    break;
                                case "ColumnId":
                                    xr.Read();
                                    ColumnId = xr.ReadString();
                                    break;
                                case "SerializedSettings":
                                    xr.Read();
                                    string json = xr.ReadString();
                                    this.BaseSettings = MerchantTribe.Web.Json.ObjectFromJson<ContentBlockSettings>(json);
                                    break;
                                case "Lists":
                                    xr.Read();
                                    string jsonlist = xr.ReadString();
                                    this.Lists = MerchantTribe.Web.Json.ObjectFromJson<ContentBlockSettingList>(jsonlist);
                                    break;
                            }
                        }
                    }
                }

                results = true;
            }

            catch (XmlException XmlEx)
            {
                EventLog.LogEvent(XmlEx);
                results = false;
            }

            return results;

        }
        public void ToXmlWriter(ref System.Xml.XmlWriter xw)
        {

            if (xw != null)
            {
                xw.WriteStartElement("ContentBlock");

                xw.WriteElementString("Bvin", this.Bvin);
                xw.WriteElementString("ControlName", this.ControlName);
                xw.WriteElementString("SortOrder", this.SortOrder.ToString());
                xw.WriteElementString("ColumnId", this.ColumnId);
                xw.WriteElementString("SerializedSettings", MerchantTribe.Web.Json.ObjectToJson(this.BaseSettings));
                xw.WriteElementString("Lists", MerchantTribe.Web.Json.ObjectToJson(this.Lists));

                xw.WriteEndElement(); // end Column
            }

        }
      	
	}
}

