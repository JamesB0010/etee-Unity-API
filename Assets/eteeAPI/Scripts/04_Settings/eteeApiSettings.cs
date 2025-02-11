using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eteeApiSettings : ScriptableObject 
{
    [SerializeField] private bool apiEnabled;
    [SerializeField] private bool suppressCommandSentDebugLog;
    
    public bool ApiEnabled
    {
        get => this.apiEnabled;
        set
        {
            bool noChangeDetected = this.apiEnabled == value;
            if (noChangeDetected)
            {
                if (value)
                    Debug.Log("etee api already enabled");
                else
                    Debug.Log("etee api already disabled");
                return;
            }
            
            this.apiEnabled = value;

            if (this.apiEnabled == true)
                Debug.Log("etee api enabled");
            else
                Debug.Log("etee api disabled");
        }
    }

    public bool SuppressCommandSentDebugLog
    {
        get => this.suppressCommandSentDebugLog;
        set
        {
            bool noChangeDetected = this.suppressCommandSentDebugLog == value;
            if (noChangeDetected)
            {
                if(value)
                    Debug.Log("Command Sent debug log already suppressed");
                else
                    Debug.Log("Command Sent debug log already unsuppressed");
                return;
            }
            
            
            this.suppressCommandSentDebugLog = value;

            if (this.suppressCommandSentDebugLog)
                Debug.Log("Now suppressing command sent debug log");
            else
                Debug.Log("Command sent debug log unsuppressed");
        }
    }
}
