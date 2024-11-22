using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MarkerPreviewWindow : UIWindow
{
    private Marker _marker;

    [SerializeField] private MarkerEditInfoWindow _markerEditInfoWindow;
    [SerializeField] private TextMeshProUGUI _titleUI;
    [SerializeField] private Sprite _defaultPicture;
    [SerializeField] private Image _picture;
    [SerializeField] private GameObject _loading;
    [SerializeField] private string _defaultTitleText = "(безымянный пин)";
    [SerializeField] private Vector2 _offset;

    public void Set(Marker marker)
    {
        _marker = marker;

        _titleUI.text = string.IsNullOrEmpty(marker.MarkerInfo.Title) ? _defaultTitleText : marker.MarkerInfo.Title;

        if (marker.MarkerInfo.Picture.IsEmpty)
        {
            SetLoading(false);
            _picture.sprite = _defaultPicture;
        }
        else
        {
            SetLoading(true);

            _marker.MarkerInfo.Picture.Get().Then(picture =>
            {
                SetLoading(false);

                _picture.sprite = picture;
            });
        }
    }

    private void LateUpdate()
    {
        if (_marker.MarkerState == Marker.State.Unavailable)
            return;

        transform.position = Camera.main.WorldToScreenPoint(_marker.MarkerInfo.WorldPosition) + (Vector3)_offset;
    }

    public void GoToEdit()
    {
        Hide();

        _markerEditInfoWindow.Set(_marker);
        _markerEditInfoWindow.Show();
    }

    private void SetLoading(bool value)
    {
        _loading.SetActive(value);
        _picture.gameObject.SetActive(!value);
    }
}
