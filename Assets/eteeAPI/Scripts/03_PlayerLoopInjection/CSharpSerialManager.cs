using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public static class CSharpSerialManager
{
    private static CSharpSerial cSharpSerial;
    public static CSharpSerial CSharpSerial => cSharpSerial;
    static CSharpSerialManager()
    {
        cSharpSerial = new CSharpSerial();
    }

    /// <summary>
    /// This method is injected as a child system of the fixed update system in the UnityEngine Player
    /// </summary>
    public static void FixedUpdateSerial()
    {
        CSharpSerialManager.cSharpSerial.FixedUpdate();
    }

    /// <summary>
    /// This method is injected as a child system of the update system in the UnityEngine Player
    /// </summary>
    public static void UpdateSerial()
    {
        CSharpSerialManager.cSharpSerial.Update();
    }

    //ensures that our static variable is cleared
    public static void Clear()
    {
        CSharpSerialManager.cSharpSerial = null;
    }
}
