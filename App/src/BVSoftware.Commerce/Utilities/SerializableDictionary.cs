using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BVSoftware.Commerce.Utilities
{

	[XmlRoot("dictionary")]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
#region "IXmlSerializable Members"

		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
		{
			return null;
		}

		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

			bool wasEmpty = reader.IsEmptyElement;
			reader.Read();

			if ((wasEmpty)) {
				return;
			}

			while (reader.NodeType != System.Xml.XmlNodeType.EndElement) {

				reader.ReadStartElement("item");
				reader.ReadStartElement("key");

				TKey key = (TKey)keySerializer.Deserialize(reader);
				reader.ReadEndElement();
				reader.ReadStartElement("value");

				TValue value = (TValue)valueSerializer.Deserialize(reader);

				reader.ReadEndElement();
				this.Add(key, value);

				reader.ReadEndElement();

				reader.MoveToContent();
			}

			reader.ReadEndElement();
		}

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

			foreach (TKey key in this.Keys) {
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");

				keySerializer.Serialize(writer, key);

				writer.WriteEndElement();

				writer.WriteStartElement("value");

				TValue value = this[key];

				valueSerializer.Serialize(writer, value);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}

#endregion

	}
}
