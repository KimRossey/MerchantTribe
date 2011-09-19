using System;

namespace MerchantTribe.Shipping.FedEx
{

    public class ScanInfo
    {

        private string _Description = string.Empty;
        private string _Status = string.Empty;
        private string _StatusDescription = string.Empty;
        private string _City = string.Empty;
        private string _State = string.Empty;
        private string _CountryCode = string.Empty;
        private DateTime _Date = DateTime.Now;
        private string _HoldAtLocation = string.Empty;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        public string StatusDescription
        {
            get { return _StatusDescription; }
            set { _StatusDescription = value; }
        }
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }
        public string State
        {
            get { return _State; }
            set { _State = value; }
        }
        public string CountryCode
        {
            get { return _CountryCode; }
            set { _CountryCode = value; }
        }
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }
        public string HoldAtLocation
        {
            get { return _HoldAtLocation; }
            set { _HoldAtLocation = value; }
        }

    }
}