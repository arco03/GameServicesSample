namespace Data.Storage
{
    public class JsonStorageData : IStorageData
    {
        public T GetData<T>(string fileName) where T : new()
        {
            return default(T);
        }

        public void SaveData<T>(string fileName, T data) where T : new()
        {
        }
    }
}