using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace BVSoftware.Cryptography
{
    public class TripleDesEncryption
    {
        private byte[] _KeyBytes = new byte[24]; //23 in vb
        private byte[] _IV = new byte[8]; //7 in vb

        // BV 5 Default Key
        private string _Key = "EDBE6BF8A92A417cBCD3DB23120861B5DE780BA44DB44166888707607A2A16FBBADFD3E111D54396A5701CE43E0EC3FFAE5543370AF54228B65CB87D7E346048";

        public TripleDesEncryption()
        {
            byte[] arrKey = new byte[_Key.Length];
            arrKey = Conversion.StringToBytes(_Key);
            byte[] arrHash = new byte[arrKey.Length];
            arrHash = ConvertToHash(arrKey);
            if (!SetKeys(arrHash))
            {
                throw new ArgumentException("Triple DES Encryption Key Failed to Set");
            }
        }

        public TripleDesEncryption(string requestedKey)
        {
            if (requestedKey != null)
            {
                _Key = requestedKey;
            }
            byte[] arrKey = new byte[_Key.Length];
            arrKey = Conversion.StringToBytes(_Key);
            byte[] arrHash = new byte[arrKey.Length];
            arrHash = ConvertToHash(arrKey);
            if (!SetKeys(arrHash))
            {
                throw new ArgumentException("Triple DES Encryption Key Failed to Set");
            }
        }

        public string Encode(string message)
        {
            string sOutput = string.Empty;

            try
            {
                if (message == null)
                {
                    throw new ArgumentNullException("message");
                }

                // Convert input to byte array
                byte[] arrInput = new byte[message.Length];
                arrInput = Conversion.StringToBytes(message);

                TripleDESCryptoServiceProvider TripleDESProvider;

                ICryptoTransform TripleDESEncryptor;
                CryptoStream TripleDESStream;
                MemoryStream outStream;
                TripleDESProvider = new TripleDESCryptoServiceProvider();
                TripleDESEncryptor = TripleDESProvider.CreateEncryptor(_KeyBytes, _IV);
                outStream = new MemoryStream();
                TripleDESStream = new CryptoStream(outStream, TripleDESEncryptor, CryptoStreamMode.Write);
                TripleDESStream.Write(arrInput, 0, arrInput.Length);
                TripleDESStream.FlushFinalBlock();

                if (outStream.Length == 0)
                {
                    sOutput = "";
                } else {
                    sOutput = Base64.ConvertToBase64(outStream.ToArray());
                }

                TripleDESStream.Close();
            }
            catch{
            }

            return sOutput;
        }

        public string Decode(string message)
        {
            string sOutput = string.Empty;

            byte[] arrInput;
            TripleDESCryptoServiceProvider TripleDESProvider;
            ICryptoTransform TripleDESDecryptor;
            CryptoStream TripleDESStream  = null;
            MemoryStream outStream;

            try
            {
                arrInput = Base64.ConvertFromBase64(message);
                TripleDESProvider = new TripleDESCryptoServiceProvider();
                TripleDESDecryptor = TripleDESProvider.CreateDecryptor(_KeyBytes, _IV);
                outStream = new MemoryStream();
                TripleDESStream = new CryptoStream(outStream, TripleDESDecryptor, CryptoStreamMode.Write);
                TripleDESStream.Write(arrInput, 0, arrInput.Length);
                TripleDESStream.FlushFinalBlock();

                if (outStream.Length == 0)
                {
                    sOutput = "";
                } else {                
                    sOutput = ASCIIEncoding.ASCII.GetString(outStream.GetBuffer(), 0, Convert.ToInt32(outStream.Length));
                }
        }
            catch{
            }

            return sOutput;
        }
              
        private static byte[] ConvertToHash(byte[] arraryInput)
        {            
            byte[] arrOutput;
            SHA256Managed sha = new SHA256Managed();
            arrOutput = sha.ComputeHash(arraryInput);
            return arrOutput;
        }

        private bool SetKeys(byte[] arraryHash)
        {
            if (arraryHash.Length < 32)
            {
                throw new ArgumentOutOfRangeException("Encryption Key Length is Too Short");
            }
            
            try
            {
                int i = 0;
                for (i = 0;i < 8;i++)
                {
                    _IV[i] = arraryHash[i];
                }
                for (i = 8;i < 32;i++)
                {
                    _KeyBytes[i - 8] = arraryHash[i];
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}
