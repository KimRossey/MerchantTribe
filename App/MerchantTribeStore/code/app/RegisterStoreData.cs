﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for RegisterStoreData
/// </summary>
/// 
namespace MerchantTribeStore.app
{
    public class RegisterStoreData
    {        
        public int plan { get; set; }
        public string plandetails { get; set; }        
        public string cardnumber { get; set; }
        public string billingzipcode { get; set; }
        public int expmonth { get; set; }
        public int expyear { get; set; }
        public string cardholder { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string storename { get; set; }

        public RegisterStoreData()
        {
            plan = 0;
            plandetails = string.Empty;            
            cardnumber = string.Empty; // "4111-1111-1111-1111";
            billingzipcode = string.Empty; //"12345";
            email = string.Empty;
            password = string.Empty;
            storename = string.Empty;
            expmonth = 1;
            expyear = DateTime.Now.Year;
            cardholder = string.Empty;
        }
        
    }
}