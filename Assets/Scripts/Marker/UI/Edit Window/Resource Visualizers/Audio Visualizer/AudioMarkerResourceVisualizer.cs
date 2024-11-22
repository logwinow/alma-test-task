using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMarkerResourceVisualizer : MarkerResourceVisualizer<AudioClip>
{
    [SerializeField] private AudioUIPlayer _audioPlayer;

    protected override void OnResourceGot(AudioClip value)
    {
        _audioPlayer.Set(value);
    }
}
