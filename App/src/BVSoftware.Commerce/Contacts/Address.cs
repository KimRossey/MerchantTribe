using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using MerchantTribe.Web.Geography;
using MerchantTribe.Web;
using BVSoftware.CommerceDTO.v1.Contacts;

namespace BVSoftware.Commerce.Contacts
{
 	public class Address : MerchantTribe.Web.Geography.IAddress
	{
        private XmlWriterSettings _BVXmlWriterSettings = new XmlWriterSettings();
        private XmlReaderSettings _BVXmlReaderSettings = new XmlReaderSettings();

        public string Bvin { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public long StoreId { get; set; }
 		public string NickName {get;set;}
 		public string FirstName {get;set;}
 		public string MiddleInitial {get;set;}
 		public string LastName {get;set;}
 		public string Company {get;set;}
 		public string Line1 {get;set;}
 		public string Line2 {get;set;}
 		public string Line3 {get;set;}
 		public string City {get;set;}
 		public string RegionName {get;set;}
 		public string RegionBvin {get;set;}

        private string _PostalCode = string.Empty;        
 		public string PostalCode {
			get { return _PostalCode; }
			set { _PostalCode = value.Replace("-", ""); }
		}

		public string CountryName {get;set;}
		public string CountryBvin {get;set;}
		public string Phone {get;set;}
		public string Fax {get;set;}
		public string WebSiteUrl {get;set;}
		public string CountyName {get;set;}
		public string CountyBvin {get;set;}
		public string UserBvin {get;set;}
        //public bool Residential {get;set;}
        public AddressTypes AddressType {get;set;}

        public bool IsShipping()
        {
            if (AddressType == AddressTypes.Shipping ||
                AddressType == AddressTypes.BillingAndShipping)
                return true;
            return false;
        }
        public bool IsBilling()
        {
            if (AddressType == AddressTypes.Billing ||
                AddressType == AddressTypes.BillingAndShipping)
                return true;
            return false;
        }
        public void ClearBillingSetting()
        {
            if (AddressType == AddressTypes.BillingAndShipping || AddressType == AddressTypes.Shipping)
            {
                AddressType = AddressTypes.Shipping;
            }
            else
            {
                AddressType = AddressTypes.General;
            }
        }
        public void ClearShippingSetting()
        {
            if (AddressType == AddressTypes.BillingAndShipping || AddressType == AddressTypes.Billing)
            {
                AddressType = AddressTypes.Billing;
            }
            else
            {
                AddressType = AddressTypes.General;
            }
        }
        public void MakeShipping()
        {
            if (AddressType == AddressTypes.Billing)
            {
                AddressType = AddressTypes.BillingAndShipping;
            }
            else
            {
                AddressType = AddressTypes.Shipping;
            }
        }
        public void MakeBilling()
        {
            if (AddressType == AddressTypes.Shipping)
            {
                AddressType = AddressTypes.BillingAndShipping;
            }
            else
            {
                AddressType = AddressTypes.Billing;
            }
        }
		
        public Address()
		{
            this.Init();
		}

        private void Init()
        {
            this.StoreId = 0;
            this.NickName = string.Empty;
            this.FirstName = string.Empty;
            this.MiddleInitial = string.Empty;
            this.LastName = string.Empty;
            this.Company = string.Empty;
            this.Line1 = string.Empty;
            this.Line2 = string.Empty;
            this.Line3 = string.Empty;
            this.City = string.Empty;
            this.RegionName = string.Empty;
            this.RegionBvin = string.Empty;
            this.PostalCode = string.Empty;
            this.CountryName = "US";
            this.CountryBvin = MerchantTribe.Web.Geography.Country.FindByISOCode("US").Bvin;
            this.CountyBvin = string.Empty;
            this.CountyName = string.Empty;
            this.Phone = string.Empty;
            this.Fax = string.Empty;
            this.WebSiteUrl = string.Empty;
            this.UserBvin = string.Empty;
            this.AddressType = AddressTypes.General;
            this.LastUpdatedUtc = DateTime.UtcNow;
        }

		public bool FromXml(ref XmlReader xr)
		{
			bool results = false;

			try {
				while (xr.Read()) {
					if (xr.IsStartElement()) {
						if (!xr.IsEmptyElement) {
							switch (xr.Name) {
								case "Bvin":
									xr.Read();
									Bvin = xr.ReadString();
                                    break;
								case "NickName":
									xr.Read();
									NickName = xr.ReadString();
                                    break;
								case "FirstName":
									xr.Read();
									FirstName = xr.ReadString();
                                    break;
								case "MiddleInitial":
									xr.Read();
									MiddleInitial = xr.ReadString();
                                    break;
								case "LastName":
									xr.Read();
									LastName = xr.ReadString();
                                    break;
								case "Company":
									xr.Read();
									Company = xr.ReadString();
                                    break;
								case "Line1":
									xr.Read();
									Line1 = xr.ReadString();
                                    break;
								case "Line2":
									xr.Read();
									Line2 = xr.ReadString();
                                    break;
								case "Line3":
									xr.Read();
									Line3 = xr.ReadString();
                                    break;
								case "City":
									xr.Read();
									City = xr.ReadString();
                                    break;
								case "RegionName":
									xr.Read();
									RegionName = xr.ReadString();
                                    break;
								case "RegionBvin":
									xr.Read();
									RegionBvin = xr.ReadString();
                                    break;
								case "PostalCode":
									xr.Read();
									_PostalCode = xr.ReadString();
                                    break;
								case "CountryName":
									xr.Read();
									CountryName = xr.ReadString();
                                    break;
								case "CountryBvin":
									xr.Read();
									CountryBvin = xr.ReadString();
                                    break;
								case "Phone":
									xr.Read();
									Phone = xr.ReadString();
                                    break;
								case "Fax":
									xr.Read();
									Fax = xr.ReadString();
                                    break;
								case "WebSiteUrl":
									xr.Read();
									WebSiteUrl = xr.ReadString();
                                    break;
								case "LastUpdated":
									xr.Read();
									LastUpdatedUtc = DateTime.Parse(xr.ReadString());
                                    break;
								case "CountyName":
									xr.Read();
									CountyName = xr.ReadString();
                                    break;
								case "CountyBvin":
									xr.Read();
									CountyBvin = xr.ReadString();
                                    break;
								case "UserBvin":
									xr.Read();
									UserBvin = xr.ReadString();
                                    break;
                                //case "Residential":
                                //    xr.Read();
                                //    Residential = bool.Parse(xr.ReadString());
                                //    break;
                                case "AddressType":
                                    xr.Read();
                                    int tempType = int.Parse(xr.ReadString());
                                    AddressType = (AddressTypes)tempType;
                                    break;
							}
						}
					}
				}

				results = true;
			}			

			catch (XmlException XmlEx) {
                EventLog.LogEvent(XmlEx);
				results = false;
			}

			return results;
		}

		public void ToXmlWriter(ref XmlWriter xw)
		{
			if (xw != null) {

				xw.WriteStartElement("Address");

				xw.WriteElementString("Bvin", Bvin);
				xw.WriteElementString("NickName", NickName);
				xw.WriteElementString("FirstName", FirstName);
				xw.WriteElementString("MiddleInitial", MiddleInitial);
				xw.WriteElementString("LastName", LastName);
				xw.WriteElementString("Company", Company);
				xw.WriteElementString("Line1", Line1);
				xw.WriteElementString("Line2", Line2);
				xw.WriteElementString("Line3", Line3);
				xw.WriteElementString("City", City);
				xw.WriteElementString("RegionName", RegionName);
				xw.WriteElementString("RegionBvin", RegionBvin);
				xw.WriteElementString("PostalCode", _PostalCode);
				xw.WriteElementString("CountryName", CountryName);
				xw.WriteElementString("CountryBvin", CountryBvin);
				xw.WriteElementString("Phone", Phone);
				xw.WriteElementString("Fax", Fax);
				xw.WriteElementString("WebSiteUrl", WebSiteUrl);
				xw.WriteElementString("CountyName", CountyName);
				xw.WriteElementString("CountyBvin", CountyBvin);
				xw.WriteElementString("UserBvin", UserBvin);
                //xw.WriteStartElement("Residential");
                //xw.WriteValue(_Residential);
                //xw.WriteEndElement();
				xw.WriteStartElement("LastUpdated");
				xw.WriteValue(LastUpdatedUtc);
				xw.WriteEndElement();
                xw.WriteElementString("AddressType", ((int)AddressType).ToString());
				xw.WriteEndElement();

			}
		}

		public bool IsValid()
		{
			bool result = true;

			if (FirstName.Trim().Length < 1) {
				result = false;
			}
			if (LastName.Trim().Length < 1) {
				result = false;
			}
			if (Line1.Trim().Length < 1) {
				result = false;
			}
			if (CountryBvin.Trim().Length < 1) {
				result = false;
			}
			if (CountryName.Trim().Length < 1) {
				result = false;
			}
			if (CountryBvin == "bf7389a2-9b21-4d33-b276-23c9c18ea0c0") {
				if (RegionBvin.Trim().Length < 1) {
					if (RegionName.Trim().Length < 1) {
						result = false;
					}
				}
			}
			if (_PostalCode.Trim().Length < 1) {
				result = false;
			}

			return result;
		}

		public bool IsPOBox()
		{
			bool result = false;

			string check = Line1.Trim().ToLower();

			if (check.Contains("p.o. box")) {
				return true;
			}
			if (check.Contains("po box")) {
				return true;
			}
			if (check.Contains("p o box")) {
				return true;
			}
			if (check.Contains("p.o.box")) {
				return true;
			}
			if (check.Contains("p.o box")) {
				return true;
			}
			if (check.Contains("po. box")) {
				return true;
			}
			if (check.Contains("p-o box")) {
				return true;
			}
			if (check.Contains("pobox")) {
				return true;
			}
			if (check.Contains(" po ")) {
				return true;
			}
			if (check.Contains(" box ")) {
				return true;
			}

			return result;
		}

		public bool IsEmpty()
		{
			bool result = true;

			if (this.Bvin.Trim().Length > 0) {
				return false;
			}
			if (this.NickName.Trim().Length > 0) {
				return false;
			}
			if (this.FirstName.Trim().Length > 0) {
				return false;
			}
			if (this.MiddleInitial.Trim().Length > 0) {
				return false;
			}
			if (this.LastName.Trim().Length > 0) {
				return false;
			}
			if (this.Company.Trim().Length > 0) {
				return false;
			}
			if (this.Line1.Trim().Length > 0) {
				return false;
			}
			if (this.Line2.Trim().Length > 0) {
				return false;
			}
			if (this.Line3.Trim().Length > 0) {
				return false;
			}
			if (this.RegionBvin.Trim().Length > 0) {
				return false;
			}
			if (this.RegionName.Trim().Length > 0) {
				return false;
			}
			if (this.PostalCode.Trim().Length > 0) {
				return false;
			}
			if (this.CountryBvin.Trim().Length > 0) {
				return false;
			}
			if (this.CountryName.Trim().Length > 0) {
				return false;
			}
			if (this.Phone.Trim().Length > 0) {
				return false;
			}
			if (this.Fax.Trim().Length > 0) {
				return false;
			}
			if (this.WebSiteUrl.Trim().Length > 0) {
				return false;
			}
			if (this.CountryName.Trim().Length > 0) {
				return false;
			}
			if (this.CountyBvin.Trim().Length > 0) {
				return false;
			}
			if (this.CountyName.Trim().Length > 0) {
				return false;
			}

			return result;
		}

		public string ToHtmlString()
		{
			StringBuilder sb = new StringBuilder();
			if (NickName.Trim().Length > 0) {
				sb.Append("<em>" + NickName + "</em><br />");
			}
            if (LastName.Length > 0 || FirstName.Length > 0)
            {
                sb.Append(FirstName);
                if (MiddleInitial.Trim().Length > 0)
                {
                    sb.Append(" " + MiddleInitial);
                }           
                sb.Append(" " + LastName + "<br />");
                if (Company.Trim().Length > 0)
                {
                    sb.Append(Company + "<br />");
                }
            }
            if (Line1.Length > 0)
            {
                sb.Append(Line1 + "<br />");
            }
			if (Line2.Trim().Length > 0) {
				sb.Append(Line2 + "<br />");
			}
			if (Line3.Trim().Length > 0) {
				sb.Append(Line3 + "<br />");
			}
			

            MerchantTribe.Web.Geography.Country c = MerchantTribe.Web.Geography.Country.FindByBvin(CountryBvin);
            MerchantTribe.Web.Geography.Region r = c.Regions.Where(y => y.Abbreviation == RegionBvin).FirstOrDefault();

            if (r != null)
			{								
			    sb.Append(City + ", " + r.Abbreviation + " " + _PostalCode + "<br />");				
			}
			else {
				if (RegionName.Trim().Length > 0) {
					sb.Append(City + ", " + RegionName + " " + _PostalCode + "<br />");
				}
				else {
					sb.Append(City + ", " + _PostalCode + "<br />");
				}
			}

            //if (CountyName.Trim().Length > 0)
            //{
            //    sb.Append(CountyName + "<br />");
            //}

            if (c != null)
            {
                sb.Append(c.DisplayName + "<br />");
            }
			
			if (Phone.Trim().Length > 0) {
				sb.Append(Phone + "<br />");
			}
			if (Fax.Trim().Length > 0) {
				sb.Append("Fax: " + Fax + "<br />");
			}
			if (WebSiteUrl.Trim().Length > 0) {
				sb.Append(WebSiteUrl + "<br />");
			}
			return sb.ToString();
		}
		
		public static void WriteAddressListToXml(Collection<Contacts.Address> source, ref XmlTextWriter xw)
		{
			if (xw != null) {
				xw.WriteStartElement("AddressBook");
				for (int i = 0; i <= source.Count - 1; i++) {
                    XmlWriter temp = (XmlWriter)xw;
					source[i].ToXmlWriter(ref temp);
				}
				xw.WriteEndElement();
			}
		}

		public static string WriteAddressListToXmlString(Collection<Contacts.Address> source)
		{
			string result = string.Empty;

			try {
				StringWriter sw = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);
				XmlTextWriter xw = new XmlTextWriter(sw);

				xw.Formatting = Formatting.Indented;
				xw.Indentation = 3;
				xw.WriteStartDocument();

				WriteAddressListToXml(source,ref xw);

				xw.WriteEndDocument();
				xw.Flush();
				xw.Close();

				result = sw.GetStringBuilder().ToString();
				sw.Close();
			}

			catch (Exception ex) {
				EventLog.LogEvent(ex);
			}

			return result;
		}

		public static Collection<Contacts.Address> ReadAddressesFromXml(string xmlData)
		{
			Collection<Contacts.Address> results = new Collection<Contacts.Address>();

			try {
				if (xmlData.Trim().Length > 0) {
					XmlDocument xdoc = new XmlDocument();
					xdoc.LoadXml(xmlData);

					XmlNodeList addressNodes;
					addressNodes = xdoc.SelectNodes("/AddressBook/Address");

					if (addressNodes != null) {
						for (int i = 0; i <= addressNodes.Count - 1; i++) {
							Contacts.Address a = new Contacts.Address();
							if (a.FromXmlString(addressNodes[i].OuterXml) == true) {
								results.Add(a);
							}
						}
					}
				}
			}
			catch (Exception ex) {
                EventLog.LogEvent(ex);
				results = new Collection<Contacts.Address>();
			}

			return results;
		}

        public void FromFormFields(System.Collections.Specialized.NameValueCollection formFields, string prefix)
        {
            this.CountryBvin = ParseFromRequest(formFields, prefix, "countryname");
            this.FirstName = ParseFromRequest(formFields, prefix, "firstname");
        }
        private string ParseFromRequest(System.Collections.Specialized.NameValueCollection formFields, string prefix, string fieldname)
        {
            string output = string.Empty;
            if (formFields[prefix + fieldname] != null)
            {
                output = formFields[prefix + fieldname];
            }
            return output;
        }

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

		public bool IsEqualTo(Contacts.Address a2)
		{
			if (a2 == null) {
				return false;
			}

			bool result = true;

			if (string.Compare(this.NickName.Trim(), a2.NickName.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.FirstName.Trim(), a2.FirstName.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.MiddleInitial.Trim(), a2.MiddleInitial.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.LastName.Trim(), a2.LastName.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.Company.Trim(), a2.Company.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.Line1.Trim(), a2.Line1.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.Line2.Trim(), a2.Line2.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.Line3.Trim(), a2.Line3.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.RegionName.Trim(), a2.RegionName.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.RegionBvin.Trim(), a2.RegionBvin.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.City.Trim(), a2.City.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.PostalCode.Trim(), a2.PostalCode.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.CountryBvin.Trim(), a2.CountryBvin.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.CountryName.Trim(), a2.CountryName.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.Phone.Trim(), a2.Phone.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.Fax.Trim(), a2.Fax.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.WebSiteUrl.Trim(), a2.WebSiteUrl.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.CountyBvin.Trim(), a2.CountyBvin.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
			if (string.Compare(this.CountyName.Trim(), a2.CountyName.Trim(), true, System.Globalization.CultureInfo.InvariantCulture) != 0) {
				result = false;
			}
            //if (this.Residential != a2.Residential) {
            //    result = false;
            //}

			return result;
		}

		public bool CopyTo(Contacts.Address destinationAddress)
		{
			bool result = true;

			try {
                    destinationAddress.NickName = this.NickName;
                    destinationAddress.FirstName = this.FirstName;
                    destinationAddress.MiddleInitial = this.MiddleInitial;
                    destinationAddress.LastName = this.LastName;
                    destinationAddress.Company = this.Company;
                    destinationAddress.Line1 = this.Line1;
                    destinationAddress.Line2 = this.Line2;
                    destinationAddress.Line3 = this.Line3;
                    destinationAddress.City = this.City;
                    destinationAddress.RegionBvin = this.RegionBvin;
                    destinationAddress.RegionName = this.RegionName;
                    destinationAddress.PostalCode = this.PostalCode;
                    destinationAddress.CountryBvin = this.CountryBvin;
                    destinationAddress.CountryName = this.CountryName;
                    destinationAddress.Phone = this.Phone;
                    destinationAddress.Fax = this.Fax;
                    destinationAddress.WebSiteUrl = this.WebSiteUrl;
                    destinationAddress.CountyBvin = this.CountyBvin;
                    destinationAddress.CountyName = this.CountyName;
                    //destinationAddress.Residential = this.Residential;                
			}
			catch  {
				result = false;
			}

			return result;
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

        //DTO
        public AddressDTO ToDto()
        {
            AddressDTO dto = new AddressDTO();

            dto.StoreId = this.StoreId;
            dto.NickName = this.NickName ?? string.Empty;
            dto.FirstName = this.FirstName ?? string.Empty;
            dto.MiddleInitial = this.MiddleInitial ?? string.Empty;
            dto.LastName = this.LastName ?? string.Empty;
            dto.Company = this.Company ?? string.Empty;
            dto.Line1 = this.Line1 ?? string.Empty;
            dto.Line2 = this.Line2 ?? string.Empty;
            dto.Line3 = this.Line3 ?? string.Empty;
            dto.City = this.City ?? string.Empty;
            dto.RegionName = this.RegionName ?? string.Empty;
            dto.RegionBvin = this.RegionBvin ?? string.Empty;
            dto.PostalCode = this.PostalCode ?? string.Empty;
            dto.CountryName = this.CountryName ?? string.Empty;
            dto.CountryBvin = this.CountryBvin ?? string.Empty;
            dto.CountyBvin = this.CountyBvin ?? string.Empty;
            dto.CountyName = this.CountyName ?? string.Empty;
            dto.Phone = this.Phone ?? string.Empty;
            dto.Fax = this.Fax ?? string.Empty;
            dto.WebSiteUrl = this.WebSiteUrl ?? string.Empty;
            dto.UserBvin = this.UserBvin ?? string.Empty;
            dto.AddressType = (AddressTypesDTO)((int)this.AddressType);
            dto.LastUpdatedUtc = this.LastUpdatedUtc;

            return dto;
        }
        public void FromDto(AddressDTO dto)
        {
            this.StoreId = dto.StoreId;
            this.NickName = dto.NickName ?? string.Empty;
            this.FirstName = dto.FirstName ?? string.Empty;
            this.MiddleInitial = dto.MiddleInitial ?? string.Empty;
            this.LastName = dto.LastName ?? string.Empty;
            this.Company = dto.Company ?? string.Empty;
            this.Line1 = dto.Line1 ?? string.Empty;
            this.Line2 = dto.Line2 ?? string.Empty;
            this.Line3 = dto.Line3 ?? string.Empty;
            this.City = dto.City ?? string.Empty;
            this.RegionName = dto.RegionName ?? string.Empty;
            this.RegionBvin = dto.RegionBvin ?? string.Empty;
            this.PostalCode = dto.PostalCode ?? string.Empty;
            this.CountryName = dto.CountryName ?? string.Empty;
            this.CountryBvin = dto.CountryBvin ?? string.Empty;
            this.CountyBvin = dto.CountyBvin ?? string.Empty;
            this.CountyName = dto.CountyName ?? string.Empty;
            this.Phone = dto.Phone ?? string.Empty;
            this.Fax = dto.Fax ?? string.Empty;
            this.WebSiteUrl = dto.WebSiteUrl ?? string.Empty;
            this.UserBvin = dto.UserBvin ?? string.Empty;
            this.AddressType = (AddressTypes)((int)dto.AddressType);
            this.LastUpdatedUtc = dto.LastUpdatedUtc;
        }
        
        #region IAddress Members

        public string Street
        {
            get
            {
                return Line1;
            }
            set
            {
                Line1 = value;
            }
        }
        public string Street2
        {
            get { return Line2; }
            set { Line2 = value; }
        }

        public RegionSnapShot RegionData
        {
            get
            {
                RegionSnapShot snap = new RegionSnapShot();
                snap.Abbreviation = this.RegionBvin;
                snap.Name = this.RegionName;
                return snap;
            }
            set
            {
                this.RegionName = value.Name;
                this.RegionBvin = value.Abbreviation;
            }
        }

        public CountrySnapShot CountryData
        {
            get
            {
                CountrySnapShot snap = new CountrySnapShot();
                snap.Name = this.CountryName;
                snap.Bvin = this.CountryBvin;
                return snap;
            }
            set
            {
                this.CountyName = value.Name;
                this.CountryBvin = value.Bvin;
            }
        }

        #endregion
    }
}
