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
        CSharpSerialManager.cSharpSerial = new CSharpSerial();
    }
    public static void FixedUpdateSerial() => CSharpSerialManager.cSharpSerial.FixedUpdate();

    public static void UpdateSerial() => CSharpSerialManager.cSharpSerial.Update();

    public static void Clear() => CSharpSerialManager.cSharpSerial = null;
}
