using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : SingletonBehaviour<CollectableManager>
{
    private readonly Vector2 COLLECTABLE_WAVE_SPEED = new Vector2(3, 5);
    private readonly Vector2 COLLECTABLE_WAVE_AMOUNT = new Vector2(0.5f, 0.5f);

    private float COLLECTABLE_LERP_SPEED = 1f;

    List<CollectableSpawner> _spawners = new List<CollectableSpawner>();

    private Camera _mainCamera;

    protected override void Initialize()
    {

    }

    public void Register(CollectableSpawner spawner)
    {
        if(!_spawners.Contains(spawner))
        {
            _spawners.Add(spawner);
        }
    }

    public void Unregister(CollectableSpawner spawner)
    {
        _spawners.Remove(spawner);
    }

    public void Spawn()
    {
        if (_mainCamera == null)
        {
            _mainCamera = CameraController.Instance.GetCamera(CameraController.GAMEPLAY_CAMERA_ID);
        }

        foreach (var spawner in _spawners)
        {
            spawner.Spawn();
        }
    }

    public void Update()
    {
        if (_mainCamera != null)
        {
            Vector3 cameraPosition = _mainCamera.transform.position;

            for (int i = 0; i < _spawners.Count; i++)
            {
                Vector3 spawnerPosition = _spawners[i].transform.position;
                Vector3 position = spawnerPosition;

                if (_spawners[i].Collectable.CanCollect())
                {
                    if (_spawners[i].Collectable.IsOriginVisible())
                    {
                        position.x += Mathf.Sin((Time.time + (_spawners[i].transform.position.x * _spawners[i].transform.position.y)) * COLLECTABLE_WAVE_SPEED.x) * COLLECTABLE_WAVE_AMOUNT.x;
                        position.y += Mathf.Cos((Time.time + (_spawners[i].transform.position.x * _spawners[i].transform.position.y)) * COLLECTABLE_WAVE_SPEED.y) * COLLECTABLE_WAVE_AMOUNT.y;

                        position = Vector3.Lerp(_spawners[i].Collectable.GetVisualPosition(), position, COLLECTABLE_LERP_SPEED * Time.deltaTime);
                    }
                    else
                    {
                        cameraPosition.z = _spawners[i].transform.position.z;

                        Vector3 targetPosition = ((_spawners[i].transform.position - cameraPosition) * 1000).normalized * 50;

                        position = Vector3.Lerp(_spawners[i].Collectable.GetVisualPosition(), position, COLLECTABLE_LERP_SPEED * Time.deltaTime);
                    }

                    _spawners[i].Collectable.SetVisualPosition(position);
                }
            }
        }
    }
}
