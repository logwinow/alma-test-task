using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DiractionTeam.Utils;
using RSG;
using UnityEngine;
using UnityEngine.UI;

public class UIWindowFade : MonoBehaviour, IWindowInitializer, IWindowOpener, IWindowCloser
{
    private Image _fade;
    private Tween _fadeTween;

    [SerializeField] private float _fadeDuration = 1f;
    [SerializeField, Range(0, 1f)] private float _fadeAlpha = 0.7f;
    [SerializeField] private States _states = States.Everything;

    [Flags]
    private enum States
    {
        Open = 1,
        Close = 2,
        Everything = Open | Close
    }

    public void Initialize(UIWindow window)
    {
        var fadeGO = new GameObject($"{window.name}'s Fade");
        fadeGO.transform.parent = window.transform.parent;
        fadeGO.transform.SetSiblingIndex(window.transform.GetSiblingIndex());

        var rectTransform  = fadeGO.AddComponent<RectTransform>();
        rectTransform.SetFullscreenSize();

        _fade = fadeGO.AddComponent<Image>();
        _fade.color = Color.black;

        _fade.gameObject.SetActive(false);
    }

    public void Open(UIWindow window)
    {
        _fade.gameObject.SetActive(true);
        _fade.raycastTarget = true;

        _fadeTween?.Kill();

        if (_states.HasFlag(States.Open))
        {
            var fadeColor = _fade.color;
            fadeColor.a = 0;
            _fade.color = fadeColor;

            _fadeTween = _fade.DOFade(_fadeAlpha, _fadeDuration)
                .SetUpdate(true)
                .Play();
        }
        else
        {
            var fadeColor = _fade.color;
            fadeColor.a = 1;
            _fade.color = fadeColor;
        }
    }

    public IPromise Close(UIWindow window)
    {
        _fade.raycastTarget = false;

        _fadeTween?.Kill();

        if (_states.HasFlag(States.Close))
        {
            _fadeTween = _fade.DOFade(0, _fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => _fade.gameObject.SetActive(false))
                .Play();
        }
        else
        {
            var fadeColor = _fade.color;
            fadeColor.a = 0;
            _fade.color = fadeColor;

            _fade.gameObject.SetActive(false);
        }

        var promise = new Promise();
        promise.Resolve();

        return promise;
    }
}
