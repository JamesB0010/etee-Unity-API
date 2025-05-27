using System;
using UnityEngine;

/// <summary>
/// Retrieves values from the API,
/// through Get() commands.
/// </summary>
public class eteeAPI
{
    private static eteeAPI
        instance; // Singleton pattern used here static properties and methods used to access fields and methods on this instance

    public I_CSharpSerial serialRead; // Serial reader class component reference.

    public static I_CSharpSerial SerialRead // Static get and set to expose the instance serial read to users 
    {
        get => instance.serialRead;
        set => instance.serialRead = value;
    }


    public eteeDevice leftDevice; // etee left device from where the data is retrieved class component refernece.

    public static eteeDevice LeftDevice // Static get and set to expose the instance left device to users
    {
        get => instance.leftDevice;

        set => instance.leftDevice = value;
    }

    public eteeDevice rightDevice; // etee right device from where the data is retrieved class component reference.

    public static eteeDevice RightDevice // static get and set to expose the instance right device to users
    {
        get => instance.rightDevice;

        set => instance.rightDevice = value;
    }


    private eteeAPI()
    {
    } // Private constructor ensures control over when this class is instantiated


    static eteeAPI()
    {
        instance = new eteeAPI(); // As we are using the singleton pattern we will allow the static constructor to instantiate our instance
    }

    /// <summary>
    /// Reset the controller parameters to 0
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    public static void ResetControllerValues(int device)
    {
        instance._ResetControllerValues(device);
    }

    public static void SuppressCommandSentDebugLog()
    {
        ((CSharpSerial)SerialRead).SuppressCommandSentDebugLog = true;
    }

    public static void UnsuppressCommandSentDebugLog()
    {
        ((CSharpSerial)SerialRead).SuppressCommandSentDebugLog = false;
    }

    /// <summary>
    /// Reset the controller parameters to 0
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    private void _ResetControllerValues(int device)
    {
        Debug.Log("Resetting controller values in API");
        if (device == 0)
        {
            leftDevice.ResetValues();
        }

        else if (device == 1)
        {
            rightDevice.ResetValues();
        }
    }


    /// <summary>
    /// Restart data streaming on controllers
    /// </summary>
    public static void RestartStreaming()
    {
        instance._RestartStreaming();
    }

    /// <summary>
    /// Restart data streaming on controllers
    /// </summary>
    private void _RestartStreaming()
    {
        serialRead.DisableDataStreaming();
        serialRead.EnableDataStreaming();
    }

    // ==================================== Status ====================================

    /// <summary>
    /// Checks if the dongle is
    /// connected.
    /// </summary>
    /// <returns>bool</returns>
    public static bool IsDongleDeviceConnected()
    {
        return instance.serialRead.IsDongleConnected();
    }

    /// <summary>
    /// Disconnects the system.
    /// Stops serial read reading
    /// data thread.
    /// </summary>
    /// <returns>void</returns>
    public static void Disconnect()
    {
        instance.serialRead.StopThread();
    }

    /// <summary>
    /// Checks if both controllers 
    /// are connected.
    /// </summary>
    /// <returns>bool</returns>
    public static bool IsBothDevicesConnected()
    {
        return instance._IsBothDevicesConnected();
    }

    /// <summary>
    /// Checks if both controllers 
    /// are connected.
    /// </summary>
    /// <returns>bool</returns>
    private bool _IsBothDevicesConnected()
    {
        if (serialRead.IsDeviceConnected(0) && serialRead.IsDeviceConnected(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// Checks if either controller 
    /// is connected.
    /// </summary>
    /// <returns>bool</returns>
    public static bool IsAnyDeviceConnected()
    {
        return instance._IsAnyDeviceConnected();
    }

    /// <summary>
    /// Checks if either controller 
    /// is connected.
    /// </summary>
    /// <returns>bool</returns>
    private bool _IsAnyDeviceConnected()
    {
        if (serialRead.IsDeviceConnected(0) || serialRead.IsDeviceConnected(1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// Checks if left device
    /// is connected.
    /// </summary>
    /// <returns>bool</returns>
    public static bool IsLeftDeviceConnected()
    {
        // 0 is used for left device as standard in all the etee api library.
        return instance.serialRead.IsDeviceConnected(0);
    }

    /// <summary>
    /// Check if right device
    /// is connected.
    /// </summary>
    /// <returns>bool</returns>
    public static bool IsRightDeviceConnected()
    {
        // 1 us used for right device as standard in all the etee api library.
        return instance.serialRead.IsDeviceConnected(1);
    }

    /// <summary>
    /// Get port name used
    /// to establish the connection.
    /// </summary>
    /// <returns>string</returns>
    public static string GetPortName()
    {
        return instance.serialRead.serialPort;
    }


    /// <summary>
    /// Get battery value.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>float</returns>
    public static float GetBattery(int device)
    {
        return instance._GetBattery(device);
    }

    /// <summary>
    /// Get battery value.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>float</returns>
    private float _GetBattery(int device)
    {
        // check the parameter is correct.
        if (device > 1)
        {
            return 0f;
        }

        return (device == 0) ? leftDevice.battery : rightDevice.battery;
    }


    /// <summary>
    /// Wrapper method to check to
    /// which port the dongle is
    /// connected. You need to pass
    /// the port name as a parameter.
    /// </summary>
    /// <param name="portName">string - name of the port to check</param>
    public static bool CheckPort(string portName)
    {
        return instance._CheckPort(portName);
    }


    /// <summary>
    /// Wrapper method to check to
    /// which port the dongle is
    /// connected. You need to pass
    /// the port name as a parameter.
    /// </summary>
    /// <param name="portName">string - name of the port to check</param>
    private bool _CheckPort(string portName)
    {
        if (GetPortName() == portName)
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// Wrapper method for checking
    /// if the device requested is
    /// the right hand.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    public static bool IsRightHand(int device)
    {
        return instance._IsRightHand(device);
    }

    /// <summary>
    /// Wrapper method for checking
    /// if the device requested is
    /// the right hand.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    private bool _IsRightHand(int device)
    {
        if (device == 0)
        {
            return false;
        }

        return true;
    }

    public static bool IsLeftHand(int device)
    {
        return !instance._IsRightHand(device);
    }

    // ==================================== Finger ====================================

    /// <summary>
    /// Get single finger data.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <parma name="fingerIndex">int - finger index. The correlation is the following: 0 - Thumb, 1 - Index, 2 - Middle, 3 - Ring, 4 - Pinky</param>
    /// <returns>float</returns>
    public static Tuple<float, float> GetFinger(int device, int fingerIndex)
    {
        return instance._GetFinger(device, fingerIndex);
    }

    /// <summary>
    /// Get single finger data.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <parma name="fingerIndex">int - finger index. The correlation is the following: 0 - Thumb, 1 - Index, 2 - Middle, 3 - Ring, 4 - Pinky</param>
    /// <returns>float</returns>
    private Tuple<float, float> _GetFinger(int device, int fingerIndex)
    {
        Tuple<float, float> value = new Tuple<float, float>(0f, 0f);

        // check that the value requested is valid.
        if (fingerIndex < 0 || fingerIndex > 4)
        {
            return value;
        }

        switch (fingerIndex)
        {
            case 0:
                value = (device == 0) ? leftDevice.thumb : rightDevice.thumb;
                break;
            case 1:
                value = (device == 0) ? leftDevice.index : rightDevice.index;
                break;
            case 2:
                value = (device == 0) ? leftDevice.middle : rightDevice.middle;
                break;
            case 3:
                value = (device == 0) ? leftDevice.ring : rightDevice.ring;
                break;
            case 4:
                value = (device == 0) ? leftDevice.pinky : rightDevice.pinky;
                break;
            default:
                value = new Tuple<float, float>(0f, 0f);
                break;
        }

        return value;
    }

    /// <summary>
    /// Get all fingers data pull pressure data
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>float[]</returns>
    public static float[] GetAllFingersPull(int device)
    {
        return instance._GetAllFingersPull(device);
    }

    /// <summary>
    /// Get all fingers data pull pressure data
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>float[]</returns>
    private float[] _GetAllFingersPull(int device)
    {
        if (device > 1)
        {
            float[] nullOp = new float[1];
            return nullOp;
        }

        return (device == 0) ? leftDevice.fingerPullData : rightDevice.fingerPullData;
    }

    /// <summary>
    /// Get all fingers data force pressure data
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>float[]</returns>
    public static float[] GetAllFingersForce(int device)
    {
        return instance._GetAllFingersForce(device);
    }

    /// <summary>
    /// Get all fingers data force pressure data
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>float[]</returns>
    private float[] _GetAllFingersForce(int device)
    {
        if (device > 1)
        {
            float[] nullOp = new float[1];
            return nullOp;
        }

        return (device == 0) ? leftDevice.fingerForceData : rightDevice.fingerForceData;
    }


    /// <summary>
    /// Starts calibration of fingers.
    /// </summary>
    public static void CalibrateFingers()
    {
        instance._CalibrateFingers();
    }

    /// <summary>
    /// Starts calibration of fingers.
    /// </summary>
    private void _CalibrateFingers()
    {
        serialRead.SendStartCalibrationCommand();
    }

    // ==================================== Trackpad ====================================


    /// <summary>
    /// Get trackpad axis value.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <param name="axis">char - axis whose value you want to retrieve.true Values allowed are 'x' or 'y'</param>
    /// <returns>float</returns>
    public static float GetTrackpadPositionSingleAxis(int device, char axis)
    {
        return instance._GetTrackpadPositionSingleAxis(device, axis);
    }

    /// <summary>
    /// Get trackpad axis value.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <param name="axis">char - axis whose value you want to retrieve.true Values allowed are 'x' or 'y'</param>
    /// <returns>float</returns>
    private float _GetTrackpadPositionSingleAxis(int device, char axis)
    {
        // check that the value requested is valid.
        if (axis != 'x' && axis != 'y')
        {
            return 0f;
        }
        else
        {
            Vector2 data = (device == 0) ? leftDevice.trackpadCoordinates : rightDevice.trackpadCoordinates;
            return (axis == 'x') ? data.x : data.y;
        }
    }


    /// <summary>
    /// Get trackpad axis values.
    /// </sumamry>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector2</returns>
    public static Vector2 GetTrackpadPosition(int device)
    {
        return instance._GetTrackpadPosition(device);
    }

    /// <summary>
    /// Get trackpad axis values.
    /// </sumamry>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector2</returns>
    private Vector2 _GetTrackpadPosition(int device)
    {
        // check that parameter is correct.
        if (device > 1)
        {
            return Vector2.zero;
        }

        return (device == 0) ? leftDevice.trackpadCoordinates : rightDevice.trackpadCoordinates;
    }


    /// <summary>
    /// Get trackpad pressure values.
    /// </sumamry>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector2</returns>
    public static Tuple<float, float> GetTrackpadPressures(int device)
    {
        return instance._GetTrackpadPressures(device);
    }

    /// <summary>
    /// Get trackpad pressure values.
    /// </sumamry>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector2</returns>
    private Tuple<float, float> _GetTrackpadPressures(int device)
    {
        // check that parameter is correct.
        if (device > 1 | device < 0)
        {
            return new Tuple<float, float>(0f, 0f);
        }

        return (device == 0) ? leftDevice.trackpadPressures : rightDevice.trackpadPressures;
    }


    /// <summary>
    /// Check if the trackpad
    /// has been tapped.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static bool GetTrackpadTapped(int device)
    {
        return instance._GetTrackpadTapped(device);
    }


    /// <summary>
    /// Check if the trackpad
    /// has been tapped.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private bool _GetTrackpadTapped(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.trackpadTapped : rightDevice.trackpadTapped;
    }


    /// <summary>
    /// Check if tap has been
    /// performed by the user.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>void</returns>
    public static Tuple<bool, bool> GetTap(int device)
    {
        return instance._GetTap(device);
    }

    /// <summary>
    /// Check if tap has been
    /// performed by the user.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>void</returns>
    private Tuple<bool, bool> _GetTap(int device)
    {
        // check that device parameter is correct.
        if (device > 1 | device < 0)
        {
            return new Tuple<bool, bool>(false, false);
        }

        return (device == 0) ? leftDevice.taps : rightDevice.taps;
    }

    // ==================================== Slider ====================================


    /// <summary>
    /// Wrapper method for retrieving the Y-location value
    /// of the slider.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    public static float GetSliderPosition(int device)
    {
        return instance._GetSliderPosition(device);
    }


    /// <summary>
    /// Wrapper method for retrieving the Y-location value
    /// of the slider.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    private float _GetSliderPosition(int device)
    {
        if (device > 1)
        {
            return 0f;
        }

        return (device == 0) ? leftDevice.sliderValue : rightDevice.sliderValue;
    }


    /// <summary>
    /// Check if the Slider
    /// button is pressed.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static bool GetSliderTouched(int device)
    {
        return instance._GetSliderTouched(device);
    }


    /// <summary>
    /// Check if the Slider
    /// button is pressed.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private bool _GetSliderTouched(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.sliderButton : rightDevice.sliderButton;
    }


    /// <summary>
    /// Check if the Slider up or down buttons are pressed.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static Tuple<bool, bool> GetSliderUpDownTouched(int device)
    {
        return instance._GetSliderUpDownTouched(device);
    }

    /// <summary>
    /// Check if the Slider up or down buttons are pressed.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private Tuple<bool, bool> _GetSliderUpDownTouched(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return new Tuple<bool, bool>(false, false);
        }

        return (device == 0)
            ? new Tuple<bool, bool>(leftDevice.sliderUpButton, leftDevice.sliderDownButton)
            : new Tuple<bool, bool>(rightDevice.sliderUpButton, rightDevice.sliderDownButton);
    }


    // ==================================== Rotation ====================================


    /// <summary>
    /// Grabs roll, pitch and yaw rotation data from
    /// either left or right hand.
    /// </summary>
    /// <param name="device">int - device from where you get the data from. 0 for left and 1 for right</param>
    /// <returns></returns>
    public static Vector3 GetRotations(int device)
    {
        return instance._GetRotations(device);
    }

    /// <summary>
    /// Grabs roll, pitch and yaw rotation data from
    /// either left or right hand.
    /// </summary>
    /// <param name="device">int - device from where you get the data from. 0 for left and 1 for right</param>
    /// <returns></returns>
    private Vector3 _GetRotations(int device)
    {
        // check that device parameter is correct.
        if (device > 1 | device < 0)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return (device == 0)
            ? new Vector3(leftDevice.roll, leftDevice.pitch, leftDevice.yaw)
            : new Vector3(rightDevice.roll, rightDevice.pitch, rightDevice.yaw);
    }


    /// <summary>
    /// Wrapper method to get all
    /// the quaternions for rotation.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    public static float[] GetQuaternionValues(int device)
    {
        return instance._GetQuaternionValues(device);
    }

    /// <summary>
    /// Wrapper method to get all
    /// the quaternions for rotation.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    private float[] _GetQuaternionValues(int device)
    {
        // get quaternions data from the get single quaternion method from the API
        char[] quaternionKeys = { 'w', 'x', 'y', 'z' };
        float[] data = new float[quaternionKeys.Length];

        for (int i = 0; i < quaternionKeys.Length; i++)
        {
            data[i] = _GetQuaternionComponent(device, quaternionKeys[i]);
        }

        return data;
    }


    /// <summary>
    /// Get single quaternion component
    /// value for rotation.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <param name="component">char - component name. Values allowed are: 'x', 'y', 'z' and 'w'</param>
    /// <returns>float</returns>
    public static float GetQuaternionComponent(int device, char component)
    {
        return instance._GetQuaternionComponent(device, component);
    }

    /// <summary>
    /// Get single quaternion component
    /// value for rotation.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <param name="component">char - component name. Values allowed are: 'x', 'y', 'z' and 'w'</param>
    /// <returns>float</returns>
    private float _GetQuaternionComponent(int device, char component)
    {
        float value = 0f;

        // check that parameter device is correct.
        if (device > 1)
        {
            return 0f;
        }

        Quaternion data = (device == 0) ? leftDevice.quaternions : rightDevice.quaternions;

        switch (component)
        {
            case 'x':
                value = data.x;
                break;
            case 'y':
                value = data.y;
                break;
            case 'z':
                value = data.z;
                break;
            case 'w':
                value = data.w;
                break;
            default:
                value = 0f;
                break;
        }

        return value;
    }


    /// <summary>
    /// Get quaternions data
    /// values for rotation.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Quaternion</returns>
    public static Quaternion GetQuaternions(int device)
    {
        return instance._GetQuaternions(device);
    }


    /// <summary>
    /// Get quaternions data
    /// values for rotation.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Quaternion</returns>
    private Quaternion _GetQuaternions(int device)
    {
        // check that device number is correct.
        if (device > 1)
        {
            return new Quaternion(0f, 0f, 0f, 0f);
        }

        return (device == 0) ? leftDevice.quaternions : rightDevice.quaternions;
    }


    /// <summary>
    /// Get euler data
    /// for velocity.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector3</returns>
    public static Vector3 GetEuler(int device)
    {
        return instance._GetEuler(device);
    }


    /// <summary>
    /// Get euler data
    /// for velocity.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector3</returns>
    private Vector3 _GetEuler(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return (device == 0) ? leftDevice.euler : rightDevice.euler;
    }


    /// <summary>
    /// Get acceleromenter axis
    /// data for velocity.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <parma name="axis">char - Axis to be retrieved. Allowed values are: 'x', 'y', 'z'</param>
    /// <returns>float</returns>
    public static float GetAccelerometerSingleAxis(int device, char axis)
    {
        return instance._GetAccelerometerSingleAxis(device, axis);
    }

    /// <summary>
    /// Get acceleromenter axis
    /// data for velocity.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <parma name="axis">char - Axis to be retrieved. Allowed values are: 'x', 'y', 'z'</param>
    /// <returns>float</returns>
    private float _GetAccelerometerSingleAxis(int device, char axis)
    {
        float value = 0f;

        // check that the device parameter is correct.
        if (device > 1)
        {
            return value;
        }

        Vector3 data = (device == 0) ? leftDevice.accelerometer : rightDevice.accelerometer;

        switch (axis)
        {
            case 'x':
                value = data.x;
                break;
            case 'y':
                value = data.y;
                break;
            case 'z':
                value = data.z;
                break;
            default:
                value = 0f;
                break;
        }

        return value;
    }


    /// <summary>
    /// Wraper method to get all
    /// the accelerometer values from
    /// the device.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    public static float[] GetAccelerometer(int device)
    {
        return instance._GetAccelerometer(device);
    }


    /// <summary>
    /// Wraper method to get all
    /// the accelerometer values from
    /// the device.
    /// </summary>
    /// <param name="device">int - device to from where you get the data from. 0 for left and 1 for right</param>
    private float[] _GetAccelerometer(int device)
    {
        char[] keys = { 'x', 'y', 'z' };
        float[] data = new float[keys.Length];

        for (int i = 0; i < keys.Length; i++)
        {
            data[i] = GetAccelerometerSingleAxis(device, keys[i]);
        }

        return data;
    }


    /// <summary>
    /// Get acceleromenter data
    /// for velocity.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector3</returns>
    public static Vector3 GetAccel(int device)
    {
        return instance._GetAccel(device);
    }


    /// <summary>
    /// Get acceleromenter data
    /// for velocity.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector3</returns>
    private Vector3 _GetAccel(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return (device == 0) ? leftDevice.accelerometer : rightDevice.accelerometer;
    }


    /// <summary>
    /// Wrapper method to get the gyro of a device
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector3</returns>
    public static Vector3 GetGyro(int device)
    {
        return instance._GetGyro(device);
    }

    /// <summary>
    /// Wrapper method to get the gyro of a device
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>Vector3</returns>
    private Vector3 _GetGyro(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return (device == 0) ? leftDevice.gyroscope : rightDevice.gyroscope;
    }

    /// <summary>
    /// Flags both controllers gyroscopes
    /// to be calibrated.
    /// </summary>
    public static void CalibrateDevicesGyro()
    {
        instance.leftDevice.gyroCalibrationDone = true;
        instance.rightDevice.gyroCalibrationDone = true;
    }


    /// <summary>
    /// Checks if the input device's gyroscope
    /// has been calibrated.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns></returns>
    public static bool GetIfDeviceGyroIsCalibrated(int device)
    {
        return instance._GetIfDeviceGyroIsCalibrated(device);
    }

    /// <summary>
    /// Checks if the input device's gyroscope
    /// has been calibrated.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns></returns>
    private bool _GetIfDeviceGyroIsCalibrated(int device)
    {
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.gyroCalibrated : rightDevice.gyroCalibrated;
    }


    public static Vector3 GetMag(int device)
    {
        return instance._GetMag(device);
    }


    private Vector3 _GetMag(int device)
    {
        // check that device parameter is correct.
        if (device > 1)
        {
            return new Vector3(0f, 0f, 0f);
        }

        return (device == 0) ? leftDevice.magnetometer : rightDevice.magnetometer;
    }


    /// <summary>
    /// Flags both controllers magnometer
    /// to be calibrated.
    /// </summary>
    /// <param name="enable">bool - if the magnometer enabled or not.</param>
    public static void CalibrateDevicesMag(bool enable)
    {
        instance._CalibrateDevicesMag(enable);
    }


    /// <summary>
    /// Flags both controllers magnometer
    /// to be calibrated.
    /// </summary>
    /// <param name="enable">bool - if the magnometer enabled or not.</param>
    private void _CalibrateDevicesMag(bool enable)
    {
        leftDevice.magCalibrated = !enable;
        rightDevice.magCalibrated = !enable;
        if (!enable)
        {
            leftDevice.UploadMagValues();
            rightDevice.UploadMagValues();
        }
    }

    // ==================================== Gestures ====================================

    /// <summary>
    /// Checks if a squeeze
    /// gesture is being performed
    /// by the user.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static bool GetIsSqueezeGesture(int device)
    {
        return instance._GetIsSqueezeGesture(device);
    }

    /// <summary>
    /// Checks if a squeeze
    /// gesture is being performed
    /// by the user.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private bool _GetIsSqueezeGesture(int device)
    {
        // check that the device number is correct.
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.squeeze : rightDevice.squeeze;
    }


    /// <summary>
    /// Check if the user
    /// is performing a Point Independent
    /// gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static bool GetIsPointIndependentGesture(int device)
    {
        return instance._GetIsPointIndependentGesture(device);
    }

    /// <summary>
    /// Check if the user
    /// is performing a Point Independent
    /// gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private bool _GetIsPointIndependentGesture(int device)
    {
        // check if device number is correct.
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.pointIndependent : rightDevice.pointIndependent;
    }


    /// <summary>
    /// Check if the user
    /// is performing a Point 
    /// Exclude Trackpad gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static bool GetIsPointExcludeTrackpadGesture(int device)
    {
        return instance._GetIsPointExcludeTrackpadGesture(device);
    }

    /// <summary>
    /// Check if the user
    /// is performing a Point 
    /// Exclude Trackpad gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private bool _GetIsPointExcludeTrackpadGesture(int device)
    {
        // check if device number is correct.
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.pointExcludeTrackpad : rightDevice.pointExcludeTrackpad;
    }


    /// <summary>
    /// Check if the user 
    /// is performing a pinch trackpad
    /// gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public static bool GetIsPinchTrackpadGesture(int device)
    {
        return instance._GetIsPinchTrackpadGesture(device);
    }

    /// <summary>
    /// Check if the user 
    /// is performing a pinch trackpad
    /// gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    private bool _GetIsPinchTrackpadGesture(int device)
    {
        // check if device number is correct.
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.pinchTrackpad : rightDevice.pinchTrackpad;
    }

    public static bool GetIsPinchThumbFingerGesture(int device)
    {
        return instance._GetIsPinchThumbFingerGesture(device);
    }

    /// <summary>
    /// Check if the user 
    /// is performing a pinch thumbfinger
    /// gesture.
    /// </summary>
    /// <param name="device">int - device number. Use 0 for left and 1 for right</param>
    /// <returns>bool</returns>
    public bool _GetIsPinchThumbFingerGesture(int device)
    {
        // check if device number is correct
        if (device > 1)
        {
            return false;
        }

        return (device == 0) ? leftDevice.pinchThumbFinger : rightDevice.pinchThumbFinger;
    }

    // ==================================== Haptics ====================================

    /// <summary>
    /// Enables controller
    /// haptic feedback.
    /// </summary>
    public static void EnableHaptics()
    {
        instance.serialRead.EnableHaptics();
    }

    /// <summary>
    /// Disables controller
    /// haptic feedback.
    /// </summary>
    public static void DisableHaptics()
    {
        instance.serialRead.DisableHaptics();
    }

    public static void VibrateDevice(int deviceIndex)
    {
        string hand = deviceIndex == 0 ? "left" : "right";
        
        instance.serialRead.SendVibrationsCommands(hand);
    }
}