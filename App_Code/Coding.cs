using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace RioManager.App_Code
{
    public class Coding
    {
        public static string Encrypt(string str)
        {
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            string key = WebConfig.key;
            string iv = WebConfig.iv;
            byte[] plainTextData = Encoding.Unicode.GetBytes(str);
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes(iv));
            ICryptoTransform transform = AES.CreateEncryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(plainTextData, 0, plainTextData.Length);
            return Convert.ToBase64String(outputData);
        }
        public static string Decrypt(string str)
        {
            byte[] cipherTextData = Convert.FromBase64String(str);
            RijndaelManaged AES = new RijndaelManaged();
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            string key = WebConfig.key;
            string iv = WebConfig.iv;
            byte[] keyData = MD5.ComputeHash(Encoding.Unicode.GetBytes(key));
            byte[] IVData = MD5.ComputeHash(Encoding.Unicode.GetBytes(iv));
            ICryptoTransform transform = AES.CreateDecryptor(keyData, IVData);
            byte[] outputData = transform.TransformFinalBlock(cipherTextData, 0, cipherTextData.Length);
            return Encoding.Unicode.GetString(outputData);
        }

        public static string stringToSHA512(string str)
        {
            SHA512 sha = new SHA512CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(str);
            source = sha.ComputeHash(source);
            str = Convert.ToBase64String(source);

            return str;
        }
    }
}