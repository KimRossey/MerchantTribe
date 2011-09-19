using System;
using System.Data;
using System.Xml;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace MerchantTribe.Commerce.Membership
{
	public class UserQuestion
	{
        public string Bvin { get; set; }
        public long StoreId { get; set; }
        public DateTime LastUpdated { get; set; }		
		public string Name {get;set;}
		public UserQuestionType Type {get;set;}
		public Collection<UserQuestionOption> Values {get; private set;}
		public int Order {get;set;}

        public UserQuestion()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.Name = string.Empty;
            this.Type = UserQuestionType.FreeAnswer;
            this.Values = new Collection<UserQuestionOption>();
            this.Order = 0;
        }

        public void ReadValuesFromXml(string xml)
        {                   
            XmlSerializer xs = new XmlSerializer(typeof(Collection<UserQuestionOption>));
            System.IO.StringReader sr = new System.IO.StringReader(xml);
            try
            {
                this.Values = (Collection<UserQuestionOption>)xs.Deserialize(sr);
            }
            finally
            {
                this.Values = new Collection<UserQuestionOption>();
                sr.Close();
            }
        }
        public string WriteValuesToXml()
        {
            string result = string.Empty;

            XmlSerializer xs = new XmlSerializer(typeof(Collection<UserQuestionOption>));
            System.IO.StringWriter sw = new System.IO.StringWriter();
            try
            {
                xs.Serialize(sw, this.Values);
                result = sw.ToString();
            }
            finally
            {
                sw.Close();
            }

            return result;
        }

        

	}
}
