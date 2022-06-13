using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesMenuPanel : MonoBehaviour
{
    public enum ResourcesMenuOption
    {
        Back = 0
    }

    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private MenuInputController _menuInputController;

    private ResourcesMenuOption _currentOption = ResourcesMenuOption.Back;

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
            SetCurrentOption(ResourcesMenuOption.Back);
            SelectMenuOption();
            return;
        }
    }

    public void SetCurrentOption(ResourcesMenuOption option)
    {
        _currentOption = option;
    }

    private void SelectMenuOption(bool isPositive = true)
    {
        switch (_currentOption)
        {
            case ResourcesMenuOption.Back:
                _isInteractable = false;
                _mainMenuController.SetCurrentMenuState(MainMenuController.MainMenuState.Main);
                break;
        }
    }
}
