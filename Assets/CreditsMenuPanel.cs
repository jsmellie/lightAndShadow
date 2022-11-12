using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

public class CreditsMenuPanel : MonoBehaviour
{
    public enum CreditsMenuOption
    {
        Back = 0
    }

    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private MenuInputController _menuInputController;

    [SerializeField] private RectTransform _creditsSectionParent;
    [SerializeField] private RectTransform _layoutGroup;

    private CreditsMenuOption _currentOption = CreditsMenuOption.Back;

    private bool _isInteractable = false;

    private bool _isLoaded = false;

    public void SetInteractable(bool interactable)
    {
        _isInteractable = interactable;
    }

    private void Awake()
    {
        if (!_isLoaded)
        {
            _isLoaded = true;
            CreditsSection creditsSectionPrefab = Resources.Load<CreditsSection>("CreditsSection");
            List<CreditsSectionData> creditsSections = JsonConvert.DeserializeObject<List<CreditsSectionData>>(Resources.Load<TextAsset>("CreditsData").text);
            foreach (CreditsSectionData data in creditsSections)
            {
                CreditsSection section = GameObject.Instantiate<CreditsSection>(creditsSectionPrefab);
                section.GetComponent<RectTransform>().SetParent(_creditsSectionParent);
                section.SetData(data);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup);
        }
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
