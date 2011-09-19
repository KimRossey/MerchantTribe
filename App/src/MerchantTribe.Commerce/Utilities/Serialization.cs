using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Utilities
{	

	public class Serialization
	{
		public static string SerializeToXml(object item)
		{
			StringWriter sw = new StringWriter();
			XmlSerializer formatter = new XmlSerializer(item.GetType());
			formatter.Serialize(sw, item);
			return sw.ToString();
		}

		public static object DeserializeFromXml(string item, System.Type type)
		{
			StringReader tr = new StringReader(item);
			XmlSerializer formatter = new XmlSerializer(type);
			return formatter.Deserialize(tr);
		}
	}
}
