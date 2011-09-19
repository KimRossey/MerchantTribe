using System;
using System.Xml;
using GCheckout;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Utilities
{

	public class GoogleCheckoutUtilities
	{
		public static string ParseOrderId(XmlElement element)
		{
			string result = string.Empty;
			foreach (XmlNode node in element.ChildNodes) {
				if (node is XmlElement) {
					if (string.Compare(node.Name, "orderid", true) == 0) {
						result = node.InnerText;
					}
				}
			}
			return result;
		}

		public static string ParseUserId(XmlElement element)
		{
			string result = string.Empty;
			foreach (XmlNode node in element.ChildNodes) {
				if (node is XmlElement) {
					if (string.Compare(node.Name, "userid", true) == 0) {
						result = node.InnerText.Trim();
					}
				}
			}
			return result;
		}

		public static Contacts.Address ConvertGoogleAddress(GCheckout.AutoGen.Address address)
		{
			Contacts.Address newAddress = new Contacts.Address();
			newAddress.Bvin = System.Guid.NewGuid().ToString();

			string[] name = address.contactname.Split(' ');

			if (name.Length == 2) {
				newAddress.FirstName = name[0];
				newAddress.LastName = name[1];
			}
			else if (name.Length == 3) {
				newAddress.FirstName = name[0];
				if (name[1].Length > 0) {
					newAddress.MiddleInitial = name[1].Substring(0, 1);
				}
				newAddress.LastName = name[2];
			}

			newAddress.Line1 = address.address1;
			newAddress.Line2 = address.address2;
			newAddress.City = address.city;
			newAddress.PostalCode = address.postalcode;
			newAddress.Phone = address.phone;

            MerchantTribe.Web.Geography.Country country = MerchantTribe.Web.Geography.Country.FindByISOCode(address.countrycode);
			newAddress.CountryName = country.DisplayName;
			newAddress.CountryBvin = country.Bvin;

            foreach (MerchantTribe.Web.Geography.Region region in country.Regions)
            {
				if (string.Compare(region.Abbreviation, address.region, true) == 0) {
					newAddress.RegionName = region.Abbreviation;
					newAddress.RegionBvin = region.Abbreviation;
					break; 
				}
			}

			return newAddress;
		}

		public static Contacts.Address ConvertGoogleAddress(GCheckout.MerchantCalculation.AnonymousAddress address)
		{
			Contacts.Address newAddress = new Contacts.Address();
			newAddress.Bvin = System.Guid.NewGuid().ToString();

			newAddress.City = address.City;
			newAddress.PostalCode = address.PostalCode;

			MerchantTribe.Web.Geography.Country country = MerchantTribe.Web.Geography.Country.FindByISOCode(address.CountryCode);
			newAddress.CountryName = country.DisplayName;
			newAddress.CountryBvin = country.Bvin;

            foreach (MerchantTribe.Web.Geography.Region region in country.Regions)
            {
				if (string.Compare(region.Abbreviation, address.Region, true) == 0) {
					newAddress.RegionName = region.Abbreviation;
					newAddress.RegionBvin = region.Abbreviation;
					break;
				}
			}

			return newAddress;
		}
	}
}
