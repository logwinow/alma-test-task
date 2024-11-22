using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static UnityEngine.PlayerLoop.PostLateUpdate;

public class MarkerEditInfoWindow : UIWindow
{
    private Marker _marker;
    private MarkerInfo _virtualMarkerInfo;
    private bool _dirty;

    [SerializeField] private TMP_InputField _titleUI;
    [SerializeField] private TMP_InputField _descriptionUI;
    [SerializeField] private SpriteMarkerResourceVisualizer _pictureVisualizer;
    [SerializeField] private AudioMarkerResourceVisualizer _audioVisualizer;
    [SerializeField] private ConfirmationWindow _confirmationWindow;
    [SerializeField] private string _closeConfirmationMessage = "Сохранить изменения?";
    [SerializeField] private string _deleteConfirmationMessage = "Вы уверены, что хотите удалить пин?";
    [SerializeField] private Uploader _uploader;

    public Marker Marker => _marker;

    [field: SerializeField] public UnityEvent<Marker> OnDelete { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Marker> OnChanged { get; private set; } = new();

    protected override void OnInitialize()
    {
        _titleUI.onEndEdit.AddListener(OnTitleChanged);
        _descriptionUI.onEndEdit.AddListener(OnDescriptionChanged);
    }

    public void Set(Marker marker)
    {
        _dirty = false;
        _marker = marker;
        _virtualMarkerInfo = new MarkerInfo(_marker.MarkerInfo);

        UpdateUI();
    }

    public void Delete()
    {
        _confirmationWindow.SetTitle(_deleteConfirmationMessage);
        _confirmationWindow.Popup()
            .Then(value =>
            {
                if (value)
                {
                    DeleteMarkersDynamicResource(_marker.MarkerInfo.Picture, _virtualMarkerInfo.Picture);
                    DeleteMarkersDynamicResource(_marker.MarkerInfo.Audio, _virtualMarkerInfo.Audio);

                    OnDelete.Invoke(_marker);
                }
            });
    }

    private void DeleteMarkersDynamicResource(MarkerResourceBase markerResource, MarkerResourceBase virtualMarkerResource)
    {
        if (markerResource.IsEmpty && virtualMarkerResource.IsEmpty)
            return;

        if (markerResource.FileName == virtualMarkerResource.FileName)
        {
            DynamicResources.Delete(markerResource.FileName);
        }
        else
        {
            if (!markerResource.IsEmpty)
            {
                DynamicResources.Delete(markerResource.FileName);
            }

            if (!virtualMarkerResource.IsEmpty)
            {
                DynamicResources.Delete(virtualMarkerResource.FileName);
            }
        }
    }

    private void UpdateUI()
    {
        _titleUI.text = _virtualMarkerInfo.Title;
        _descriptionUI.text = _virtualMarkerInfo.Description;

        UpdatePicture();
        UpdateAudio();
    }

    private void UpdatePicture()
    {
        _pictureVisualizer.UpdateUI(_virtualMarkerInfo.Picture);
    }

    private void UpdateAudio()
    {
        _audioVisualizer.UpdateUI(_virtualMarkerInfo.Audio);
    }

    private void UploadResource(Uploader.FileType fileType, MarkerResourceBase markerResource)
    {
        var fileNameWithoutExtension = _virtualMarkerInfo.ID.ToString();

        if (!_marker.MarkerInfo.Picture.IsEmpty)
        {
            fileNameWithoutExtension += "1";
        }

        if (!_uploader.TryUpload(fileType, fileNameWithoutExtension, out var fileName))
            return;

        markerResource.FileName = fileName;

        _dirty = true;
    }

    private void DeleteResource<T>(MarkerResource<T> markerResource, MarkerResource<T> virtualMarkerResource)
        where T : class
    {
        if (markerResource.FileName != virtualMarkerResource.FileName)
        {
            DynamicResources.Delete(virtualMarkerResource.FileName);
        }

        virtualMarkerResource.Clear();

        _dirty = true;
    }

    public void UploadPicture()
    {
        UploadResource(Uploader.FileType.Image, _virtualMarkerInfo.Picture);

        UpdatePicture();
    }

    public void DeletePicture()
    {
        DeleteResource(_marker.MarkerInfo.Picture, _virtualMarkerInfo.Picture);

        UpdatePicture();
    }

    public void UploadAudio()
    {
        UploadResource(Uploader.FileType.Audio, _virtualMarkerInfo.Audio);

        UpdateAudio();
    }

    public void DeleteAudio()
    {
        DeleteResource(_marker.MarkerInfo.Audio, _virtualMarkerInfo.Audio);

        UpdateAudio();
    }

    public void CheckAndHide()
    {
        if (!_dirty)
        {
            Hide();
            return;
        }

        _confirmationWindow.SetTitle(_closeConfirmationMessage);

        _confirmationWindow.Popup()
            .Then(result =>
            {
                if (result)
                {
                    ProcessSaveResource(_marker.MarkerInfo.Picture, _virtualMarkerInfo.Picture);
                    ProcessSaveResource(_marker.MarkerInfo.Audio, _virtualMarkerInfo.Audio);

                    _marker.Setup(_virtualMarkerInfo);
                    OnChanged.Invoke(_marker);
                }
                else
                {
                    ProcessNotSaveResource(_marker.MarkerInfo.Picture, _virtualMarkerInfo.Picture);
                    ProcessNotSaveResource(_marker.MarkerInfo.Audio, _virtualMarkerInfo.Audio);
                }
            })
            .Finally(() =>
            {
                Hide();
            });
    }

    private void ProcessSaveResource(MarkerResourceBase markerResource, MarkerResourceBase virtualMarkerResource)
    {
        if (virtualMarkerResource.IsEmpty && !markerResource.IsEmpty)
        {
            DynamicResources.Delete(markerResource.FileName);
        }
        else if (!virtualMarkerResource.IsEmpty && !markerResource.IsEmpty && markerResource.FileName != virtualMarkerResource.FileName)
        {
            DynamicResources.Delete(markerResource.FileName);
            DynamicResources.Rename(virtualMarkerResource.FileName, markerResource.FileName);
            virtualMarkerResource.FileName = markerResource.FileName;
        }
    }

    private void ProcessNotSaveResource(MarkerResourceBase markerResource, MarkerResourceBase virtualMarkerResource)
    {
        if (!virtualMarkerResource.IsEmpty && (markerResource.IsEmpty || markerResource.FileName != virtualMarkerResource.FileName))
        {
            DynamicResources.Delete(virtualMarkerResource.FileName);
        }
    }

    private void OnTitleChanged(string text)
    {
        _virtualMarkerInfo.Title = text;

        _dirty = true;
    }

    private void OnDescriptionChanged(string text)
    {
        _virtualMarkerInfo.Description = text;

        _dirty = true;
    }
}