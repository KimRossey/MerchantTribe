using System;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Utilities
{
	public class UrlRewriterParts
	{
		private string _url = string.Empty;
		private string _query = string.Empty;
		private bool _hasQuery = false;

		const int UrlConst = 0;
		const int QueryConst = 1;

		public string Url {
			get { return _url; }
			set { _url = value; }
		}

		public string Query {
			get { return _query; }
			set { _query = value; }
		}

		public bool HasQuery {
			get { return _hasQuery; }
			set { _hasQuery = value; }
		}

		public UrlRewriterParts(System.Uri url)
		{
			string decodedPathAndQuery = System.Web.HttpUtility.UrlDecode(url.PathAndQuery);
			string[] parts = decodedPathAndQuery.Split('?');

			if (parts.Length > 0) {
				parts[UrlConst] = parts[UrlConst].ToLower();
			}

			if (parts.Length > 1) {
				this.HasQuery = true;
			}

			this.Url = parts[UrlConst];
			if (this.HasQuery) {
				this.Query = parts[QueryConst];
			}
		}

	}
}
