using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitApi 
{
    //Run this method once the assemblies are loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]  
    internal static void Init()
    {
        CSharpSerialBootstrapper.Init();
        var serial = CSharpSerialManager.CSharpSerial;
        eteeDevice leftDevice = new GameObject("Left etee Device").AddComponent<eteeDevice>();
        eteeDevice rightDevice = new GameObject("Right etee Device").AddComponent<eteeDevice>();
        
        
        eteeAPI.instance.serialRead = serial;
        eteeAPI.instance.leftDevice = leftDevice;
        eteeAPI.instance.rightDevice = rightDevice;

        InitApi.SetupCSharpSerialDependencies(serial, leftDevice, rightDevice);
        InitApi.SetupEteeDevice(leftDevice, isLeftHand: true, serial);
        InitApi.SetupEteeDevice(rightDevice, isLeftHand: false, serial);
    }

    private static void SetupEteeDevice(eteeDevice device, bool isLeftHand, CSharpSerial serial)
    {
        device.stream = serial;
        device.isLeft = isLeftHand;
    }

    private static void SetupCSharpSerialDependencies(CSharpSerial serial, eteeDevice leftDevice, eteeDevice rightDevice)
    {
        serial.leftDevice = leftDevice;
        serial.rightDevice = rightDevice;
    }
}
