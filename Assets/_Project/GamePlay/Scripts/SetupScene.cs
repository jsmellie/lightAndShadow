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

    private void Start()
    {
        _splashScreen.color = new Color(1, 1, 1, 0);
        _splashScreen2.color = new Color(1, 1, 1, 0);
        ShowSplashScreen();
    }

    private void ShowSplashScreen()
    {
        DOVirtual.Float(0, 1, 1.5f, (x) =>
        {
            _splashScreen.color = new Color(1, 1, 1, x);
        })
        .SetDelay(0.5f)
        .SetEase(Ease.InOutQuad).onComplete += () =>
        {
            FadeSplashScreen();
        };
    }

    private void FadeSplashScreen()
    {
        DOVirtual.Float(1, 0, 1.5f, (x) =>
        {
            _splashScreen.color = new Color(1, 1, 1, x);
        })
        .SetDelay(2f)
        .SetEase(Ease.InOutQuad).onComplete += () =>
        {
            ShowSplashScreen2();
        };
    }

    private void ShowSplashScreen2()
    {
        DOVirtual.Float(0, 1, 1.5f, (x) =>
        {
            _splashScreen2.color = new Color(1, 1, 1, x);
        })
        .SetEase(Ease.InOutQuad).onComplete += () =>
        {
            FadeSplashScreen2();
        };
    }

    private void FadeSplashScreen2()
    {
        DOVirtual.Float(1, 0, 1.5f, (x) =>
        {
            _splashScreen2.color = new Color(1, 1, 1, x);
        })
        .SetDelay(2f)
        .SetEase(Ease.InOutQuad).onComplete += () =>
        {
            FullScreenWipe.FadeIn(0);
            GameController.Instance.LoadMenu().ContinueWith(task => Debug.LogException(task.Exception), TaskContinuationOptions.OnlyOnFaulted);
        };
    }
}
