using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private AssetReference _firstScene;
    private void Awake()
    {
        AddListeners(); 
    }

    private void AddListeners()
    {
        _playButton.onClick.AddListener(LoadGameScene);
    }

    private void LoadGameScene()
    {
        AddressableSceneManager.Instance.LoadScene(_firstScene, LoadSceneMode.Single);
    }
}
