using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RSG;
using UnityEngine;

[RequireComponent(typeof(UIWindow))]
public class UIWindowScale : MonoBehaviour, IWindowOpener, IWindowCloser
{
    private Tween _scaleTween;

    [SerializeField] private float _openDuration = 1f;
    [SerializeField] private float _closeDuration = 0.5f;

    public void Open(UIWindow window)
    {
        transform.localScale = Vector3.zero;
        _scaleTween?.Kill();
        _scaleTween = transform.DOScale(Vector3.one, _openDuration)
            .SetEase(Ease.OutBack)
            .SetUpdate(true)
            .Play();
    }

    public IPromise Close(UIWindow window)
    {
        var promise = new Promise();

        _scaleTween?.Kill();
        _scaleTween = transform
            .DOScale(0, _closeDuration)
            .SetUpdate(true)
            .OnKill(() => promise.Resolve())
            .Play();

        return promise;
    }
}
