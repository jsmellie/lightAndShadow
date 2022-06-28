using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseInputManager : MonoBehaviour
{

    [SerializeField] private GameObject _pauseMenu;
    // Update is called once per frame
    void Update()
    {
        if(InputController.Instance.GetButtonDown(InputController.eButtons.Cancel))
        {
            if(!PauseController.IsPaused)
            {

                GameObject.Instantiate(_pauseMenu);
                PauseController.SetPause(true);
            }
        }
    }
}
