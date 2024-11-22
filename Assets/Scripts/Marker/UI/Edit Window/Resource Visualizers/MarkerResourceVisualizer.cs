using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MarkerResourceVisualizer<T> : MonoBehaviour
    where T : class
{
    [SerializeField] private GameObject _placeholder;
    [SerializeField] private GameObject _loading;
    [SerializeField] private GameObject _actual;

    public void UpdateUI(MarkerResource<T> resource)
    {
        if (resource.IsEmpty)
        {
            _loading.SetActive(false);
            _actual.SetActive(false);
            _placeholder.SetActive(true);
        }
        else
        {
            _loading.SetActive(true);
            _actual.SetActive(false);
            _placeholder.SetActive(false);

            resource.Get()
                .Then(value =>
                {
                    _loading.SetActive(false);
                    _actual.SetActive(true);
                    _placeholder.SetActive(false);

                    OnResourceGot(value);
                });
        }
    }

    protected abstract void OnResourceGot(T value);
}
