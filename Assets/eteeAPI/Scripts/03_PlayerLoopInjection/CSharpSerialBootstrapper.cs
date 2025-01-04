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

        /// <summary>
        /// this inserts the fixedUpdate and update Player loop systems into the unity engine player loop
        /// </summary>
        /// <returns>the current (modified) playerLoop</returns>
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

        
        /// <summary>
        /// this adds our CSharpSerial fixed update system into the player loop as a subsystem of FixedUpdate
        /// </summary>
        /// <param name="loop">the player loop to add our subsystem to (ideally should be the current player loop)</param>
        /// <param name="index">where in the subsystem of the target system we want to position this</param>
        /// <typeparam name="T">T represents what system we want our system to be a subsystem of</typeparam>
        /// <returns></returns>
        static void InsertCSharpSerialFixedUpdateManager(ref PlayerLoopSystem loop, int index = 0)
        {
            CreateFixedUpdateSystem();
            bool insertionFailed = !PlayerLoopUtils.InsertSystem<UnityEngine.PlayerLoop.FixedUpdate>(ref loop,
                in CSharpSerialBootstrapper.serialSystemFixedUpdateLoop, index);
            if (insertionFailed)
                throw (new Exception(
                    "CSharpSerialManager not initialized, unable to register CSharpSerialManager into the fixed update loop"));
        }
        
        

        /// <summary>
        /// this adds our CSharpSerial update system into the player loop as a subsystem of Update
        /// </summary>
        /// <param name="loop">the player loop to add our subsystem to (ideally should be the current player loop)</param>
        /// <param name="index">where in the subsystem of the target system we want to position this</param>
        /// <typeparam name="T">T represents what system we want our system to be a subsystem of</typeparam>
        /// <returns></returns>
        static void InsertCSharpSerialUpdateManager(ref PlayerLoopSystem loop, int index = 0)
        {
            CreateUpdateSystem();
            bool insertionFailed =
                !PlayerLoopUtils.InsertSystem<UnityEngine.PlayerLoop.Update>(ref loop,
                    CSharpSerialBootstrapper.serialSystemUpdateLoop, index);
            if (insertionFailed)
                throw (new Exception(
                    "CSharpSerialManager not initialized, unable to register CSharpSerialManager into the update loop"));
        }


        /// <summary>
        /// This creates a player loop system to be inserted as a subsystem of FixedUpdate
        /// the update delegate calls the CSharpSerialManagers fixed update function which in turn calls the fixedUpdate method on the CSharpSerial object
        /// </summary>
        private static void CreateFixedUpdateSystem()
        {
            serialSystemFixedUpdateLoop = new PlayerLoopSystem()
            {
                type = typeof(CSharpSerialManager),
                updateDelegate = CSharpSerialManager.FixedUpdateSerial,
                subSystemList = null
            };
        }

        /// <summary>
        /// This creates a player loop system to be inserted as a subsystem of Update
        /// the update delegate calls the CSharpSerialManagers update function which in turn calls the Update method on the CSharpSerial object
        /// </summary>
        private static void CreateUpdateSystem()
        {
            serialSystemUpdateLoop = new PlayerLoopSystem()
            {
                type = typeof(CSharpSerialManager),
                updateDelegate = CSharpSerialManager.UpdateSerial,
                subSystemList = null
            };
        }


        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                //remove the CSharpSerial manager FixedUpdate and update systems from the unity engine player loop
                //this is done because we add these systems when playmode starts
                //if we didnt remove them there would be duplicate CSharpSerial systems in the unity engine player loop
                PlayerLoopSystem playerLoop = RemoveCSharpSerialManager();
                PlayerLoop.SetPlayerLoop(playerLoop);

                //dispsose the CSharpSerial object
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

        /// <summary>
        /// A wrapper method to remove the fixedUpdate subsystem from the playerloop
        /// </summary>
        /// <param name="loop">the current unity engine player loop</param>
        static void RemoveCSharpSerialFixedUpdateManager(ref PlayerLoopSystem loop)
        {
            PlayerLoopUtils.RemoveSystem<UnityEngine.PlayerLoop.FixedUpdate>(ref loop, in serialSystemFixedUpdateLoop);
        }

        /// <summary>
        /// A wrapper method to remove the Update subsystem from the playerloop
        /// </summary>
        /// <param name="loop">the current unity engine player loop</param>
        static void RemoveCSharpSerialUpdateManager(ref PlayerLoopSystem loop)
        {
            PlayerLoopUtils.RemoveSystem<UnityEngine.PlayerLoop.Update>(ref loop, in serialSystemUpdateLoop);
        }
    }
}


