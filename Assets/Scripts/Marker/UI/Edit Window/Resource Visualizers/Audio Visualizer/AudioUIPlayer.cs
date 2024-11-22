using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioUIPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    [field: SerializeField] public UnityEvent OnPlay { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnStop { get; private set; } = new();

    public void Set(AudioClip audio)
    {
        _audioSource.clip = audio;
    }

    public void Play()
    {
        _audioSource.Play();

        OnPlay.Invoke();
    }

    public void Stop()
    {
        _audioSource.Stop();

        OnStop.Invoke();
    }
}
