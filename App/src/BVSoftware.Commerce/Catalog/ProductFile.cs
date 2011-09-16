using System;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Security;
using System.Security.Permissions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using BVSoftware.CommerceDTO.v1.Catalog;
using System.Linq;

namespace BVSoftware.Commerce.Catalog
{
    [Serializable]
	public class ProductFile
	{

        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }
        public long StoreId {get;set;}
		public string ProductId {get;set;}
        public int AvailableMinutes {get;set;}
        public int MaxDownloads {get;set;}
		public string FileName {get;set;}
		public string ShortDescription {get;set;}

        public string CombinedDisplay
        {
            get { return ShortDescription + " [" + FileName + "]"; }
        }

        public ProductFile()
        {
            this.StoreId = 0;
            this.ProductId = string.Empty;
            this.AvailableMinutes = 0;
            this.MaxDownloads = 0;
            this.FileName = string.Empty;
            this.ShortDescription = string.Empty;
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
        }     

        //DTO
        public ProductFileDTO ToDto()
        {
            ProductFileDTO dto = new ProductFileDTO();

            dto.AvailableMinutes = this.AvailableMinutes;
            dto.Bvin = this.Bvin;
            dto.FileName = this.FileName;
            dto.LastUpdated = this.LastUpdated;
            dto.MaxDownloads = this.MaxDownloads;
            dto.ProductId = this.ProductId;
            dto.ShortDescription = this.ShortDescription;
            dto.StoreId = this.StoreId;
            
            return dto;
        }
        public void FromDto(ProductFileDTO dto)
        {
            if (dto == null) return;

            this.AvailableMinutes = dto.AvailableMinutes;
            this.Bvin = dto.Bvin;
            this.FileName = dto.FileName;
            this.LastUpdated = dto.LastUpdated;
            this.MaxDownloads = dto.MaxDownloads;
            this.ProductId = dto.ProductId;
            this.ShortDescription = dto.ShortDescription;
            this.StoreId = dto.StoreId;
        }

        // File Management
        public static bool SaveFile(long storeId, string fileid, string fileName, HttpPostedFile file)
		{
            string diskFileName = fileid + "_" + fileName + ".config";
            Storage.DiskStorage.FileVaultUpload(storeId, diskFileName, file);            
			return true;
		}
        public static bool SaveFile(long storeId, string fileid, string fileName, System.IO.FileStream stream)
        {
            string diskFileName = fileid + "_" + fileName + ".config";
            Storage.DiskStorage.FileVaultUpload(storeId, diskFileName, stream);
            return true;
        }
        public static bool SaveFile(long storeId, string fileId, string fileName, byte[] fileData)
        {
            string diskFileName = fileId + "_" + fileName + ".config";
            Storage.DiskStorage.FileVaultUpload(storeId, diskFileName, fileData);
            return true;
        }
		private static void ReadWriteStream(System.IO.Stream readStream, System.IO.Stream writeStream)
		{
			int length = 256;
			byte[] buffer = new byte[length];
			int bytesRead = readStream.Read(buffer, 0, length);
			while ((bytesRead > 0)) {
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, length);
			}
			readStream.Close();
			writeStream.Close();
		}
        public static System.IO.FileStream GetPhysicalFile(ProductFile f, string path)
        {            
            string diskFileName = f.Bvin + "_" + f.FileName + ".config";
            return Storage.DiskStorage.FileVaultGetStream(f.StoreId, diskFileName);
        }

        // Time Helper
		public void SetMinutes(int Months, int Days, int Hours, int Minutes)
		{
			int TotalMinutes = 0;
			TotalMinutes += (Months * 43200);
			TotalMinutes += (Days * 1440);
			TotalMinutes += (Hours * 60);
			TotalMinutes += (Minutes);

			this.AvailableMinutes = TotalMinutes;
		}
        		
	}
}

