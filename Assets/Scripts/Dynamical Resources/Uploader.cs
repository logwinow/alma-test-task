using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Uploader : MonoBehaviour
{
    [SerializeField] private string _openFileWindowName = "Âûבונטעו פאיכ";
    [SerializeField] private FileTypeMeta[] _fileTypesMeta;

    public enum FileType
    {
        Image, Audio
    }

    [Serializable]
    private class FileTypeMeta
    {
        [SerializeField] private FileType _fileType;
        [SerializeField] private string _fileTypeName;
        [SerializeField] private string[] _extensions;

        public FileType FileType => _fileType;
        public string FileTypeName => _fileTypeName;
        public string[] Extensions => _extensions;
    }

    public bool TryUpload(FileType fileType, string fileNameWithoutExtension, out string filename)
    {
        var meta = _fileTypesMeta.First(ft => ft.FileType == fileType);
        var paths = StandaloneFileBrowser.OpenFilePanel(_openFileWindowName, "", new[] { new ExtensionFilter(meta.FileTypeName, meta.Extensions) }, false);

        if (paths.Length == 0)
        {
            filename = null;
            return false;
        }

        var source = paths[0];
        var extension = Path.GetExtension(source);

        filename = fileNameWithoutExtension + extension;

        DynamicResources.Upload(source, filename);

        return true;
    }
}
