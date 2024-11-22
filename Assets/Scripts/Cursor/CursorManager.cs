using DiractionTeam.Utils.Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SingletonMono<CursorManager>
{
    [SerializeField] private Texture2D _defaultCursor;

    private void Start()
    {
        SetDefault();
    }

    public void SetCursor(Texture2D cursor)
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetDefault()
    {
        SetCursor(_defaultCursor);
    }
}
