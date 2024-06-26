using System;

namespace Data
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