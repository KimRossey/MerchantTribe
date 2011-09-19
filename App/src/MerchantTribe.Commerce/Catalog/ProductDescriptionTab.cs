using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    public class ProductDescriptionTab
    {
        private string _bvin = string.Empty;
        private System.DateTime _LastUpdated = System.DateTime.MinValue;
        
        private string _TabTitle = string.Empty;
        private string _HtmlData = string.Empty;
        private int _SortOrder = 1;

        public virtual string Bvin
        {
            get { return _bvin; }
            set { _bvin = value; }
        }
        public virtual System.DateTime LastUpdated
        {
            get { return _LastUpdated; }
            set { _LastUpdated = value; }
        }
        public string TabTitle
        {
            get { return _TabTitle; }
            set { _TabTitle = value; }
        }
        public string HtmlData
        {
            get { return _HtmlData; }
            set { _HtmlData = value; }
        }
        public int SortOrder
        {
            get { return _SortOrder; }
            set { _SortOrder = value; }
        }

        public ProductDescriptionTab()
        {
            this.Bvin = System.Guid.NewGuid().ToString().Replace("{", "").Replace("}", "");
        }


        private XmlWriterSettings _BVXmlWriterSettings = new XmlWriterSettings();
        private XmlReaderSettings _BVXmlReaderSettings = new XmlReaderSettings();
        protected XmlWriterSettings BVXmlWriterSettings
        {
            get { return _BVXmlWriterSettings; }
        }
        protected XmlReaderSettings BVXmlReaderSettings
        {
            get { return _BVXmlReaderSettings; }
        }
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
                                    this.Bvin = xr.ReadString();
                                    break;
                                case "TabTitle":
                                    xr.Read();
                                    this.TabTitle = xr.ReadString();
                                    break;
                                case "HtmlData":
                                    xr.Read();
                                    this.HtmlData = xr.ReadString();
                                    break;
                                case "SortOrder":
                                    xr.Read();
                                    this.SortOrder = int.Parse(xr.ReadString());
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
                xw.WriteStartElement("ProductDescriptionTab");
                xw.WriteElementString("Bvin", this.Bvin);
                xw.WriteElementString("TabTitle", this.TabTitle);
                xw.WriteElementString("HtmlData", this.HtmlData);
                xw.WriteElementString("SortOrder", this.SortOrder.ToString());
                xw.WriteEndElement();
            }
        }

        public void FromDto(ProductDescriptionTabDTO dto)
        {
            this.Bvin = dto.Bvin;
            this.HtmlData = dto.HtmlData;
            this.SortOrder = dto.SortOrder;
            this.TabTitle = dto.TabTitle;            
        }
        public ProductDescriptionTabDTO ToDto()
        {
            ProductDescriptionTabDTO dto = new ProductDescriptionTabDTO();
            dto.Bvin = this.Bvin;
            dto.HtmlData = this.HtmlData;
            dto.SortOrder = this.SortOrder;
            dto.TabTitle = this.TabTitle;
            return dto;
        }

    }
}
