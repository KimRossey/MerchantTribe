
using System.IO;
using System.Text;
using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Orders
{

	public class GiftWrapDetails
	{
        private Guid _hash;
        private string _bvin = string.Empty;
        private System.DateTime _LastUpdated = System.DateTime.MinValue;
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
		private bool _GiftWrapEnabled = false;
		private string _ToField = string.Empty;
		private string _FromField = string.Empty;
		private string _MessageField = string.Empty;

        public bool GiftWrapEnabled {
			get { return _GiftWrapEnabled; }
			set { _GiftWrapEnabled = value; }
		}
		public string ToField {
			get { return _ToField; }
			set { _ToField = value; }
		}
		public string FromField {
			get { return _FromField; }
			set { _FromField = value; }
		}
		public string MessageField {
			get { return _MessageField; }
			set { _MessageField = value; }
		}

		public GiftWrapDetails()
		{
            _hash = System.Guid.NewGuid();
			//Me.Bvin = System.Guid.NewGuid.ToString
		}

        //public override bool FromXml(ref XmlReader xr)
        //{
        //    bool results = false;

        //    try {
        //        while (xr.Read()) {
        //            if (xr.IsStartElement()) {
        //                if (!xr.IsEmptyElement) {
        //                    switch (xr.Name) {
        //                        case "GiftWrapChecked":
        //                            xr.Read();
        //                            _GiftWrapEnabled = bool.Parse(xr.ReadString());
        //                            break;
        //                        case "To":
        //                            xr.Read();
        //                            _ToField = xr.ReadString();
        //                            break;
        //                        case "From":
        //                            xr.Read();
        //                            _FromField = xr.ReadString();
        //                            break;
        //                        case "Message":
        //                            xr.Read();
        //                            _MessageField = xr.ReadString();
        //                            break;
        //                    }
        //                }
        //            }
        //        }

        //        results = true;
        //    }

        //    catch (XmlException) {
        //        results = false;
        //    }

        //    return results;
        //}
        //public override void ToXmlWriter(ref XmlWriter xw)
        //{
        //    if (xw != null) {

        //        xw.WriteStartElement("GiftWrapDetail");

        //        xw.WriteElementString("GiftWrapChecked", _GiftWrapEnabled.ToString());
        //        xw.WriteElementString("To", _ToField);
        //        xw.WriteElementString("From", _FromField);
        //        xw.WriteElementString("Message", _MessageField);

        //        xw.WriteEndElement();
        //    }
        //}

		public bool IsValid()
		{
			bool result = true;

			if (_ToField.Trim().Length < 1) {
				result = false;
			}
			if (_FromField.Trim().Length < 1) {
				result = false;
			}
			if (_MessageField.Trim().Length < 1) {
				result = false;
			}

			return result;
		}

		public bool IsEmpty()
		{
			bool result = true;

			if (this.ToField.Trim().Length > 0) {
				return false;
			}
			if (this.FromField.Trim().Length > 0) {
				return false;
			}
			if (this.MessageField.Trim().Length > 0) {
				return false;
			}

			return result;
		}
	
        //public static void WriteGiftWrapDetailsToXml(Collection<Orders.GiftWrapDetails> source, ref XmlTextWriter xw)
        //{
        //    if (xw != null) {
        //        xw.WriteStartElement("GiftWrapDetails");
        //        for (int i = 0; i <= source.Count - 1; i++) {
        //            XmlWriter temp = (XmlWriter)xw;
        //            source[i].ToXmlWriter(ref temp);
        //        }
        //        xw.WriteEndElement();
        //    }
        //}

        //public static string WriteGiftWrapDetailsToXmlString(Collection<Orders.GiftWrapDetails> source)
        //{
        //    string result = string.Empty;

        //    try {
        //        StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
        //        XmlTextWriter xw = new XmlTextWriter(sw);

        //        xw.Formatting = Formatting.Indented;
        //        xw.Indentation = 3;
        //        xw.WriteStartDocument();

        //        WriteGiftWrapDetailsToXml(source,ref xw);

        //        xw.WriteEndDocument();
        //        xw.Flush();
        //        xw.Close();

        //        result = sw.GetStringBuilder().ToString();
        //        sw.Close();
        //    }

        //    catch (Exception ex) {
        //        EventLog.LogEvent(ex);
        //    }

        //    return result;
        //}

        //public static Collection<Orders.GiftWrapDetails> ReadGiftWrapDetailsFromXml(string xmlData)
        //{
        //    Collection<Orders.GiftWrapDetails> results = new Collection<Orders.GiftWrapDetails>();

        //    try {
        //        if (xmlData.Trim().Length > 0) {
        //            XmlDocument xdoc = new XmlDocument();
        //            xdoc.LoadXml(xmlData);

        //            XmlNodeList addressNodes;
        //            addressNodes = xdoc.SelectNodes("/GiftWrapDetails/GiftWrapDetail");

        //            if (addressNodes != null) {
        //                for (int i = 0; i <= addressNodes.Count - 1; i++) {
        //                    Orders.GiftWrapDetails a = new Orders.GiftWrapDetails();
        //                    if (a.FromXmlString(addressNodes[i].OuterXml) == true) {
        //                        results.Add(a);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex) {
        //        EventLog.LogEvent(ex);
        //        results = new Collection<Orders.GiftWrapDetails>();
        //    }

        //    return results;
        //}

        //public override bool Equals(object obj)
        //{
        //    bool result = false;
        //    if (obj is Contacts.Address) {
        //        if (string.Equals(((Contacts.Address)obj).ToXml(true), this.ToXml(true), StringComparison.InvariantCultureIgnoreCase) == true) {
        //            result = true;
        //        }
        //    }
        //    return result;
        //}

        public override int GetHashCode()
        {
            return _hash.GetHashCode();
        }

		public bool IsEqualTo(Orders.GiftWrapDetails a2)
		{
			if (a2 == null) {
				return false;
			}

			bool result = true;

			if (string.Compare(this.ToField.Trim(), a2.ToField.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.FromField.Trim(), a2.FromField.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.MessageField.Trim(), a2.MessageField.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}

			return result;
		}

		public bool CopyTo(Orders.GiftWrapDetails destinationgiftwrapdetails)
		{
			bool result = true;

			try {
				  destinationgiftwrapdetails.ToField = this.ToField;
                    destinationgiftwrapdetails.FromField = this.FromField;
                    destinationgiftwrapdetails.MessageField = this.MessageField;

			}
			catch  {
				result = false;
			}

			return result;
		}


	}
}
