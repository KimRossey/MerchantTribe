using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BVSoftware.AcumaticaTools
{
    public class IntegrationMapping
    {
        public string BVId { get; set; }
        public string BVExtra { get; set; }
        public string AcumaticaId { get; set; }

        public IntegrationMapping(string data)
        {
            Deserialize(data);
        }
        public IntegrationMapping()
        {
            BVId = string.Empty;
            BVExtra = string.Empty;
            AcumaticaId = string.Empty;
        }

        public string Serialize()
        {
            string output = BVId + "," + BVExtra + "," + AcumaticaId;
            return output;
        }
        public bool Deserialize(string data)
        {
            string[] parts = data.Split(',');
            if (parts.Length > 2)
            {
                BVId = parts[0];
                BVExtra = parts[1];
                AcumaticaId = parts[2];
                return true;
            }
            return false;
        }
    }
}
