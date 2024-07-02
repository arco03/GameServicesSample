using Data.Storage;
using UnityEngine;
using Zenject;

namespace Data
{
    public class DataInstaller : MonoInstaller
    {
        [SerializeField] private DataConfiguration dataConfiguration;
        
        public override void InstallBindings()
        {
            Container.Bind<DataConfiguration>().FromScriptableObject(dataConfiguration).AsSingle();
            Container.Bind<IStorageData>().To<JsonStorageData>().AsSingle();
        }
    }
}