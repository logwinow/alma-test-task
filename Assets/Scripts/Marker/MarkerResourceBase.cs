using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MarkerResourceBase
{
    private string _fileName;
    [NonSerialized] private bool? _isEmpty;

    public bool IsEmpty => _isEmpty ??= string.IsNullOrEmpty(_fileName);
    public virtual string FileName
    {
        get => _fileName;
        set 
        {
            _fileName = value;
            _isEmpty = null;
        }
    }
}
