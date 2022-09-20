using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SetupScene : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _splashScreen;
    [SerializeField] private SpriteRenderer _splashScreen2;

    [SerializeField] private SetupInputController _setupInputController;

    private bool _skipPressed = false;
    private Coroutine _coroutine;

    private void Start()
    {
        _splashScreen.color = new Color(1, 1, 1, 0);
        _splashScreen2.color = new Color(1, 1, 1, 0);
        _coroutine = StartCoroutine(SplashScreenCoroutine());
    }

    private void OnDestroy()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private void Update()
    {
        if (!_skipPressed && _setupInputController.GetSkipButtonDown())
        {
            _skipPressed = true;
        }
    }

    private IEnumerator SplashScreenCoroutine()
    {
        float time = 0.5f;
        float velocity = 0;

        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        time = 1.5f;
        _skipPressed = false;

        while (time > 0 && !_skipPressed)
        {
            _splashScreen.color = new Color(1, 1, 1, Mathf.SmoothDamp(_splashScreen.color.a, 1, ref velocity, 0.5f));
            time -= Time.deltaTime;
            yield return null;
        }

        _splashScreen.color = new Color(1, 1, 1, 1);

        time = 2f;

        while (time > 0 && !_skipPressed)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        time = 1.5f;
        velocity = 0;

        while (time > 0 && !_skipPressed)
        {
            _splashScreen.color = new Color(1, 1, 1, Mathf.SmoothDamp(_splashScreen.color.a, 0, ref velocity, 0.5f));
            time -= Time.deltaTime;
            yield return null;
        }

        _splashScreen.color = new Color(1, 1, 1, 0);

        time = 1.5f;
        velocity = 0;
        _skipPressed = false;

        while (time > 0 && !_skipPressed)
        {
            _splashScreen2.color = new Color(1, 1, 1, Mathf.SmoothDamp(_splashScreen2.color.a, 1, ref velocity, 0.5f));
            time -= Time.deltaTime;
            yield return null;
        }

        _splashScreen2.color = new Color(1, 1, 1, 1);

        time = 2f;

        while (time > 0 && !_skipPressed)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        time = 1.5f;
        velocity = 0;

        while (time > 0 && !_skipPressed)
        {
            _splashScreen2.color = new Color(1, 1, 1, Mathf.SmoothDamp(_splashScreen2.color.a, 0, ref velocity, 1f));
            time -= Time.deltaTime;
            yield return null;
        }

        _splashScreen2.color = new Color(1, 1, 1, 0);

        yield return null;

        FullScreenWipe.FadeToBlack(0);
        GameController.Instance.LoadMenu().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
    }
}
