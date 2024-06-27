using Cysharp.Threading.Tasks;
using Network.PlayFab.Data;
using Network.PlayFab.Responses;
using PlayFab;
using PlayFab.ClientModels;

namespace Network.PlayFab.Services
{
    public class PlayFabAuthService
    {
        private readonly PlayFabAuthData _playFabAuthData;

        public PlayFabAuthService(PlayFabAuthData playFabAuthData)
        {
            _playFabAuthData = playFabAuthData;
        }

        public void Initialize()
        {
            _playFabAuthData.InstanceAPI = new PlayFabClientInstanceAPI(PlayFabSettings.staticSettings);
        }
        
        #region Login & Register
        public async UniTask<ApiResponse> Login(string id)
        {
            LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                CustomId = id,
            };
            
            UniTaskCompletionSource<ApiResponse> taskCompletionSource = new UniTaskCompletionSource<ApiResponse>();
            
            _playFabAuthData.InstanceAPI.LoginWithCustomID(
                request,
                result =>
                {
                    ApiResponse response = HandleLoginSuccess(result);
                    taskCompletionSource.TrySetResult(response);
                },
                error =>
                {
                    ApiResponse response = HandleLoginFailed(error);
                    taskCompletionSource.TrySetResult(response);
                }
            );
            
            return await taskCompletionSource.Task;
        }
        
        public async UniTask<ApiResponse> Login(string email, string password)
        {
            LoginWithEmailAddressRequest request = new LoginWithEmailAddressRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email,
                Password = password,
            };
            
            UniTaskCompletionSource<ApiResponse> taskCompletionSource = new UniTaskCompletionSource<ApiResponse>();
            
            _playFabAuthData.InstanceAPI.LoginWithEmailAddress(
                request,
                result =>
                {
                    ApiResponse response = HandleLoginSuccess(result);
                    taskCompletionSource.TrySetResult(response);
                },
                error =>
                {
                    ApiResponse response = HandleLoginFailed(error);
                    taskCompletionSource.TrySetResult(response);
                }
            );
            
            return await taskCompletionSource.Task;
        }

        public async UniTask<ApiResponse> Register(string email, string password)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
            {
                TitleId = PlayFabSettings.TitleId,
                Email = email,
                Password = password,
            };

            UniTaskCompletionSource<ApiResponse> taskCompletionSource = new UniTaskCompletionSource<ApiResponse>();
            
            _playFabAuthData.InstanceAPI.RegisterPlayFabUser(
                request,
                result =>
                {
                    ApiResponse response = HandleRegisterSuccess(result);
                    taskCompletionSource.TrySetResult(response);
                },
                error =>
                {
                    ApiResponse response = HandleLoginFailed(error);
                    taskCompletionSource.TrySetResult(response);
                }
            );
            
            return await taskCompletionSource.Task;
        }
        
        #endregion
        
        #region Handlers
        private ApiResponse HandleLoginSuccess(LoginResult result)
        {
            _playFabAuthData.DataAPI =  new PlayFabDataInstanceAPI(_playFabAuthData.InstanceAPI.authenticationContext);
            
            ApiResponse apiResponse = new ApiResponse()
            {
                success = true,
                data = result.LastLoginTime.ToString()
            };
            
            return apiResponse;
        }
        
        private ApiResponse HandleRegisterSuccess(RegisterPlayFabUserResult result)
        {
            _playFabAuthData.DataAPI =  new PlayFabDataInstanceAPI(_playFabAuthData.InstanceAPI.authenticationContext);
            
            ApiResponse apiResponse = new ApiResponse()
            {
                success = true,
                data = result.Username
            };
            
            return apiResponse;
        }
        
        private ApiResponse HandleLoginFailed(PlayFabError error)
        {
            ApiResponse apiResponse = new ApiResponse()
            {
                success = false,
                data = error.GenerateErrorReport()
            };
            return apiResponse;
        }
        
        #endregion
    }
}