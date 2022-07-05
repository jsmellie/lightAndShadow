using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private List<Sprite> _spriteOptions;
    [SerializeField] private Transform _spriteStickTarget;

    void Start()
    {
        int random = Random.Range(0,_spriteOptions.Count);
        PickSprite(random);
        StickController.Instance.AddStickTarget(_spriteStickTarget, StickController.StickDirection.Closest);
    }

    private void PickSprite(int random)
    {
        _renderer.sprite = _spriteOptions[random];
    }
}
