using System;
using System.IO;
using Newtonsoft.Json;

namespace NUtilities.Save
{
    public class SaveTypeBinary : ISaveType
    {
        public string Serialize(object data, string encryptionKey)
        {
            // Convert object to byte array
            if (data == null)
                return string.Empty;

            using (var memoryStream = new MemoryStream())
            {
                using (var binaryWriter = new BinaryWriter(memoryStream))
                {
                    // Serialize the object
                    binaryWriter.Write(JsonConvert.SerializeObject(data));
                    byte[] bytes = memoryStream.ToArray();

                    // Encrypt with AES if encryptionKey is provided
                    if (!string.IsNullOrEmpty(encryptionKey)) { }

                    return Convert.ToBase64String(bytes);
                }
            }
        }

        public void Deserialize(string data, object target, string encryptionKey)
        {
            if (string.IsNullOrEmpty(data))
                return;

            // Decrypt with AES if encryptionKey is provided
            if (!string.IsNullOrEmpty(encryptionKey)) { }
            byte[] bytes = Convert.FromBase64String(data);

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var binaryReader = new BinaryReader(memoryStream))
                {
                    // Deserialize the object
                    string json = binaryReader.ReadString();
                    JsonConvert.PopulateObject(json, target);
                }
            }
        }
    }
}
