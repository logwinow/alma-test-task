using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// http://orbcreation.com

public static class RectTransformExtensions
{
    public static RectTransform GetRectTransform(this GameObject gameObject) => gameObject.transform.GetRectTransform();
    
    public static RectTransform GetRectTransform(this Transform transform) => transform as RectTransform;
    
    public static void DestroyChildren(this RectTransform go)
    {
        var children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        children.ForEach(Object.Destroy);
    }
    
    public static void SetDefaultScale(this RectTransform trans)
    {
        trans.localScale = new Vector3(1, 1, 1);
    }
    public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
    {
        trans.pivot = aVec;
        trans.anchorMin = aVec;
        trans.anchorMax = aVec;
    }

    public static Vector2 GetSize(this RectTransform trans)
    {
        return trans.rect.size;
    }
    public static float GetWidth(this RectTransform trans)
    {
        return trans.rect.width;
    }
    public static float GetHeight(this RectTransform trans)
    {
        return trans.rect.height;
    }

    public static void SetLocalPositionOfPivot(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
    }

    public static void SetLocalLeftBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 GetLeftBottomPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x - (trans.pivot.x * trans.rect.width), trans.position.y - (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetLocalLeftBottomPosition(this RectTransform trans)
    {
        return new Vector3(trans.localPosition.x - (trans.pivot.x * trans.rect.width), trans.localPosition.y - (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetLocalLeftTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetLeftTopPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x - (trans.pivot.x * trans.rect.width), trans.position.y + ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static void SetLocalRightBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetRightBottomPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x + ((1f - trans.pivot.x) * trans.rect.width), trans.position.y - (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetLocalRightTopPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetRightTopPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x + ((1f - trans.pivot.x) * trans.rect.width), trans.position.y + ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetLocalRightTopPosition(this RectTransform trans)
    {
        return new Vector3(trans.localPosition.x + ((1f - trans.pivot.x) * trans.rect.width), trans.localPosition.y + ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetTopPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x + ((0.5f - trans.pivot.x) * trans.rect.width), trans.position.y + ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetLocalTopPosition(this RectTransform trans)
    {
        return new Vector3(trans.localPosition.x + ((0.5f - trans.pivot.x) * trans.rect.width), trans.localPosition.y + ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetBottomPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x + ((0.5f - trans.pivot.x) * trans.rect.width), trans.position.y - (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }
    
    public static Vector2 GetLocalBottomPosition(this RectTransform trans)
    {
        return new Vector3(trans.localPosition.x + ((0.5f - trans.pivot.x) * trans.rect.width), trans.localPosition.y - (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 GetLeftPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x - (trans.pivot.x * trans.rect.width), trans.position.y + ((0.5f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 GetLocalLeftPosition(this RectTransform trans)
    {
        return new Vector3(trans.localPosition.x - (trans.pivot.x * trans.rect.width), trans.localPosition.y + ((0.5f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 GetRightPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x + ((1f - trans.pivot.x) * trans.rect.width), trans.position.y + ((0.5f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }

    public static Vector2 GetCenterPosition(this RectTransform trans)
    {
        return new Vector3(trans.position.x + ((0.5f - trans.pivot.x) * trans.rect.width), trans.position.y + ((0.5f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
    }
    
    public static void SetBottomPosition(this RectTransform trans, Vector2 newPos)
    {
        trans.position = new Vector3(newPos.x - ((0.5f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
    }

    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    public static void SetFullscreenSize(this RectTransform trans)
    {
        trans.position = new Vector2(Screen.width / 2, Screen.height / 2);
        trans.anchorMin = Vector2.zero;
        trans.anchorMax = Vector2.one;
        trans.sizeDelta = Vector2.zero;
    }

    public static void SetSize(this RectTransform trans, float x, float y)
    {
        trans.SetSize(new Vector2(x, y));
    }

    public static void SetWidth(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(newSize, trans.rect.size.y));
    }
    public static void SetHeight(this RectTransform trans, float newSize)
    {
        SetSize(trans, new Vector2(trans.rect.size.x, newSize));
    }

    public static Vector2 GetPosition(this RectTransform trans)
    {
        var position = trans.position;
        var size = trans.GetSize();
        position -= new Vector3(size.x * trans.pivot.x, size.y * trans.pivot.y, 0);

        return position;
    }

    public static void MultiplySize(this RectTransform rectTransform, float multiplier)
    {
        var size = rectTransform.GetSize();
        
        rectTransform.SetSize(size * multiplier);
    }
}
