namespace Data.Storage
{
    public interface IStorageData
    {
        T GetData<T>(string fileName);
        void SaveData<T>(string fileName, T data);
    }
}