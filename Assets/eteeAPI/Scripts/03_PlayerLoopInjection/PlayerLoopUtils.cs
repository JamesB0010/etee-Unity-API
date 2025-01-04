using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;

//credit git-ammend - https://github.com/adammyhre/Unity-Improved-Timers/tree/master

public static class PlayerLoopUtils
{
    /// <summary>
    /// Remove a system from the player loop
    /// </summary>
    /// <param name="loop">the root loop to start looking from</param>
    /// <param name="systemToRemove">the system to remove</param>
    /// <typeparam name="T">The parent system of loop</typeparam>
    public static void RemoveSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToRemove)
    {
        bool reachedLeafNode = loop.subSystemList == null;
        if (reachedLeafNode) return;

        //iterate over all sub systems of this node
        var playerLoopSystemList = new List<PlayerLoopSystem>(loop.subSystemList);
        for (int i = 0; i < playerLoopSystemList.Count; ++i)
        {
            bool systemToRemoveFound = playerLoopSystemList[i].type == systemToRemove.type && playerLoopSystemList[i].updateDelegate == systemToRemove.updateDelegate;
            if (systemToRemoveFound)
            {
                //remove the system
                playerLoopSystemList.RemoveAt(i);
                loop.subSystemList = playerLoopSystemList.ToArray();
                return;
            }
        }

        //system not found check children
        HandleSubSystemLoopForRemoval<T>(ref loop, systemToRemove);
    }

    /// <summary>
    /// For each child of the loop recursivley call the removeSystem function
    /// </summary>
    /// <param name="loop">the current Player loop system</param>
    /// <param name="systemToRemove">the system to remove</param>
    /// <typeparam name="T">the parent of loop</typeparam>
    static void HandleSubSystemLoopForRemoval<T>(ref PlayerLoopSystem loop, PlayerLoopSystem systemToRemove)
    {
        bool leafNodeReached = loop.subSystemList == null;
        if (leafNodeReached) return;

        //recursively check all nodes at this level
        for (int i = 0; i < loop.subSystemList.Length; ++i)
        {
            RemoveSystem<T>(ref loop.subSystemList[i], systemToRemove);
        }
    }

    /// <summary>
    /// Insert a system into the player loop
    /// </summary>
    /// <param name="loop">The root node of the player loop tree</param>
    /// <param name="systemToInsert">the system to insert into</param>
    /// <param name="index">the subsystem index to insert into once we have found the correct system</param>
    /// <typeparam name="T">the type is the system that we want to insert</typeparam>
    /// <returns>success</returns>
    public static bool InsertSystem<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
    {
        //this checks if we have found the system in the PlayerLoop system tree
        //if we haven't then recursively keep searching the tree
        if (loop.type != typeof(T)) return HandleSubSystemLoop<T>(ref loop, systemToInsert, index);

        //found the node so insert the system here
        
        var playerLoopSystemList = new List<PlayerLoopSystem>();
        if (loop.subSystemList != null) playerLoopSystemList.AddRange(loop.subSystemList);
        playerLoopSystemList.Insert(index, systemToInsert);
        loop.subSystemList = playerLoopSystemList.ToArray();
        return true;
    }

    /// <summary>
    /// The recursive part of the tree traversal used when adding a system 
    /// </summary>
    /// <param name="loop"></param>
    /// <param name="systemToInsert"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    static bool HandleSubSystemLoop<T>(ref PlayerLoopSystem loop, in PlayerLoopSystem systemToInsert, int index)
    {
        bool reachedLeafNode = loop.subSystemList == null;
        if (reachedLeafNode) return false;

        //iterate over sub systems
        for (int i = 0; i < loop.subSystemList.Length; ++i)
        {
            if (!InsertSystem<T>(ref loop.subSystemList[i], in systemToInsert, index)) continue;
            return true;
        }

        //type t not found in tree
        return false;
    }

    /// <summary>
    /// Prints of every system and sub system in the provided player loop system
    /// </summary>
    /// <param name="loop">This can be retrieved using PlayerLoop.GetCurrentPlayerLoop()</param>
    public static void PrintPlayerLoop(PlayerLoopSystem loop)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Unity Player Loop");
        //traverse every system
        foreach (PlayerLoopSystem subSystem in loop.subSystemList)
        {
            //traverse each sub system
            PrintSubsystem(subSystem, sb);
        }

        Debug.Log(sb.ToString());
    }

    /// <summary>
    /// Recursive helper for printing all children of this system
    /// </summary>
    /// <param name="system">The system to traverse</param>
    /// <param name="sb">A string builder to append the type name of nodes found</param>
    /// <param name="level">tracks the current depth of traversal for printing. default at 0 (root)</param>
    static void PrintSubsystem(PlayerLoopSystem system, StringBuilder sb, int level = 0)
    {
        sb.Append(' ', level * 2).AppendLine(system.type.ToString());
        
        bool leafNodeReached = system.subSystemList == null || system.subSystemList.Length == 0;
        if (leafNodeReached) return;

        //if this node has children then traverse all the children recursively
        foreach (PlayerLoopSystem subSystem in system.subSystemList)
        {
            PrintSubsystem(subSystem, sb, level + 1);
        }
    }
}