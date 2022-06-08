using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LaunchPoint : MonoBehaviour
{
    [SerializeField] private string scene;

    void Awake()
    {
        //AddressableSceneManager.Instance.LoadScene(scene);
    }

}
