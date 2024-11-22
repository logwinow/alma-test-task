using System;
using UnityEngine;

[Serializable]
public class MarkerInfo
{
    public MarkerInfo(Guid id)
    {
        _id = id;
    }

    public MarkerInfo(MarkerInfo markerInfo) : this(markerInfo.ID)
    {
        CopyDataFrom(markerInfo);
    }

    private Guid _id;
    private string _title;
    private string _description;
    private float _x;
    private float _y;
    private SpriteMarkerResource _picture;
    private AudioClipMarkerResource _audio;

    public Guid ID => _id;

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public Vector2 WorldPosition
    {
        get => new Vector2(_x, _y);
        set
        {
            _x = value.x;
            _y = value.y;
        }
    }
    public SpriteMarkerResource Picture => _picture ??= new SpriteMarkerResource();
    public AudioClipMarkerResource Audio => _audio ??= new AudioClipMarkerResource();

    public void CopyDataFrom(MarkerInfo markerInfo)
    {
        _title = markerInfo.Title;
        _description = markerInfo.Description;
        Picture.FileName = markerInfo.Picture.FileName;
        Audio.FileName = markerInfo.Audio.FileName;
        _x = markerInfo._x;
        _y = markerInfo._y;
    }
}
