using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreenWipe : MonoBehaviour
{
    public delegate void OnScreenWipeCompletedDelegate();
    public delegate void OnScreenWipeUpdated(float ratio);

    private const string RatioShaderKey = "_Ratio";

    private static Material s_fullWipeMaterial = null;
    private static float s_timeLeft = 0;
    private static float s_animationLength = 0;
    private static OnScreenWipeCompletedDelegate s_onCompletedCallback = null;
    private static OnScreenWipeUpdated s_wipeUpdateMethod = null;

    [SerializeField] private Material _fullWipeMaterial = null;


    private void Awake()
    {
        s_fullWipeMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        if (s_timeLeft > 0)
        {
            s_timeLeft -= Time.deltaTime;
            CheckIfWipeCompleted();
            if (s_wipeUpdateMethod != null)
            {
                s_wipeUpdateMethod(1 - (s_timeLeft / s_animationLength));
            }
        }
    }

    public static void FadeOut(float time = 1f, OnScreenWipeCompletedDelegate onCompleteCallback = null)
    {
        s_onCompletedCallback = onCompleteCallback;
        s_wipeUpdateMethod = FadeOutUpdate;

        s_animationLength = s_timeLeft = time;
        CheckIfWipeCompleted();
    }

    public static void FadeIn(float time = 1f, OnScreenWipeCompletedDelegate onCompleteCallback = null)
    {
        s_onCompletedCallback = onCompleteCallback;
        s_wipeUpdateMethod = FadeInUpdate;

        s_animationLength = s_timeLeft = time;
        CheckIfWipeCompleted();
    }

    private static void FadeOutUpdate(float ratio)
    {
        s_fullWipeMaterial.SetFloat(RatioShaderKey, 1-ratio);
    }

    private static void FadeInUpdate(float ratio)
    {
        s_fullWipeMaterial.SetFloat(RatioShaderKey, ratio);
    }

    private static void CheckIfWipeCompleted()
    {
        if (s_timeLeft <= 0)
        {
            s_wipeUpdateMethod = null;
            if (s_onCompletedCallback != null)
            {
                s_onCompletedCallback();
                s_onCompletedCallback = null;
            }
        }
    }
}
