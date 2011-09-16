using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BVSoftware.Cryptography
{
    public sealed class AesEncryption
    {
        private static byte[] IV = { 0x63, 0x49, 0x41, 0x2F, 0xCE, 0x44, 0xF1, 0x6E, 0x5A, 0x32, 0x05, 0xC4, 0x82, 0x93, 0x12, 0xF5};

        public static string Encode(string message, string key)
        {
            string result = string.Empty;

            try
            {
                if (message == null)
                {
                    throw new ArgumentNullException("message");
                }

                // Convert input to byte array
                byte[] messageBytes = Conversion.StringToBytes(message);
                byte[] keyBytes = Conversion.HexToByteArray(key);

                RijndaelManaged provider = new RijndaelManaged();                
                ICryptoTransform transform = provider.CreateEncryptor(keyBytes, IV);                
                MemoryStream stream = new MemoryStream();
                CryptoStream crypto = new CryptoStream(stream,transform, CryptoStreamMode.Write);
                crypto.Write(messageBytes,0,messageBytes.Length);
                crypto.FlushFinalBlock();
                                
                if (stream.Length > 0)
                {                   
                    result = Base64.ConvertToBase64(stream.ToArray());
                }                    
                crypto.Close();             
            }
            catch(Exception ex)
            {
                throw new ArgumentException("BVSoftware.Cryptography.AesEncryption: " + ex.Message);
            }

            return result;
        }

        public static string Decode(string encoded, string key)
        {
            string result = string.Empty;
                                                
            try
            {
                byte[] encodedBytes = Base64.ConvertFromBase64(encoded);
                byte[] keyBytes = Conversion.HexToByteArray(key);

                RijndaelManaged provider = new RijndaelManaged();
                ICryptoTransform transform = provider.CreateDecryptor(keyBytes,IV);
                MemoryStream stream = new MemoryStream();
                CryptoStream crypto = new CryptoStream(stream,transform,CryptoStreamMode.Write);                
                crypto.Write(encodedBytes,0,encodedBytes.Length);                                                                
                crypto.FlushFinalBlock();

                if (stream.Length > 0)
                {                
                    result = UTF8Encoding.UTF8.GetString(stream.GetBuffer(), 0, Convert.ToInt32(stream.Length));
                }

                if (crypto != null)
                {
                    crypto.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("BVSoftware.Payment.AesEncryption.Decode: " + ex.Message);
            }

            return result;
        }
    }
}
