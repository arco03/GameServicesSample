using UnityEngine;

namespace Network.Azure
{
    [CreateAssetMenu(menuName = "Azure/AzureConfigurations", fileName = "AzureConfigurations", order = 0)]
    public class AzureConfigurations : ScriptableObject
    {
        [Header("Azure Storage")]
        public string connectionString;
        public string containerName;
    }
}