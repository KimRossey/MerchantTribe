using System;

namespace MerchantTribe.Commerce.Utilities
{

	public class DateHelpers
	{

		public static string MonthToString(int m)
		{
			string result = m.ToString();
			switch (m) {
				case 1:
					result = "Jan";
                    break;
				case 2:
					result = "Feb";
                    break;
				case 3:
					result = "Mar";
                    break;
				case 4:
					result = "Apr";
                    break;
				case 5:
					result = "May";
                    break;
				case 6:
					result = "Jun";
                    break;
				case 7:
					result = "Jul";
                    break;
				case 8:
					result = "Aug";
                    break;
				case 9:
					result = "Sep";
                    break;
				case 10:
					result = "Oct";
                    break;
				case 11:
					result = "Nov";
                    break;
				case 12:
					result = "Dec";
                    break;
			}
			return result;
		}

		public static string BVShortDate(DateTime d)
		{
			string result = string.Empty;
			result = MonthToString(d.Month) + "-" + d.Day.ToString();
			if (d.Year < DateTime.Now.Year) {
				string fullYear = d.Year.ToString();
				if (fullYear.Length > 2) {
					result = result + " " + fullYear.Substring(fullYear.Length - 2, 2);
				}
			}
			return result;
		}

	}
}
