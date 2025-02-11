using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InitApi 
{
    //Run this method once the assemblies are loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]  
    private static void Init()
    {
        eteeApiSettings eteeApiSettings = Resources.Load<eteeApiSettings>("eteeApiSettings/eteeApiSettings");
        if (eteeApiSettings.ApiEnabled == false)
        {
            Debug.LogWarning("etee api is disabled, etee api calls will only return dummy data \n You can re enable the etee api inside of the etee api settings object located in Assets/Resources/eteeApiSettings");
            CSharpSerialManager.SetCSharpSerial(new DummyCSharpSerial());
            var s = CSharpSerialManager.CSharpSerial;
            eteeDevice lDevice = new GameObject("Left etee Device").AddComponent<eteeDevice>();
            eteeDevice rDevice = new GameObject("Right etee Device").AddComponent<eteeDevice>();
            SetupApiDependencies(s, lDevice, rDevice);
            InitApi.SetupCSharpSerialDependencies(s, lDevice, rDevice);
            InitApi.SetupEteeDevice(lDevice, isLeftHand: true, s);
            InitApi.SetupEteeDevice(rDevice, isLeftHand: false, s);
            Application.quitting += s.OnApplicationQuit;
            return;
        }
        
        
        //injects the CSharpSerialManager into the PlayerLoop among other setup logic
        eteePlayerLoop.CSharpSerialBootstrapper.Init();
        
        
        CSharpSerialManager.SetCSharpSerial(new CSharpSerial());
        
        //get a reference tp the CSharpSerial Object
        var serial = CSharpSerialManager.CSharpSerial;
        ((CSharpSerial)serial).SuppressCommandSentDebugLog = eteeApiSettings.SuppressCommandSentDebugLog;
        
        //Create the left and right devices
        eteeDevice leftDevice = new GameObject("Left etee Device").AddComponent<eteeDevice>();
        eteeDevice rightDevice = new GameObject("Right etee Device").AddComponent<eteeDevice>();
        

        SetupApiDependencies(serial, leftDevice, rightDevice);
        InitApi.SetupCSharpSerialDependencies(serial, leftDevice, rightDevice);
        InitApi.SetupEteeDevice(leftDevice, isLeftHand: true, serial);
        InitApi.SetupEteeDevice(rightDevice, isLeftHand: false, serial);
        Application.quitting += serial.OnApplicationQuit;
    }

    private static void SetupApiDependencies(I_CSharpSerial serial, eteeDevice leftDevice, eteeDevice rightDevice)
    {
        eteeAPI.SerialRead = serial;
        eteeAPI.LeftDevice = leftDevice;
        eteeAPI.RightDevice = rightDevice;
    }

    private static void SetupEteeDevice(eteeDevice device, bool isLeftHand, I_CSharpSerial serial)
    {
        device.stream = serial;
        device.isLeft = isLeftHand;
    }

    private static void SetupCSharpSerialDependencies(I_CSharpSerial serial, eteeDevice leftDevice, eteeDevice rightDevice)
    {
        serial.leftDevice = leftDevice;
        serial.rightDevice = rightDevice;
    }
}
