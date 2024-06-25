namespace Data.Storage
{
    public interface IStorageData
    {
        T GetData<T>(string fileName) where T : new();
        void SaveData<T>(string fileName, T data) where T : new();
    }
}