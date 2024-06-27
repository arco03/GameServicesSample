using Network.PlayFab.Data;
using Network.PlayFab.Services;
using UnityEngine;
using Zenject;

namespace Network.PlayFab
{
    public class PlayFabInstaller : MonoInstaller
    {
        [SerializeField] private PlayFabAuthData playFabAuthData;
        
        public override void InstallBindings()
        {
            Container.Bind<PlayFabAuthData>().FromScriptableObject(playFabAuthData).AsSingle();

            Container.Bind<PlayFabAuthService>().AsSingle();
            Container.Bind<PlayFabTitleService>().AsSingle();
        }
    }
}