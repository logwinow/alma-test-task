using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioClipMarkerResource : MarkerResource<AudioClip>
{
    protected override IPromise<AudioClip> LoadResource(string filePath)
    {
        return AudioClipLoader.Load(filePath);
    }
}
