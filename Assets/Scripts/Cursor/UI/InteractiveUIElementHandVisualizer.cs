using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveUIElementHandVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Texture2D _hand;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetCursor(_hand);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();
    }
}
