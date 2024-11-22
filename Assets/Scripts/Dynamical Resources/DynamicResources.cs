using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class DynamicResources
{
    static DynamicResources()
    {
        _dynamicResourcesDirectoryInfo = new DirectoryInfo(Path.Combine(Application.persistentDataPath, "Dynamical Resources"));

        if (!_dynamicResourcesDirectoryInfo.Exists)
            _dynamicResourcesDirectoryInfo.Create();
    }

    private static DirectoryInfo _dynamicResourcesDirectoryInfo;

    public static string Upload(string sourcePath, string filename)
    {
        var destination = Path.Combine(_dynamicResourcesDirectoryInfo.FullName, filename);
        File.Copy(sourcePath, destination, true);

        return destination;
    }

    public static bool TryLoad(string filename, out string resourcePath)
    {
        var filePath = Path.Combine(_dynamicResourcesDirectoryInfo.FullName, filename);

        if (!File.Exists(filePath))
        {
            resourcePath = null;
            return false;
        }

        resourcePath = filePath;

        return true;
    }

    public static void Rename(string originalFileName, string newFileName)
    {
        var originalFilePath = Path.Combine(_dynamicResourcesDirectoryInfo.FullName, originalFileName);
        var newFilePath = Path.Combine(_dynamicResourcesDirectoryInfo.FullName, newFileName);

        File.Move(originalFilePath, newFilePath);
    }

    //public static void DeleteAllAssociatedWith(string fileNameWithoutExtension)
    //{
    //    foreach (var file in _dynamicResourcesDirectoryInfo.EnumerateFiles(fileNameWithoutExtension + ".*"))
    //    {
    //        file.Delete();
    //    }
    //}

    public static void Delete(string fileName)
    {
        var filePath = Path.Combine(_dynamicResourcesDirectoryInfo.FullName, fileName);

        if (!File.Exists(filePath))
            return;

        File.Delete(filePath);
    }
}
