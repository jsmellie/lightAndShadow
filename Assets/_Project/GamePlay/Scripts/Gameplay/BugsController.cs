using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BugsController : MonoBehaviour
{
    private const float BUGS_MOVE_SPEED = 4f;

    private const float MAX_BUGS_RADIUS = 43.54f;
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

    private Material _scrollingBugsMaterial;

    [Range(0,1)]
    [SerializeField] private float _radius = 0;

    private void Awake()
    {
        _scrollingBugsMaterial = _bugSprites[0].material;

        for (int i = 0; i < _bugSprites.Count; i++)
        {
            _bugSpritePositions.Add(_bugSprites[i].transform.localPosition);
            _bugSpriteOffsets.Add(UnityEngine.Random.insideUnitCircle);

            _bugSprites[i].material = _scrollingBugsMaterial;
        }        
    }

    private void FixedUpdate()
    {
        transform.position = _currentPosition;

        _scrollingBugsMaterial.SetVector("_Offset", new Vector4(transform.position.x / transform.localScale.x, transform.position.y / transform.localScale.y, 0, 0));
    }

    private void Update()
    {
        float fullRadius = _alphaCurve.Evaluate(_radius) * _bugSprites.Count;

        for (int i = 0; i < _bugSprites.Count; i++)
        {
            Vector3 offset = new Vector3(Mathf.PerlinNoise(_bugSpriteOffsets[i].x + Time.time, _bugSpriteOffsets[i].x + Time.time), Mathf.PerlinNoise(_bugSpriteOffsets[i].y + Time.time, _bugSpriteOffsets[i].y + Time.time), 0) * 0.4f;
            _bugSprites[i].transform.localPosition = _bugSpritePositions[i] + offset;

            //float alpha = fullRadius;
            //fullRadius /= 1.5f;
            //_bugSprites[i].color = new Color(0, 0, 0, alpha);
        }

        _bugMask.alphaCutoff = 1 - _radius;

        Particle[] particles = new Particle[_individualBugs.main.maxParticles];

        int numParticles = _individualBugs.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            float perlinX = (Mathf.PerlinNoise(particles[i].remainingLifetime * BUG_SPEED.x, 0f) * -BUG_OFFSET_AMOUNT.x * (1 - particles[i].startSize)) + 2f - _radius;
            float perlinY = Mathf.PerlinNoise(0f, particles[i].remainingLifetime * BUG_SPEED.y) * BUG_OFFSET_AMOUNT.y;

            Vector3 position = transform.position;
            float perlinDirection = Mathf.PerlinNoise(particles[i].randomSeed * 0.000001f, 0.01f) * 9720f;

            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3((1 -_radius) * MAX_BUGS_RADIUS, 0, 0);
            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3(perlinX, perlinY);
            particles[i].position = position;
        }

        _individualBugs.SetParticles(particles, numParticles);

        particles = new Particle[_mediumBugs.main.maxParticles];

        numParticles = _mediumBugs.GetParticles(particles);

        for (int i = 0; i < numParticles;i++)
        {
            float perlinX = (Mathf.PerlinNoise(particles[i].remainingLifetime * BUG_SPEED.x, 0f) * -BUG_OFFSET_AMOUNT.x * 0.5f) + 3.5f;
            float perlinY = Mathf.PerlinNoise(0f, particles[i].remainingLifetime * BUG_SPEED.y) * BUG_OFFSET_AMOUNT.y;

            Vector3 position = transform.position;
            float perlinDirection = Mathf.PerlinNoise(particles[i].randomSeed * 0.000001f, 0.01f) * 9620f;

            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3((1 - _radius) * MAX_BUGS_RADIUS, 0, 0);
            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3(perlinX, perlinY);
            particles[i].position = position;
        }

        _mediumBugs.SetParticles(particles, numParticles);

        particles = new Particle[_largeBugs.main.maxParticles];

        numParticles = _largeBugs.GetParticles(particles);

        for (int i = 0; i < numParticles; i++)
        {
            float perlinX = (Mathf.PerlinNoise(particles[i].remainingLifetime * BUG_SPEED.x, 0f) * -BUG_OFFSET_AMOUNT.x * 0.5f) + 4.5f;
            float perlinY = Mathf.PerlinNoise(0f, particles[i].remainingLifetime * BUG_SPEED.y) * BUG_OFFSET_AMOUNT.y;

            Vector3 position = transform.position;
            float perlinDirection = Mathf.PerlinNoise(particles[i].randomSeed * 0.000001f, 0.01f) * 9520f;

            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3((1 - _radius) * MAX_BUGS_RADIUS, 0, 0);
            position += Quaternion.Euler(0, 0, perlinDirection) * new Vector3(perlinX, perlinY);
            particles[i].position = position;
        }

        _largeBugs.SetParticles(particles, numParticles);

        if (_playerTransform != null)
        {
            _targetPosition = _playerTransform.position;
            _currentPosition = Vector3.Lerp(_currentPosition, _targetPosition, Time.deltaTime * BUGS_MOVE_SPEED);
        }
    }

    public void SetPlayerTransform(Transform player)
    {
        _playerTransform = player;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }
}
