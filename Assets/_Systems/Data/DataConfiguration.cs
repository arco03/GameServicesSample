using System.IO;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DataConfiguration", menuName = "Data/DataConfiguration", order = 0)]
    public class DataConfiguration : ScriptableObject
    {
        [SerializeField] private string userDataFile = "userData.json";
        
        public string PersistentDataPath => Application.persistentDataPath;
        public string UserDataFile => Path.Combine(PersistentDataPath, userDataFile);
    }
}