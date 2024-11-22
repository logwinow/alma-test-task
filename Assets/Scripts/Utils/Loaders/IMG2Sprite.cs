using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using RSG;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using UnityEngine.Networking;

/// <summary>
///     <href>https://discussions.unity.com/t/generating-sprites-dynamically-from-png-or-jpeg-files-in-c/591103/4</href>
/// </summary>
public static class IMG2Sprite
{

    //Static class instead of _instance
    // Usage from any other script:
    // MySprite = IMG2Sprite.LoadNewSprite(FilePath, [PixelsPerUnit (optional)], [spriteType(optional)])

    public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.FullRect)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Texture2D SpriteTexture = LoadTexture(FilePath);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    public static IPromise<Sprite> LoadNewSpriePromise(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.FullRect)
    {
        var promise = new Promise<Sprite>();

        LoadTextureAsync(FilePath)
            .ContinueWith(task => 
            {
                if (task.IsFaulted)
                {
                    promise.Reject(task.Exception);

                    return;
                }

                promise.Resolve(ConvertTextureToSprite(task.Result, PixelsPerUnit, spriteType));
            }, TaskContinuationOptions.ExecuteSynchronously);

        return promise;
    }

    public static Sprite ConvertTextureToSprite(Texture2D texture, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.FullRect)
    {
        // Converts a Texture2D to a sprite, assign this texture to a new sprite and return its reference

        Sprite NewSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    public static Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public static async Task<Texture2D> LoadTextureAsync(string FilePath)
    {
        Texture2D texture = null;
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(FilePath, true))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                    throw new Exception($"{uwr.error}");
                else
                {
                    texture = DownloadHandlerTexture.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                throw new Exception($"{err.Message}, {err.StackTrace}");
            }
        }

        return texture;
    }
}