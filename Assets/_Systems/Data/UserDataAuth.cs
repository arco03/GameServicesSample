using System;

namespace Data
{
    [Serializable]
    public class UserDataAuth
    {
        public bool rememberMe;
        public string customId;
        public string email;
    }
}