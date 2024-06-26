using System;
using System.Collections.Generic;
using UnityEngine;

namespace Network.PlayFab.Data
{
    [CreateAssetMenu(menuName = "PlayFabServices/PlayFabTitleData", fileName = "PlayFabTitleData", order = 0)]
    public class PlayFabTitleData : ScriptableObject
    {
        public List<string> titleDataKeys;

        private void OnValidate()
        {
            titleDataKeys = new List<string>();
        }
    }
}