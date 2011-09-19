using System;
using System.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace MerchantTribe.Commerce.Content
{	
	public class CustomUrl
	{
        public string Bvin { get; set; }
        public DateTime LastUpdated { get; set; }		
		public bool IsPermanentRedirect {get;set;}
        public CustomUrlType SystemDataType {get;set;}
		public string SystemData {get;set;}
        public long StoreId {get;set;}

        public CustomUrl()
        {
            this.Bvin = string.Empty;
            this.LastUpdated = DateTime.UtcNow;
            this.IsPermanentRedirect = true;
            this.SystemDataType = CustomUrlType.General;
            this.SystemData = string.Empty;
            this.StoreId = 0;
        }

        private string _RequestedUrl = string.Empty;
        private string _RedirectToUrl = string.Empty;
        public string RequestedUrl
        {
            get { return _RequestedUrl; }
            set
            {
                if (value.Trim() != string.Empty)
                {
                    string trimValue = "/";
                    if (value.StartsWith(trimValue) == false)
                    {
                        _RequestedUrl = trimValue + value.TrimEnd(trimValue.ToCharArray());
                    }
                    else
                    {
                        _RequestedUrl = value.TrimEnd(trimValue.ToCharArray());
                    }
                }
                else
                {
                    _RequestedUrl = string.Empty;
                }
                _RequestedUrl = _RequestedUrl.ToLowerInvariant();
            }
        }
        public string RedirectToUrl
        {
            get { return _RedirectToUrl; }
            set
            {
                if (value.Trim() != string.Empty)
                {
                    string trimValue = "/";
                    if (value.StartsWith(trimValue) == false)
                    {
                        if (value.ToLower().StartsWith("http"))
                        {
                            _RedirectToUrl = value;
                        }
                        else
                        {
                            _RedirectToUrl = trimValue + value.TrimEnd(trimValue.ToCharArray());
                        }
                    }
                    else
                    {
                        _RedirectToUrl = value.TrimEnd(trimValue.ToCharArray());
                    }
                }
                else
                {
                    _RedirectToUrl = string.Empty;
                }
                _RedirectToUrl = _RedirectToUrl.ToLowerInvariant();
            }
        }        
                
    }
}
