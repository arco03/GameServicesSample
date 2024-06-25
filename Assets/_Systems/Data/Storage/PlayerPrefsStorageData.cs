using System;
using System.Reflection;
using UnityEngine;

namespace Data.Storage
{
    public class PlayerPrefsStorageData : IStorageData
    {
        public T GetData<T>(string fileName) where T : new()
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            FieldInfo[] fields = type.GetFields();

            if (properties.Length >= 1)
            {
                throw new Exception($"{nameof(PlayerPrefsStorageData)} does not support properties.");
            }
            
            T data = new T();
            foreach (FieldInfo field in fields)
            {
                string key = field.Name;
                switch (field.FieldType.Name)
                {
                    case nameof(String):
                        field.SetValue(data, PlayerPrefs.GetString(key));
                        break;
                    case nameof(Boolean):
                        field.SetValue(data, PlayerPrefs.GetInt(key) == 1);
                        break;
                    case nameof(Int32):
                    case nameof(Int64):
                        field.SetValue(data, PlayerPrefs.GetInt(key, 0));
                        break;
                }
                
            }
            
            return data;
        }

        public void SaveData<T>(string fileName, T data) where T : new()
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            FieldInfo[] fields = type.GetFields();

            if (properties.Length >= 1)
            {
                throw new Exception($"{nameof(PlayerPrefsStorageData)} does not support properties.");
            }

            foreach (FieldInfo field in fields)
            {
                string key = field.Name;
                switch (field.FieldType.Name)
                {
                    case nameof(String):
                        PlayerPrefs.SetString(key, field.GetValue(data).ToString());
                        break;
                    case nameof(Boolean):
                        PlayerPrefs.SetInt(key, Convert.ToBoolean(field.GetValue(data)) ? 1 : 0);
                        break;
                    case nameof(Int32):
                    case nameof(Int64):
                        PlayerPrefs.SetInt(key, Convert.ToInt32(field.GetValue(data)));
                        break;
                }
            }
        }
    }
}