using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using System;

public class MainMenuController : MonoBehaviour
{
    public enum MainMenuState
    {
        Main,
        Settings,
        Resources,
        Credits
    }

    public enum MainMenuOption
    {
        Play = 0,
        Settings = 1,
        Resources = 2,
        Credits = 3,
        Quit = 4
    }

    private const float MENU_OPTION_SCALE_LERP_SPEED = 10;
    private const float MENU_OPTION_OFFSET_LERP_SPEED = 10;

    [SerializeField] private List<GameObject> _mainMenuOptions;
    [SerializeField] private RectTransform _mainMenuOptionsParent;
    [SerializeField] private MenuInputController _menuInputController;

    [SerializeField] private SpriteRenderer _title;
    [SerializeField] private CanvasGroup _buttons;

    [SerializeField] private SettingsMenuPanel _settingsPanel;
    [SerializeField] private ResourcesMenuPanel _resourcesPanel;
    [SerializeField] private CreditsMenuPanel _creditsPanel;

    private bool _isInteractable = false;

    private MainMenuOption _currentOption;
    private MainMenuState _currentState = MainMenuState.Main;

    private List<Vector3> _optionTargetScale = new List<Vector3>();
    private Vector3 _optionsTargetPosition;

    private void Awake()
    {
        for (int i = 0; i < _mainMenuOptions.Count; i++)
        {
            _optionTargetScale.Add(Vector3.one);
        }

        _buttons.alpha = 0;

        SetCurrentOption(MainMenuOption.Play);

        _settingsPanel.GetComponent<CanvasGroup>().alpha = 0;
        _resourcesPanel.GetComponent<CanvasGroup>().alpha = 0;
        _creditsPanel.GetComponent<CanvasGroup>().alpha = 0;

        GameController.Instance.SetState(GameController.GameState.Menu);

        FullScreenWipe.FadeOut(1, () =>
        {
            DOVirtual.Float(0, 1, 1, (x) =>
            {
                _buttons.alpha = x;
            })
            .SetDelay(1)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                _isInteractable = true;
            });

        });
    }

    private void LoadGameScene()
    {
        _isInteractable = false;

        DOVirtual.Float(1, 0, 1.5f, (x) =>
        {
            _buttons.alpha = x;
            _title.color = new Color(1, 1, 1, x);
        })
        .SetEase(Ease.InOutQuad).onComplete += () =>
        {
            gameObject.SetActive(false);
        };

        GameController.Instance.SetState(GameController.GameState.Playing);
    }

    private void Update()
    {
        for (int i = 0; i < _mainMenuOptions.Count; i++)
        {
            _mainMenuOptions[i].transform.localScale = Vector3.Lerp(_mainMenuOptions[i].transform.localScale, _optionTargetScale[i], Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
            CanvasGroup canvasGroup = _mainMenuOptions[i].GetComponent<CanvasGroup>();

            if (i == (int)_currentOption)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
            }
            else
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.3f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
            }
        }

        _mainMenuOptionsParent.localPosition = Vector3.Lerp(_mainMenuOptionsParent.localPosition, _optionsTargetPosition, Time.unscaledDeltaTime * MENU_OPTION_OFFSET_LERP_SPEED);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_mainMenuOptionsParent);

        HandleInput();
    }

    private void HandleInput()
    {
        if (!_isInteractable)
        {
            return;
        }

        if (_menuInputController.GetSelectDown())
        {
            SelectMenuOption();
            return;
        }

        if (_menuInputController.GetRightDown())
        {
            NextOption();
            return;
        }

        if (_menuInputController.GetLeftDown())
        {
            PreviousOption();
            return;
        }
    }

    private void SelectMenuOption()
    {
        switch (_currentOption)
        {
            case MainMenuOption.Play:
                LoadGameScene();
                break;
            case MainMenuOption.Settings:
                SetCurrentMenuState(MainMenuState.Settings);
                break;
            case MainMenuOption.Resources:
                SetCurrentMenuState(MainMenuState.Resources);
                break;
            case MainMenuOption.Credits:
                SetCurrentMenuState(MainMenuState.Credits);
                break;
            case MainMenuOption.Quit:
                Application.Quit();
                break;
        }
    }

    private void NextOption()
    {
        if ((int)_currentOption < (int)MainMenuOption.Quit)
        {
            SetCurrentOption((MainMenuOption)((int)_currentOption + 1));
        }
        else
        {
            SetCurrentOption(MainMenuOption.Play);
        }
    }

    private void PreviousOption()
    {
        if ((int)_currentOption > (int)MainMenuOption.Play)
        {
            SetCurrentOption((MainMenuOption)((int)_currentOption - 1));
        }
        else
        {
            SetCurrentOption(MainMenuOption.Quit);
        }
    }

    private void SetCurrentOption(MainMenuOption option)
    {
        _currentOption = option;

        float totalWidth = 0;

        for (int i = 0; i < _optionTargetScale.Count; i++)
        {
            int distance = Mathf.Clamp(Mathf.Abs(i - (int)_currentOption), 0, 2);

            float scale = 1.5f - Mathf.Lerp(0f, 1f, (float)distance / 2f);

            _optionTargetScale[i] = new Vector3(scale, scale, scale);

            if (i < (int)_currentOption)
            {
                totalWidth += scale * 100f;
            }
        }

        totalWidth += (1.5f * 100f) / 2f;

        Vector3 position = _mainMenuOptionsParent.localPosition;
        position.x = -totalWidth;

        _optionsTargetPosition = position;
    }

    public void SetCurrentMenuState(MainMenuState state)
    {
        _currentState = state;

        _isInteractable = false;
        _settingsPanel.SetInteractable(false);
        _resourcesPanel.SetInteractable(false);
        _creditsPanel.SetInteractable(false);

        switch (state)
        {
            case MainMenuState.Main:
                FadeInMain(0.5f);
                FadeOutPanel(_settingsPanel.GetComponent<CanvasGroup>(), 0.5f);
                FadeOutPanel(_resourcesPanel.GetComponent<CanvasGroup>(), 0.5f);
                FadeOutPanel(_creditsPanel.GetComponent<CanvasGroup>(), 0.5f);
                break;
            case MainMenuState.Settings:
                FadeOutMain(0.5f);
                _settingsPanel.SetCurrentOption(SettingsMenuPanel.SettingsMenuOption.Back);
                FadeInPanel(_settingsPanel.GetComponent<CanvasGroup>(), 0.5f, () => { _settingsPanel.SetInteractable(true); });
                break;
            case MainMenuState.Resources:
                FadeOutMain(0.5f);
                _resourcesPanel.SetCurrentOption(ResourcesMenuPanel.ResourcesMenuOption.Back);
                FadeInPanel(_resourcesPanel.GetComponent<CanvasGroup>(), 0.5f, () => { _resourcesPanel.SetInteractable(true); });
                break;
            case MainMenuState.Credits:
                FadeOutMain(0.5f);
                _creditsPanel.SetCurrentOption(CreditsMenuPanel.CreditsMenuOption.Back);
                FadeInPanel(_creditsPanel.GetComponent<CanvasGroup>(), 0.5f, () => { _creditsPanel.SetInteractable(true); });
                break;
        }
    }

    private void FadeInPanel(CanvasGroup panel, float fadeTime, Action onDone = null)
    {
        DOVirtual.Float(panel.alpha, 1, fadeTime, (x) =>
        {
            panel.alpha = x;
        })
        .SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            onDone?.Invoke();
        });
    }

    private void FadeOutPanel(CanvasGroup panel, float fadeTime, Action onDone = null)
    {
        DOVirtual.Float(panel.alpha, 0, fadeTime, (x) =>
        {
            panel.alpha = x;
        })
        .SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            onDone?.Invoke();
        });
    }

    private void FadeInMain(float fadeTime)
    {
        DOVirtual.Float(0, 1, fadeTime, (x) =>
        {
            _buttons.alpha = x;
            _title.color = new Color(1, 1, 1, x);
        })
        .SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            _isInteractable = true;
        });
    }

    private void FadeOutMain(float fadeTime)
    {
        DOVirtual.Float(1, 0, fadeTime, (x) =>
        {
            _buttons.alpha = x;
            _title.color = new Color(1, 1, 1, x);
        })
        .SetEase(Ease.InOutQuad);
    }
}
