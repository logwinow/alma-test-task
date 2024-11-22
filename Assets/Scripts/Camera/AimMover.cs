using Cinemachine;
using DiractionTeam.Utils;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AimMover : MonoBehaviour
{
    private Coroutine _moveCoroutine;
    private Bounds _bounds;
    private float _sqrDeadzone;

    [SerializeField] private Transform _aim;
    [SerializeField] private Collider2D _boundsCollider;
    [SerializeField] private CinemachineVirtualCamera _vc;
    [SerializeField] private InputActionReference _pointerPressInput;
    [SerializeField] private InputActionReference _pointerDeltaInput;
    [SerializeField] private InputActionReference _pointerPositionInput;
    [SerializeField]
#if UNITY_EDITOR
        [OnValueChanged(nameof(OnDeadzoneValueChanged)), RangeSlider(0, 1f)]
#endif
    private float _deadzone = 0.1f;
    [SerializeField] private float _sensitive = 1f;
    [SerializeField] private float _referrentOrthographicSize = 10;

    [field: SerializeField] public UnityEvent OnDragBegin { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnDragEnd { get; private set; } = new();

    private void Awake()
    {
        _sqrDeadzone = _deadzone * _deadzone;

        _pointerPressInput.action.Enable();
        _pointerDeltaInput.action.Enable();
        _pointerPositionInput.action.Enable();

        _pointerPressInput.action.started += OnPointerPressed;
        _pointerPressInput.action.canceled += OnPointerReleased;

        _bounds = _boundsCollider.bounds;
    }

    private void OnDestroy()
    {
        _pointerPressInput.action.started -= OnPointerPressed;
        _pointerPressInput.action.canceled -= OnPointerReleased;
    }

    private void OnPointerPressed(InputAction.CallbackContext context)
    {
        if (UIUtils.IsPointerAboveUI(_pointerPositionInput.action.ReadValue<Vector2>()))
        {
            _moveCoroutine = null;
            return;
        }

        _moveCoroutine = StartCoroutine(MoveCameraCoroutine());
    }

    private void OnPointerReleased(InputAction.CallbackContext context)
    {
        if (_moveCoroutine is null)
            return;

        StopCoroutine(_moveCoroutine);

        OnDragEnd.Invoke();
    }

    private IEnumerator MoveCameraCoroutine()
    {
        var pointerStartPosition = _pointerPositionInput.action.ReadValue<Vector2>();
        var invertScreenSize = new Vector2(1f / Screen.width, 1f / Screen.height);

        Vector2 positionOffset;

        do
        {
            yield return null;

            positionOffset = _pointerPositionInput.action.ReadValue<Vector2>() - pointerStartPosition;
            positionOffset.Scale(invertScreenSize);
        }
        while (positionOffset.sqrMagnitude < _sqrDeadzone);

        OnDragBegin.Invoke();

        while (true)
        {
            var pointerDelta = _pointerDeltaInput.action.ReadValue<Vector2>();
            pointerDelta.Scale(invertScreenSize);

            var aimPosition = _aim.transform.position;
            var vcHalfSize = new Vector2(_vc.m_Lens.OrthographicSize * _vc.m_Lens.Aspect, _vc.m_Lens.OrthographicSize);

            aimPosition -= (Vector3)pointerDelta * _sensitive * vcHalfSize.y / _referrentOrthographicSize;
            aimPosition.x = Mathf.Clamp(aimPosition.x, _bounds.min.x + vcHalfSize.x, _bounds.max.x - vcHalfSize.x);
            aimPosition.y = Mathf.Clamp(aimPosition.y, _bounds.min.y + vcHalfSize.y, _bounds.max.y - vcHalfSize.y);

            _aim.transform.position = aimPosition;

            yield return null;
        }
    }

    private void Reset()
    {
        _aim = transform;
    }

#if UNITY_EDITOR

    private void OnDeadzoneValueChanged()
    {
        _sqrDeadzone = _deadzone * _deadzone;
    }

#endif
}
