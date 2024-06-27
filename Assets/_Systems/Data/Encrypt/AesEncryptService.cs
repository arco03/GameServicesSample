using System;
using System.IO;
using System.Security.Cryptography;

namespace Data.Encrypt
{
    public static class AesEncryptService
    {
        private static byte[] GetValidKey(string key, int keySize)
        {
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
            byte[] validKey = new byte[keySize];
            
            if (keyBytes.Length == keySize)
            {
                return keyBytes;
            }
            
            Array.Copy(keyBytes, validKey, Math.Min(keyBytes.Length, keySize));
            return validKey;
        }
        
        public static string Encrypt(string plainText, string key)
        {
            try
            {
                using Aes aesAlg = Aes.Create();

                byte[] validKey = GetValidKey(key, 32); // 32 bytes for AES-256

                aesAlg.Key = validKey;
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
            } catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Error encrypting text:\n {e}");
                return string.Empty;
            }
        }

        public static string Decrypt(string cipherText, string key)
        {
            try
            {
                byte[] fullCipherText = Convert.FromBase64String(cipherText);
                byte[] iv = new byte[16];
                byte[] cipher = new byte[fullCipherText.Length - iv.Length];

                Array.Copy(fullCipherText, iv, iv.Length);
                Array.Copy(fullCipherText, iv.Length, cipher, 0, cipher.Length);

                using Aes aesAlg = Aes.Create();

                byte[] validKey = GetValidKey(key, 32); // 32 bytes for AES-256

                aesAlg.Key = validKey;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new MemoryStream(cipher);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(csDecrypt);

                return srDecrypt.ReadToEnd();
            } catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Error decrypting text:\n {e}");
                return string.Empty;
            }
        }
    }
}