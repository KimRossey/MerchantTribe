using System;
using System.IO;

namespace BVSoftware.Commerce.Utilities
{	
	public class TempFiles
	{

		public static FileInfo GetTemporaryFileInfo()
		{
			string tempFileName;
			FileInfo myFileInfo;
			try {
				tempFileName = Path.GetTempFileName();
				myFileInfo = new FileInfo(tempFileName);
				myFileInfo.Attributes = FileAttributes.Temporary;
			}
			catch (Exception e) {
				EventLog.LogEvent("Unable to create temporary file: {0}", e.Message, Metrics.EventLogSeverity.Error);
				return null;
			}
			return myFileInfo;
		}


	}
}
