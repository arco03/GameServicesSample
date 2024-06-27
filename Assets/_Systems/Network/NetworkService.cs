using Data;
using Data.Encrypt;
using Data.Storage;
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
        
        private IStorageData _storageData;
        
        [Inject] private PlayFabAuthService _playFabAuthService;
        [Inject] private PlayFabTitleService _playFabTitleService;
        
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