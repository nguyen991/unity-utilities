using UnityEngine;

namespace NUtilities.Save
{
    public interface ISaveType
    {
        string Serialize(object data, string encryptionKey);
        void Deserialize(string data, object target, string encryptionKey);
    }
}
