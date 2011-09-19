using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MerchantTribe.Commerce.Contacts
{
    public class GravatarHelper
    {

        public static string GetGravatarUrlForEmail(string email)
        {
            return "http://www.gravatar.com/avatar/" + HashUserEmail(email) + ".jpg";
        }

        public static string GetGravatarUrlForEmailWithSize(string email, int size)
        {
            return GetGravatarUrlForEmail(email) + "?s=" + size.ToString();
        }

        public static string GetGravatarUrlForEmailWithSizeAndDefault(string email, int size, string defaultImageUrl)
        {
            return GetGravatarUrlForEmailWithSize(email, size) + "&d=" + System.Web.HttpUtility.UrlEncode(defaultImageUrl);
        }

        private static string HashUserEmail(string theEmail)
        {
            string email = theEmail.Trim().ToLower();
            System.Security.Cryptography.MD5CryptoServiceProvider md5Obj =
                new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bytesToHash = System.Text.Encoding.ASCII.GetBytes(email); 
            bytesToHash = md5Obj.ComputeHash(bytesToHash);
           string strResult = "";
            foreach (byte b in bytesToHash)
            {
                strResult += b.ToString("x2");
            }
            return strResult;
        }

    }
}
