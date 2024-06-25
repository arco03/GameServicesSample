using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Network
{
    public class PlayFabAuthService : MonoBehaviour
    {
        
        public void CheckLoginStatus(string email, string password)
        {
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                Email = email,
                Password = password,
            };
            PlayFabClientAPI.LoginWithEmailAddress(request,
                // OnSuccess
                result =>
                {
                    
                },
                // OnError
                error =>
                {
                    
                });
        }
        
    }
}