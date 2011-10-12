using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Content.Parts
{
    public static class PartHelper
    {
        public static string RenderEditTools(string partBvin)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"edittools\">");
            sb.Append("<a href=\"#\" class=\"deletepart\" id=\"dp" + partBvin + "\">&nbsp;</a>");            
            sb.Append("<a href=\"#\" class=\"sorthandle\">&nbsp;</a>");
            sb.Append("<a href=\"#\" class=\"edithandle\">&nbsp;</a>");            
            sb.Append("</div>");
            return sb.ToString();
        }
    }
}
