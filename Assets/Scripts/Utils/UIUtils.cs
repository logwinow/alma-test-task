using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DiractionTeam.Utils
{
    public static class UIUtils
    {
        private static GraphicRaycaster _graphicRaycaster;
        private static GraphicRaycaster GraphicRaycaster => _graphicRaycaster ? _graphicRaycaster : _graphicRaycaster = GameObject.FindObjectOfType<GraphicRaycaster>();

        public static List<RaycastResult> GetCastedElements(Vector2 pointerPosition)
        {
            return GetCastedElements(pointerPosition, GraphicRaycaster);
        }

        public static List<RaycastResult> GetCastedElements(Vector2 pointerPosition, GraphicRaycaster graphicRaycaster)
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = pointerPosition
            };

            var clickResults = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, clickResults);

            return clickResults;
        }

        public static bool IsPointerAboveUI(Vector2 pointerPosition)
        {
            return IsPointerAboveUI(pointerPosition, GraphicRaycaster);
        }

        public static bool IsPointerAboveUI(Vector2 pointerPosition, GraphicRaycaster graphicRaycaster)
        {
            return GetCastedElements(pointerPosition, graphicRaycaster).Count > 0;
        }

        public static bool IsPointerAbove<T>(Vector2 pointerPosition) where T : Component
        {
            return IsPointerAbove<T>(pointerPosition, GraphicRaycaster);
        }

        public static bool IsPointerAbove<T>(Vector2 pointerPosition, GraphicRaycaster graphicRaycaster) where T : Component
        {
            var clickResults = GetCastedElements(pointerPosition, graphicRaycaster);

            return clickResults.Count > 0 && clickResults[0].gameObject.GetComponent<T>();
        }
    }
}