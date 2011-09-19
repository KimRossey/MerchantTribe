using System;

namespace MerchantTribe.Commerce.Utilities
{	
	public class MimeTypes
	{

		public static string FindTypeForExtension(string extension)
		{
			string result = "";

			if (extension != null) {
				switch (extension.ToLower()) {
					case ".bin":
					case ".exe":
						result = "application/octet-stream";
                        break;
					case ".htm":
					case ".html":
						result = "text/html";
                        break;
					case ".txt":
						result = "text/plain";
                        break;
					case ".doc":
						result = "application/ms-word";
                        break;
					case ".csv":
					case ".xls":
						result = "application/ms-excel";
                        break;
					case ".ppt":
						result = "application/ms-powerpoint";
                        break;
					case ".zip":
						result = "application/x-zip";
                        break;
					case ".css":
						result = "text/css";
                        break;
					case ".gif":
						result = "image/gif";
                        break;
					case ".jpeg":
					case ".jpg":
						result = "image/jpeg";
                        break;
					case ".png":
						result = "image/png";
                        break;
					case ".tif":
					case ".tiff":
						result = "image/tiff";
                        break;
					case ".pict":
						result = "image/x-pict";
                        break;
					case ".bmp":
						result = "image/x-ms-bmp";
                        break;
					case ".aif":
					case ".aiff":
					case ".aifc":
						result = "audio/x-aiff";
                        break;
					case ".wav":
						result = "audio/x-wav";
                        break;
					case ".mp3":
						result = "audio/x-mpeg-3";
                        break;
					case ".mpeg":
					case ".mpg":
					case ".mpe":
                        result = "video/mpeg"; 
                        break;
					case ".avi":
						result = "video/x-msvideo";
                        break;
					case ".qt":
					case ".mov":
						result = "video/quicktime";
                        break;
					case ".rtf":
						result = "application/rtf";
                        break;
					case ".pdf":
						result = "application/pdf";
                        break;
					case ".gtar":
						result = "application/x-gtar";
                        break;
					case ".tar":
						result = "application/x-tar";
                        break;
					case ".hqx":
						result = "application/mac-binhex40";
                        break;
					case ".sit":
					case ".sea":
						result = "application/x-stuffit";
                        break;
				}
			}


			return result;
		}

	}
}
