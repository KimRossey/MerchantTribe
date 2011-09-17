using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MerchantTribe.Payment
{
    public class TextHelper
    {
        public static string XmlEncode(string code)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            XmlNode node = doc.CreateNode(XmlNodeType.Element, "xmlencoder", null);
            node.InnerText = code;
            return node.InnerXml;
        }


    }
}
