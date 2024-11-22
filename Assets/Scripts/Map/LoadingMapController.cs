using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadingMapController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _map;
    [SerializeField] private AssetReferenceSprite _mapReference;

    [field: SerializeField] public UnityEvent OnMapLoadBegin { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnMapLoadEnd { get; private set; } = new();

    private void Start()
    {
        LoadMap();
    }

    private async Task LoadMap()
    {
        OnMapLoadBegin.Invoke();

        var operation = _mapReference.LoadAssetAsync();
        await operation.Task;

        if (operation.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError(operation.OperationException);
            return;
        }

        _map.sprite = operation.Task.Result;

        OnMapLoadEnd.Invoke();
    }
}
