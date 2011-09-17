using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Payment
{
    public class ResultData
    {
        public bool Succeeded { get; set; }

        public AvsResponseType AvsCode { get; set; }
        public string AvsCodeDescription { get; set; }
        public CvnResponseType CvvCode { get; set; }
        public string CvvCodeDescription { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseCodeDescription { get; set; }
        public List<Message> Messages { get; set; }
        public string ReferenceNumber { get; set; }
        public string ReferenceNumber2 { get; set; }
        public decimal BalanceAvailable { get; set; }
        public int PointsAvailable { get; set; }

        #region Helper Properties

        public List<Message> Warnings
        {
            get
            {
                List<Message> result = new List<Message>();
                foreach (Message m in Messages)
                {
                    if (m.Severity == MessageType.Warning)
                    {
                        result.Add(m);
                    }
                }
                return result;
            }
        }
        public bool HasWarnings { get { return (Warnings.Count > 0); } }
        public List<Message> Errors
        {
            get
            {
                List<Message> result = new List<Message>();
                foreach (Message m in Messages)
                {
                    if (m.Severity == MessageType.Error)
                    {
                        result.Add(m);
                    }
                }
                return result;
            }
        }
        public bool HasErrors { get { return (Errors.Count > 0); } }

        #endregion

        public ResultData()
        {
            Succeeded = false;
            AvsCode = AvsResponseType.Unavailable;
            AvsCodeDescription = string.Empty;
            CvvCode = CvnResponseType.Unavailable;
            CvvCodeDescription = string.Empty;
            ResponseCode = string.Empty;
            ResponseCodeDescription = string.Empty;
            Messages = new List<Message>();
            ReferenceNumber = string.Empty;
            ReferenceNumber2 = string.Empty;
            BalanceAvailable = 0;
            PointsAvailable = 0;
        }
    }
}
