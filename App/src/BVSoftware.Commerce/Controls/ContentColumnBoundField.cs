using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Globalization;

namespace BVSoftware.Commerce.Controls
{
    //public class ContentColumnBoundField : System.Web.UI.WebControls.BoundField
    //{

    //    protected override string FormatDataValue(object dataValue, bool encode)
    //    {
    //        if (dataValue is string) {
    //            Content.ContentColumn result = Content.ContentColumn.FindByBvin((string)dataValue);
    //            if (result != null) {
    //                string val;
    //                if ((encode)) {
    //                    val = HttpUtility.HtmlEncode(result.DisplayName);
    //                }
    //                else {
    //                    val = result.DisplayName;
    //                }
    //                if ((encode) && (this.DataFormatString.Length > 0)) {
    //                    return string.Format(CultureInfo.CurrentCulture, this.DataFormatString, new object[] { val });
    //                }
    //                else {
    //                    return val;
    //                }
    //            }
    //            else {
    //                return this.NullDisplayText;
    //            }
    //        }
    //        else {
    //            return this.NullDisplayText;
    //        }
    //    }
    //}
}
