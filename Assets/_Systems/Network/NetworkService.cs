using Data;
using Data.Encrypt;
using Data.Models;
using Data.Storage;
using Network.Azure.BlobStorage;
using Network.PlayFab.Responses;
using Network.PlayFab.Services;
using UnityEngine;
using Zenject;

namespace Network
{
    public class NetworkService : MonoBehaviour
    {
        public string text;
        public string key;
        public UserAuthData userAuthData;
        public TitleData titleData;
        
        [Inject] private IStorageData _storageData;
        [Inject] private PlayFabAuthService _playFabAuthService;
        [Inject] private PlayFabTitleService _playFabTitleService;
        [Inject] private StorageManager _storageManager;
        
        [ContextMenu("Test")]
        public async void Test()
        {
            _playFabAuthService.Initialize();
            ApiResponse login = await _playFabAuthService.Login("CamiloGato");
            if (!login)
            {
                Debug.Log(login);
                return;
            }
            
            _playFabTitleService.Initialize();
            ApiResponse<TitleData> data = await _playFabTitleService.GetTitleData();
            titleData = data.data;
            
            await _storageManager.DownloadFileAsync("titleData.json", "C:/Users/cmand/Documents/Git/MyPersonalGame/Assets/_Data/titleData.json");
        }
        
        [ContextMenu("Load")]
        public void LoadData()
        {
            userAuthData = _storageData.GetData<UserAuthData>();
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