using System;

namespace MerchantTribe.Commerce.Utilities
{

	public class ImageInfo
	{

		private int _Width;
		private int _Height;
		private decimal _SizeInBytes = 0m;
		private string _FormattedSize = string.Empty;
		private string _FormattedDimensions = string.Empty;
		private string _PhysicalPath = string.Empty;
		private string _Url = string.Empty;

		public int Width {
			get { return _Width; }
			set { _Width = value; }
		}
		public int Height {
			get { return _Height; }
			set { _Height = value; }
		}
		public decimal SizeInBytes {
			get { return _SizeInBytes; }
			set { _SizeInBytes = value; }
		}
		public string FormattedSize {
			get { return _FormattedSize; }
			set { _FormattedSize = value; }
		}
		public string FormattedDimensions {
			get { return _FormattedDimensions; }
			set { _FormattedDimensions = value; }
		}
		public string PhysicalPath {
			get { return _PhysicalPath; }
			set { _PhysicalPath = value; }
		}
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings")]
		public string Url {
			get { return _Url; }
			set { _Url = value; }
		}

		public ImageInfo()
		{

		}

	}
}
