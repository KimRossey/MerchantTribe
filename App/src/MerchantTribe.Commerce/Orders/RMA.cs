//using System;
//using System.Data;
//using System.Collections.ObjectModel;
//using System.ComponentModel;

//namespace MerchantTribe.Commerce.Orders
//{
//    public class RMA
//    {
//        public string Bvin { get; set; }
//        public DateTime LastUpdated { get; set; }					       
//        public string OrderBvin {get;set;}
//        public string Name {get;set;}
//        public string EmailAddress {get;set;}
//        public string PhoneNumber {get;set;}			
//        public string Comments {get;set;}
//        public int Number {get; private set;}
//        public DateTime DateOfReturn {get;set;}

//        public RMA()
//        {
//            this.Bvin = string.Empty;
//            this.LastUpdated = DateTime.UtcNow;
//            this.OrderBvin = string.Empty;
//            this.Name = string.Empty;
//            this.EmailAddress = string.Empty;
//            this.PhoneNumber = string.Empty;
//            this.Comments = string.Empty;
//            this.Number = -1;
//            this.DateOfReturn = DateTime.UtcNow;
//        }

//        private Orders.Order _order;

//        private Collection<RMAItem> _items;
//        public Collection<RMAItem> Items
//        {
//            get
//            {
//                if (_items == null)
//                {
//                    _items = Orders.RMAItem.FindByRMABvin(this.Bvin);
//                }
//                return _items;
//            }
//        }
//        private RMAStatus _status = RMAStatus.Unsubmitted;
//        public RMAStatus Status {
//            get { return _status; }
//            set {
//                CheckForNotification(value, _status);
//                _status = value;
//            }
//        }

//        public string StatusText {
//            get {
//                switch (this.Status) {
//                    case RMAStatus.None:
//                        return "None";
//                    case RMAStatus.Closed:
//                        return "Closed";
//                    case RMAStatus.Open:
//                        return "Open";
//                    case RMAStatus.Pending:
//                        return "Pending";
//                    case RMAStatus.Unsubmitted:
//                        return "Unsubmitted";
//                    case RMAStatus.Rejected:
//                        return "Rejected";
//                    default:
//                        return "None";
//                }
//            }
//                // do nothing
//            set { }
//        }

		

//        //public Orders.Order Order {
//        //    get {
//        //        if (_order == null) {
//        //            _order = Orders.Order.FindByBvin(this.OrderBvin);
//        //        }
//        //        return _order;
//        //    }
//        //        // do nothing
//        //    set { }
//        //}

//        //public string OrderNumber {
//        //    get { return this.Order.OrderNumber; }
//        //        // do nothing
//        //    set { }
//        //}

//        //public decimal Amount {
//        //    get {
//        //        decimal val = 0m;
//        //        foreach (RMAItem item in this.Items) {
//        //            val += (item.Quantity * item.LineItem.AdjustedPrice);
//        //        }
//        //        return val;
//        //    }
//        //        // do nothing
//        //    set { }
//        //}

	
//        private void CheckForNotification(RMAStatus status, RMAStatus previousStatus)
//        {
//            if (this.EmailAddress != string.Empty) {
//                if ((status == RMAStatus.Open) & (previousStatus == RMAStatus.Pending)) {
//                    Content.EmailTemplate t = Content.EmailTemplate.FindByBvin(WebAppSettings.RMAAcceptedEmailTemplate);
//                    if (t != null) {
//                        if (t.Bvin != string.Empty) {
//                            try {
//                                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
//                                msg = t.ConvertToMailMessage(t.From, this.EmailAddress, this);
//                                if (msg != null) {
//                                    Utilities.MailServices.SendMail(msg);
//                                }
//                            }
//                            catch (Exception ex) {
//                                EventLog.LogEvent(ex);
//                            }
//                        }
//                    }
//                }
//                else if ((status == RMAStatus.Rejected) & (previousStatus == RMAStatus.Pending)) {
//                    Content.EmailTemplate t = Content.EmailTemplate.FindByBvin(WebAppSettings.RMARejectedEmailTemplate);
//                    if (t != null) {
//                        if (t.Bvin != string.Empty) {
//                            try {
//                                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
//                                msg = t.ConvertToMailMessage(t.From, this.EmailAddress, this);
//                                if (msg != null) {
//                                    Utilities.MailServices.SendMail(msg);
//                                }
//                            }
//                            catch (Exception ex) {
//                                EventLog.LogEvent(ex);
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        public Collection<Content.EmailTemplateTag> ReplacementTags()
//        {
//            Collection<Content.EmailTemplateTag> result = new Collection<Content.EmailTemplateTag>();
//            result.Add(new Content.EmailTemplateTag("[[RMA.EmailAddress]]", this.EmailAddress));
//            result.Add(new Content.EmailTemplateTag("[[RMA.Name]]", this.Name));
//            result.Add(new Content.EmailTemplateTag("[[RMA.Number]]", this.Number.ToString()));
//            result.Add(new Content.EmailTemplateTag("[[RMA.StatusText]]", this.StatusText));
//            //result.Add(new Content.EmailTemplateTag("[[RMA.OrderNumber]]", this.OrderNumber));
//            return result;
//        }

	
//        public static bool Insert(Orders.RMA r)
//        {
//            return Mapper.Insert(r);
//        }

//        public static bool Update(Orders.RMA r)
//        {
//            return Mapper.Update(r);
//        }

//        public static bool Delete(string bvin)
//        {
//            return Mapper.Delete(bvin);
//        }

//        public static bool Update(Orders.RMA r, bool AdministratorRequest)
//        {
//            return Mapper.Update(r, AdministratorRequest);
//        }

//        public static Orders.RMA FindByBvin(string bvin)
//        {
//            return Mapper.FindByBvin(bvin);
//        }

//        [DataObjectMethod(DataObjectMethodType.Select)]
//        public static Collection<Orders.RMA> FindAll(RMAStatus status, int startRowIndex, int maximumRows, ref int rowCount)
//        {
//            return Mapper.FindAll(status, startRowIndex, maximumRows,ref rowCount);
//        }

//        public static int GetRowCount(RMAStatus status, int rowCount)
//        {
//            return rowCount;
//        }

//        public static Collection<Orders.RMA> FindAll()
//        {
//            return Mapper.FindAll();
//        }

//        private class Mapper : Datalayer.MapperBase
//        {

//            public static Orders.RMA FindByBvin(string bvin)
//            {
//                Orders.RMA result = new Orders.RMA();
//                DataSet ds = SelectDataSetByBvin(bvin, "bvc_RMA_s");
//                if (ds != null) {
//                    if (ds.Tables.Count > 0) {
//                        if (ds.Tables[0].Rows.Count > 0) {
//                            result = ConvertDataRow(ds.Tables[0].Rows[0]);
//                        }
//                    }
//                }
//                return result;
//            }

//            public static Collection<Orders.RMA> FindAll()
//            {
//                int temp = -1;
//                return FindAll(RMAStatus.None, -1, -1,ref temp);
//            }

//            public static Collection<Orders.RMA> FindAll(RMAStatus status, int startRowIndex, int maximumRows, ref int rowCount)
//            {
//                Collection<Orders.RMA> result = new Collection<Orders.RMA>();
//                Datalayer.DataRequest request = new Datalayer.DataRequest();
//                request.Command = "bvc_RMA_All_s";
//                request.CommandType = CommandType.StoredProcedure;
//                request.Transactional = false;
//                request.AddParameter("@Status", status);
//                request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
//                DataSet ds = Datalayer.SqlDataHelper.ExecuteDataSet(request, startRowIndex, maximumRows,ref rowCount);
//                if (ds != null) {
//                    if (ds.Tables.Count > 0) {
//                        if (ds.Tables[0].Rows.Count > 0) {
//                            foreach (DataRow row in ds.Tables[0].Rows) {
//                                result.Add(ConvertDataRow(row));
//                            }
//                        }
//                    }
//                }
//                return result;
//            }

//            private static Orders.RMA ConvertDataRow(DataRow dr)
//            {
//                Orders.RMA result = new Orders.RMA();
//                if (dr != null) {
//                    result.Bvin = (string)dr["bvin"];
//                    result.OrderBvin = (string)dr["OrderBvin"];
//                    result.Name = (string)dr["Name"];
//                    result.Number = (int)dr["Number"];
//                    result.EmailAddress = (string)dr["EmailAddress"];
//                    result.PhoneNumber = (string)dr["PhoneNumber"];
//                    result.Comments = (string)dr["Comments"];
//                    result.Status = (RMAStatus)dr["Status"];
//                    result.DateOfReturn = (DateTime)dr["DateOfReturn"];
//                    result.LastUpdated = (DateTime)dr["LastUpdated"];
//                }

//                return result;
//            }

//            public static bool Insert(Orders.RMA r)
//            {
//                if (r == null) {
//                    return false;
//                }
//                else {
//                    Datalayer.DataRequest request = new Datalayer.DataRequest();
//                    if (r.Bvin == string.Empty) {
//                        r.Bvin = System.Guid.NewGuid().ToString();
//                    }
//                    request.Command = "bvc_RMA_i";
//                    request.CommandType = CommandType.StoredProcedure;
//                    request.Transactional = false;
//                    AddParametersToRequest(r, request);
//                    request.AddParameter("@Number", -1, ParameterDirection.Output);
//                    if (Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request)) {
//                        foreach (Datalayer.DataRequestParameter param in request.OutputParameters) {
//                            if (param.ParamName == "@Number") {
//                                r.Number = (int)param.ParamValue;
//                            }
//                        }

//                        foreach (RMAItem item in r.Items) {
//                            item.RMABvin = r.Bvin;
//                            RMAItem.Insert(item);
//                        }
//                        return true;
//                    }
//                    else {
//                        return false;
//                    }
//                }
//            }

//            public static bool Update(Orders.RMA r)
//            {
//                return Update(r, false);
//            }

//            public static bool Update(Orders.RMA r, bool AdministratorRequest)
//            {
//                Datalayer.DataRequest request = new Datalayer.DataRequest();
//                request.Command = "bvc_RMA_u";
//                request.CommandType = CommandType.StoredProcedure;
//                request.Transactional = false;
//                AddParametersToRequest(r, request);
//                request.AddParameter("@AdministratorRequest", AdministratorRequest);
//                if (Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request)) {
//                    foreach (RMAItem item in r.Items) {
//                        item.RMABvin = r.Bvin;
//                        if (item.Bvin == string.Empty) {
//                            RMAItem.Insert(item);
//                        }
//                        else {
//                            RMAItem.Update(item);
//                        }
//                    }
//                    return true;
//                }
//                else {
//                    return false;
//                }
//            }

//            private static void AddParametersToRequest(Orders.RMA r, Datalayer.DataRequest request)
//            {
//                request.AddParameter("@bvin", r.Bvin);
//                request.AddParameter("@OrderBvin", r.OrderBvin);
//                request.AddParameter("@Name", r.Name);
//                request.AddParameter("@EmailAddress", r.EmailAddress);
//                request.AddParameter("@PhoneNumber", r.PhoneNumber);
//                request.AddParameter("@Comments", r.Comments);
//                request.AddParameter("@Status", r.Status);
//                request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
//            }

//            public static bool Delete(string bvin)
//            {
//                return DeleteByBvin(bvin, "bvc_RMA_d");
//            }
//        }
//    }
//}
