using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace MerchantTribe.Commerce.Datalayer
{

	public class CacheManager
	{

		private static Dictionary<string, byte> products = new Dictionary<string, byte>();
		private static Dictionary<string, byte> strings = new Dictionary<string, byte>();

		public static string GetStringFromCache(string key)
		{
			if (HttpContext.Current == null) {
				return null;
			}
			else {
				return (string)HttpContext.Current.Cache.Get(key);
			}
		}

		public static void AddStringToCache(string key, string value, int minutes)
		{
			if ((HttpContext.Current != null)) {
				HttpContext.Current.Cache.Insert(key, value, null, 
                                                DateTime.Now.AddMinutes(minutes), 
                                                System.Web.Caching.Cache.NoSlidingExpiration, 
                                                System.Web.Caching.CacheItemPriority.Normal, 
                                                onStringRemovedCallBack);                
                                        
				lock(strings) {
					if (!strings.ContainsKey(key)) {
						strings.Add(key, 0);
					}
				}
			}
		}

		public static void UpsertStringInCache(string key, string value, int minutes)
		{
			RemoveStringFromCache(key);
			AddStringToCache(key, value, minutes);
		}

		public static void RemoveStringFromCache(string key)
		{
			if ((HttpContext.Current != null)) {
				HttpContext.Current.Cache.Remove(key);
			}
		}

		public static void onProductRemovedCallBack(string key, object value, CacheItemRemovedReason reason)
		{
			lock (products) {
				products.Remove(key);
			}
		}

        public static void onStringRemovedCallBack(string key, object value, CacheItemRemovedReason reason)
		{
			lock (strings) {
				strings.Remove(key);
			}            
		}

	}
}
