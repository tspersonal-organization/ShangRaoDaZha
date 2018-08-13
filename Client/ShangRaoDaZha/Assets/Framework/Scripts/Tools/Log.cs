using System.Collections;
using System.Collections.Generic;
using System;

public class Log
{
    public static void Debug(object text)
    {
//#if UNITY_EDITOR
        UnityEngine.Debug.Log(text.ToString());
//#endif
    }
}
