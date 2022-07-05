using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStickController : MonoBehaviour
{
    [SerializeField] private List<Transform> _stickTransforms;

    public void AddSticks()
    {
        for (int i = 0; i < _stickTransforms.Count;i++)
        {
            StickController.Instance.AddStickTarget(_stickTransforms[i], StickController.StickDirection.Down);
        }
    }
}
