using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MerchantTribe.Commerce;
using MerchantTribeStore.app;

namespace MerchantTribeStore.Areas.signup.Models
{
    public class RegisterViewModel
    {
        public RegisterStoreData RegistrationData { get; set; }
        public bool HideCreditCard { get; set; }
        public bool Agreed { get; set; }
        public bool FromHomePage { get; set; }
        public List<string> InvalidFields { get; set; }
        public string PlanName { get; set; }

        public RegisterViewModel()
        {
            this.RegistrationData = new RegisterStoreData();
            this.HideCreditCard = false;
            this.Agreed = false;
            this.FromHomePage = false;
            this.InvalidFields = new List<string>();
            this.PlanName = string.Empty;
        }
    }
}