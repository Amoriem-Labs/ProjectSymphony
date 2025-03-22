using System.Collections.Generic;
using UnityEngine;

public static class AssetCache
{
    // Dictionaries for caching sprites and audio clips
    private static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
    private static Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

    // Loads a sprite from the Resources folder and caches it.
    public static Sprite GetSprite(string path)
    {
        if (spriteCache.TryGetValue(path, out Sprite cachedSprite))
        {
            return cachedSprite;
        }
        else
        {
            Sprite newSprite = Resources.Load<Sprite>(path);
            if (newSprite != null)
            {
                spriteCache[path] = newSprite;
            }
            else
            {
                Debug.LogWarning($"Sprite not found at: {path}");
            }
            return newSprite;
        }
    }

    // Loads audio clip from the Resources folder and caches it.
    public static AudioClip GetAudioClip(string path)
    {
        if (audioCache.TryGetValue(path, out AudioClip cachedClip))
        {
            return cachedClip;
        }
        else
        {
            AudioClip newClip = Resources.Load<AudioClip>(path);
            if (newClip != null)
            {
                audioCache[path] = newClip;
            }
            else
            {
                Debug.LogWarning($"AudioClip not found at: {path}");
            }
            return newClip;
        }
    }

    ///  clear the cache 
    public static void ClearCache()
    {
        spriteCache.Clear();
        audioCache.Clear();
    }
}
