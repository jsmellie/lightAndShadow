using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : Singleton<T>, new()
{
    private static T _instance;

    public static bool IsInstanceNull
    {
        get { return _instance == null; }
    }

    public static T Instance
    {
        get
        {
            if (IsInstanceNull)
            {
                _instance = new T();
                _instance.Initialize();
            }

            return _instance;
        }
    }

    public abstract void Initialize();
}
