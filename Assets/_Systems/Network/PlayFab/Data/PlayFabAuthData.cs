using PlayFab;
using UnityEngine;

namespace Network.PlayFab.Data
{
    [CreateAssetMenu(menuName = "PlayFabServices/PlayFabAuthData", fileName = "PlayFabAuthData", order = 0)]
    public class PlayFabAuthData : ScriptableObject
    {
        public PlayFabClientInstanceAPI InstanceAPI;
        public PlayFabDataInstanceAPI DataAPI;
    }
}