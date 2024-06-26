using System;
using System.Collections.Generic;

namespace Data.Mappers
{
    public static class TitleDataMapper
    {
        public static TitleData MapToTitleData(this Dictionary<string, string> data)
        {
            Type type = typeof(TitleData);
            TitleData titleData = Activator.CreateInstance(type) as TitleData;
            
            return titleData;
        }
    }
}