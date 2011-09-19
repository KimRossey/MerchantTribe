
using System.Xml;

namespace MerchantTribe.Shipping.FedEx
{

public class Address
{

	private string _Line1 = string.Empty;
	private string _Line2 = string.Empty;
	private string _City = string.Empty;
	private string _StateOrProvinceCode = string.Empty;
	private string _PostalCode = string.Empty;
	private string _CountryCode = "US";

	public string Line1 {
		get { return _Line1; }
        set { _Line1 = MerchantTribe.Web.Text.TrimToLength(value, 35); }
	}
	public string Line2 {
		get { return _Line2; }
        set { _Line2 = MerchantTribe.Web.Text.TrimToLength(value, 35); }
	}
	public string City {
		get { return _City; }
        set { _City = MerchantTribe.Web.Text.TrimToLength(value, 35); }
	}
	public string StateOrProvinceCode {
		get { return _StateOrProvinceCode; }
        set { _StateOrProvinceCode = MerchantTribe.Web.Text.TrimToLength(value, 2); }
	}
	public string PostalCode {
		get { return _PostalCode; }
		set {
			string temp = value;
			temp = temp.Replace(" ", "");
            _PostalCode = MerchantTribe.Web.Text.TrimToLength(temp, 16);
		}
	}
	public string CountryCode {
		get { return _CountryCode; }
        set { _CountryCode = MerchantTribe.Web.Text.TrimToLength(value, 2); }
	}

	public void WriteToXml(XmlTextWriter xw, string elementName)
	{
		xw.WriteStartElement(elementName);
		WriteToXml(xw);
		xw.WriteEndElement();
	}

	public void WriteToXml(XmlTextWriter xw)
	{
		XmlHelper.WriteIfNotEmpty(xw, "Line1", _Line1);
		XmlHelper.WriteIfNotEmpty(xw, "Line2", _Line2);
		XmlHelper.WriteIfNotEmpty(xw, "City", _City);
		XmlHelper.WriteIfNotEmpty(xw, "StateOrProvinceCode", _StateOrProvinceCode);
		XmlHelper.WriteIfNotEmpty(xw, "PostalCode", _PostalCode);
		XmlHelper.WriteIfNotEmpty(xw, "CountryCode", _CountryCode);
	}

}


}
