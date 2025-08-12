using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace NUtilities.Save
{
    public class SaveTypeJson : ISaveType
    {
        public string Serialize(object data, string encryptionKey)
        {
            if (data == null)
                return string.Empty;

            var json = JsonConvert.SerializeObject(data);

            // encrypt with aes if encryptionKey is provided
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                json = EncryptAES(json, encryptionKey);
            }

            return json;
        }

        public void Deserialize(string data, object target, string encryptionKey)
        {
            if (string.IsNullOrEmpty(data))
                return;

            // decrypt with aes if encryptionKey is provided
            if (!string.IsNullOrEmpty(encryptionKey))
            {
                data = DecryptAES(data, encryptionKey);
            }
            JsonConvert.PopulateObject(data, target);
        }

        private string EncryptAES(string plainText, string password)
        {
            try
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                using (Aes aes = Aes.Create())
                {
                    // Generate salt for key derivation
                    byte[] salt = new byte[16];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }

                    // Derive key from password using PBKDF2
                    using (
                        var pbkdf2 = new Rfc2898DeriveBytes(
                            password,
                            salt,
                            10000,
                            HashAlgorithmName.SHA256
                        )
                    )
                    {
                        aes.Key = pbkdf2.GetBytes(32); // 256-bit key
                        aes.IV = pbkdf2.GetBytes(16); // 128-bit IV
                    }

                    using (var encryptor = aes.CreateEncryptor())
                    using (var msEncrypt = new MemoryStream())
                    {
                        // Prepend salt to encrypted data
                        msEncrypt.Write(salt, 0, salt.Length);

                        using (
                            var csEncrypt = new CryptoStream(
                                msEncrypt,
                                encryptor,
                                CryptoStreamMode.Write
                            )
                        )
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }

                        return Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"AES Encryption failed: {ex.Message}");
                return string.Empty;
            }
        }

        private string DecryptAES(string cipherText, string password)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    // Extract salt from the beginning of cipher data
                    byte[] salt = new byte[16];
                    Array.Copy(cipherBytes, 0, salt, 0, salt.Length);

                    // Derive key from password using PBKDF2
                    using (
                        var pbkdf2 = new Rfc2898DeriveBytes(
                            password,
                            salt,
                            10000,
                            HashAlgorithmName.SHA256
                        )
                    )
                    {
                        aes.Key = pbkdf2.GetBytes(32); // 256-bit key
                        aes.IV = pbkdf2.GetBytes(16); // 128-bit IV
                    }

                    // Extract encrypted data (skip salt)
                    byte[] encryptedData = new byte[cipherBytes.Length - salt.Length];
                    Array.Copy(cipherBytes, salt.Length, encryptedData, 0, encryptedData.Length);

                    using (var decryptor = aes.CreateDecryptor())
                    using (var msDecrypt = new MemoryStream(encryptedData))
                    using (
                        var csDecrypt = new CryptoStream(
                            msDecrypt,
                            decryptor,
                            CryptoStreamMode.Read
                        )
                    )
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"AES Decryption failed: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
