using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController
{
	#region Variables and Properties

	//Constants

	//Enums and Structs
	public enum eButtons
	{
		Interact,
		Phone,
		PhoneTab1,
		PhoneTab2,
		PhoneTab3,
		PhoneTab4,
		PhoneTab5,
		Attack,
		Jump,
		DropDown,
        Cancel,
        Submit,
		MenuRight,
		MenuLeft,
		MenuUp,
		MenuDown
	}

	public enum eAxis
	{
		Horizontal,
		Vertical,
        ScrollWheel
	}

	public enum eButtonModifiers
	{
		Shift,
		Ctrl,
		Alt
	}

	//Public

	//Protected
	protected static InputController m_Instance;

	//Private

	//Properties
	public static InputController Instance
	{
		get
		{
			if (m_Instance == null)
			{
				m_Instance = new InputController();
			}

			return m_Instance;
		}
	}

	#endregion

	#region Unity API

	#endregion

	#region Public Functions
	public AxisInput GetAxis(eAxis axis)
	{
        AxisInput axisInput = new AxisInput(axis, new bool[] { GetShift(), GetControl(), GetAlt() });

        axisInput.Value = Input.GetAxis(axis.ToString());

        return axisInput;
    }

	public Vector2 GetMousePosition()
	{
        return Input.mousePosition;
    }

	public ButtonInput GetButton(eButtons button)
	{
		ButtonInput buttonInput = new ButtonInput(button, new bool[] { GetShift(), GetControl(), GetAlt() });

		buttonInput.IsFired = Input.GetButton(button.ToString());

		return buttonInput;
	}

	public bool GetButtonDown(eButtons button)
	{
		ButtonInput buttonInput = new ButtonInput(button, new bool[] { GetShift(), GetControl(), GetAlt() });

		buttonInput.IsFired = Input.GetButtonDown(button.ToString());

		return buttonInput;
	}

	public bool GetButtonUp(eButtons button)
	{
		ButtonInput buttonInput = new ButtonInput(button, new bool[] { GetShift(), GetControl(), GetAlt() });

		buttonInput.IsFired = Input.GetButtonUp(button.ToString());

		return buttonInput;
	}

	#endregion

	#region Protected Functions

	#endregion

	#region Private Functions

	private bool GetShift()
	{
		return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
	}

	private bool GetControl()
	{
		return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
	}

	private bool GetAlt()
	{
		return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
	}
	#endregion
}
