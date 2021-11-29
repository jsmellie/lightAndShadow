using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToggler : MonoBehaviour
{
    [SerializeField] private GameObject _toggleableobject;
    [SerializeField] private int _attemptsUntilToggle = 1;
    private int _attemptCounter = 0;
    public void AttemptToggleObject()
    {
        _attemptCounter = (_attemptCounter + 1) % _attemptsUntilToggle;

        if (_attemptCounter == 0)
        {
            ToggleObject();
        }
    }

    private void ToggleObject()
    {
        _toggleableobject.SetActive(!_toggleableobject.activeSelf);
    }
}
