using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class MarkerResource<T> : MarkerResourceBase where T : class
{
    [NonSerialized] private T _resource;

    public override string FileName
    {
        get => base.FileName;
        set
        {
            if (base.FileName == value)
                return;

            base.FileName = value;
            _resource = null;
        }
    }

    public IPromise<T> Get()
    {
        var promise = new Promise<T>();

        if (_resource is not null)
        {
            promise.Resolve(_resource);
        }
        else if (!DynamicResources.TryLoad(FileName, out var resourceFilePath))
        {
            promise.Reject(new Exception($"{nameof(FileName)} is set, but resource isn't found"));
        }
        else
        {
            LoadResource(resourceFilePath)
                .Then(resource =>
                {
                    _resource = resource;

                    promise.Resolve(resource);
                })
                .Catch(e =>
                {
                    Debug.LogError(e.Message);

                    promise.Reject(e);
                });
        }

        return promise;
    }

    public void Clear()
    {
        base.FileName = null;
        _resource = null;
    }

    protected abstract IPromise<T> LoadResource(string filePath);
}
