
using System.Xml;

namespace MerchantTribe.Shipping.FedEx
{

    public class XmlHelper
    {

        public static string ParseInnerText(XmlNode n, string nodeName)
        {
            string result = string.Empty;
            if (n != null)
            {
                if (n.SelectSingleNode(nodeName) != null)
                {
                    result = n.SelectSingleNode(nodeName).InnerText;
                }
            }
            return result;
        }

        public static bool ParseBoolean(XmlNode n, string nodeName)
        {
            bool result = false;

            string temp = "0";
            if (n != null)
            {
                if (n.SelectSingleNode(nodeName) != null)
                {
                    temp = n.SelectSingleNode(nodeName).InnerText;
                }
            }

            if (temp == "1")
            {
                result = true;
            }

            return result;
        }

        public static decimal ParseDecimal(XmlNode n, string nodeName)
        {
            decimal result = 0m;

            string temp = "0";
            if (n != null)
            {
                if (n.SelectSingleNode(nodeName) != null)
                {
                    temp = n.SelectSingleNode(nodeName).InnerText;
                }
            }

            decimal.TryParse(temp, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out result);

            return result;
        }

        public static void WriteIfNotEmpty(XmlTextWriter xw, string name, string value)
        {
            if (value.Trim().Length > 0)
            {
                xw.WriteElementString(name, value);
            }
        }

    }

}
