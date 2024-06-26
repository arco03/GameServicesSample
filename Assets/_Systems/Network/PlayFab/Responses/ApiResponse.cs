using System;

namespace Network.PlayFab.Responses
{
    [Serializable]
    public class ApiResponse : ApiResponse<string> {}
    
    [Serializable]
    public class ApiResponse<T>
    {
        public bool success;
        public string error;
        public T data;

        public static implicit operator bool(ApiResponse<T> response)
        {
            return response.success;
        }
        
        public static implicit operator T(ApiResponse<T> response)
        {
            return response.data;
        }
        
        public static implicit operator string(ApiResponse<T> response)
        {
            return !response.success
                ? response.error
                : response.data.ToString();
        }
        
    }
}