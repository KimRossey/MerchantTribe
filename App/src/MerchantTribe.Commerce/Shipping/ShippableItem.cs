using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerchantTribe.Web;
using System.Xml;
using System.IO;
using MerchantTribe.CommerceDTO.v1.Shipping;

namespace MerchantTribe.Commerce.Shipping
{
    public class ShippableItem
    {
        public bool IsNonShipping { get; set; }
        public decimal ExtraShipFee { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public long ShippingScheduleId { get; set; }
        public ShippingMode ShippingSource { get; set; }
        public Contacts.Address ShippingSourceAddress { get; set; }
        public string ShippingSourceId { get; set; }
        public bool ShipSeparately { get; set; }

        public ShippableItem()
        {
            IsNonShipping = false;
            ExtraShipFee = 0m;
            Weight = 0m;
            Length = 0m;
            Width = 0m;
            Height = 0m;
            ShippingScheduleId = 0;
            ShippingSource = ShippingMode.ShipFromSite;
            ShippingSourceId = string.Empty;
            ShipSeparately = false;
            ShippingSourceAddress = new Contacts.Address();
        }


        public void FromXml(string xml)
        {
            System.IO.StringReader sw = new System.IO.StringReader(xml);
            XmlReader xr = XmlReader.Create(sw);
            FromXml(ref xr);
            sw.Dispose();
            xr.Close();
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
                                case "IsNonShipping":
                                    xr.Read();
                                    this.IsNonShipping = bool.Parse(xr.ReadString());
                                    break;
                                case "ShipSeparately":
                                    xr.Read();
                                    this.ShipSeparately = bool.Parse(xr.ReadString());
                                    break;
                                case "ExtraShipFee":
                                    xr.Read();
                                    this.ExtraShipFee = decimal.Parse(xr.ReadString());
                                    break;
                                case "Weight":
                                    xr.Read();
                                    this.Weight = decimal.Parse(xr.ReadString());
                                    break;
                                case "Length":
                                    xr.Read();
                                    this.Length = decimal.Parse(xr.ReadString());
                                    break;
                                case "Width":
                                    xr.Read();
                                    this.Width = decimal.Parse(xr.ReadString());
                                    break;
                                case "Height":
                                    xr.Read();
                                    this.Height = decimal.Parse(xr.ReadString());
                                    break;
                                case "ShippingScheduleId":
                                    xr.Read();
                                    this.ShippingScheduleId = long.Parse(xr.ReadString());
                                    break;
                                case "ShippingSource":
                                    xr.Read();
                                    this.ShippingSource = (ShippingMode)int.Parse(xr.ReadString());
                                    break;
                                case "ShippingSourceId":
                                    xr.Read();
                                    this.ShippingSourceId = xr.ReadString();
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
            xw.WriteStartElement("ShippableItem");

            xw.WriteStartElement("IsNonShipping"); xw.WriteValue(this.IsNonShipping); xw.WriteEndElement();
            xw.WriteStartElement("ShipSeparately"); xw.WriteValue(this.ShipSeparately); xw.WriteEndElement();
            Xml.WriteDecimal("ExtraShipFee", this.ExtraShipFee, ref xw);
            Xml.WriteDecimal("Weight", this.Weight, ref xw);
            Xml.WriteDecimal("Length", this.Length, ref xw);
            Xml.WriteDecimal("Width", this.Width, ref xw);
            Xml.WriteDecimal("Height", this.Height, ref xw);
            Xml.WriteLong("ShippingScheduleId", this.ShippingScheduleId, ref xw);
            Xml.WriteInt("ShippingSource", (int)this.ShippingSource, ref xw);
            xw.WriteElementString("ShippingSourceId", this.ShippingSourceId);

            xw.WriteEndElement();
        }

        public ShippableItemDTO ToDto()
        {
            ShippableItemDTO dto = new ShippableItemDTO();
            dto.ExtraShipFee = this.ExtraShipFee;
            dto.Height = this.Height;
            dto.IsNonShipping = this.IsNonShipping;
            dto.Length = this.Length;
            dto.ShippingScheduleId = this.ShippingScheduleId;
            dto.ShippingSource = (ShippingModeDTO)((int)this.ShippingSource);
            dto.ShippingSourceId = this.ShippingSourceId;
            dto.ShipSeparately = this.ShipSeparately;
            dto.Weight = this.Weight;
            dto.Width = this.Width;
            return dto;
        }
        public void FromDto(ShippableItemDTO dto)
        {
            this.ExtraShipFee = dto.ExtraShipFee;
            this.Height = dto.Height;
            this.IsNonShipping = dto.IsNonShipping;
            this.Length = dto.Length;
            this.ShippingScheduleId = dto.ShippingScheduleId;
            this.ShippingSource = (ShippingMode)((int)dto.ShippingSource);
            this.ShippingSourceId = dto.ShippingSourceId ?? string.Empty;
            this.ShipSeparately = dto.ShipSeparately;
            this.Weight = dto.Weight;
            this.Width = dto.Width;            
        }

    }
}
