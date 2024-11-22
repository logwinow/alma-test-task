using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIResizer : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    private Vector2? _referencedSize;
    private Vector3? _referencedScale;

    [SerializeField] private float _toNormalDuration = 0.2f;
    [SerializeField] private float _highlightedSizeMultiplier = 1.1f;
    [SerializeField] private float _highlightDuration = 0.2f;
    [SerializeField] private float _pressedSizeMultiplier = 0.9f;
    [SerializeField] private float _pressedDuration = 0.2f;
    [SerializeField] private bool _animateScale;
    [SerializeField] private bool _recursive;
    [SerializeField] private bool _unscaledTime = false;
    [SerializeField] private bool _activateOnAwake = true;
    [SerializeField] private RectTransform _target;
    [SerializeField] private ExecuteTriggers _executeTriggers = ExecuteTriggers.Everything;
    
    private float _sizeMultiplier = 1;
    private Tween _sizeTween;
    private bool _isPressed;
    private bool _isInside;
    private bool _isPaused;
    private Dictionary<RectTransform, Vector2> _childSizes = new();

    public Vector2 ReferenceSize => _referencedSize ??= Target.GetSize();
    private Vector3 ReferenceScale => _referencedScale ??= Target.localScale;
    private RectTransform Target => _target ? _target : transform as RectTransform;

    public float ToNormalDuration => _toNormalDuration;
    public float HighlightDuration => _highlightDuration;

    private enum SelectionState
    {
        Normal,
        Highlighted,
        Pressed,
        Selected,
    }

    [Flags]
    private enum ExecuteTriggers
    {
        PointerEnter = 1 << 0,
        PointerExit = 1 << 1,
        PointerDown = 1 << 2,
        PointerUp = 1 << 3,
        Everything = PointerEnter | PointerExit | PointerDown | PointerUp
    }

    private void Awake()
    {
        if (!_activateOnAwake)
            Pause();
    }

    private void OnDisable()
    {
        _sizeTween?.Kill();
        SetSize(1f);
    }

    private void ChangeSize(SelectionState state)
    {
        switch (state)
        {
            case SelectionState.Normal:
                TransitToSize(1f, _toNormalDuration);
                break;
            case SelectionState.Highlighted:
                TransitToSize(_highlightedSizeMultiplier, _highlightDuration);
                break;
            case SelectionState.Pressed:
                TransitToSize(_pressedSizeMultiplier, _pressedDuration);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void SetSize(float size)
    {
        if (_animateScale)
            Target.localScale = ReferenceScale * size;
        else
            Target.SetSize(ReferenceSize * size);

        if (_recursive)
        {
            foreach (RectTransform childRT in Target)
            {
                if (!_childSizes.ContainsKey(childRT))
                {
                    if (_animateScale)
                    {
                        _childSizes[childRT] = childRT.localScale;
                    }
                    else
                        _childSizes[childRT] = childRT.GetSize();
                }

                childRT.SetSize(_childSizes[childRT] * size);
            }
        }
    }

    private void TransitToSize(float size, float duration)
    {
        if (_isPaused)
            return;
        
        _sizeTween?.Kill();

        _sizeTween = DOTween.To(() => _sizeMultiplier, newValue =>
        {
            _sizeMultiplier = newValue;
            SetSize(_sizeMultiplier);
        }, size, duration).SetUpdate(_unscaledTime);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!CanPerformTrigger(ExecuteTriggers.PointerEnter))
            return;
        
        _isInside = true;

        if (_isPressed)
            return;
        
        ChangeSize(SelectionState.Highlighted);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanPerformTrigger(ExecuteTriggers.PointerDown))
            return;

        ChangeSize(SelectionState.Pressed);
        _isPressed = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CanPerformTrigger(ExecuteTriggers.PointerExit))
            return;

        _isInside = false;
        
        if (_isPressed)
            return;

        ChangeSize(SelectionState.Normal);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!CanPerformTrigger(ExecuteTriggers.PointerUp))
            return;

        if (_isPressed && !_isInside)
        {
            ChangeSize(SelectionState.Normal);
        }
        else
            ChangeSize(SelectionState.Highlighted);

        _isPressed = false;
    }

    private bool CanPerformTrigger(ExecuteTriggers trigger)
    {
        if (!Target.gameObject.activeInHierarchy)
            return false;
        
        if (!_executeTriggers.HasFlag(trigger))
            return false;

        return true;
    }

    public void Pause()
    {
        _sizeTween?.Kill();
        _isPaused = true;
    }

    public void Resume()
    {
        _isPaused = false;
    }
}
