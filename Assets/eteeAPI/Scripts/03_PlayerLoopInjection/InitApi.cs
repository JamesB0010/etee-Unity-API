using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitApi 
{
    //Run this method once the assemblies are loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]  
    private static void Init()
    {
        //injects the CSharpSerialManager into the PlayerLoop among other setup logic
        eteePlayerLoop.CSharpSerialBootstrapper.Init();
        
        //get a reference tp the CSharpSerial Object
        var serial = CSharpSerialManager.CSharpSerial;
        
        //Create the left and right devices
        eteeDevice leftDevice = new GameObject("Left etee Device").AddComponent<eteeDevice>();
        eteeDevice rightDevice = new GameObject("Right etee Device").AddComponent<eteeDevice>();
        

        SetupApiDependencies(serial, leftDevice, rightDevice);
        InitApi.SetupCSharpSerialDependencies(serial, leftDevice, rightDevice);
        InitApi.SetupEteeDevice(leftDevice, isLeftHand: true, serial);
        InitApi.SetupEteeDevice(rightDevice, isLeftHand: false, serial);
    }

    private static void SetupApiDependencies(CSharpSerial serial, eteeDevice leftDevice, eteeDevice rightDevice)
    {
        eteeAPI.SerialRead = serial;
        eteeAPI.LeftDevice = leftDevice;
        eteeAPI.RightDevice = rightDevice;
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
