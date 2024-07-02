using Network.Azure.BlobStorage;
using UnityEngine;
using Zenject;

namespace Network.Azure
{
    public class AzureInstaller : MonoInstaller
    {
        [SerializeField] private AzureConfigurations configurations;
        
        public override void InstallBindings()
        {
            Container.Bind<AzureConfigurations>().FromInstance(configurations);
            Container.Bind<StorageManager>().AsSingle();
        }
    }
}