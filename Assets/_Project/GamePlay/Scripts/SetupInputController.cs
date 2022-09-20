using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupInputController : MonoBehaviour
{
    private InputController _inputController;
    private void Awake()
    {
        _inputController = InputController.Instance;
    }

    public bool GetSkipButtonDown()
    {
        return
        _inputController.GetButtonDown(InputController.eButtons.Jump)
        || _inputController.GetButtonDown(InputController.eButtons.Interact)
        || _inputController.GetButtonDown(InputController.eButtons.Attack)
        || _inputController.GetButtonDown(InputController.eButtons.Submit);
    }
}
