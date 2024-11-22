using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragHandVisualizer : MonoBehaviour
{
    [SerializeField] private AimMover _aimMover;
    [SerializeField] private Texture2D _dragHand;

    private void Awake()
    {
        _aimMover.OnDragBegin.AddListener(OnDragBegin);
        _aimMover.OnDragEnd.AddListener(OnDragEnd);
    }

    private void OnDragBegin()
    {
        CursorManager.Instance.SetCursor(_dragHand);
    }

    private void OnDragEnd()
    {
        CursorManager.Instance.SetDefault();
    }

    private void Reset()
    {
        _aimMover = GetComponent<AimMover>();
    }
}
