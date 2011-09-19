
//using System;
//using System.Data;
//using System.Collections.Generic;

//namespace MerchantTribe.Commerce.Orders
//{
//    public class RMAItem
//    {

//        public string Bvin { get; set; }
//        public DateTime LastUpdated { get; set; }				
//        public string RMABvin {get;set;}
//        public long LineItemId {get;set;}
//        public string ItemDescription {get;set;}
//        public string ItemName {get;set;}
//        public string Note {get;set;}
//        public string Reason {get;set;}
//        public bool Replace {get;set;}
//        public int Quantity {get;set;}
//        public int QuantityReceived {get;set;}
//        public int QuantityReturnedToInventory {get;set;}

//        public RMAItem()
//        {
//            this.Bvin = string.Empty;
//            this.LastUpdated = DateTime.UtcNow;
//            this.RMABvin = string.Empty;
//            this.LineItemId = 0;
//            this.ItemName = string.Empty;
//            this.ItemDescription = string.Empty;
//            this.Note = string.Empty;
//            this.Reason = string.Empty;
//            this.Replace = false;
//            this.Quantity = 1;
//            this.QuantityReceived = 0;
//            this.QuantityReturnedToInventory = 0;

//        }

//        private Orders.LineItem _lineItem;
//        private Catalog.Product _product;		

//        //public Orders.LineItem LineItem {
//        //    get {
//        //        if (_lineItem == null) {
//        //            _lineItem = Orders.LineItem.FindByBvin(this.LineItemBvin);
//        //        }
//        //        return _lineItem;
//        //    }
//        //        // do nothing
//        //    set { }
//        //}

//        //public Catalog.Product Product {
//        //    get {
//        //        if (_product == null) {
//        //            _product = Catalog.Product.FindByBvin(this.LineItem.ProductId);
//        //        }
//        //        return _product;
//        //    }
//        //        // do nothing
//        //    set { }
//        //}

//        //public string ProductName {
//        //    get {
//        //        if (this.Product != null) {
//        //            return this.Product.ProductName;
//        //        }
//        //        else {
//        //            return this.ItemName;
//        //        }
//        //    }
//        //    set { this.ItemName = value; }
//        //}

//        //public string ProductSku {
//        //    get {
//        //        if (this.Product != null) {
//        //            return this.Product.Sku;
//        //        }
//        //        else {
//        //            return string.Empty;
//        //        }
//        //    }
//        //        // do nothing
//        //    set { }
//        //}

//        public static Orders.RMAItem ConvertDataRow(DataRow dr)
//        {
//            return Mapper.ConvertDataRow(dr);
//        }

//        public static bool Insert(Orders.RMAItem r)
//        {
//            return Mapper.Insert(r);
//        }

//        public static bool Update(Orders.RMAItem r)
//        {
//            return Mapper.Update(r);
//        }

//        public static List<RMAItem> FindByRMABvin(string bvin)
//        {
//            return Mapper.FindByRMABvin(bvin);
//        }

//        public static RMAItem FindByBvin(string bvin)
//        {
//            return Mapper.FindByBvin(bvin);
//        }

//        //public static List<RMAItem> FindByLineItemBvin(long itemId)
//        //{
//        //    return Mapper.FindByLineItemBvin(itemId);
//        //}

//        private class Mapper : Datalayer.MapperBase
//        {

//            public static Orders.RMAItem ConvertDataRow(DataRow dr)
//            {
//                Orders.RMAItem result = new Orders.RMAItem();
//                if (dr != null) {
//                    result.Bvin = (string)dr["bvin"];
//                    result.RMABvin = (string)dr["RMABvin"];
//                    result.LineItemId = (long)dr["LineItemId"];
//                    result.ItemDescription = (string)dr["ItemDescription"];
//                    result.ItemName = (string)dr["ItemName"];
//                    result.Note = (string)dr["Note"];
//                    result.Reason = (string)dr["Reason"];
//                    result.Replace = (bool)dr["Replace"];
//                    result.Quantity = (int)dr["Quantity"];
//                    result.QuantityReceived = (int)dr["QuantityReceived"];
//                    result.QuantityReturnedToInventory = (int)dr["QuantityReturnedToInventory"];
//                    result.LastUpdated = (DateTime)dr["LastUpdated"];
//                }

//                return result;
//            }

//            public static RMAItem FindByBvin(string bvin)
//            {
//                RMAItem result = null;
//                DataSet ds = SelectDataSetByBvin(bvin, "bvc_RMAItem_s");
//                if (ds != null) {
//                    if (ds.Tables.Count > 0) {
//                        if (ds.Tables[0].Rows.Count > 0) {
//                            result = ConvertDataRow(ds.Tables[0].Rows[0]);
//                        }
//                    }
//                }
//                return result;
//            }

//            public static List<RMAItem> FindByLineItemBvin(string bvin)
//            {
//                List<RMAItem> result = new List<RMAItem>();
//                DataSet ds = SelectDataSetByBvin(bvin, "bvc_RMAItem_ByLineItemBvin_s");
//                if (ds != null) {
//                    if (ds.Tables.Count > 0) {
//                        foreach (DataRow row in ds.Tables[0].Rows) {
//                            result.Add(ConvertDataRow(row));
//                        }
//                    }
//                }
//                return result;
//            }

//            public static List<RMAItem> FindByRMABvin(string bvin)
//            {
//                List<RMAItem> result = new List<RMAItem>();
//                DataSet ds = SelectDataSetByBvin(bvin, "bvc_RMAItem_ByRMABvin_s");
//                if (ds != null) {
//                    if (ds.Tables.Count > 0) {
//                        foreach (DataRow row in ds.Tables[0].Rows) {
//                            result.Add(ConvertDataRow(row));
//                        }
//                    }
//                }
//                return result;
//            }

//            public static bool Insert(Orders.RMAItem r)
//            {
//                if (r == null) {
//                    return false;
//                }
//                else {
//                    Datalayer.DataRequest request = new Datalayer.DataRequest();
//                    if (r.Bvin == string.Empty) {
//                        r.Bvin = System.Guid.NewGuid().ToString();
//                    }
//                    request.Command = "bvc_RMAItem_i";
//                    request.CommandType = CommandType.StoredProcedure;
//                    request.Transactional = false;
//                    AddParametersToRequest(r, request);
//                    return Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request);
//                }
//            }

//            public static bool Update(Orders.RMAItem r)
//            {
//                Datalayer.DataRequest request = new Datalayer.DataRequest();
//                request.Command = "bvc_RMAItem_u";
//                request.CommandType = CommandType.StoredProcedure;
//                request.Transactional = false;
//                AddParametersToRequest(r, request);
//                return Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request);
//            }

//            private static void AddParametersToRequest(Orders.RMAItem r, Datalayer.DataRequest request)
//            {
//                request.AddParameter("@bvin", r.Bvin);
//                request.AddParameter("@RMABvin", r.RMABvin);
//                request.AddParameter("@LineItemId", r.LineItemId);
//                request.AddParameter("@ItemDescription", r.ItemDescription);
//                request.AddParameter("@ItemName", r.ItemName);
//                request.AddParameter("@Note", r.Note);
//                request.AddParameter("@Reason", r.Reason);
//                request.AddParameter("@Replace", r.Replace);
//                request.AddParameter("@Quantity", r.Quantity);
//                request.AddParameter("@QuantityReceived", r.QuantityReceived);
//                request.AddParameter("@QuantityReturnedToInventory", r.QuantityReturnedToInventory);
//                request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
//            }

//            public static bool Delete(string bvin)
//            {
//                return DeleteByBvin(bvin, "bvc_RMAItem_d");
//            }
//        }
//    }
//}
