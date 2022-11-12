using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroller : MonoBehaviour
{

    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private CanvasGroup _group;

    private float _timer = 0f;
    private void Start()
    {
        _curve.postWrapMode = WrapMode.Loop;
        _timer = 0;
    }

    public void BackToTop(bool fade = false)
    {
        if (fade)
        {
            DOVirtual.Float(1, 0, 0.5f, (x) =>
            {
                _group.alpha = x;
            })
            .SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                _timer = -0.5f;
                DOVirtual.Float(0, 1, 0.5f, (x) =>
                {
                    _group.alpha = x;
                })
                .SetEase(Ease.InOutQuad);
            });
        }
        else
        {
            _timer = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_scrollRect.gameObject.activeInHierarchy)
        {
            _timer += Time.deltaTime;
            _scrollRect.verticalNormalizedPosition = _curve.Evaluate(_timer);

            if (_timer >= 60f)
            {
                BackToTop(true);
            }
        }
    }
}
