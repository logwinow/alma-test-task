using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayAudioButtonVisualizer : MonoBehaviour, IPointerClickHandler
{
    private bool _isPlaying = false;

    [SerializeField] private Image _icon;
    [SerializeField] private Sprite _playIcon;
    [SerializeField] private Sprite _stopIcon;

    [field: SerializeField] public UnityEvent OnPlay { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnStop { get; private set; } = new();

    public void OnPointerClick(PointerEventData eventData)
    {
        _isPlaying = !_isPlaying;

        SetState(_isPlaying);

        if (_isPlaying)
            OnPlay.Invoke();
        else
            OnStop.Invoke();
    }

    public void SetStopState()
    {
        SetState(false);
    }

    public void SetPlayState()
    {
        SetState(true);
    }

    private void SetState(bool isPlaying)
    {
        _isPlaying = isPlaying;

        _icon.sprite = isPlaying ? _stopIcon : _playIcon;
    }
}
