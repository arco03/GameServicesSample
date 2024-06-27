using System;

namespace Data.Models
{
    [Serializable]
    public class TitleData
    {
        public GameData gameData;
    }

    [Serializable]
    public class GameData
    {
        public string titleKey;
        public string message;
    }
}