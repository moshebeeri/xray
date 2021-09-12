// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkyboxPanoramicController : MonoBehaviour
{
    Dictionary<string, object> scene = null;
    List<string> pictures;
    int index = 0;
    bool has_prev = true;
    bool has_next = true;

    // string first_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0016.JPG?alt=media&token=c0412b8b-a107-429f-9ddb-0f9bdd63c619";
    // string next_url = "https://firebasestorage.googleapis.com/v0/b/xray-vr.appspot.com/o/TLV%2FPromenade%2FGS__0023.JPG?alt=media&token=2c23e798-ddd6-4cdf-9492-5e7da180fea0";

    public void next()
    {
        Debug.Log("next panoramic image");
        if(pictures.Count > index+1)
        {
            index++;
            StartCoroutine(setImage());
        }

    }
    public void prev()
    {
        Debug.Log("prev panoramic image");
        if(index-1 >= 0)
        {
            index--;
            StartCoroutine(setImage());
        }
    }

    private void HandleBoundaries()
    {
        has_prev = true;
        has_next = true;
        if(index == 0)
            has_prev = false;
        if(index == pictures.Count-1)
            has_next = false;
    }

    void Start()
    {
        scene = ToursInfo.CurrentSceneData;
        if (!scene.ContainsKey("pictures"))
        {
            Debug.LogError("!scene.ContainsKey(pictures)");
            return;
        }
        List<object> object_pictures = (List<object>)scene["pictures"];
        pictures = new List<string>();
        foreach(object o in object_pictures)
            pictures.Add((string)o);
        StartCoroutine(setImage());
    }

    IEnumerator setImage()
    {
        string url = pictures[index];
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        HandleBoundaries();
        RenderSettings.skybox.mainTexture = DownloadHandlerTexture.GetContent(request);
    }
}