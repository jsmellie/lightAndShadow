using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickThis : MonoBehaviour
{

    private readonly Vector2 COLLECTABLE_WAVE_SPEED = new Vector2(3, 5);
    private readonly Vector2 COLLECTABLE_WAVE_AMOUNT = new Vector2(0.5f, 0.5f);

    private float COLLECTABLE_LERP_SPEED = 1f;

    public StickController.StickDirection _stickDirection = StickController.StickDirection.Down;

    public bool _moveIt = false;
    // Start is called before the first frame update
    void Start()
    {
        StickController.Instance.AddStickTarget(transform, _stickDirection);
    }

    void Update()
    {
        if (_moveIt)
        {

            Vector3 position = transform.position;

            position.x += Mathf.Sin((Time.time + (transform.position.x * transform.position.y)) * COLLECTABLE_WAVE_SPEED.x) * COLLECTABLE_WAVE_AMOUNT.x;
            position.y += Mathf.Cos((Time.time + (transform.position.x * transform.position.y)) * COLLECTABLE_WAVE_SPEED.y) * COLLECTABLE_WAVE_AMOUNT.y;

            position = Vector3.Lerp(transform.position, position, COLLECTABLE_LERP_SPEED * Time.deltaTime);

            transform.position = position;

        }
    }
}
