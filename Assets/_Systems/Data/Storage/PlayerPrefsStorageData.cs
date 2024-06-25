using System;
using System.Reflection;

namespace Data.Storage
{
    public class PlayerPrefsStorageData : IStorageData
    {
        public T GetData<T>(string fileName)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            FieldInfo[] fields = type.GetFields();
            
            foreach (PropertyInfo property in properties)
            {
                UnityEngine.Debug.Log(property.Name);
            }
            
            foreach (FieldInfo field in fields)
            {
                UnityEngine.Debug.Log(field.Name);
            }
            
            return default(T);
        }

        public void SaveData<T>(string fileName, T data)
        {
            
        }
    }
}