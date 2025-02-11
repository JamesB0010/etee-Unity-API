using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_CSharpSerial
{
    public eteeDevice leftDevice { get; set; }
    public eteeDevice rightDevice { get; set; }
    
    public string serialPort { get; set; }
    public void Update();


    public void FixedUpdate();

    
    public bool IsDongleConnected() => false;

    public bool IsDeviceConnected(int device) => false;

    public void CheckPorts();


    public bool IsBitSet(byte b, int pos) => false;

    public void EnableHaptics();

    public void DisableHaptics();

    public void SendVibrationsCommands(string hand);


    public void CheckVibrationQueues();


    public void SendStartCalibrationCommand();


    public void SendCancelCalibrationCommand();


    public void SendResetOrientationCalibrationCommand();


    public void SendCalibratedGyroOffsetLeft(Vector3 offset);


    public void SendCalibratedGyroOffsetRight(Vector3 offset);


    public void SendCalibratedMagOffsetLeft(Vector3 offset);


    public void SendCalibratedMagOffsetRight(Vector3 offset);


    public void EnableDataStreaming();


    public void DisableDataStreaming();

    public void StopThread();

    public void OnApplicationQuit();
}
