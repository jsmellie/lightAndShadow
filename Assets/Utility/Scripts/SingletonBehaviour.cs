using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
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
                GameObject obj = GameObject.Find(typeof(T).FullName);
                if (obj != null)
                {
                    _instance = obj.GetComponent<T>();
                }
                if (_instance == null)
                {
                    obj = new GameObject();
                    obj.name = typeof(T).FullName;
                    _instance = obj.AddComponent<T>();
                }

                DontDestroyOnLoad(_instance.gameObject);

                _instance.Initialize();
            }

            return _instance;
        }
    }

    protected abstract void Initialize();
}
