using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button _playButton;

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
        Addressables.LoadScene("Assets/_Project/GamePlay/Scenes/Stages/URBAN_Stage1.unity", LoadSceneMode.Single);
    }
}
