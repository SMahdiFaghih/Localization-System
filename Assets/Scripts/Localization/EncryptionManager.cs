using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Localization
{
    public class EncryptionManager : MonoBehaviour
    {
        private static readonly byte[] Salt = new byte[] { 0x5A, 0x3B, 0x7F, 0x2E, 0x1D, 0xC, 0x4A, 0x6F };

        public static void EncryptString(string plainText)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes("Password12345");
            byte[] saltBytes = Salt;

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    File.WriteAllText("Assets/Resources/Localization/Localization-Encrypted.txt", Convert.ToBase64String(encryptedBytes));
                }
            }
        }

        public static string DecryptString(string encryptedText)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes("Password12345");
            byte[] saltBytes = Salt;
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;

                var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}