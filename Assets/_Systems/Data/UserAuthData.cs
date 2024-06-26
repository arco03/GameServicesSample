using System;

namespace Data
{
    [Serializable]
    public class UserAuthData
    {
        public bool rememberMe;
        public string customId;
        public string email;
    }
}