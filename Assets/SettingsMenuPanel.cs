using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenuPanel : MonoBehaviour
{
    private const string MUSIC_VOLUME_KEY = "MUSIC_VOLUME";
    private const string SOUNDS_VOLUME_KEY = "SOUNDS_VOLUME";

    private const float MENU_OPTION_SCALE_LERP_SPEED = 10;

    public enum SettingsMenuOption
    {
        Music = 0,
        Sounds = 1,
        Back = 2
    }

    [SerializeField] private GameObject _musicSelection;
    [SerializeField] private GameObject _soundSelection;

    [SerializeField] private Image _musicBars;
    [SerializeField] private Image _soundBars;

    [SerializeField] private Image _backButton;

    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private MenuInputController _menuInputController;

    private SettingsMenuOption _currentOption = SettingsMenuOption.Music;

    private int _musicVolume = 4;
    private int _soundVolume = 4;

    private bool _isInteractable = false;
    private bool _rightAxisUsed = false;
    private bool _leftAxisUsed = false;
    private bool _upAxisUsed = false;
    private bool _downAxisUsed = false;

    private void Awake()
    {
        _musicVolume = PlayerPrefs.GetInt(MUSIC_VOLUME_KEY, 4);
        _soundVolume = PlayerPrefs.GetInt(SOUNDS_VOLUME_KEY, 4);

        UpdateBarFillAmount();
        UpdateAudioLevels();
    }
    
    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
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
            SetCurrentOption(SettingsMenuOption.Back);
            SelectMenuOption();
            return;
        }

        if (_menuInputController.GetRightDown())
        {
            AdjustMenuOption(true);
            return;
        }

        if (_menuInputController.GetLeftDown())
        {
            AdjustMenuOption(false);
            return;
        }

        if (_menuInputController.GetUpDown())
        {
            PreviousOption();
            return;
        }

        if (_menuInputController.GetDownDown())
        {
            NextOption();
            return;
        }
    }

    private void SelectMenuOption(bool isPositive = true)
    {
        switch(_currentOption)
        {
            case SettingsMenuOption.Back:
                _isInteractable = false;
                _mainMenuController.SetCurrentMenuState(MainMenuController.MainMenuState.Main);
                break;
            case SettingsMenuOption.Music:
                IncreaseMusicVolume(true);
                break;
            case SettingsMenuOption.Sounds:
                IncreaseSoundVolume(true);
                break;
        }

        UpdateAudioLevels();
        UpdateBarFillAmount();
    }

    private void AdjustMenuOption(bool isPositive)
    {
        switch (_currentOption)
        {
            case SettingsMenuOption.Back:
                break;
            case SettingsMenuOption.Music:
                if (isPositive)
                    IncreaseMusicVolume();
                else
                    DecreaseMusicVolume();

                break;
            case SettingsMenuOption.Sounds:
                if (isPositive)
                    IncreaseSoundVolume();
                else
                    DecreaseSoundVolume();
                break;
        }

        UpdateAudioLevels();
        UpdateBarFillAmount();
    }


    private void IncreaseMusicVolume(bool wrapAround = false)
    {
        if (_musicVolume < 4)
        {
            _musicVolume++;
        }
        else if (wrapAround)
        {
            _musicVolume = 0;
        }        
    }

    private void DecreaseMusicVolume(bool wrapAround = false)
    {
        if (_musicVolume > 0)
        {
            _musicVolume--;
        }
        else if (wrapAround)
        {
            _musicVolume = 4;
        }
    }

    private void IncreaseSoundVolume(bool wrapAround = false)
    {
        if (_soundVolume < 4)
        {
            _soundVolume++;
        }
        else if (wrapAround)
        {
            _soundVolume = 0;
        }
    }

    private void DecreaseSoundVolume(bool wrapAround = false)
    {
        if (_soundVolume > 0)
        {
            _soundVolume--;
        }
        else if (wrapAround)
        {
            _soundVolume = 4;
        }
    }

    private void NextOption()
    {
        if ((int)_currentOption < (int)SettingsMenuOption.Back)
        {
            SetCurrentOption((SettingsMenuOption)((int)_currentOption + 1));
        }
        else
        {
            SetCurrentOption(SettingsMenuOption.Music);
        }
    }

    private void PreviousOption()
    {
        if ((int)_currentOption > (int)SettingsMenuOption.Music)
        {
            SetCurrentOption((SettingsMenuOption)((int)_currentOption - 1));
        }
        else
        {
            SetCurrentOption(SettingsMenuOption.Back);
        }
    }

    public void SetCurrentOption(SettingsMenuOption option)
    {
        _currentOption = option;
    }

    private void UpdateSelectionAlpha()
    {
        for (int i = 0; i <= (int)SettingsMenuOption.Back; i++)
        {
            if ((int)_currentOption == i)
            {
                float alpha = 0;

                switch ((SettingsMenuOption)i)
                {
                    case SettingsMenuOption.Back:
                        _backButton.color = Color.Lerp(_backButton.color, new Color(1, 1, 1, 1), Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _backButton.transform.localScale = Vector3.Lerp(_backButton.transform.localScale, Vector3.one * 1.25f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                    case SettingsMenuOption.Music:
                        alpha = _musicSelection.GetComponent<CanvasGroup>().alpha;
                        _musicSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(alpha, 1, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _musicSelection.transform.localScale = Vector3.Lerp(_musicSelection.transform.localScale, Vector3.one * 1.1f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                    case SettingsMenuOption.Sounds:
                        alpha = _soundSelection.GetComponent<CanvasGroup>().alpha;
                        _soundSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(alpha, 1, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _soundSelection.transform.localScale = Vector3.Lerp(_soundSelection.transform.localScale, Vector3.one * 1.1f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                }
            }
            else
            {
                float alpha = 0;

                switch ((SettingsMenuOption)i)
                {
                    case SettingsMenuOption.Back:
                        _backButton.color = Color.Lerp(_backButton.color, new Color(1, 1, 1, 0.3f), Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _backButton.transform.localScale = Vector3.Lerp(_backButton.transform.localScale, Vector3.one, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                    case SettingsMenuOption.Music:
                        alpha = _musicSelection.GetComponent<CanvasGroup>().alpha;
                        _musicSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(alpha, 0.3f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _musicSelection.transform.localScale = Vector3.Lerp(_musicSelection.transform.localScale, Vector3.one, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                    case SettingsMenuOption.Sounds:
                        alpha = _soundSelection.GetComponent<CanvasGroup>().alpha;
                        _soundSelection.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(alpha, 0.3f, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        _soundSelection.transform.localScale = Vector3.Lerp(_soundSelection.transform.localScale, Vector3.one, Time.unscaledDeltaTime * MENU_OPTION_SCALE_LERP_SPEED);
                        break;
                }
            }
        }
    }

    private void UpdateAudioLevels()
    {
        _audioMixer.SetFloat(MUSIC_VOLUME_KEY, -80f + (20f * _musicVolume));
        _audioMixer.SetFloat(SOUNDS_VOLUME_KEY, -80f + (20f * _soundVolume));

        PlayerPrefs.SetInt(MUSIC_VOLUME_KEY, _musicVolume);
        PlayerPrefs.SetInt(SOUNDS_VOLUME_KEY, _soundVolume);
    }

    private void UpdateBarFillAmount()
    {
        _musicBars.fillAmount = _musicVolume * 0.25f;
        _soundBars.fillAmount = _soundVolume * 0.25f;
    }
}
