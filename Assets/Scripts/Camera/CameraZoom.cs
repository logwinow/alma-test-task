using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private float _zoom;
    private Tween _zoomTween;
    private CinemachineConfiner2D _confiner;

    [SerializeField] private CinemachineVirtualCamera _vc;
    [SerializeField] private InputActionReference _zoomInput;
    [SerializeField] private float _minZoom = 1f;
    [SerializeField] private float _maxZoom = 20f;
    [SerializeField] private float _zoomDuration = 0.5f;
    [SerializeField] private float _zoomSensitive = 2f;

    private void Awake()
    {
        _confiner = _vc.GetComponent<CinemachineConfiner2D>();
        _zoom = _vc.m_Lens.OrthographicSize;

        _zoomInput.action.Enable();

        _zoomInput.action.performed += OnZoomPerformed;
    }

    private void OnDestroy()
    {
        _zoomInput.action.performed -= OnZoomPerformed;
    }

    private void OnZoomPerformed(InputAction.CallbackContext context)
    {
        _zoom = Mathf.Clamp(_zoom + context.ReadValue<float>() * _zoomSensitive, _minZoom, _maxZoom);

        _zoomTween?.Kill();
        _zoomTween = DOTween.To(() => _vc.m_Lens.OrthographicSize, value => _vc.m_Lens.OrthographicSize = value, _zoom, _zoomDuration)
            .OnUpdate(() => _confiner.InvalidateCache());
    }

    private void Reset()
    {
        _vc = GetComponent<CinemachineVirtualCamera>();
    }
}
