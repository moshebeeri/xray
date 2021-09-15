using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

class URLFileHandler : MonoBehaviour
{
	string link_field_name;
    public URLFileHandler(string link_field_name)
    {
        this.link_field_name = link_field_name;
    }
    public IEnumerator Fetch(StateManager stateManager, Dictionary<string, object> scene)
    {
        if(!scene.ContainsKey(link_field_name))
            yield break;
        string url = (string)scene[link_field_name];
        string filename = String.Format("{0}.mp4", url.GetHashCode().ToString("X"));
        if (!File.Exists(file(filename)))
        {
            UnityWebRequest www = new UnityWebRequest(url);
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                yield break;
            }
            // Or retrieve results as binary data
            File.WriteAllBytes(file(filename), www.downloadHandler.data);
        }
    }
    private string file(string filename)
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

    public IEnumerator RemoveLocalData(Dictionary<string, object> scene)
    {
        if(!scene.ContainsKey(link_field_name))
            yield break;
        string url = (string)scene[link_field_name];
        string filename = String.Format("{0}.mp4", url.GetHashCode().ToString("X"));
        File.Delete(file(filename));
    }
}
