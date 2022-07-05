using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseInputManager : MonoBehaviour
{

    [SerializeField] private GameObject _pauseMenu;

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus)
        {
            Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(InputController.Instance.GetButtonDown(InputController.eButtons.Cancel))
        {
            Pause();
        }
    }

    private void Pause()
    {
        if(!PauseController.IsPaused && GameController.Instance.CanPause())
        {
            GameObject.Instantiate(_pauseMenu);
            PauseController.SetPause(true);
        }
    }
}
