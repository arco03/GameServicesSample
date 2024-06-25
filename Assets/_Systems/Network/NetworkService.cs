using Data;
using Data.Encrypt;
using Data.Storage;
using UnityEngine;

namespace Network
{
    public class NetworkService : MonoBehaviour
    {
        public string text;
        public string key;
        public UserDataAuth userDataAuth;
        
        private IStorageData _storageData;
        
        
        [ContextMenu("Save")]
        public void SaveData()
        {
            _storageData = new PlayerPrefsStorageData();
            _storageData.SaveData<UserDataAuth>("userDataAuth", userDataAuth);
        }
        
        [ContextMenu("Load")]
        public void LoadData()
        {
            _storageData = new PlayerPrefsStorageData();
            userDataAuth = _storageData.GetData<UserDataAuth>("userDataAuth");
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