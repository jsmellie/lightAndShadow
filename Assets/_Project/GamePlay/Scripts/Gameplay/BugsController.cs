using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugsController : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _bugSprites;
    [SerializeField] private SpriteMask _bugMask;
    [SerializeField] private AnimationCurve _alphaCurve;

    private List<Vector3> _bugSpritePositions = new List<Vector3>();
    private List<Vector3> _bugSpriteOffsets = new List<Vector3>();

    [Range(0,1)]
    [SerializeField] private float _radius = 0;

    private void Awake()
    {
        for (int i = 0; i < _bugSprites.Count; i++)
        {
            _bugSpritePositions.Add(_bugSprites[i].transform.localPosition);
            _bugSpriteOffsets.Add(UnityEngine.Random.insideUnitCircle);
        }
    }

    private void Update()
    {
        float fullRadius = _alphaCurve.Evaluate(_radius) * _bugSprites.Count;

        for (int i = 0; i < _bugSprites.Count; i++)
        {
            Vector3 offset = new Vector3(Mathf.PerlinNoise(_bugSpriteOffsets[i].x + Time.time, _bugSpriteOffsets[i].x + Time.time), Mathf.PerlinNoise(_bugSpriteOffsets[i].y + Time.time, _bugSpriteOffsets[i].y + Time.time), 0) * 0.4f;
            _bugSprites[i].transform.localPosition = _bugSpritePositions[i] + offset;

            float alpha = fullRadius;
            fullRadius /= 2;
            _bugSprites[i].color = new Color(0, 0, 0, alpha);
        }

        _bugMask.alphaCutoff = 1 - _radius;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }
}
