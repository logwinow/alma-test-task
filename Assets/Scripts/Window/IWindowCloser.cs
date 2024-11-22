using RSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWindowCloser
{
    IPromise Close(UIWindow window);
}
