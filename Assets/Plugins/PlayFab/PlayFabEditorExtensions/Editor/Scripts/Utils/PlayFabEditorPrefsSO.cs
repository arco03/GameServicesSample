using PlayFab.PfEditor.EditorModels;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;

namespace PlayFab.PfEditor
{
#if UNITY_5_3_OR_NEWER
    [CreateAssetMenu(fileName = "PlayFabEditorPrefsSO", menuName = "PlayFab/Make Prefs SO", order = 1)]
#endif
    public class PlayFabEditorPrefsSO : ScriptableObject
    {
        public static readonly string FileDir = "_Data/Secrets/PlayFabEditorExtensions";
        public static readonly string Path = System.IO.Path.Combine(Application.dataPath, FileDir);
        
        private static PlayFabEditorPrefsSO _instance;
        public static PlayFabEditorPrefsSO Instance
        {
            get
            {
                if (_instance)
                    return _instance;

                PlayFabEditorPrefsSO settingsList = AssetDatabase.LoadAssetAtPath<PlayFabEditorPrefsSO>($"Assets/{FileDir}/PlayFabEditorPrefsSO.asset");
                if (settingsList)
                    _instance = settingsList;
                
                if (_instance)
                    return _instance;

                _instance = CreateInstance<PlayFabEditorPrefsSO>();
                
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);

                // TODO: we know the location of this file will be under  PlayFabEditorExtensions/Editor/ 
                // just need to pull that files path, and append /Resources/ and boom you have the below path.
                // consider moving this above the if directory exists so we can do the same logic beforehand.
                Directory.GetFiles(Application.dataPath, "PlayFabEditor.cs");

                AssetDatabase.CreateAsset(_instance, $"Assets/{FileDir}/PlayFabEditorPrefsSO.asset");
                AssetDatabase.SaveAssets();
                Debug.LogWarning("Created missing PlayFabEditorPrefsSO file");
                return _instance;
            }
        }

        public static void Save()
        {
            EditorUtility.SetDirty(_instance);
            AssetDatabase.SaveAssets();
        }

        public string DevAccountEmail;
        public string DevAccountToken;

        public string AadAuthorization;

        public List<Studio> StudioList = null; // Null means not fetched, empty is a possible return result from GetStudios
        public string SelectedStudio;

        public readonly Dictionary<string, string> TitleDataCache = new Dictionary<string, string>();
        public readonly Dictionary<string, string> InternalTitleDataCache = new Dictionary<string, string>();

        public string SdkPath;
        public string EdExPath;
        public string LocalCloudScriptPath;

        private string _latestSdkVersion;
        private string _latestEdExVersion;
        private DateTime _lastSdkVersionCheck;
        private DateTime _lastEdExVersionCheck;
        public bool PanelIsShown;
        public string EdSet_latestSdkVersion { get { return _latestSdkVersion; } set { _latestSdkVersion = value; _lastSdkVersionCheck = DateTime.UtcNow; } }
        public string EdSet_latestEdExVersion { get { return _latestEdExVersion; } set { _latestEdExVersion = value; _lastEdExVersionCheck = DateTime.UtcNow; } }
        public DateTime EdSet_lastSdkVersionCheck { get { return _lastSdkVersionCheck; } }
        public DateTime EdSet_lastEdExVersionCheck { get { return _lastEdExVersionCheck; } }

        public int curMainMenuIdx;
        public int curSubMenuIdx;
    }
}
