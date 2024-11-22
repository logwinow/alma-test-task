using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteMarkerResourceVisualizer : MarkerResourceVisualizer<Sprite>
{
    [SerializeField] private Image _pictureImage;

    protected override void OnResourceGot(Sprite value)
    {
        _pictureImage.sprite = value;
    }
}
