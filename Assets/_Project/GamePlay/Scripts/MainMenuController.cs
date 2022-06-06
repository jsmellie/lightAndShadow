using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    private enum MainMenuOption
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

    [SerializeField] private AssetReference _firstScene;

    [SerializeField] private SpriteRenderer _title;
    [SerializeField] private CanvasGroup _buttons;

    private bool _isInteractable = false;

    private MainMenuOption _currentOption;

    private List<Vector3> _optionTargetScale = new List<Vector3>();
    private Vector3 _optionsTargetPosition;

    private bool _rightAxisUsed = false;
    private bool _leftAxisUsed = false;


    private void Awake()
    {
        for (int i = 0; i < _mainMenuOptions.Count;i++)
        {
            _optionTargetScale.Add(Vector3.one);
        }

        _buttons.alpha = 0;
        
        SetCurrentOption(MainMenuOption.Play);

        CutsceneController.Instance.LoopMainMenu();
        CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).gameObject.SetActive(false);
        FullScreenWipe.FadeIn(0);
        FullScreenWipe.FadeOut(1, () =>
        {
            DOVirtual.Float(0, 1, 1, (x) =>
            {
                _buttons.alpha = x;
            })
            .SetDelay(2)
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
        .SetEase(Ease.InOutQuad);

        CutsceneController.Instance.QueueCutscene1(() =>
        {
            FullScreenWipe.FadeIn(1, () =>
            {
                CameraController.Instance.GetCamera(CameraController.VIDEO_CAMERA_ID).gameObject.SetActive(false);
                AddressableSceneManager.Instance.LoadScene(_firstScene, LoadSceneMode.Single);
            });
        });
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

        if (!_isInteractable)
        {
            return;
        }

        InputController inputController = InputController.Instance;

        if (inputController.GetButtonDown(InputController.eButtons.Jump) || inputController.GetButtonDown(InputController.eButtons.Interact) || inputController.GetButtonDown(InputController.eButtons.Attack))
        {
            SelectMenuOption();
        }

        AxisInput horizontal = inputController.GetAxis(InputController.eAxis.Horizontal);
        AxisInput vertical = inputController.GetAxis(InputController.eAxis.Vertical);
        AxisInput scrollWheel = inputController.GetAxis(InputController.eAxis.ScrollWheel);

        if (horizontal.IsPositive || vertical.IsPositive || scrollWheel.IsPositive)
        {
            if (!_rightAxisUsed)
            {
                _rightAxisUsed = true;
                NextOption();
            }
        }
        else
        {
            _rightAxisUsed = false;
        }

        if (horizontal.IsNegative || vertical.IsNegative || scrollWheel.IsNegative)
        {
            if (!_leftAxisUsed)
            {
                _leftAxisUsed = true;
                PreviousOption();
            }
        }
        else
        {
            _leftAxisUsed = false;
        }
    }

    private void SelectMenuOption()
    {
        switch(_currentOption)
        {
            case MainMenuOption.Play:
                LoadGameScene();
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

        totalWidth += (1.5f * 100f)/2f;

        Vector3 position = _mainMenuOptionsParent.localPosition;
        position.x = -totalWidth;

        _optionsTargetPosition = position;
    }
}
