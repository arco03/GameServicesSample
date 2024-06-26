using System;

namespace Network.PlayFab.Responses
{
    [Serializable]
    public class ApiResponse : ApiResponse<string> {}
    
    [Serializable]
    public class ApiResponse<T>
    {
        public bool success;
        public T data;
    }
}