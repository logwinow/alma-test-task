using RSG;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmationWindow : UIWindow
{
    [SerializeField] private TextMeshProUGUI _titleUI;

    [field: SerializeField] public UnityEvent<bool> OnAnswerGiven { get; private set; } = new();

    public IPromise<bool> Popup()
    {
        Show();

        var promise = new Promise<bool>();

        OnAnswerGiven.AddListener(_OnAnswerGiven);

        promise.Finally(() => OnAnswerGiven.RemoveListener(_OnAnswerGiven));

        return promise;

        void _OnAnswerGiven(bool result)
        {
            promise.Resolve(result);
        }
    }

    public void SetTitle(string title)
    {
        _titleUI.text = title;
    }

    public void Confirm()
    {
        OnAnswerGiven.Invoke(true);

        Hide();
    }

    public void Decline()
    {
        OnAnswerGiven.Invoke(false);

        Hide();
    }
}
