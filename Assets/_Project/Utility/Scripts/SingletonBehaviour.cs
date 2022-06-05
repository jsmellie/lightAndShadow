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
                CreateInstance();
            }

            return _instance;
        }
    }

    private static bool CreateInstance()
    {
        if (IsInstanceNull)
        {
            T obj = GameObject.FindObjectOfType<T>();

            if (obj != null)
            {
                _instance = obj;
            }

            if (_instance == null)
            {
                obj = new GameObject().AddComponent<T>();
                obj.name = typeof(T).FullName;
                _instance = obj;
            }

            DontDestroyOnLoad(_instance.gameObject);

            _instance.Initialize();

            return true;
        }

        return false;
    }

    protected virtual void Awake()
    {
        if (!CreateInstance())
        {
            Destroy(gameObject);
        }
    }

    protected abstract void Initialize();
}
