using System;
using System.Data;
using System.Collections.ObjectModel;

namespace MerchantTribe.Commerce.Catalog
{
    //[Serializable]
    //public class ProductFilter
    //{

    //    private string _name = string.Empty;
    //    private string _criteria = string.Empty;
    //    private string _page = string.Empty;
    //    private string _bvin = string.Empty;
    //    private System.DateTime _LastUpdated = System.DateTime.MinValue;
    //    public virtual string Bvin
    //    {
    //        get { return _bvin; }
    //        set { _bvin = value; }
    //    }
    //    public virtual System.DateTime LastUpdated
    //    {
    //        get { return _LastUpdated; }
    //        set { _LastUpdated = value; }
    //    }
    //    public string Name {
    //        get { return _name; }
    //        set { _name = value; }
    //    }
    //    public string Criteria {
    //        get { return _criteria; }
    //        set { _criteria = value; }
    //    }
    //    public string Page {
    //        get { return _page; }
    //        set { _page = value; }
    //    }
		
    //    public static bool Insert(ProductFilter item)
    //    {
    //        return Mapper.Insert(item);
    //    }

    //    public static bool Update(ProductFilter item)
    //    {
    //        return Mapper.Update(item);
    //    }

    //    public static bool Delete(string bvin)
    //    {
    //        return Mapper.Delete(bvin);
    //    }

    //    public static ProductFilter FindByBvin(string bvin)
    //    {
    //        return Mapper.FindByBvin(bvin);
    //    }

    //    public static ProductFilter FindByName(string name, string page)
    //    {
    //        return Mapper.FindByName(name, page);
    //    }

    //    public static Collection<ProductFilter> FindAllFilters(string page)
    //    {
    //        return Mapper.FindAllFilters(page);
    //    }

    //    private class Mapper : Datalayer.MapperBase
    //    {

    //        public static ProductFilter FindByBvin(string bvin)
    //        {
    //            DataTable dt = SelectByBvin(bvin, "bvc_ProductFilter_s");
    //            if (dt.Rows.Count > 0) {
    //                return ConvertDataRow(dt.Rows[0]);
    //            }
    //            else {
    //                return null;
    //            }
    //        }

    //        public static ProductFilter FindByName(string name, string page)
    //        {
    //            Datalayer.DataRequest request = new Datalayer.DataRequest();
    //            request.Command = "bvc_ProductFilter_ByName_s";
    //            request.CommandType = CommandType.StoredProcedure;
    //            request.Transactional = false;
    //            request.AddParameter("@name", name);
    //            request.AddParameter("@page", page);
    //            request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
    //            DataTable dt = Datalayer.SqlDataHelper.ExecuteDataSet(request).Tables[0];
    //            if (dt.Rows.Count > 0) {
    //                return ConvertDataRow(dt.Rows[0]);
    //            }
    //            else {
    //                return null;
    //            }
    //        }

    //        public static Collection<ProductFilter> FindAllFilters(string page)
    //        {
    //            Collection<ProductFilter> response = new Collection<ProductFilter>();
    //            Datalayer.DataRequest request = new Datalayer.DataRequest();
    //            request.Command = "bvc_ProductFilter_All_s";
    //            request.CommandType = CommandType.StoredProcedure;
    //            request.Transactional = false;
    //            request.AddParameter("@page", page);
    //            request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
    //            DataTable dt = Datalayer.SqlDataHelper.ExecuteDataSet(request).Tables[0];
    //            foreach (DataRow row in dt.Rows) {
    //                response.Add(ConvertDataRow(row));
    //            }
    //            return response;
    //        }

    //        protected static ProductFilter ConvertDataRow(DataRow row)
    //        {
    //            ProductFilter pf = new ProductFilter();
    //            pf.Bvin = (string)row["bvin"];
    //            pf.Name = (string)row["FilterName"];
    //            pf.Criteria = (string)row["Criteria"];
    //            pf.Page = (string)row["Page"];
    //            pf.LastUpdated = (DateTime)row["LastUpdated"];
    //            return pf;
    //        }

    //        public static bool Insert(ProductFilter item)
    //        {
    //            if (item.Bvin.Trim() == string.Empty) {
    //                item.Bvin = System.Guid.NewGuid().ToString();
    //            }
    //            Datalayer.DataRequest request = new Datalayer.DataRequest();
    //            request.Command = "bvc_ProductFilter_i";
    //            request.CommandType = CommandType.StoredProcedure;
    //            request.Transactional = false;
    //            AddParameters(item, request);
    //            return Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request);
    //        }

    //        public static bool Delete(string bvin)
    //        {
    //            Datalayer.DataRequest request = new Datalayer.DataRequest();
    //            request.Command = "bvc_ProductFilter_d";
    //            request.CommandType = CommandType.StoredProcedure;
    //            request.Transactional = false;
    //            request.AddParameter("@bvin", bvin);
    //            request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
    //            return Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request);
    //        }

    //        public static bool Update(ProductFilter item)
    //        {
    //            if (item.Bvin.Trim() == string.Empty) {
    //                item.Bvin = System.Guid.NewGuid().ToString();
    //            }
    //            Datalayer.DataRequest request = new Datalayer.DataRequest();
    //            request.Command = "bvc_ProductFilter_u";
    //            request.CommandType = CommandType.StoredProcedure;
    //            request.Transactional = false;
    //            AddParameters(item, request);
    //            return Datalayer.SqlDataHelper.ExecuteNonQueryAsBoolean(request);
    //        }

    //        protected static void AddParameters(ProductFilter item, Datalayer.DataRequest request)
    //        {
    //            request.AddParameter("@bvin", item.Bvin);
    //            request.AddParameter("@name", item.Name);
    //            request.AddParameter("@criteria", item.Criteria);
    //            request.AddParameter("@page", item.Page);
    //            request.AddParameter("@StoreId", CurrentRequestContext().CurrentStore.Id);
    //        }
    //    }
    //}
}
