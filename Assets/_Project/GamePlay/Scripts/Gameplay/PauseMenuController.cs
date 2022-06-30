using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public enum PauseMenuOption
    {
        Resume = 0,
        Quit = 1
    }

    [SerializeField] private MenuInputController _menuInputController;
    [SerializeField] private Image _resumeButton;
    [SerializeField] private Image _quitButton;

    private PauseMenuOption _currentOption = PauseMenuOption.Resume;

    private const float MENU_OPTION_SCALE_LERP_SPEED = 10;
    private bool _isInteractable = true;

    private void Awake()
    {
        _resumeButton.color = new Color(1, 1, 1, 1);
        _quitButton.color = new Color(1, 1, 1, 0.3f);

        _resumeButton.transform.localScale = Vector3.one * 1.3f;
        _quitButton.transform.localScale = Vector3.one;
    }

    private void Update()
    {
        UpdateSelectionAlpha();
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

        if (_menuInputController.GetEscapeDown())
        {
            SetCurrentOption(PauseMenuOption.Resume);
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

    private void SelectMenuOption(bool isPositive = true)
    {
        switch (_currentOption)
        {
            case PauseMenuOption.Resume:
                _isInteractable = false;
                Resume();
                break;
            case PauseMenuOption.Quit:
                Quit();
                break;
        }
    }

    private void NextOption()
    {
        if ((int)_currentOption < (int)PauseMenuOption.Quit)
        {
            SetCurrentOption((PauseMenuOption)((int)_currentOption + 1));
        }
        else
        {
            SetCurrentOption(PauseMenuOption.Resume);
        }
    }

    private void PreviousOption()
    {
        if ((int)_currentOption > (int)PauseMenuOption.Resume)
        {
            SetCurrentOption((PauseMenuOption)((int)_currentOption - 1));
        }
        else
        {
            SetCurrentOption(PauseMenuOption.Quit);
        }
    }

    private void UpdateSelectionAlpha()
    {
        for (int i = 0; i <= (int)PauseMenuOption.Quit; i++)
        {
            if ((int)_currentOption == i)
            {
                switch ((PauseMenuOption)i)
                {
                    case PauseMenuOption.Resume:
                        _resumeButton.color = Color.Lerp(_resumeButton.color, new Color(1, 1, 1, 1), Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _resumeButton.transform.localScale = Vector3.Lerp(_resumeButton.transform.localScale, Vector3.one * 1.2f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                    case PauseMenuOption.Quit:
                        _quitButton.color = Color.Lerp(_quitButton.color, new Color(1, 1, 1, 1), Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _quitButton.transform.localScale = Vector3.Lerp(_quitButton.transform.localScale, Vector3.one * 1.2f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                }
            }
            else
            {
                switch ((PauseMenuOption)i)
                {
                    case PauseMenuOption.Resume:
                        _resumeButton.color = Color.Lerp(_resumeButton.color, new Color(1, 1, 1, 0.3f), Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _resumeButton.transform.localScale = Vector3.Lerp(_resumeButton.transform.localScale, Vector3.one, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                    case PauseMenuOption.Quit:
                        _quitButton.color = Color.Lerp(_quitButton.color, new Color(1, 1, 1, 0.3f), Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _quitButton.transform.localScale = Vector3.Lerp(_quitButton.transform.localScale, Vector3.one, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                }
            }
        }
    }

    public void SetCurrentOption(PauseMenuOption option)
    {
        _currentOption = option;
    }

    public void Resume()
    {
        PauseController.SetPause(false);
        Destroy(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
