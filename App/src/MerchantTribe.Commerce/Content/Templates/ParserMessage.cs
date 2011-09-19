using System;

namespace MerchantTribe.Commerce.Content.Templates
{

	public enum ParserMessageType
	{
		Information = 0,
		Warning = 1,
		Error = 2
	}

	public class ParserMessage
	{

		private string _Message;
		private int _LineNumber = -1;
		private ParserMessageType _MessageType = ParserMessageType.Information;

		public string Message {
			get { return _Message; }
			set { _Message = value; }
		}
		public int LineNumber {
			get { return _LineNumber; }
			set { _LineNumber = value; }
		}
		public ParserMessageType MessageType {
			get { return _MessageType; }
			set { _MessageType = value; }
		}

		public ParserMessage()
		{

		}

		public ParserMessage(string message, int lineNumber, ParserMessageType type)
		{
			_Message = message;
			_LineNumber = lineNumber;
			_MessageType = type;
		}

	}
}

