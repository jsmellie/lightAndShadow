using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuPanel : MonoBehaviour
{
    public enum CreditsMenuOption
    {
        Back = 0
    }

    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private MenuInputController _menuInputController;

    private CreditsMenuOption _currentOption = CreditsMenuOption.Back;

    private bool _isInteractable = false;

    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
    }

    private void Update()
    {
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
            SetCurrentOption(CreditsMenuOption.Back);
            SelectMenuOption();
            return;
        }
    }

    public void SetCurrentOption(CreditsMenuOption option)
    {
        _currentOption = option;
    }

    private void SelectMenuOption(bool isPositive = true)
    {
        switch (_currentOption)
        {
            case CreditsMenuOption.Back:
                _isInteractable = false;
                _mainMenuController.SetCurrentMenuState(MainMenuController.MainMenuState.Main);
                break;
        }
    }
}
