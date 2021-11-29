using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
