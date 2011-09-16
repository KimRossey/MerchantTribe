using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace BVSoftware.Cryptography
{
    public sealed class Hashing
    {
        public static string Md5Hash(string message)
        {            
            return Base64.ConvertToBase64(Md5HashToBytes(message));
        }

        public static byte[] Md5HashToBytes(string message)
        {
            //The string we wish to encrypt
            string plainText = message;

            //The array of bytes that will contain the encrypted value of strPlainText
            byte[] hashedDataBytes;

            //The encoder class used to convert strPlainText to an array of bytes
            UTF8Encoding encoder = new UTF8Encoding();

            //Create an instance of the MD5CryptoServiceProvider class
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            //Call ComputeHash, passing in the plain-text string as an array of bytes
            //The return value is the encrypted value, as an array of bytes
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(plainText));

            //Dispose of hasher            
            md5Hasher = null;

            return hashedDataBytes;
        }

        public static string Md5Hash(string message, string salt)
        {
            return Md5Hash(salt + message);
        }

    }
}
