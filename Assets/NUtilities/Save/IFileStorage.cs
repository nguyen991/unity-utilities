using UnityEngine;

namespace NUtilities.Save
{
    public interface IFileStorage
    {
        void Save(string data, string fileName);
        string Get(string fileName);
        bool Delete(string fileName);
        bool Exists(string fileName);
    }
}
