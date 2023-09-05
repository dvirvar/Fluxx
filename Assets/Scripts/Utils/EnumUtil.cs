using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnumUtil
{
    public static T[] GetArrayOf<T>() where T : Enum
    {
        return (T[])Enum.GetValues(typeof(T));
    }

    public static T GetRandomOf<T>() where T : Enum
    {
        var array = GetArrayOf<T>();
        return (T)array.GetValue(UnityEngine.Random.Range(0, array.Length));
    }
}