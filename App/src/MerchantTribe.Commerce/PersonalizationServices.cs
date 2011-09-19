using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using MerchantTribe.Commerce.Catalog;
using System.Data.Objects;
using System.Data.Objects.DataClasses;

namespace MerchantTribe.Commerce
{

    public class PersonalizationServices
    {

        private PersonalizationServices()
        {

        }

        public static void RecordProductViews(string bvin)
        {
            if (WebAppSettings.LastProductsViewedCookieName != string.Empty)
            {
                string SavedProductIDs = SessionManager.GetCookieString(WebAppSettings.LastProductsViewedCookieName);
                if (SavedProductIDs != string.Empty)
                {
                    string[] AllIDs = SavedProductIDs.Split(',');
                    System.Collections.Generic.Queue<string> q = new System.Collections.Generic.Queue<string>();
                    q.Enqueue(bvin);
                    foreach (string id in AllIDs)
                    {
                        if (q.Count < 10)
                        {
                            if (!q.Contains(id))
                            {
                                q.Enqueue(id);
                            }
                        }
                    }
                    SessionManager.SetCookieString(WebAppSettings.LastProductsViewedCookieName, string.Join(",", q.ToArray()));
                }
                else
                {
                    SessionManager.SetCookieString(WebAppSettings.LastProductsViewedCookieName, bvin);
                }
            }
        }

        public static List<Catalog.Product> GetProductsViewed(BVApplication bvapp)
        {
            string SavedProductIDs = SessionManager.GetCookieString(WebAppSettings.LastProductsViewedCookieName);
            List<Catalog.Product> result = new List<Catalog.Product>();
            if (SavedProductIDs != string.Empty)
            {
                string[] AllIDs = SavedProductIDs.Split(',');
                List<string> ids = new List<string>();
                foreach (string id in AllIDs)
                {
                    ids.Add(id);
                }
                result = bvapp.CatalogServices.Products.FindMany(ids);
            }
            return result;
        }


    }
}