using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class eteeMenuItems 
{
    [MenuItem("etee/ToggleApi/EnableApi")]
    public static void EnableApi()
    {
        if (Application.isPlaying) 
            Debug.LogWarning(
                "etee api enabled, however changes will not come into effect until play mode is entered again");
        
        eteeApiSettings settings = Resources.Load<eteeApiSettings>("eteeApiSettings/eteeApiSettings");
        settings.ApiEnabled = true;
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("etee/ToggleApi/DisableApi")]
    public static void DisableApi()
    {
        if (Application.isPlaying)
            Debug.LogWarning(
                        "etee api disabled, however changes will not come into effect until play mode is entered again");
        
        eteeApiSettings settings = Resources.Load<eteeApiSettings>("eteeApiSettings/eteeApiSettings");
        settings.ApiEnabled = false;
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    [MenuItem("etee/DebugLog/SuppressCommandSentMessage")]
    public static void SuppressCommandSentMessage()
    {
        eteeApiSettings settings = Resources.Load<eteeApiSettings>("eteeApiSettings/eteeApiSettings");
        settings.SuppressCommandSentDebugLog = true;
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        eteeAPI.SuppressCommandSentDebugLog();
    }
    
    [MenuItem("etee/DebugLog/UnsuppressCommandSentMessage")]
    public static void UnsuppressCommandSentMessage()
    {
        eteeApiSettings settings = Resources.Load<eteeApiSettings>("eteeApiSettings/eteeApiSettings");
        settings.SuppressCommandSentDebugLog = false;
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        eteeAPI.UnsuppressCommandSentDebugLog();
    }
}
