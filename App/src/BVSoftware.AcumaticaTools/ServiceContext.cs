using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BVSoftware.AcumaticaTools.Acumatica;

namespace BVSoftware.AcumaticaTools
{
    public class ServiceContext
    {
        private StringBuilder log = new StringBuilder();

        public Screen Gate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SiteAddress { get; set; }
        public List<ServiceError> Errors { get; set; }
        public bool HasLoggedIn { get; set; }

        public string NewItemTaxAccountId { get; set; }
        public string NewItemIncomeAccountId { get; set; }
        public string NewItemItemClassId { get; set; }
        public string NewItemWarehouseId { get; set; }
        public string NewLineItemWarehouseId { get; set; }
        public List<IntegrationMapping> PaymentMappings { get; set; }
        public List<IntegrationMapping> ShippingMappings { get; set; }

        public bool UseFullCustomerNameInsteadOfAutoId { get; set; }
        public string DefaultCustomerPaymentMethod { get; set; }

		#region Screens Schemas
		private CR302000Content _CR302000_Schema;
		public CR302000Content CR302000_Schema
		{
			get
			{
				if (_CR302000_Schema == null) _CR302000_Schema = Gate.CR302000GetSchema();
				return _CR302000_Schema;
			}
		}
		private AR303000Content _AR303000_Schema;
		public AR303000Content AR303000_Schema
		{
			get
			{
				if (_AR303000_Schema == null) _AR303000_Schema = Gate.AR303000GetSchema();
				return _AR303000_Schema;
			}
		}
		private SO301000Content _SO301000_Schema;
		public SO301000Content SO301000_Schema
		{
			get
			{
				if (_SO301000_Schema == null) _SO301000_Schema = Gate.SO301000GetSchema();
				return _SO301000_Schema;
			}
		}
        private SO302000Content _SO302000_Schema;
        public SO302000Content SO302000_Schema
        {
            get
            {
                if (_SO302000_Schema == null) _SO302000_Schema = Gate.SO302000GetSchema();
                return _SO302000_Schema;
            }
        }

        private IN202000Content _IN202000_Schema;
        public IN202000Content IN202000_Schema
        {
            get
            {
                if (_IN202000_Schema == null) _IN202000_Schema = Gate.IN202000GetSchema();
                return _IN202000_Schema;
            }
        }       

		private IN202500Content _IN202500_Schema;
		public IN202500Content IN202500_Schema
		{
			get
			{
				if (_IN202500_Schema == null) _IN202500_Schema = Gate.IN202500GetSchema();
				return _IN202500_Schema;
			}
		}
		private TX205500Content _TX205500_Schema;
		public TX205500Content TX205500_Schema
		{
			get
			{
				if (_TX205500_Schema == null) _TX205500_Schema = Gate.TX205500GetSchema();
				return _TX205500_Schema;
			}
		}
		private TX206000Content _TX206000_Schema;
		public TX206000Content TX206000_Schema
		{
			get
			{
				if (_TX206000_Schema == null) _TX206000_Schema = Gate.TX206000GetSchema();
				return _TX206000_Schema;
			}
		}

		private CA204000Content _CA204000_Schema;
		public CA204000Content CA204000_Schema
		{
			get
			{
				if (_CA204000_Schema == null) _CA204000_Schema = Gate.CA204000GetSchema();
				return _CA204000_Schema;
			}
		}
		private CS207500Content _CS207500_Schema;
		public CS207500Content CS207500_Schema
		{
			get
			{
				if (_CS207500_Schema == null) _CS207500_Schema = Gate.CS207500GetSchema();
				return _CS207500_Schema;
			}
		}
		private IN204000Content _IN204000_Schema;
		public IN204000Content IN204000_Schema
		{
			get
			{
				if (_IN204000_Schema == null) _IN204000_Schema = Gate.IN204000GetSchema();
				return _IN204000_Schema;
			}
		}
		#endregion

        public ServiceContext()
        {
            Gate = new Screen();
            Errors = new List<ServiceError>();
        }

        public void LogMessage(string message)
        {
            log.Append(message);
            log.Append(System.Environment.NewLine);
        }
        public void ClearLog()
        {
            log.Clear();
        }
        public string GetLog()
        {
            return log.ToString();
        }

    }
}
