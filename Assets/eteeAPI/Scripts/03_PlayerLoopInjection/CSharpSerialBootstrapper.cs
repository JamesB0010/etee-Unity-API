using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;

//thanks to git-ammend for presenting how to inject custom processes into the player loop https://www.youtube.com/watch?v=ilvmOQtl57c


internal static class CSharpSerialBootstrapper
{
    private static PlayerLoopSystem serialSystemFixedUpdateLoop;
    private static PlayerLoopSystem serialSystemUpdateLoop;
    
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType
        .AfterAssembliesLoaded)] //Run this method once the assemblies are loaded
    internal static void Init()
    {
        CSharpSerialManager.Init();
        
        
        var currentPlayerLoop = InsertLoopSystemsIntoPlayerloop();


        PlayerLoopUtils.PrintPlayerLoop(currentPlayerLoop);
        
        
        #if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
    }


    private static PlayerLoopSystem InsertLoopSystemsIntoPlayerloop()
    {
        PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

        try
        {
            InsertCSharpSerialFixedUpdateManager<UnityEngine.PlayerLoop.FixedUpdate>(ref currentPlayerLoop, 0);
            InsertCSharpSerialUpdateManager<UnityEngine.PlayerLoop.Update>(ref currentPlayerLoop, 0);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        return currentPlayerLoop;
    }

    static void InsertCSharpSerialFixedUpdateManager<T>(ref PlayerLoopSystem loop, int index)
    {
        CreateFixedUpdateSystem();
        bool insertionFailed = !PlayerLoopUtils.InsertSystem<T>(ref loop, in CSharpSerialBootstrapper.serialSystemFixedUpdateLoop, index);
        if (insertionFailed)
            throw (new Exception("CSharpSerialManager not initialized, unable to register CSharpSerialManager into the fixed update loop"));
    }

    /// <summary>
    /// this adds our system into the player loop as a subsystem
    /// </summary>
    /// <param name="loop">the player loop to add our subsystem to (ideally should be the current player loop)</param>
    /// <param name="index">where in the subsystem we want to position this</param>
    /// <typeparam name="T">T represents what system we want our system to be a subsystem of</typeparam>
    /// <returns></returns>
    static void InsertCSharpSerialUpdateManager<T>(ref PlayerLoopSystem loop, int index)
    {
        CreateUpdateSystem();
        bool insertionFailed = !PlayerLoopUtils.InsertSystem<T>(ref loop, CSharpSerialBootstrapper.serialSystemUpdateLoop, index);
        if (insertionFailed)
            throw (new Exception("CSharpSerialManager not initialized, unable to register CSharpSerialManager into the update loop"));
    }
    private static void CreateFixedUpdateSystem()
    {
        CSharpSerialBootstrapper.serialSystemFixedUpdateLoop = new PlayerLoopSystem()
        {
            type = typeof(CSharpSerialManager),
            updateDelegate = CSharpSerialManager.FixedUpdateSerial,
            subSystemList = null
        };
    }
    
    private static void CreateUpdateSystem()
    {
        CSharpSerialBootstrapper.serialSystemUpdateLoop = new PlayerLoopSystem()
        {
            type = typeof(CSharpSerialManager),
            updateDelegate = CSharpSerialManager.UpdateSerial,
            subSystemList = null
        };
    }

    static void RemoveCSharpSerialFixedUpdateManager<T>(ref PlayerLoopSystem loop)
    {
        PlayerLoopUtils.RemoveSystem<T>(ref loop, in serialSystemFixedUpdateLoop);
    }

    static void RemoveCSharpSerialUpdateManager<T>(ref PlayerLoopSystem loop)
    {
        PlayerLoopUtils.RemoveSystem<T>(ref loop, in serialSystemUpdateLoop);
    }
    
    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            PlayerLoopSystem playerLoop = RemoveCSharpSerialManager();
            PlayerLoop.SetPlayerLoop(playerLoop);
            
            CSharpSerialManager.Clear();
        }
    }

    private static PlayerLoopSystem RemoveCSharpSerialManager()
    {
        PlayerLoopSystem currentPlayerloop = PlayerLoop.GetCurrentPlayerLoop();
        RemoveCSharpSerialFixedUpdateManager<UnityEngine.PlayerLoop.FixedUpdate>(ref currentPlayerloop);
        RemoveCSharpSerialUpdateManager<UnityEngine.PlayerLoop.Update>(ref currentPlayerloop);
        return currentPlayerloop;
    }
}


