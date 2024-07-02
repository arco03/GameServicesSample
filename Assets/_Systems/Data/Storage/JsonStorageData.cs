using System.IO;
using Zenject;

namespace Data.Storage
{
    public class JsonStorageData : IStorageData
    {
        [Inject] private DataConfiguration _dataConfiguration;
        
        public T GetData<T>() where T : new()
        {
            if (!File.Exists(_dataConfiguration.UserDataFile))
            {
                return default(T);
            }
            
            string json = File.ReadAllText(_dataConfiguration.UserDataFile);
            T data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            return data;
        }

        public void SaveData<T>(T data) where T : new()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            File.WriteAllText(_dataConfiguration.UserDataFile, json);
        }
    }
}