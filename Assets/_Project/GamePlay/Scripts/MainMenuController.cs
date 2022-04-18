using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        SceneManager.LoadScene("FOREST_Stage1", LoadSceneMode.Single);
    }
}
