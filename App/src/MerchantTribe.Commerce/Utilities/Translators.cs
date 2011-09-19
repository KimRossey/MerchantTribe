
using System;
using System.Data;

namespace MerchantTribe.Commerce.Utilities
{
	
	public class Translators
	{

		private Translators()
		{
		}

		/// <summary>
		/// Converts a single column DataTable into an Array of Strings
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string[] DataTableToArray(DataTable dt)
		{
			int rowCnt = dt.Rows.Count;
			string[] arr = new string[rowCnt];

			if (dt.Columns.Count > 0) {
				int irow;
				for (irow = 0; irow <= dt.Rows.Count - 1; irow++) {
					DataRow dr = dt.Rows[irow];
					string str = string.Empty;
					str = dr[0].ToString();
					arr[irow] = str;
				}
			}

			return arr;
		}

	}

}
