using System;

namespace Data.Models
{
    [Serializable]
    public class UserAuthData
    {
        public bool rememberMe;
        public string customId;
        public string email;
    }
}