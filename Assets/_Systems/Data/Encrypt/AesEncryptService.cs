using System;
using System.IO;
using System.Security.Cryptography;

namespace Data.Encrypt
{
    public class AesEncryptService
    {
        public static string Encrypt(string plainText, string key)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);

            using Aes aesAlg = Aes.Create();
            
            aesAlg.Key = keyBytes;
            aesAlg.GenerateIV();
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);


            using MemoryStream msEncrypt = new MemoryStream();
            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
            
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
            }
                    
            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Decrypt(string cipherText, string key)
        {
            byte[] fullCipherText = Convert.FromBase64String(cipherText);
            byte[] iv = new byte[16];
            byte[] cipher = new byte[16];

            Array.Copy(
                fullCipherText,
                iv,
                iv.Length
            );
            Array.Copy(
                fullCipherText,
                iv.Length,
                cipher,
                0,
                cipher.Length - iv.Length
            );
            
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            
            using Aes aesAlg = Aes.Create();
            
            aesAlg.Key = keyBytes;
            aesAlg.IV = iv;
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(cipher);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);
            
            return srDecrypt.ReadToEnd();
        }
        
    }
}