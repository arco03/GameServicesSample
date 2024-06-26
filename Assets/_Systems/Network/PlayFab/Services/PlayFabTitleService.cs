using System;
using Cysharp.Threading.Tasks;
using Data;
using Data.Mappers;
using Network.PlayFab.Data;
using Network.PlayFab.Responses;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Network.PlayFab.Services
{
    public class PlayFabTitleService : MonoBehaviour, IPlayFabService
    {
        
        [SerializeField] private PlayFabAuthData playFabAuthData;
        [SerializeField] private PlayFabTitleData playFabTitleData;
        
        private PlayFabClientInstanceAPI _instanceAPI;

        public void InitializeService()
        {
            _instanceAPI = playFabAuthData.InstanceAPI;
        }
        
        #region Title Data
        
        public async UniTask<ApiResponse<TitleData>> GetTitleData()
        {
            GetTitleDataRequest request = new GetTitleDataRequest();
            
            UniTaskCompletionSource<ApiResponse<TitleData>> taskCompletionSource = new UniTaskCompletionSource<ApiResponse<TitleData>>();
            
            if (_instanceAPI == null) throw new Exception("PlayFab Instance API is null");
            
            _instanceAPI.GetTitleData(
                request,
                result =>
                {
                    ApiResponse<TitleData> response = HandleGetTitleDataSuccess(result);
                    taskCompletionSource.TrySetResult(response);
                },
                error =>
                {
                    ApiResponse<TitleData> response = HandleGetTitleDataFailed(error);
                    taskCompletionSource.TrySetResult(response);
                }
            );
            
            return await taskCompletionSource.Task;
        }

        #endregion
        
        #region Handlers
        
        private ApiResponse<TitleData> HandleGetTitleDataSuccess(GetTitleDataResult result)
        {
            ApiResponse<TitleData> data = new ApiResponse<TitleData>()
            {
                success = true,
                data = result.Data.MapToType<TitleData>()
            };

            return data;
        }

        private ApiResponse<TitleData> HandleGetTitleDataFailed(PlayFabError error)
        {
            ApiResponse<TitleData> data = new ApiResponse<TitleData>()
            {
                success = false,
                error = error.GenerateErrorReport(),
            };
            
            return data;
        }

        #endregion
    }
}