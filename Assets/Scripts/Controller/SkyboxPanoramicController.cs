// https://stackoverflow.com/questions/45032579/editing-a-cubemap-skybox-from-remote-image
// https://forum.unity.com/threads/how-to-set-a-skybox-from-an-image-url.420476/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkyboxPanoramicController : MonoBehaviour
{
    [Header("Next Prev Buttons auto hide")]
    public GameObject NextButton;
    public GameObject PrevButton;
    Dictionary<string, object> scene = null;
    List<string> pictures;
    int index = 0;

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
        bool has_prev = true;
        bool has_next = true;
        if(index == 0)
            has_prev = false;
        if(index == pictures.Count-1)
            has_next = false;

        NextButton.SetActive(has_next);
        PrevButton.SetActive(has_prev);

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