using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteMarkerResource : MarkerResource<Sprite>
{
    protected override IPromise<Sprite> LoadResource(string filePath)
    {
        return IMG2Sprite.LoadNewSpriePromise(filePath);
    }
}
