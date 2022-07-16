using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BugsController : MonoBehaviour
{
    private const float BUGS_MOVE_SPEED = 4f;
    private const float BUGS_RADIUS_LERP_SPEED = 0.5f;

    private const float MAX_BUGS_RADIUS = 30f;
    private readonly Vector2 BUG_OFFSET_AMOUNT = new Vector2(6f, 1f);
    private readonly Vector2 BUG_SPEED = new Vector2(0.75f, 2.5f);

    [SerializeField] private List<SpriteRenderer> _bugSprites;
    [SerializeField] private SpriteMask _bugMask;
    [SerializeField] private AnimationCurve _alphaCurve;

    [SerializeField] private ParticleSystem _individualBugs;
    [SerializeField] private ParticleSystem _mediumBugs;
    [SerializeField] private ParticleSystem _largeBugs;

    private List<Vector3> _bugSpritePositions = new List<Vector3>();
    private List<Vector3> _bugSpriteOffsets = new List<Vector3>();

    private Transform _playerTransform;

    private Vector3 _targetPosition;
    private Vector3 _currentPosition;

    private Particle[] _smallParticles;
    private Particle[] _mediumParticles;
    private Particle[] _largeParticles;

    private Material _scrollingBugsMaterial;

    [Range(0,1)]
    [SerializeField] private float _radius = 0;
    private float _targetRadius = 0;
    private Vector3 _velocity;

    private bool _overrideHealth = false;

    public bool OverrideHealth
    {
        get { return _overrideHealth; }
        set { _overrideHealth = value; }
    }

    private void Awake()
    {
        for (int i = 0; i < _bugSprites.Count; i++)
        {
            _bugSpritePositions.Add(_bugSprites[i].transform.localPosition);
            _bugSpriteOffsets.Add(UnityEngine.Random.insideUnitCircle);
        }
        _smallParticles = new Particle[_individualBugs.main.maxParticles];
        _mediumParticles = new Particle[_mediumBugs.main.maxParticles];
        _largeParticles = new Particle[_largeBugs.main.maxParticles];

        GameController.Instance.SetBugsController(this);
    }

    private void FixedUpdate()
    {
        _currentPosition = Vector3.SmoothDamp(_currentPosition, _targetPosition, ref _velocity, 0.1f);
        transform.position = _currentPosition;
    }

    private void Update()
    {
        float fullRadius = _alphaCurve.Evaluate(_radius) * _bugSprites.Count;

        for (int i = 0; i < _bugSprites.Count; i++)
        {
            Vector3 offset = new Vector3(Mathf.PerlinNoise(_bugSpriteOffsets[i].x + Time.time, _bugSpriteOffsets[i].x + Time.time), Mathf.PerlinNoise(_bugSpriteOffsets[i].y + Time.time, _bugSpriteOffsets[i].y + Time.time), 0) * 0.4f;
            _bugSprites[i].transform.localPosition = _bugSpritePositions[i] + offset;
        }

        _bugMask.alphaCutoff = 1 - _radius;

        int numParticles = _individualBugs.GetParticles(_smallParticles);
        
        for (int i = 0; i < numParticles; i++)
        {
            float perlinX = (Mathf.PerlinNoise(_smallParticles[i].remainingLifetime * BUG_SPEED.x, 0f) * -BUG_OFFSET_AMOUNT.x * (1 - _smallParticles[i].startSize)) + 2f - _radius;
            float perlinY = Mathf.PerlinNoise(0f, _smallParticles[i].remainingLifetime * BUG_SPEED.y) * BUG_OFFSET_AMOUNT.y;

            Vector3 position = transform.position;
            position.z = -1f;
            float perlinDirection = Mathf.PerlinNoise(_smallParticles[i].randomSeed * 0.000001f, 0.01f) * 9720f;

            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3((1 -_radius) * MAX_BUGS_RADIUS, 0, 0);
            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3(perlinX, perlinY);
            _smallParticles[i].position = position;
        }

        _individualBugs.SetParticles(_smallParticles, numParticles);

        numParticles = _mediumBugs.GetParticles(_mediumParticles);

        for (int i = 0; i < numParticles;i++)
        {
            float perlinX = (Mathf.PerlinNoise(_mediumParticles[i].remainingLifetime * BUG_SPEED.x, 0f) * -BUG_OFFSET_AMOUNT.x * 0.5f) + 3.5f;
            float perlinY = Mathf.PerlinNoise(0f, _mediumParticles[i].remainingLifetime * BUG_SPEED.y) * BUG_OFFSET_AMOUNT.y;

            Vector3 position = transform.position;
            float perlinDirection = Mathf.PerlinNoise(_mediumParticles[i].randomSeed * 0.000001f, 0.01f) * 9620f;

            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3((1 - _radius) * MAX_BUGS_RADIUS, 0, 0);
            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3(perlinX, perlinY);
            _mediumParticles[i].position = position;
        }

        _mediumBugs.SetParticles(_mediumParticles, numParticles);

        numParticles = _largeBugs.GetParticles(_largeParticles);

        for (int i = 0; i < numParticles; i++)
        {
            float perlinX = (Mathf.PerlinNoise(_largeParticles[i].remainingLifetime * BUG_SPEED.x, 0f) * -BUG_OFFSET_AMOUNT.x * 0.5f) + 4.5f;
            float perlinY = Mathf.PerlinNoise(0f, _largeParticles[i].remainingLifetime * BUG_SPEED.y) * BUG_OFFSET_AMOUNT.y;

            Vector3 position = transform.position;
            float perlinDirection = Mathf.PerlinNoise(_largeParticles[i].randomSeed * 0.000001f, 0.01f) * 9520f;

            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3((1 - _radius) * MAX_BUGS_RADIUS, 0, 0);
            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3(perlinX, perlinY);
            _largeParticles[i].position = position;
        }

        _largeBugs.SetParticles(_largeParticles, numParticles);

        if (_playerTransform != null)
        {
            _targetPosition = _playerTransform.position;
        }

        _radius = Mathf.Lerp(_radius, _targetRadius, Time.deltaTime * BUGS_RADIUS_LERP_SPEED);
    }

    public void ForceTargetPosition()
    {
        if (_playerTransform != null)
        {
            _targetPosition = _playerTransform.position;
        }
        _currentPosition = _targetPosition;

        transform.position = _currentPosition;
    }

    public void ForceRadius()
    {
        _radius = _targetRadius;
    }

    public void SetPlayerTransform(Transform player)
    {
        _playerTransform = player;
    }

    public void SetRadius(float radius, bool instant = false)
    {
        _targetRadius = radius;

        if (instant)
        {
            _radius = _targetRadius;
        }
    }
}
