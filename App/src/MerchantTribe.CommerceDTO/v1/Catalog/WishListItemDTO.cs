using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MerchantTribe.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class WishListItemDTO
    {
        [DataMember]              
        public long Id { get; set; }
        [DataMember]              
        public long StoreId { get; set; }
        [DataMember]
        public string CustomerId { get; set; }
        [DataMember]              
        public System.DateTime LastUpdatedUtc { get; set; }                        
        [DataMember]              
        public string ProductId { get; set; }                
        [DataMember]              
        public int Quantity { get; set; }
        [DataMember]           
        public List<Catalog.OptionSelectionDTO> SelectionData { get; set; }           

        private void Init()
        {
            this.Id = 0;
            this.StoreId = 0;
            this.CustomerId = string.Empty;
            this.LastUpdatedUtc = DateTime.UtcNow;
            this.ProductId = string.Empty;
            this.Quantity = 1;
            this.SelectionData = new List<Catalog.OptionSelectionDTO>();
        }

        public WishListItemDTO()
        {
            Init();
        }

    }
}
