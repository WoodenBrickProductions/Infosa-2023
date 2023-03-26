using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public static Fade instance;
    [SerializeField] private Image black;

    private void Awake()
    {
        instance = this;
    }

    public void FadeOut(float duration, Action callback)
    {
        StartCoroutine(FadeOutCoroutine(duration, callback));
    }

    const float tick = 1 / 60f;
    WaitForSecondsRealtime wait = new WaitForSecondsRealtime(tick);

    IEnumerator FadeOutCoroutine(float duration, Action callback)
    {
        while(black.color.a < 1)
        {
            var color = black.color;
            color.a += duration * tick;
            black.color = color;
            yield return wait;
        }

        callback?.Invoke();
        yield return 0;
    }

    public void FadeIn(float duration, Action callback)
    {
        StartCoroutine(FadeInCoroutine(duration, callback));
    }

    IEnumerator FadeInCoroutine(float duration, Action callback)
    {
        while (black.color.a > 0)
        {
            var color = black.color;
            color.a -= duration * tick;
            black.color = color;
            yield return wait;
        }

        callback?.Invoke();
        yield return 0;
    }
}
