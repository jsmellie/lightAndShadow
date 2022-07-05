using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickController : SingletonBehaviour<StickController>
{
    private const float CLOSEST_STICK_LERP_SPEED = 1f;

    public enum StickDirection
    {
        Up,
        Down,
        Left,
        Right,
        Closest
    }

    public class StickInfo
    {
        public Transform Target;
        public Vector3 OriginOffset;
        public StickDirection StickDirection;
    }

    private List<StickInfo> _downSticks = new List<StickInfo>();
    private List<StickInfo> _leftSticks = new List<StickInfo>();
    private List<StickInfo> _rightSticks = new List<StickInfo>();
    private List<StickInfo> _upSticks = new List<StickInfo>();
    private List<StickInfo> _closestSticks = new List<StickInfo>();

    [SerializeField] private LineRenderer _downRenderer;
    [SerializeField] private LineRenderer _upRenderer;
    [SerializeField] private LineRenderer _rightRenderer;
    [SerializeField] private LineRenderer _leftRenderer;
    [SerializeField] private List<LineRenderer> _closestRenderers;

    private List<LineRenderer> _inUseClosest = new List<LineRenderer>();
    private List<LineRenderer> _availableClosest = new List<LineRenderer>();

    private Dictionary<int, LineRenderer> _stickToRendererDictionary = new Dictionary<int, LineRenderer>();

    private Camera _mainCamera;

    protected override void Initialize()
    {
        for (int i = 0; i < _closestRenderers.Count;i++)
        {
            _closestRenderers[i].positionCount = 2;
            _availableClosest.Add(_closestRenderers[i]);
        }
    }

    public void AddStickTarget(Transform target, StickDirection direction)
    {
        AddStickTarget(target, UnityEngine.Random.Range(-10f, 10f), direction);
    }

    public void AddStickTarget(Transform target, float offset, StickDirection direction)
    {
        if (_mainCamera == null)
        {
            _mainCamera = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID);
        }

        StickInfo stickInfo = new StickInfo();
        stickInfo.Target = target;
        stickInfo.StickDirection = direction;

        switch (direction)
        {
            case StickDirection.Up:
                stickInfo.OriginOffset = new Vector3(offset, 100f, 0);
                _upSticks.Add(stickInfo);
                break;
            case StickDirection.Down:
                _downSticks.Add(stickInfo);
                stickInfo.OriginOffset = new Vector3(offset, -100f, 0);
                break;
            case StickDirection.Left:
                stickInfo.OriginOffset = new Vector3(-100f, offset, 0);
                _leftSticks.Add(stickInfo);
                break;
            case StickDirection.Right:
                stickInfo.OriginOffset = new Vector3(100f, offset, 0);
                _rightSticks.Add(stickInfo);
                break;
            case StickDirection.Closest:
                _closestSticks.Add(stickInfo);
                break;
        }
    }

    private void Update()
    {
        UpdateDownSticks();
        UpdateUpSticks();
        UpdateRightSticks();
        UpdateLeftSticks();
        UpdateClosestSticks();
    }
    
    private void UpdateDownSticks()
    {
        _downRenderer.positionCount = _downSticks.Count * 3;

        for (int i = 0; i < _downSticks.Count; i++)
        {
            int startIndex = i * 3;

            int originIndex = startIndex;
            int targetIndex = startIndex + 1;
            int backIndex = startIndex + 2;

            Vector3 targetPosition = new Vector3(0, -1000, 0);
            Vector3 originPosition = new Vector3(0, -1000, 0);

            if (_downSticks[i].Target != null && _downSticks[i].Target.gameObject.activeInHierarchy && _downSticks[i].Target.GetComponent<Renderer>().isVisible)
            {
                targetPosition = _downSticks[i].Target.position;
                originPosition = _downSticks[i].Target.position + _downSticks[i].OriginOffset;
            }

            _downRenderer.SetPosition(originIndex, originPosition);
            _downRenderer.SetPosition(targetIndex, targetPosition);
            _downRenderer.SetPosition(backIndex, originPosition);
        }
    }
    
    private void UpdateUpSticks()
    {
        _upRenderer.positionCount = _upSticks.Count * 3;

        for (int i = 0; i < _upSticks.Count; i++)
        {
            int startIndex = i * 3;

            int originIndex = startIndex;
            int targetIndex = startIndex + 1;
            int backIndex = startIndex + 2;

            Vector3 targetPosition = new Vector3(0, 1000, 0);
            Vector3 originPosition = new Vector3(0, 1000, 0);

            if (_upSticks[i].Target != null && _upSticks[i].Target.gameObject.activeInHierarchy && _upSticks[i].Target.GetComponent<Renderer>().isVisible)
            {
                targetPosition = _upSticks[i].Target.position;
                originPosition = _upSticks[i].Target.position + _upSticks[i].OriginOffset;
            }

            _upRenderer.SetPosition(originIndex, originPosition);
            _upRenderer.SetPosition(targetIndex, targetPosition);
            _upRenderer.SetPosition(backIndex, originPosition);
        }
    }

    private void UpdateRightSticks()
    {
        _rightRenderer.positionCount = _rightSticks.Count * 3;

        for (int i = 0; i < _rightSticks.Count; i++)
        {
            int startIndex = i * 3;

            int originIndex = startIndex;
            int targetIndex = startIndex + 1;
            int backIndex = startIndex + 2;

            Vector3 targetPosition = new Vector3(1000, 0, 0);
            Vector3 originPosition = new Vector3(1000, 0, 0);

            if (_rightSticks[i].Target != null && _rightSticks[i].Target.gameObject.activeInHierarchy && _rightSticks[i].Target.GetComponent<Renderer>().isVisible)
            {
                targetPosition = _rightSticks[i].Target.position;
                originPosition = _rightSticks[i].Target.position + _rightSticks[i].OriginOffset;
            }

            _rightRenderer.SetPosition(originIndex, originPosition);
            _rightRenderer.SetPosition(targetIndex, targetPosition);
            _rightRenderer.SetPosition(backIndex, originPosition);
        }
    }

    private void UpdateLeftSticks()
    {
        _leftRenderer.positionCount = _leftSticks.Count * 3;

        for (int i = 0; i < _leftSticks.Count; i++)
        {
            int startIndex = i * 3;

            int originIndex = startIndex;
            int targetIndex = startIndex + 1;
            int backIndex = startIndex + 2;

            Vector3 targetPosition = new Vector3(-1000, 0, 0);
            Vector3 originPosition = new Vector3(-1000, 0, 0);

            if (_leftSticks[i].Target != null && _leftSticks[i].Target.gameObject.activeInHierarchy && _leftSticks[i].Target.GetComponent<Renderer>().isVisible)
            {
                targetPosition = _leftSticks[i].Target.position;
                originPosition = _leftSticks[i].Target.position + _leftSticks[i].OriginOffset;
            }

            _leftRenderer.SetPosition(originIndex, originPosition);
            _leftRenderer.SetPosition(targetIndex, targetPosition);
            _leftRenderer.SetPosition(backIndex, originPosition);
        }
    }

    private void UpdateClosestSticks()
    {
        List<int> sticksInView = new List<int>();

        for (int i = 0; i < _closestSticks.Count; i++)
        {
            if (_closestSticks[i].Target != null && _closestSticks[i].Target.gameObject.activeInHierarchy)
            {
                Renderer renderer = GetRenderer(_closestSticks[i].Target);

                if (renderer != null && renderer.isVisible)
                {
                    sticksInView.Add(i);
                }
            }
        }

        if (_availableClosest.Count < sticksInView.Count)
        {
            int difference = sticksInView.Count - _availableClosest.Count;

            for (int i = 0; i < difference;i++)
            {
                GameObject newRenderer = GameObject.Instantiate<GameObject>(_closestRenderers[0].gameObject);
                newRenderer.transform.parent = transform;
                
                LineRenderer renderer = newRenderer.GetComponent<LineRenderer>();
                renderer.SetPosition(0, new Vector3(0, -1000, 0));
                renderer.SetPosition(1, new Vector3(0, -1000, 0));

                _closestRenderers.Add(renderer);
                _availableClosest.Add(renderer);
            }
        }

        ReleaseUnusedSticks(sticksInView);

        InitializeNewlyVisibleSticks(sticksInView);

        UpdateStickPositions(sticksInView);
    }

    private Renderer GetRenderer(Transform target)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = target.GetComponentInChildren<Renderer>();
        }

        if (renderer == null)
        {
            renderer = target.GetComponentInParent<Renderer>();
        }

        return renderer;
    }

    private void ReleaseUnusedSticks(List<int> sticksInView)
    {
        List<int> sticksToRelease = new List<int>();

        foreach (int inUseStick in _stickToRendererDictionary.Keys)
        {
            if (!sticksInView.Contains(inUseStick))
            {
                sticksToRelease.Add(inUseStick);
            }
        }

        for (int i = 0; i < sticksToRelease.Count; i++)
        {
            LineRenderer renderer = _stickToRendererDictionary[sticksToRelease[i]];
            renderer.SetPosition(0, new Vector3(0, -1000, 0));
            renderer.SetPosition(1, new Vector3(0, -1000, 0));
            _inUseClosest.Remove(renderer);
            _availableClosest.Add(renderer);
            _stickToRendererDictionary.Remove(sticksToRelease[i]);
        }
    }

    private void UpdateStickPositions(List<int> sticksInView)
    {
        for (int i = 0; i < sticksInView.Count; i++)
        {
            Vector3 targetPosition = _closestSticks[sticksInView[i]].Target.position;

            Vector3 cameraPosition = _mainCamera.transform.position;
            cameraPosition.z = targetPosition.z;

            Vector3 currentOriginPosition = _stickToRendererDictionary[sticksInView[i]].GetPosition(0);

            Vector3 originPosition = ((targetPosition - cameraPosition) * 1000).normalized;
            originPosition.z = 0;
            originPosition *= 50;
            originPosition = targetPosition + originPosition;

            originPosition.z = targetPosition.z;

            Vector3 finalOriginPosition = Vector3.Lerp(currentOriginPosition, originPosition, Time.deltaTime * CLOSEST_STICK_LERP_SPEED);

            if ((finalOriginPosition - cameraPosition).magnitude < 50)
            {
                finalOriginPosition = cameraPosition + (finalOriginPosition - cameraPosition).normalized * 50;
            }

            _stickToRendererDictionary[sticksInView[i]].SetPosition(0, finalOriginPosition);
            _stickToRendererDictionary[sticksInView[i]].SetPosition(1, targetPosition);
        }
    }

    private void InitializeNewlyVisibleSticks(List<int> sticksInView)
    {
        for (int i = 0; i < sticksInView.Count; i++)
        {
            if (!_stickToRendererDictionary.ContainsKey(sticksInView[i]))
            {
                LineRenderer renderer = _availableClosest[0];
                _availableClosest.RemoveAt(0);
                _inUseClosest.Add(renderer);

                _stickToRendererDictionary.Add(sticksInView[i], renderer);

                Vector3 targetPosition = _closestSticks[sticksInView[i]].Target.position;

                Vector3 cameraPosition = _mainCamera.transform.position;
                cameraPosition.z = targetPosition.z;

                Vector3 originPosition = ((targetPosition - cameraPosition) * 1000).normalized;
                originPosition.z = 0;
                originPosition *= 10;
                originPosition = targetPosition + originPosition;

                originPosition.z = targetPosition.z;

                renderer.SetPosition(0, originPosition);
                renderer.SetPosition(1, targetPosition);
            }
        }
    }
}
