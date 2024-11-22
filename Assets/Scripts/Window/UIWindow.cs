using RSG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIWindow : MonoBehaviour
{
    private bool _initialized;
    private State _state = State.Closed;
    private IWindowOpener[] _openers;
    private IWindowCloser[] _closers;

    public bool IsShown => gameObject.activeSelf;

    [field: SerializeField] public UnityEvent OnShowEvent { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnHideEvent { get; private set; } = new();

    private enum State
    {
        Closed,
        Closing,
        Opened
    }

    private void Initialize()
    {
        if (_initialized)
            return;

        OnInitialize();

        foreach (var initializer in GetComponents<IWindowInitializer>())
        {
            initializer.Initialize(this);
        }

        _openers = GetComponents<IWindowOpener>();
        _closers = GetComponents<IWindowCloser>();

        _initialized = true;
    }

    protected virtual void OnInitialize()
    {

    }

    public void Show()
    {
        if (_state == State.Opened)
            return;

        Initialize();

        gameObject.SetActive(true);

        _state = State.Opened;

        OnShow();

        foreach (var opener in _openers)
        {
            opener.Open(this);
        }
    }

    protected virtual void OnShow()
    {

    }

    public void Hide()
    {
        if (_state == State.Closing || _state == State.Closed)
            return;

        _state = State.Closing;

        OnHide();

        Promise.All(_closers.Select(c => c.Close(this)).ToArray())
            .Then(() =>
            {
                gameObject.SetActive(false);
                _state = State.Closed;
            });
    }

    protected virtual void OnHide()
    {

    }
}
