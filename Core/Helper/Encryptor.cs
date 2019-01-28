using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Customs.Helper
{
    public class Encryptor
    {
        public static string PasswordKey = "kthcmskey123";
        public static string Encrypt(string plainText, string key)
        {
            try
            {
                string encryptText;

                RijndaelManaged rijndael = new RijndaelManaged()
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    BlockSize = 128,
                    Padding = PaddingMode.Zeros
                };
                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                            streamWriter.Flush();
                        }
                        encryptText = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                encryptor.Dispose();
                rijndael = null;
                return encryptText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string Decrypt(string encryptText, string key)
        {
            try
            {
                string plainText;
                byte[] cipherArray = Convert.FromBase64String(encryptText);
                RijndaelManaged rijndael = new RijndaelManaged()
                {
                    Key = Encoding.UTF8.GetBytes(key),
                    Mode = CipherMode.ECB,
                    BlockSize = 128,
                    Padding = PaddingMode.Zeros
                };
                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                using (MemoryStream memoryStream = new MemoryStream(cipherArray))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plainText = streamReader.ReadToEnd();
                            plainText = Regex.Replace(plainText, @"\t|\n|\r|\0", "");
                        }
                    }
                }
                rijndael = null;
                decryptor = null;
                return plainText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string EncryptPass(string strPWD)
        {
            if (string.IsNullOrEmpty(strPWD))
                return "";
            return Encrypt(strPWD, PasswordKey);
        }
        public static string DecryptPass(string strPWD)
        {
            try
            {
                if (string.IsNullOrEmpty(strPWD))
                    return "";
                if (strPWD.Length > 0)
                {
                    return Decrypt(strPWD, PasswordKey);
                }
            }
            catch (Exception ex)
            {
                return strPWD;
            }
            return "";
        }
    }
}