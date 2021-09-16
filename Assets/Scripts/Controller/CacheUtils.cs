using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

class CacheUtils
{
    public static string file(string filename)
    {
        if(Application.platform == RuntimePlatform.OSXEditor)
            return Path.Combine("/Users/m/temp/", filename);
        if(Application.platform == RuntimePlatform.Android)
        {
            //"/sdcard/Download/"
            return Path.Combine(Application.persistentDataPath, filename);
        }
        return Path.Combine(Application.persistentDataPath, filename);
    }
    public static string fileForUrl(string url, string extension)
    {
        string filename = String.Format("xyray_{0}.{1}", url.GetHashCode().ToString("X"), extension);
        return file(filename);
    }
    public void DeleteAllCachedFiles()
    {
        var dir = new DirectoryInfo(file(""));
        foreach (var file in dir.EnumerateFiles("xyray_*")) {
            file.Delete();
        }
    }

    public static void RemoveCacheVideo(Dictionary<string, object> scene)
    {
        if(!scene.ContainsKey("url"))
            return;
        string filename = fileForUrl((string)scene["url"], "mp4");
        if (File.Exists(filename))
            File.Delete(filename);
    }

    public static void RemoveCacheGallery(Dictionary<string, object> scene)
    {
        if(scene == null || !scene.ContainsKey("pictures"))
        {
            Debug.LogError("scene == null || !scene.ContainsKey(pictures)");
            return;
        }
        List<object> object_pictures = (List<object>)scene["pictures"];
        foreach (object pic_obj in object_pictures)
        {
            string pic_url = (string)pic_obj;
            string filename = fileForUrl(pic_url, "JPG");
            if (File.Exists(filename))
                File.Delete(filename);
        }
    }
}
