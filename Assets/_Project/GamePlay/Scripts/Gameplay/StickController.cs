using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : SingletonBehaviour<StickController>
{
    public class StickInfo
    {
        public Transform Target;
        public Vector3 OriginOffset;
    }
    [SerializeField] private List<StickInfo> _stickTargets = new List<StickInfo>();

    [SerializeField] private LineRenderer _lineRenderer;

    protected override void Initialize()
    {
        
    }

    public void AddStickTarget(Transform target)
    {
        AddStickTarget(target, UnityEngine.Random.Range(-10f, 10f));
    }

    public void AddStickTarget(Transform target, float offset)
    {
        StickInfo stickInfo = new StickInfo();
        stickInfo.Target = target;
        stickInfo.OriginOffset = new Vector3(offset, -50f, 0);

        _stickTargets.Add(stickInfo);
    }

    private void Update()
    {
        _lineRenderer.positionCount = _stickTargets.Count * 3;

        for (int i = 0; i < _stickTargets.Count;i++)
        {
            int startIndex = i * 3;

            int originIndex = startIndex;
            int targetIndex = startIndex + 1;
            int backIndex = startIndex + 2;

            Vector3 targetPosition = _stickTargets[i].Target.position;
            Vector3 originPosition = _stickTargets[i].Target.position + _stickTargets[i].OriginOffset;

            _lineRenderer.SetPosition(originIndex, originPosition);
            _lineRenderer.SetPosition(targetIndex, targetPosition);
            _lineRenderer.SetPosition(backIndex, originPosition);
        }
    }
}
