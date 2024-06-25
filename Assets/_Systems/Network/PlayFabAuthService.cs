using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Network
{
    public class PlayFabAuthService : MonoBehaviour
    {

        public void Login(string id)
        {
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                CustomId = id,
            };
            PlayFabClientAPI.LoginWithCustomID(request,
                // OnSuccess
                result =>
                {
                    
                },
                // OnError
                error =>
                {
                    
                }
            );
        }
        
        public void Login(string email, string password)
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
                    
                }
            );
        }

        public void Logout()
        {
            
        }
    }
}