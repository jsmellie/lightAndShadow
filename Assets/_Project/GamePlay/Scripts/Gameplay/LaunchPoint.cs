using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LaunchPoint : MonoBehaviour
{
    [SerializeField] private AssetReference scene;

    void Awake()
    {
        AddressableSceneManager.Instance.LoadScene(scene);
    }

}
