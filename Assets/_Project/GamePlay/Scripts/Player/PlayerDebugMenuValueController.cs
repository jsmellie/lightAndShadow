using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDebugMenuValueController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _submitButton;
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _defaultButton;

    [Space]
    [SerializeField] private InputField _gravityInputField;
    [SerializeField] private InputField _horizontalSpeedInputField;
    [SerializeField] private InputField _jumpSpeedInputField;
    [SerializeField] private InputField _doubleJumpSpeedInputField;

    [Header("Player")]
    [SerializeField] private Rigidbody2D _playerBody;

    private float _defaultGravity;
    private float _defaultHorizontalSpeed;
    private float _defaultJumpSpeed;
    private float _defaultDoubleJumpSpeed;

    private void Awake()
    {
        _submitButton.onClick.AddListener(SubmitValues);
        _resetButton.onClick.AddListener(ResetValues);
        _defaultButton.onClick.AddListener(DefaultValues);

        _defaultGravity = _playerBody.gravityScale;
        //_playerController.DebugGetValues(out _defaultHorizontalSpeed, out _defaultJumpSpeed, out _defaultDoubleJumpSpeed);
    }

    private void OnDestroy()
    {
        if (_submitButton != null)
        {
            _submitButton.onClick.RemoveListener(SubmitValues);
            _resetButton.onClick.RemoveListener(ResetValues);
            _defaultButton.onClick.RemoveListener(DefaultValues);
        }
    }

    private void OnEnable()
    {
        ResetValues();
    }

    private void ResetValues()
    {
        float gravity;
        float horizontalSpeed;
        float jumpSpeed;
        float doubleJumpSpeed;

        //_playerController.DebugGetValues(out horizontalSpeed, out jumpSpeed, out doubleJumpSpeed);
        gravity = _playerBody.gravityScale;

        //SetInputFields(gravity, horizontalSpeed, jumpSpeed, doubleJumpSpeed);
    }

    private void DefaultValues()
    {
        SetInputFields(_defaultGravity, _defaultHorizontalSpeed, _defaultJumpSpeed, _defaultDoubleJumpSpeed);
    }

    private void SetInputFields(float gravity, float horizontalSpeed, float jumpSpeed, float doubleJumpSpeed)
    {
        _gravityInputField.SetTextWithoutNotify(gravity.ToString());
        _horizontalSpeedInputField.SetTextWithoutNotify(horizontalSpeed.ToString());
        _jumpSpeedInputField.SetTextWithoutNotify(jumpSpeed.ToString());
        _doubleJumpSpeedInputField.SetTextWithoutNotify(doubleJumpSpeed.ToString());
    }

    private void SubmitValues()
    {
        float gravity = float.NaN;
        float horizontalSpeed = float.NaN;
        float jumpSpeed = float.NaN;
        float doubleJumpSpeed = float.NaN;

        if (float.TryParse(_gravityInputField.text, out gravity))
        {
            _playerBody.gravityScale = gravity;
        }

        if (float.TryParse(_horizontalSpeedInputField.text, out horizontalSpeed) && float.TryParse(_jumpSpeedInputField.text, out jumpSpeed) && float.TryParse(_doubleJumpSpeedInputField.text, out doubleJumpSpeed))
        {
            //_playerController.DebugSetValues(horizontalSpeed, jumpSpeed, doubleJumpSpeed);
        }
    }
}
