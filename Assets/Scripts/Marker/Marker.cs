using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Marker : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Coroutine _holdCoroutine;

    private MarkerInfo _markerInfo;

    [SerializeField] private TextMeshProUGUI _titleUI;
    [SerializeField] private float _holdTimeToDrag = 0.5f;
    [SerializeField] private InputActionReference _pointerPositionInput;

    public MarkerInfo MarkerInfo => _markerInfo;
    public State MarkerState { get; private set; } = State.Available;

    [field: SerializeField] public UnityEvent OnInitialized { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Marker> OnSelected { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Marker> OnDragBegin { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Marker> OnDragEnd { get; private set; } = new();

    public enum State
    {
        Available,
        Unavailable
    }

    private void Awake()
    {
        _pointerPositionInput.action.Enable();
    }

    public void Setup(MarkerInfo markerInfo)
    {
        _markerInfo = markerInfo;

        _titleUI.text = markerInfo.Title;

        UpdateTitleVisualization();
    }

    private void UpdateTitleVisualization()
    {
        _titleUI.gameObject.SetActive(!string.IsNullOrEmpty(MarkerInfo.Title));
    }

    public void UpdateScreenPosition()
    {
        if (MarkerState == State.Unavailable)
            return;

        transform.position = Camera.main.WorldToScreenPoint(MarkerInfo.WorldPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine(_holdCoroutine);

        if (MarkerState == State.Unavailable)
        {
            MarkerInfo.WorldPosition = Camera.main.ScreenToWorldPoint(transform.position);

            MarkerState = State.Available;

            OnDragEnd.Invoke(this);
        }
        else
            OnSelected.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _holdCoroutine = StartCoroutine(HoldCoroutine());
    }

    private IEnumerator HoldCoroutine()
    {
        yield return new WaitForSeconds(_holdTimeToDrag);

        MarkerState = State.Unavailable;

        OnDragBegin.Invoke(this);

        while (true)
        {
            transform.position = _pointerPositionInput.action.ReadValue<Vector2>();

            yield return null;
        }
    }
}
