using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisInput
{
    #region Variables and Properties

    //Constants

    //Enums and Structs

    //Public

    //Protected
    protected InputController.eAxis m_Axis;
    protected float m_Value;
    protected bool m_IsShiftModified;
    protected bool m_IsControlModified;
    protected bool m_IsAltModified;

    //Private

    //Properties
    public InputController.eAxis Button
    {
        get { return m_Axis; }
        set { m_Axis = value; }
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

    public float Value
    {
        get { return m_Value; }
        set { m_Value = value; }
    }

    public bool IsNegative
    {
        get { return m_Value < 0; }
    }

    public bool IsPositive
    {
        get { return m_Value > 0; }
    }

    #endregion

    #region Unity API

    #endregion

    #region Public Functions	

    public AxisInput(InputController.eAxis axis, bool[] modifiers, float value = 0)
    {
        m_Axis = axis;

        m_IsShiftModified = modifiers[0];

        m_IsControlModified = modifiers[1];

        m_IsAltModified = modifiers[2];

        m_Value = value;
    }

    public static implicit operator bool(AxisInput axisInput)
    {
        return !ReferenceEquals(axisInput, null) && Mathf.Approximately(axisInput.m_Value, 0f);
    }

    #endregion

    #region Protected Functions

    #endregion

    #region Private Functions

    #endregion
}
