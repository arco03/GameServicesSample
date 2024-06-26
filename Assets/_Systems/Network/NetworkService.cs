using System;
using Data;
using Data.Encrypt;
using Data.Storage;
using Network.PlayFab.Responses;
using Network.PlayFab.Services;
using UnityEngine;

namespace Network
{
    public class NetworkService : MonoBehaviour
    {
        public string text;
        public string key;
        public UserAuthData userAuthData;
        
        private IStorageData _storageData;
        public PlayFabAuthService playFabAuthService;
        public PlayFabTitleService playFabTitleService;

        private void Awake()
        {
            playFabAuthService.AddCallbacks(HandleAuthSuccess, HandleGetTitleDataFailed);
            playFabTitleService.AddCallbacks(HandleGetTitleDataSuccess, HandleGetTitleDataFailed);
        }

        private void HandleAuthSuccess(ApiResponse obj)
        {
            playFabTitleService.InitializeService();
            playFabTitleService.GetTitleData();
        }

        private void HandleGetTitleDataFailed(ApiResponse obj)
        {
            Debug.Log(obj.data);
        }

        private void HandleGetTitleDataSuccess(ApiResponse<TitleData> obj)
        {
            Debug.Log(obj.data);
        }

        private void Start()
        {
            playFabAuthService.InitializeService();
            playFabAuthService.Login("CamiloGato");
        }

        [ContextMenu("Save")]
        public void SaveData()
        {
            _storageData = new PlayerPrefsStorageData();
            _storageData.SaveData<UserAuthData>("userDataAuth", userAuthData);
        }
        
        [ContextMenu("Load")]
        public void LoadData()
        {
            _storageData = new PlayerPrefsStorageData();
            userAuthData = _storageData.GetData<UserAuthData>("userDataAuth");
        }
        
        [ContextMenu("Decrypt")]
        public void Decrypt()
        {
            text = AesEncryptService.Decrypt(text, key);
        }

        [ContextMenu("Encrypt")]
        public void Encrypt()
        {
            text = AesEncryptService.Encrypt(text, key);
        }
    }
}