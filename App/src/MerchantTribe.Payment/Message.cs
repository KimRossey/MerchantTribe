using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class Message
    {
        public string Description { get; set; }
        public string Code { get; set; }
        public MessageType Severity { get; set; }

        public Message()
        {
            Description = string.Empty;
            Code = string.Empty;
            Severity = MessageType.Unknown;
        }

        public Message(string description, string code, MessageType severity)
        {
            Description = description;
            Code = code;
            Severity = severity;
        }

    }
}
