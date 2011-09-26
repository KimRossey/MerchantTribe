using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Xml;
using System.IO;
using System.Text;
using MerchantTribe.CommerceDTO.v1.Catalog;

namespace MerchantTribe.Commerce.Catalog
{
    [Serializable]
	public class ProductReview
	{
        public string Bvin {get;set;}
        public long StoreId { get; set; }
        public DateTime LastUpdated {get;set;}
		public string UserID {get;set;}
		public string ProductBvin {get;set;}
		public DateTime ReviewDate {
			get { return ReviewDateUtc.ToLocalTime(); }
			set { ReviewDateUtc = value.ToUniversalTime(); }
		}
        public System.DateTime ReviewDateUtc {get;set;}
        public System.DateTime ReviewDateForTimeZone(TimeZoneInfo tz)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(ReviewDateUtc, tz);
        }
		public ProductReviewRating Rating {get;set;}
        public int RatingAsInteger
        {
            get { return (int)Rating; }
        }

		public int Karma {get;set;}
		public string Description {get;set;}
		public bool Approved {get;set;}
		public string ProductName {get;set;}

        public ProductReview()
        {
            this.Bvin = string.Empty;
            this.StoreId = 0;
            this.LastUpdated = DateTime.UtcNow;
            this.UserID = string.Empty;
            this.ProductBvin = string.Empty;
            this.ReviewDateUtc = DateTime.UtcNow;
            this.Rating = ProductReviewRating.ThreeStars;
            this.Karma = 0;
            this.Description = string.Empty;
            this.Approved = false;
            this.ProductName = string.Empty;
        }        
        
        //DTO
        public ProductReviewDTO ToDto()
        {
            ProductReviewDTO dto = new ProductReviewDTO();

            dto.Approved = this.Approved;
            dto.Bvin = this.Bvin;
            dto.Description = this.Description;
            dto.Karma = this.Karma;
            dto.ProductBvin = this.ProductBvin;
            dto.ProductName = this.ProductName;
            dto.Rating = (ProductReviewRatingDTO)((int)this.Rating);
            dto.ReviewDateUtc = this.ReviewDateUtc;
            dto.UserID = this.UserID;

            return dto;
        }
        public void FromDto(ProductReviewDTO dto)
        {
            if (dto == null) return;

            this.Approved = dto.Approved;
            this.Bvin = dto.Bvin;
            this.Description = dto.Description;
            this.Karma = dto.Karma;
            this.ProductBvin = dto.ProductBvin;
            this.ProductName = dto.ProductName;
            this.Rating = (ProductReviewRating)((int)dto.Rating);
            this.ReviewDateUtc = dto.ReviewDateUtc;
            this.UserID = dto.UserID;
        }

	}

}
