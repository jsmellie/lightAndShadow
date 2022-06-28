using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    
    void Update()
    {
        if(PauseController.IsPaused && InputController.Instance.GetButtonDown(InputController.eButtons.Cancel))
        {
            Resume();
        }
    }

    public void Resume()
    {
        PauseController.SetPause(false);
        Destroy(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
