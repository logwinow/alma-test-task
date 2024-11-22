using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AudioProgressVisualizer : MonoBehaviour
{
    private Coroutine _monitorProgressCoroutine;

    [SerializeField] private AudioSource _source;
    [SerializeField] private Slider _slider;

    public void Play()
    {
        _monitorProgressCoroutine = StartCoroutine(MonitorProgresCoroutine());
    }

    public void Stop()
    {
        _slider.value = 0;

        if (_monitorProgressCoroutine is null)
            return;

        StopCoroutine(_monitorProgressCoroutine);
    }

    private IEnumerator MonitorProgresCoroutine()
    {
        if (_source.clip is null)
            yield break;

        float _clipLength = _source.clip.length;

        while(_source.isPlaying)
        {
            _slider.value = _source.time / _clipLength;

            yield return null;
        }
    }
}
