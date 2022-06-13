using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    private const float AXIS_DEAD_ZONE = 0.7f;

    private bool _upAxisDown;
    private bool _downAxisDown;
    private bool _rightAxisDown;
    private bool _leftAxisDown;


    private bool _upAxisUsed;
    private bool _downAxisUsed;
    private bool _rightAxisUsed;
    private bool _leftAxisUsed;

    private InputController _inputController;

    private void Awake()
    {
        _inputController = InputController.Instance;
    }

    private void Update()
    {
        AxisInput horizontal = _inputController.GetAxis(InputController.eAxis.Horizontal);
        AxisInput vertical = _inputController.GetAxis(InputController.eAxis.Vertical);
        AxisInput scrollWheel = _inputController.GetAxis(InputController.eAxis.ScrollWheel);

        if (horizontal.Value > AXIS_DEAD_ZONE)
        {
            _rightAxisDown = true;
        }
        else
        {
            _rightAxisDown = false;
            _rightAxisUsed = false;
        }

        if (vertical.Value > AXIS_DEAD_ZONE || scrollWheel.IsPositive)
        {
            _upAxisDown = true;
        }
        else
        {
            _upAxisDown = false;
            _upAxisUsed = false;
        }

        if (horizontal.Value < -AXIS_DEAD_ZONE)
        {
            _leftAxisDown = true;
        }
        else
        {
            _leftAxisDown = false;
            _leftAxisUsed = false;
        }

        if (vertical.Value < -AXIS_DEAD_ZONE || scrollWheel.IsNegative)
        {
            _downAxisDown = true;
        }
        else
        {
            _downAxisDown = false;
            _downAxisUsed = false;
        }
    }


    public bool GetRightDown()
    {
        if (_rightAxisDown && !_rightAxisUsed)
        {
            _rightAxisUsed = true;
            return true;
        }

        return false;
    }

    public bool GetLeftDown()
    {
        if (_leftAxisDown && !_leftAxisUsed)
        {
            _leftAxisUsed = true;
            return true;
        }

        return false;
    }

    public bool GetUpDown()
    {
        if (_upAxisDown && !_upAxisUsed)
        {
            _upAxisUsed = true;
            return true;
        }

        return false;
    }

    public bool GetDownDown()
    {
        if (_downAxisDown && !_downAxisUsed)
        {
            _downAxisUsed = true;
            return true;
        }

        return false;
    }

    public bool GetEscapeDown()
    {
        return _inputController.GetButtonDown(InputController.eButtons.Cancel);
    }

    public bool GetSelectDown()
    {
        return
        _inputController.GetButtonDown(InputController.eButtons.Jump)
        || _inputController.GetButtonDown(InputController.eButtons.Interact)
        || _inputController.GetButtonDown(InputController.eButtons.Attack)
        || _inputController.GetButtonDown(InputController.eButtons.Submit);
    }
}
