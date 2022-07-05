using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Collectable : BaseTrigger
{
    [SerializeField] protected ParticleSystem _particles;
    [SerializeField] protected Collider _collider;
    [SerializeField] protected GameObject _visual;
    [SerializeField] protected Renderer _originVisual;
    [SerializeField] protected int _healthRestored = 10;

    private bool _canCollect = true;

    public bool CanCollect()
    {
        return _canCollect;
    }

    public bool IsOriginVisible()
    {
        return _originVisual.isVisible;
    }

    public Vector3 GetVisualPosition()
    {
        return _visual.transform.position;
    }

    public void SetVisualPosition(Vector3 position)
    {
        _visual.transform.position = position;
    }

    public override void OnTriggerEnter(Collider collider)
    {
        if (!_canCollect) return;

        _canCollect = false;

        if (_particles != null)
        {
            _particles.Play();
        }

        if (_collider != null)
        {
            _collider.enabled = false;
        }

        Vector3 cameraPosition = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID).transform.position;
        cameraPosition.z = _visual.transform.position.z;

        Vector3 targetPosition = ((_visual.transform.position - cameraPosition)*1000).normalized * 10;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_visual.transform.DOMove(_visual.transform.position + targetPosition, 1.2f).SetEase(Ease.InBack));
        sequence.Append(_visual.transform.DOMove(_visual.transform.position + (targetPosition * 10), 2f).SetEase(Ease.Linear));
        sequence.AppendCallback(() =>
        {
            _visual.SetActive(false);
        });

        sequence.Play();

        PlayerHealthController.Instance.MakeSafe();
        PlayerHealthController.Instance.Heal(_healthRestored);

        base.OnTriggerEnter(collider);
    }
}
