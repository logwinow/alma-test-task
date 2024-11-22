using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingMapVisualizer : MonoBehaviour
{
    [SerializeField] private Image _loadingIcon;
    [SerializeField] private float _loadingIconFadeOutDuration = 1f;
    [SerializeField] private float _mapFadeInDuration = 1f;
    [SerializeField] private LoadingMapController _loadingMapController;
    [SerializeField] private SpriteRenderer _map;

    private void Awake()
    {
        _loadingMapController.OnMapLoadBegin.AddListener(OnMapLoadBegin);
        _loadingMapController.OnMapLoadEnd.AddListener(OnMapLoadEnd);
    }

    private void OnMapLoadBegin()
    {
        var mapColor = _map.color;
        mapColor.a = 0;
        _map.color = mapColor;
    }

    private void OnMapLoadEnd()
    {
        _loadingIcon.DOFade(0, _loadingIconFadeOutDuration)
            .OnComplete(() => _loadingIcon.gameObject.SetActive(false));

        _map.DOFade(1, _mapFadeInDuration);
    }

    private void Reset()
    {
        _loadingMapController = GetComponent<LoadingMapController>();
    }
}
