
using System.Xml;

namespace MerchantTribe.Shipping.FedEx
{

    public class FedExError
    {

        private string _Code = string.Empty;
        private string _Message = string.Empty;
        private string _Type = string.Empty;

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        public string ErrorType
        {
            get { return _Type; }
            set { _Type = value; }
        }

        public FedExError()
        {

        }

        public FedExError(XmlNode n)
        {
            ParseNode(n);
        }

        public void ParseNode(XmlNode n)
        {
            _Code = XmlHelper.ParseInnerText(n, "Code");
            _Message = XmlHelper.ParseInnerText(n, "Message");
            _Type = XmlHelper.ParseInnerText(n, "Type");
        }

    }

}

