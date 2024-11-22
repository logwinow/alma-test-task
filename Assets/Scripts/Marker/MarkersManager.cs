using DiractionTeam.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarkersManager : MonoBehaviour
{
    private Vector2? _pressPosition;
    private List<Marker> _markers = new();

    [SerializeField] private Marker _markerPrefab;
    [SerializeField] private InputActionReference _pointerDownInput;
    [SerializeField] private InputActionReference _pointerPositionInput;
    [SerializeField] private MarkerEditInfoWindow _markerEditInfoWindow;
    [SerializeField] private MarkerPreviewWindow _markerPreviewWindow;

    private void Awake()
    {
        _pointerDownInput.action.Enable();
        _pointerPositionInput.action.Enable();

        _pointerDownInput.action.performed += OnPointerDown;
        _pointerDownInput.action.canceled += OnPointerUp;

        _markerEditInfoWindow.OnDelete.AddListener(OnMarkerDelete);
        _markerEditInfoWindow.OnChanged.AddListener(OnMarkerChanged);
    }

    private void Start()
    {
        if (Saving.TryLoad("Markers", out MarkerInfo[] markerInfos))
        {
            foreach (var markerInfo in markerInfos)
            {
                CreateMarker(markerInfo);
            }
        }
    }

    private void OnDestroy()
    {
        _pointerDownInput.action.performed -= OnPointerDown;
        _pointerDownInput.action.canceled -= OnPointerUp;
    }

    private void LateUpdate()
    {
        UpdateMarkerPositions();
    }

    private void OnPointerDown(InputAction.CallbackContext context)
    {
        var pointerPosition = _pointerPositionInput.action.ReadValue<Vector2>();

        if (UIUtils.IsPointerAboveUI(pointerPosition))
        {
            _pressPosition = null;
            return;
        }

        _pressPosition = pointerPosition;
    }

    private void OnPointerUp(InputAction.CallbackContext context)
    {
        if (_pressPosition is null)
            return;

        if (_pointerPositionInput.action.ReadValue<Vector2>() != _pressPosition)
        {
            return;
        }

        var markerInfo = new MarkerInfo(Guid.NewGuid())
        {
            WorldPosition = Camera.main.ScreenToWorldPoint(_pressPosition.Value)
        };

        CreateMarker(markerInfo);

        UpdatesSaves();
    }

    private void CreateMarker(MarkerInfo markerInfo)
    {
        var marker = Instantiate(_markerPrefab, transform);

        marker.Setup(markerInfo);

        marker.OnSelected.AddListener(OnMarkerSelected);
        marker.OnDragEnd.AddListener(OnMarkerChanged);

        _markers.Add(marker);
    }

    private void UpdateMarkerPositions()
    {
        foreach (var marker in _markers)
        {
            marker.UpdateScreenPosition();
        }
    }

    private void OnMarkerSelected(Marker marker)
    {
        _markerPreviewWindow.Set(marker);
        _markerPreviewWindow.Show();
    }

    private void OnMarkerDelete(Marker marker)
    {
        _markers.Remove(marker);

        UpdatesSaves();

        Destroy(marker.gameObject);

        _markerEditInfoWindow.Hide();
    }

    private void OnMarkerChanged(Marker marker)
    {
        UpdatesSaves();
    }

    private void UpdatesSaves()
    {
        Saving.Set("Markers", _markers.Select(m => m.MarkerInfo).ToArray());
        Saving.Save();
    }
}
