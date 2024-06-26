using System;
using Network.PlayFab.Data;
using Network.PlayFab.Responses;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Network.PlayFab.Services
{
    public class PlayFabAuthService : MonoBehaviour, IPlayFabService
    {
        #region Callbacks
        
        private Action<ApiResponse> _loginSuccess;
        private Action<ApiResponse> _loginFailed;

        public void AddCallbacks(Action<ApiResponse> loginSuccess, Action<ApiResponse> loginFailed)
        {
            _loginSuccess += loginSuccess;
            _loginFailed += loginFailed;
        }
        
        public void RemoveCallbacks(Action<ApiResponse> loginSuccess, Action<ApiResponse> loginFailed)
        {
            _loginSuccess -= loginSuccess;
            _loginFailed -= loginFailed;
        }
        
        #endregion
        
        [SerializeField] private PlayFabAuthData playFabAuthData;

        public void InitializeService()
        {
            playFabAuthData.InstanceAPI = new PlayFabClientInstanceAPI(PlayFabSettings.staticSettings);
        }
        
        #region Login & Register
        public void Login(string id)
        {
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = id,
                CreateAccount = true
            };
            
            playFabAuthData.InstanceAPI.LoginWithCustomID(
                request,
                HandleLoginSuccess,
                HandleLoginFailed
            );
        }
        
        public void Login(string email, string password)
        {
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email,
                Password = password,
            };
            
            playFabAuthData.InstanceAPI.LoginWithEmailAddress(
                request,
                HandleLoginSuccess,
                HandleLoginFailed
            );
        }

        public void Register(string email, string password)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email,
                Password = password,
            };
            
            playFabAuthData.InstanceAPI.RegisterPlayFabUser(
                request,
                HandleRegisterSuccess,
                HandleLoginFailed
            );
        }
        
        #endregion
        
        #region Handlers
        private void HandleLoginSuccess(LoginResult result)
        {
            playFabAuthData.DataAPI =  new PlayFabDataInstanceAPI(playFabAuthData.InstanceAPI.authenticationContext);
            
            ApiResponse apiResponse = new ApiResponse()
            {
                success = true,
                data = "Logged in successfully"
            };
            
            _loginSuccess?.Invoke(apiResponse);
        }
        
        private void HandleRegisterSuccess(RegisterPlayFabUserResult result)
        {
            playFabAuthData.DataAPI =  new PlayFabDataInstanceAPI(playFabAuthData.InstanceAPI.authenticationContext);
            
            ApiResponse apiResponse = new ApiResponse()
            {
                success = true,
                data = "Registered successfully"
            };
            
            _loginSuccess?.Invoke(apiResponse);
        }
        
        private void HandleLoginFailed(PlayFabError error)
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                success = false,
                data = error.GenerateErrorReport()
            };
            
            _loginFailed?.Invoke(apiResponse);
        }
        
        #endregion
    }
}