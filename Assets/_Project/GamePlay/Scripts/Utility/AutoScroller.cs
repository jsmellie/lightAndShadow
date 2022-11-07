using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroller : MonoBehaviour
{

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private AnimationCurve _curve;

    private float _timer = 0f;
    private void Start()
    {
        _curve.postWrapMode = WrapMode.Loop;
        _timer = 0;
    }

    public void BackToTop()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_scrollRect.gameObject.activeInHierarchy)
        {
            _timer += Time.deltaTime;
            _scrollRect.verticalNormalizedPosition = _curve.Evaluate(_timer);
        }
    }
}
