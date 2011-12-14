using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content
{
    public class AreaData : Dictionary<string, string>
    {
        public string GetAreaContent(string areaName)
        {
            if (HasArea(areaName))
            {
                return this[areaName.ToLowerInvariant()];
            }
            return string.Empty;
        }

        public bool HasArea(string areaName)
        {
            return this.ContainsKey(areaName.ToLowerInvariant());
        }

        public void SetAreaContent(string areaName, string content)
        {
            if (HasArea(areaName))
            {
                this[areaName.ToLowerInvariant()] = content;
            }
            else
            {
                this.Add(areaName.ToLowerInvariant(), content);
            }
        }

    }
}
