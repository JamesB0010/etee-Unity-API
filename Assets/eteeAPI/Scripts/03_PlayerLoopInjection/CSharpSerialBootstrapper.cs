using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;

//thanks to git-ammend for presenting how to inject custom processes into the player loop https://www.youtube.com/watch?v=ilvmOQtl57c


namespace eteePlayerLoop
{
/// <summary>
/// This class is responsible for injecting the CSharpSerialManagers fixed update and update methods into the
/// UnityEngine Player Loop
/// </summary>
public static class CSharpSerialBootstrapper
{
    private static PlayerLoopSystem serialSystemFixedUpdateLoop;
    private static PlayerLoopSystem serialSystemUpdateLoop;
    
    public static void Init()
    {
        var currentPlayerLoop = InsertLoopSystemsIntoPlayerLoop();

        //PlayerLoopUtils.PrintPlayerLoop(currentPlayerLoop);
        
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
    }


    private static PlayerLoopSystem InsertLoopSystemsIntoPlayerLoop()
    {
        PlayerLoopSystem currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

        try
        {
            InsertCSharpSerialFixedUpdateManager(ref currentPlayerLoop);
            InsertCSharpSerialUpdateManager(ref currentPlayerLoop);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        PlayerLoop.SetPlayerLoop(currentPlayerLoop);
        return currentPlayerLoop;
    }

    static void InsertCSharpSerialFixedUpdateManager(ref PlayerLoopSystem loop, int index = 0)
    {
        CreateFixedUpdateSystem();
        bool insertionFailed = !PlayerLoopUtils.InsertSystem<UnityEngine.PlayerLoop.FixedUpdate>(ref loop, in CSharpSerialBootstrapper.serialSystemFixedUpdateLoop, index);
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
    static void InsertCSharpSerialUpdateManager(ref PlayerLoopSystem loop, int index = 0)
    {
        CreateUpdateSystem();
        bool insertionFailed = !PlayerLoopUtils.InsertSystem<UnityEngine.PlayerLoop.Update>(ref loop, CSharpSerialBootstrapper.serialSystemUpdateLoop, index);
        if (insertionFailed)
            throw (new Exception("CSharpSerialManager not initialized, unable to register CSharpSerialManager into the update loop"));
    }
    private static void CreateFixedUpdateSystem()
    {
        serialSystemFixedUpdateLoop = new PlayerLoopSystem()
        {
            type = typeof(CSharpSerialManager),
            updateDelegate = CSharpSerialManager.FixedUpdateSerial,
            subSystemList = null
        };
    }
    
    private static void CreateUpdateSystem()
    {
        serialSystemUpdateLoop = new PlayerLoopSystem()
        {
            type = typeof(CSharpSerialManager),
            updateDelegate = CSharpSerialManager.UpdateSerial,
            subSystemList = null
        };
    }

    static void RemoveCSharpSerialFixedUpdateManager(ref PlayerLoopSystem loop)
    {
        PlayerLoopUtils.RemoveSystem<UnityEngine.PlayerLoop.FixedUpdate>(ref loop, in serialSystemFixedUpdateLoop);
    }

    static void RemoveCSharpSerialUpdateManager(ref PlayerLoopSystem loop)
    {
        PlayerLoopUtils.RemoveSystem<UnityEngine.PlayerLoop.Update>(ref loop, in serialSystemUpdateLoop);
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
        RemoveCSharpSerialFixedUpdateManager(ref currentPlayerloop);
        RemoveCSharpSerialUpdateManager(ref currentPlayerloop);
        return currentPlayerloop;
    }
}
}


