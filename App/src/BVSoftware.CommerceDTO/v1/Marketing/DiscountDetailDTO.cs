﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Marketing
{
    [DataContract]
    public class DiscountDetailDTO
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public decimal Amount { get; set; }

        public DiscountDetailDTO()
        {
            Id = new Guid();
            Description = string.Empty;
            Amount = 0;
        }
    }
}
