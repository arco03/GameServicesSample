#if UNITY_2017_1_OR_NEWER
using System.IO;
using UnityEditor;
using UnityEngine;

public class MakeScriptableObject
{
    public static readonly string FileDir = "_Data/Secrets/PlayFabSdk";
    public static readonly string Path = System.IO.Path.Combine(Application.dataPath, FileDir);
    
    [MenuItem("PlayFab/MakePlayFabSharedSettings")]
    public static void MakePlayFabSharedSettings()
    {
        PlayFabSharedSettings asset = ScriptableObject.CreateInstance<PlayFabSharedSettings>();
        
        if (!Directory.Exists(Path))
            Directory.CreateDirectory(Path);
        
        AssetDatabase.CreateAsset(asset, $"Assets/{FileDir}/PlayFabSharedSettings.asset"); // TODO: Path should not be hard coded
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
        
        Debug.LogWarning("Created PlayFabSharedSettings file");
    }
}
#endif
