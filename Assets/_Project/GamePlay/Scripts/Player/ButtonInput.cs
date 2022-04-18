using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInput
{
	#region Variables and Properties

	//Constants

	//Enums and Structs

	//Public

	//Protected
	protected InputController.eButtons m_Button;
	protected bool m_IsFired;
	protected bool m_IsShiftModified;
	protected bool m_IsControlModified;
	protected bool m_IsAltModified;

	//Private

	//Properties
	public InputController.eButtons Button
	{
		get { return m_Button; }
		set { m_Button = value; }
	}

	public bool IsShiftModified
	{
		get { return m_IsShiftModified; }
		set { m_IsShiftModified = value; }
	}

	public bool IsControlModified
	{
		get { return m_IsControlModified; }
		set { m_IsControlModified = value; }
	}

	public bool IsAltModified
	{
		get { return m_IsAltModified; }
		set { m_IsAltModified = value; }
	}

	public bool IsFired
	{
		get { return m_IsFired; }
		set { m_IsFired = value; }
	}

	#endregion

	#region Unity API

	#endregion

	#region Public Functions	

	public ButtonInput(InputController.eButtons button, bool[] modifiers, bool isFired = false)
	{
		m_Button = button;

		m_IsShiftModified = modifiers[0];

		m_IsControlModified = modifiers[1];

		m_IsAltModified = modifiers[2];

		m_IsFired = isFired;
	}

	public static implicit operator bool(ButtonInput buttonInput)
	{
		return !ReferenceEquals(buttonInput, null) && buttonInput.IsFired;
	}

	#endregion

	#region Protected Functions

	#endregion

	#region Private Functions

	#endregion
}
