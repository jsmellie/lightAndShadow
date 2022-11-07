using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkClicker : MonoBehaviour
{
    [SerializeField] private string _url;
    public void OnClicked()
    {
        Application.OpenURL(_url);
    }
}
