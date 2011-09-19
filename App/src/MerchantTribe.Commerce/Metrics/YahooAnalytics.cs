using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Metrics
{
    public class YahooAnalytics
    {
        public static string RenderYahooTracker(MerchantTribe.Commerce.Orders.Order o, string accountId)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("<SCRIPT language=\"JavaScript\" type=\"text/javascript\">" + System.Environment.NewLine);
            sb.Append("<!-- Yahoo! Inc." + System.Environment.NewLine);
            sb.Append("window.ysm_customData = new Object();" + System.Environment.NewLine);
            sb.Append("window.ysm_customData.conversion = \"transId=" + o.OrderNumber + ",currency=USD,amount=" + o.TotalGrand + "\";" + System.Environment.NewLine);
            sb.Append("var ysm_accountid  = \"" + accountId + "\";" + System.Environment.NewLine);
            sb.Append("document.write(\"<SCR\" + \"IPT language='JavaScript' type='text/javascript' \"" + System.Environment.NewLine);
            sb.Append(" + \"SRC=//\" + \"srv1.wa.marketingsolutions.yahoo.com\" + " + System.Environment.NewLine);
            sb.Append("\"/script/ScriptServlet\" + \"?aid=\" + ysm_accountid " + System.Environment.NewLine);
            sb.Append(" + \"></SCR\" + \"IPT>\");" + System.Environment.NewLine);
            sb.Append("// -->" + System.Environment.NewLine);
            sb.Append("</SCRIPT>");

            return sb.ToString();
        }

    }
}
