namespace Data.Storage
{
    public interface IStorageData
    {
        T GetData<T>() where T : new();
        void SaveData<T>(T data) where T : new();
    }
}