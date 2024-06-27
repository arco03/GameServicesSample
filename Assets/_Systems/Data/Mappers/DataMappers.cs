using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace Data.Mappers
{
    public static class DataMappers
    {
        public static T MapToType<T>(this Dictionary<string, string> data) where T : class
        {
            Type type = typeof(T);
            T titleData = Activator.CreateInstance(type) as T;
            
            FieldInfo[] fields = type.GetFields();
            
            foreach (FieldInfo field in fields)
            {
                string key = field.Name;
                Type fieldType = field.FieldType;

                if (!data.TryGetValue(key, out string keyValue))
                {
                    continue;
                }

                object value = ConvertFieldValue(fieldType, keyValue);
                field.SetValue(titleData, value);
            }
            
            return titleData;
        }

        private static object ConvertFieldValue(Type fieldType, string value)
        {
            // TODO: Change this method
            return 
                fieldType == typeof(string) || fieldType == typeof(int) ||
                fieldType == typeof(float) || fieldType == typeof(bool)
                    ? Convert.ChangeType(value, fieldType)
                    : JsonConvert.DeserializeObject(value, fieldType);
        }
    }
}