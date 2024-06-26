using System;
using Data;
using Network.PlayFab.Data;
using Network.PlayFab.Responses;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Network.PlayFab.Services
{
    public class PlayFabTitleService : MonoBehaviour, IPlayFabService
    {
        #region Callbacks
        
        private Action<ApiResponse<TitleData>> _getTitleDataSuccess;
        private Action<ApiResponse> _getTitleDataFailed;
        
        public void AddCallbacks(
            Action<ApiResponse<TitleData>> getTitleDataSuccess,
            Action<ApiResponse> getTitleDataFailed)
        {
            _getTitleDataSuccess += getTitleDataSuccess;
            _getTitleDataFailed += getTitleDataFailed;
        }
        
        public void RemoveCallbacks(
            Action<ApiResponse<TitleData>> getTitleDataSuccess,
            Action<ApiResponse> getTitleDataFailed)
        {
            _getTitleDataSuccess -= getTitleDataSuccess;
            _getTitleDataFailed -= getTitleDataFailed;
        }
        
        #endregion
        
        [SerializeField] private PlayFabAuthData playFabAuthData;
        [SerializeField] private PlayFabTitleData playFabTitleData;
        
        private PlayFabClientInstanceAPI _instanceAPI;
        private PlayFabDataInstanceAPI _dataAPI;
        
        public void InitializeService()
        {
            _instanceAPI = playFabAuthData.InstanceAPI;
            _dataAPI = playFabAuthData.DataAPI;
        }
        
        #region Title Data
        
        public void GetTitleData()
        {
            GetTitleDataRequest request = new GetTitleDataRequest()
            {
                
            };
            
            _instanceAPI.GetTitleData(
                request,
                HandleGetTitleDataSuccess,
                HandleGetTitleDataFailed
            );
        }

        #endregion
        
        #region Handlers
        
        private void HandleGetTitleDataSuccess(GetTitleDataResult result)
        {
            ApiResponse<TitleData> data = new ApiResponse<TitleData>()
            {
                success = true,
                // data = 
            };
            _getTitleDataSuccess?.Invoke(data);
        }

        private void HandleGetTitleDataFailed(PlayFabError error)
        {
            ApiResponse data = new ApiResponse()
            {
                success = false,
                data = error.GenerateErrorReport()
            };
            _getTitleDataFailed?.Invoke(data);
        }

        #endregion
    }
}